using clsSocialDataAccess.Volunteers;
using clsSocialServicesDataAccess.Posts;
using clsSocialServicesDataAccess.Services;
using DTOs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsServiceApplication
    {
        private readonly  IServiceApplicationRepository _repo;
        private readonly IVolunteerRepository _volunteerRepository;
        private readonly IPostRepository _post;
        private readonly clsNotification _Notifcation;

        public clsServiceApplication(IServiceApplicationRepository repo,IPostRepository post, IVolunteerRepository _vol,clsNotification notification)
        {
            _volunteerRepository = _vol;
            _Notifcation= notification;
            _repo = repo;
            _post = post;
        }

        private ServiceApplicationEntity MapAddServiceDTO(AddServiceDTO dto ,int userID,int volunteerID)
        {
            return new ServiceApplicationEntity
            {
                PostID = dto.PostID,
                Description = dto.Description,
                UserID = userID,
                ApplyDateTime = DateTime.Now,
                Status = 1,
                VolunteerID = volunteerID
            };
        }

        public async Task<bool> isVolunteerAllowedToCreateService(int userID,int postID)
        {
            PostEntity post=await _post.GetPostById(postID);
            if (post.RequiredServicesCount != post.AcceptedServiceApplicationsCount) {
            
            List<ServiceApplicationEntity> services = getServicesForPost(postID);
                 
                if(services.Where(service=>service.UserID == userID).Any())
                {
                    return false;
                }
                return true;
            }
            return false;


        }
        public bool addService(AddServiceDTO dto,int userID)
        {
            VolunteerEntity entity=_volunteerRepository.GetVolunteerByUserID(userID);
            if (entity == null)
                return false;

            int serviceID= _repo.Create(MapAddServiceDTO(dto, userID, entity.VolunteerID));
            if(serviceID == 0) return false;
            
            return _Notifcation.CreateNotification(entity.UserID, serviceID, clsNotification.NotificaitonType.ServiceApplication, "Service Application Added",
                "Your Service Application has been successfully Added!");
        }
        public List<ServiceApplicationEntity> getServicesForPost(int postID)
        {
            return _repo.GetServicesForPost(postID);
        }
        public List<ServiceApplicationEntity> getServicesForUser(int userID)
        {
            return _repo.GetListForUser(userID);
        }
        public bool doesServiceBelongToUser(int serviceID,int userID)
        {
            return _repo.doesServiceBelongToUser(serviceID,userID);
        }
        public bool AcceptSerivceApplication(int userID,int serviceApplicationID,bool isAccepted,string? AcceptanceMessage)
        {

            PostEntity post= _repo.GetPostByServiceApplication(serviceApplicationID);
            if (post.RequiredServicesCount == post.AcceptedServiceApplicationsCount)
            {
                return false ;
            }
            string notTitle;
            string notDescription;
            if (isAccepted)
            {
                post.AcceptedServiceApplicationsCount += 1;
                notTitle = "Service Application Accepted";
                notDescription = $"Your Service Application For \"{post.PostTitle}\" has been accepted!";
            }
            else
            {
                notTitle = "Service Application Rejected";
                notDescription = $"Your Service Application For \"{post.PostTitle}\" has been rejected!";

            }
                _post.UpdatePost(post);

            _Notifcation.CreateNotification(userID, serviceApplicationID, clsNotification.NotificaitonType.ServiceApplication, notTitle,
                notDescription);
            return _repo.RespondToServiceApplication(userID,serviceApplicationID,isAccepted,AcceptanceMessage);

        }
        public ServiceApplicationDTO Find(int serviceID)
        {
            ServiceApplicationEntity entity = _repo.Find(serviceID);

            if (entity == null)
                return null!;
            return  new ServiceApplicationDTO {ServiceApplicationID=entity.ServiceApplicationID,AcceptanceMessage=entity.AcceptanceMessage
           ,Status=entity.Status,ApplyDateTime=entity.ApplyDateTime,Description=entity.Description,PostID=entity.PostID,UserID=entity.UserID,VolunteerID=entity.VolunteerID};
        }
        public bool DeleteServiceApplication(int serviceApplicationID)
        {
            ServiceApplicationEntity entity= _repo.Find(serviceApplicationID);

            _Notifcation.CreateNotification(entity.UserID, serviceApplicationID, clsNotification.NotificaitonType.ServiceApplication, "Service Application Deleted",
                $"Your Service application \"{entity.Description}\" has been deleted.");
            return _repo.Delete(serviceApplicationID);
        }
    }
}
