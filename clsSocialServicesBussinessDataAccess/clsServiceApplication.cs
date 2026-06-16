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

        public clsServiceApplication(IServiceApplicationRepository repo,IPostRepository post, IVolunteerRepository _vol)
        {
            _volunteerRepository = _vol;
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
                Accepted = false,
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
            

            return _repo.Create(MapAddServiceDTO(dto,userID,entity.VolunteerID));
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
        public async Task<bool> AcceptSerivceApplication(int userID,int serviceApplicationID,string? AcceptanceMessage)
        {

            PostEntity post= _repo.GetPostByServiceApplication(serviceApplicationID);
            if (post.RequiredServicesCount == post.AcceptedServiceApplicationsCount)
            {
                return false ;
            }

            post.AcceptedServiceApplicationsCount += 1;
            _post.UpdatePost(post);
            return _repo.AcceptServiceApplication(userID,serviceApplicationID,AcceptanceMessage);

        }
        public ServiceApplicationEntity Find(int serviceID)
        {
            return _repo.Find(serviceID);
        }
        public bool DeleteServiceApplication(int serviceApplicationID)
        {
            return _repo.Delete(serviceApplicationID);
        }
    }
}
