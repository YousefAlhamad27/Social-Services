using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clsSocialServicesDataAccess.Feedback;
using DTOs.Services;
 

namespace clsSocialServicesBussiness
{
    public class clsFeedBack
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public clsFeedBack(IFeedbackRepository feedbackRepository)
        {
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
            return _feedbackRepository.Create(MapFeedbackDtoToEntity(feedback,userID));
        }
        public bool IsUserEligibleForPostingFeedback(int serviceApplicationID, int userID)
        {
            return _feedbackRepository.IsUserEligibleForPostingFeedback(serviceApplicationID, userID);
        }
        public double GetAverageRatingForUser(int userID)
        {
            return _feedbackRepository.GetAverageRatingForUser(userID);
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
