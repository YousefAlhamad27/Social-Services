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

        public bool Create(ServiceApplicationEntity service);
        public bool Delete(int serviceID);
        public bool Update(ServiceApplicationEntity service);
        public ServiceApplicationEntity Find(int serviceID);
        public List<ServiceApplicationEntity> GetListForUser(int userID);
        public List<ServiceApplicationEntity> GetServicesForPost(int postID);
        public bool doesServiceBelongToUser(int serviceID, int userID);
        public bool AcceptServiceApplication(int userID, int serviceApplicationID, string? AcceptanceMessage);
        public bool DoesPostBelongToUser(int userID, int? postID);
        public PostEntity GetPostByServiceApplication(int serviceApplicationID);


    }
}
