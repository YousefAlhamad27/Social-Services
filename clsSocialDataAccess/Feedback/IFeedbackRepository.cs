using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Feedback
{
    public interface IFeedbackRepository
    {
        public bool Create(FeedbackEntity feedback);
   //     public bool Delete(int feedbackID);
   //     public bool Update(FeedbackEntity feedback);
     //   public FeedbackEntity Find(int feedbackID);
       // public List<FeedbackEntity> GetListForUser(int userID);
      //  public List<FeedbackEntity> GetFeedbackForServiceApplication(int serviceApplicationID);

    }
}
