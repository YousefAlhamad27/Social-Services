using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Feedback
{
    public class FeedbackEntity
    {

        public int FeedbackID { get; set; }
        public int UserID { get; set; }
        public int ServiceApplicationID { get; set; }
        public byte Rating { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
