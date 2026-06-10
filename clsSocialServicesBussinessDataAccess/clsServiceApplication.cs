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

        public clsServiceApplication(IServiceApplicationRepository repo)
        {
            _repo = repo;
        }

        private ServiceApplicationEntity MapAddServiceDTO(AddServiceDTO dto ,int userID)
        {
            return new ServiceApplicationEntity
            {
                PostID = dto.PostID,
                Description = dto.Description,
                UserID = userID,
                ApplyDateTime = DateTime.Now,
                Accepted = false,
                
            };
        }

        public bool addService(AddServiceDTO dto,int userID)
        {
            return _repo.Create(MapAddServiceDTO(dto,userID));
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
        public bool AcceptSerivceApplication(int userID,int serviceApplicationID,string? AcceptanceMessage)
        {
            
         

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
