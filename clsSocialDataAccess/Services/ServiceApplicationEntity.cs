using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Services
{
    public class ServiceApplicationEntity
    {
        public int ServiceApplicationID { get; set; }
        public int UserID { get; set; }
        public int VolunteerID { get; set; }
        public int PostID { get; set; }
        public DateTime ApplyDateTime { get; set; }
        public bool Accepted { get; set; }
        public string? Description { get; set; }
        public string? AcceptanceMessage { get; set; }


    }
}
