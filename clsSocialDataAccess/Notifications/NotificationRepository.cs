using clsSocialServicesDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Notifications
{
    public class NotificationRepository:INotifcationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateNotification(NotificationEntity notification)
        {
            try
            {
                 _context.Notifications.Add(notification);
                _context.SaveChanges();
                return true;

            }
            catch
            {

                return false;
            }
        }
        public bool DeleteNotification(int notificationID) {
            try
            {
              NotificationEntity entity=  _context.Notifications.FirstOrDefault(n=>n.NotificationID==notificationID)!;
                if (entity != null)
                {
                    _context.Notifications.Remove(entity);
                    _context.SaveChanges();
                    return true;
                }
                return false;
                
            }
            catch
            {
                return false;
            }
        
        }
        public NotificationEntity GetNotification(int notificationID)
        {
            try
            {
                return _context.Notifications.FirstOrDefault(n => n.NotificationID == notificationID)!;

            }
            catch
            {
                return null;
            }
        }
        public List<NotificationEntity> GetAllNotifications(int userID) {
            try
            {
               return _context.Notifications.Where(n => n.UserID == userID).ToList();

            }
            catch
            {
                return null;
            }
        }
        public bool Update(NotificationEntity entity) {
            try
            {
                _context.Notifications.Update(entity);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
