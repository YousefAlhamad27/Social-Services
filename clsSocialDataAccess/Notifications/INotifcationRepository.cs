using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Notifications
{
    public interface INotifcationRepository
    {
        public bool CreateNotification(NotificationEntity notification);
        public List<NotificationEntity> GetAllNotifications(int userID);
        public NotificationEntity GetNotification(int notificationID);
        public bool DeleteNotification(int notificationID);
        public bool Update(NotificationEntity entity);
    }
}
