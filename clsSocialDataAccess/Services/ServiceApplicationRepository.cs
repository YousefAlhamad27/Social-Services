using clsSocialServicesDataAccess.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Services
{
    public class ServiceApplicationRepository:IServiceApplicationRepository
    {
        private readonly AppDbContext _context;
        public ServiceApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Create(ServiceApplicationEntity service)
        {
            try
            {
                _context.ServiceApplications.Add(service);
                _context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool doesServiceBelongToUser(int serviceID,int userID)
        {
            try
            {
                ServiceApplicationEntity service = _context.ServiceApplications.Find(serviceID)!;
                if(service.UserID == userID)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int serviceID)
        {
            try
            {
                ServiceApplicationEntity service = _context.ServiceApplications.Find(serviceID)!;
                _context.ServiceApplications.Remove(service);
                _context.SaveChanges();
                return false;
            }
            catch
            {
                return false;
            }

            
        }
        public ServiceApplicationEntity Find(int serviceID)
        {
            try
            {
                return _context.ServiceApplications.Find(serviceID)!;
            }
            catch
            {
                return null!;
            }
        }
        public List<ServiceApplicationEntity> GetListForUser(int userID)
        {
            try
            {
                IQueryable<ServiceApplicationEntity> query = _context.ServiceApplications.Where(s => s.UserID == userID);

                query = query.OrderBy(s => s.ApplyDateTime);
                return query.ToList();
            }
            catch
            {
                return null!;
            }
        }
        public List<ServiceApplicationEntity> GetServicesForPost(int postID)
        {
            try
            {
                IQueryable<ServiceApplicationEntity> query = _context.ServiceApplications.Where(s => s.PostID == postID);

                query = query.OrderBy(s => s.ApplyDateTime);
                return query.ToList();
            }
            catch {      
                return null!;
            }




        }
        public bool AcceptServiceApplication(int userID,int serviceApplicationID,string? AcceptanceMessage)
        {

            try
            {
                ServiceApplicationEntity serviceApplication = _context.ServiceApplications.FirstOrDefault(s => s.ServiceApplicationID == serviceApplicationID)!;
            if (!DoesPostBelongToUser(userID, serviceApplication.PostID)) {
                return false;
            }
            PostEntity post
                    = _context.Posts.FirstOrDefault(p => p.PostID == serviceApplication.PostID)!;
                if (post == null)
                    return false;
                if (post.IsComplete || post.LockDate != null)
                    return false;

                    serviceApplication.Accepted = true;
                serviceApplication.AcceptanceMessage = AcceptanceMessage;
                _context.ServiceApplications.Update(serviceApplication);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DoesPostBelongToUser(int userID, int? postID)
        {
            try
            {
                PostEntity post = _context.Posts.FirstOrDefault(p => p.PostID == postID)!;
                if (postID != null && post != null)
                {
                    if (post.UserID == userID)
                        return true;
                    else
                        return false;
                }
                return false;
            }
            catch
            {
                return false;
            }
           

        }
        public bool Update(ServiceApplicationEntity service)
        {
            try
            {
                _context.ServiceApplications.Update(service);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public PostEntity GetPostByServiceApplication(int serviceApplicationID) {
            try
            {
                ServiceApplicationEntity serviceApplication = _context.ServiceApplications.FirstOrDefault(p => p.ServiceApplicationID == serviceApplicationID)!;

                return  _context.Posts.FirstOrDefault(post => post.PostID == serviceApplication.PostID)!;
                 
            }
            catch
            {
                return null;
            }
        
        }

    }
}
 