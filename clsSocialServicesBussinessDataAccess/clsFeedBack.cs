using clsSocialDataAccess.Notifications;
using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess.Feedback;
using clsSocialServicesDataAccess.Services;
using DTOs.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace clsSocialServicesBussiness
{
    public class clsFeedBack
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly clsNotification _notifcation;
        private readonly IServiceApplicationRepository serviceApplicationRepository;

        public clsFeedBack(IFeedbackRepository feedbackRepository,IUserRepository user, clsNotification notifcation,IServiceApplicationRepository repository)
        {
            serviceApplicationRepository= repository;
            _notifcation= notifcation;
            _userRepository = user;
            _feedbackRepository = feedbackRepository;
        }
        private FeedbackEntity MapFeedbackDtoToEntity(AddFeedbackDTO feedbackDto,int userID)
        {
            return new FeedbackEntity
            {
               
               UserID=userID,
                ServiceApplicationID = feedbackDto.ServiceApplicationID,
                Rating = feedbackDto.Rating,
               Notes = feedbackDto.Notes,
               CreatedAt = DateTime.Now
            };
        }

        public bool CreateFeedback(AddFeedbackDTO feedback,int userID)
        {
            ServiceApplicationEntity service=serviceApplicationRepository.GetServiceApplicationById(feedback.ServiceApplicationID);
            int feedbackID;
            if (service != null) {
              feedbackID=  _feedbackRepository.Create(MapFeedbackDtoToEntity(feedback, userID));
                if(feedbackID != 0) 
                    
                return _notifcation.CreateNotification(service.UserID, feedbackID , clsNotification.NotificaitonType.Feedback, "Feedback Received",
                    $"You got a feedback for your service -> {service.Description} <- with rating of {feedback.Rating}");
            }
            return false;
        }
        public bool IsUserEligibleForPostingFeedback(int serviceApplicationID, int userID)
        {
            if (_feedbackRepository.GetFeedbacksAppliedByUser(userID).Where(feedback => feedback.ServiceApplicationID == serviceApplicationID).Any())
            {
                return false;
            }

            return _feedbackRepository.IsUserEligibleForPostingFeedback(serviceApplicationID, userID);
        }
        public double GetAverageRatingForUser(string username)
        {
            UserEntity user=_userRepository.FindUserName(username);
            if (user == null)
                return 0;
            return _feedbackRepository.GetAverageRatingForUser(user.UserID);
        }
        public List<FeedbackEntity> GetFeedbacksForUser(string username)
        {
            return _feedbackRepository.GetFeedbacksForUser(username);
        }
        public List<FeedbackEntity> GetFeedbacksAppliedByUser(int userID)
        {
            return _feedbackRepository.GetFeedbacksAppliedByUser(userID);
        }
    }
}
