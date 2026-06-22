using clsSocialServicesDataAccess.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Services
{
    public interface IServiceApplicationRepository
    {

        public int Create(ServiceApplicationEntity service);
        public bool Delete(int serviceID);
        public bool Update(ServiceApplicationEntity service);
        public ServiceApplicationEntity Find(int serviceID);
        public List<ServiceApplicationEntity> GetListForUser(int userID);
        public List<ServiceApplicationEntity> GetServicesForPost(int postID);
        public bool doesServiceBelongToUser(int serviceID, int userID);
        public bool RespondToServiceApplication(int userID, int serviceApplicationID, bool isAccepted, string? Message);
        public bool DoesPostBelongToUser(int userID, int? postID);
        public PostEntity GetPostByServiceApplication(int serviceApplicationID);
        public ServiceApplicationEntity GetServiceApplicationById(int serviceApplicationID);

    }
}
