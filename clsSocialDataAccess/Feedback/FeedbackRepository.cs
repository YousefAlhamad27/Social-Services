using clsSocialServicesDataAccess.Posts;
using clsSocialServicesDataAccess.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Feedback
{
    public class FeedbackRepository: IFeedbackRepository
    {
        private readonly AppDbContext _context;
        public FeedbackRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Create(FeedbackEntity feedback)
        {
            try
            {
                _context.Add(feedback);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<FeedbackEntity> GetFeedbacksAppliedByUser(int userID)
        {
            try
            {
                var feedbacks = _context.Feedbacks
                    .Where(f => f.UserID == userID)
                    .ToList();
                return feedbacks;
            }
            catch (Exception)
            {
                return null!;
            }
        }
        public List<FeedbackEntity> GetFeedbacksForUser(string  username)
        {
            try
            {
                var feedbacks = (from f in _context.Feedbacks
                                 join sa in _context.ServiceApplications on f.ServiceApplicationID equals sa.ServiceApplicationID
                                 join u in _context.Users on sa.UserID equals u.UserID
                                    where u.Username == username
                                 select f).ToList();
                return feedbacks;
            }
            catch (Exception)
            {
                return null!;
            }
        }
        public double GetAverageRatingForUser(int userID)
        {
            try
            {
                var feedbacks = _context.Feedbacks
                    .Where(f => f.UserID == userID)
                    .ToList();
                if (feedbacks.Count == 0)
                {
                    return 0.0;
                }
                double averageRating = feedbacks.Average(f => f.Rating);
                return averageRating;
            }
            catch (Exception)
            {
                return 0.0;
            }
        }
        public bool IsUserEligibleForPostingFeedback(int serviceApplicationID, int userID)
        {
            try
            {
                ServiceApplicationEntity serviceApplication = _context.ServiceApplications.FirstOrDefault(s => s.ServiceApplicationID == serviceApplicationID)!;
                PostEntity post = _context.Posts.FirstOrDefault(p => p.PostID == serviceApplication.PostID)!;

                if (post.UserID == userID && serviceApplication.UserID != userID&&serviceApplication.Accepted)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { 
                return false;
            }
        }
    }
}
