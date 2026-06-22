using clsSocialDataAccess.Notifications;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsNotification
    {
        public enum NotificaitonType
        {
            Volunteer=1,
            ServiceApplication=2,
            Post=3,
            Feedback=4,
            VolunteerApplication=5,
            Certificate=6,
        }

        private readonly INotifcationRepository _notificationRepository;

        public clsNotification(INotifcationRepository notificationRepository)
        {

            _notificationRepository = notificationRepository;
        }

        private string TypeString(NotificaitonType type)
        {
            switch (type)
            {
                case NotificaitonType.Volunteer:
                    return "Volunteer";

                case NotificaitonType.ServiceApplication:
                    return "ServiceApplication";
                case NotificaitonType.Post:
                    return "Post";
                case NotificaitonType.Feedback:
                    return "Feedback";
                case NotificaitonType.VolunteerApplication:
                    return "VolunteerApplication";
                case NotificaitonType.Certificate:
                    return "Certificate";
                default:
                    return "";
            }
        }

        public bool CreateNotification(int userID,int typeID,NotificaitonType type,string title,string description)
        {
            
            NotificationEntity notification = new NotificationEntity();
            notification.TypeName = TypeString(type);
            if (notification.TypeName == "")
                return false;
            notification.UserID = userID;
            notification.TypeID = typeID;
            notification.Title = title;
            notification.Description = description;
            notification.CreatedDate = DateTime.Now;   
            
            
            return _notificationRepository.CreateNotification(notification);
        }
        public List<NotificationDTO> GetNotificationList(int userID) {

            List<NotificationEntity> notificationList = _notificationRepository.GetAllNotifications(userID);
            List<NotificationDTO> DTOList= new List<NotificationDTO>();

            foreach (NotificationEntity notification in notificationList)
            {
                NotificationDTO notificationDTO = new NotificationDTO();
                notificationDTO.NotificationID = notification.NotificationID;
                notificationDTO.UserID = userID;
                notificationDTO.Title=notification.Title;
                notificationDTO.Description=notification.Description;
                notificationDTO.CreatedDate = notification.CreatedDate;
                notificationDTO.TypeName = notification.TypeName;
                notificationDTO.TypeID = notification.TypeID;
                notificationDTO.IsViewed=notification.IsViewed;
                DTOList.Add(notificationDTO);

            }
        return DTOList;
        }
        public bool DeleteNotification(int notificationID)
        {
            return _notificationRepository.DeleteNotification(notificationID);
        }
        public NotificationDTO GetNotification(int notificationID) {
      NotificationEntity entity=  _notificationRepository.GetNotification(notificationID);
            if(entity!=null)

            return new NotificationDTO
            {
                NotificationID = notificationID,
                CreatedDate = entity.CreatedDate,
                Description = entity.Description,
                Title =
                entity.Title,
                TypeID = entity.TypeID,
                TypeName = entity.TypeName,
                UserID = entity.UserID,
                IsViewed = entity.IsViewed
            };
            
            return null!;

        }
        public bool ViewNotificaiton(int notificationID) {
            NotificationEntity entity = _notificationRepository.GetNotification(notificationID);

            entity.IsViewed = true;

            return _notificationRepository.Update(entity);
        
        }
    }
}
