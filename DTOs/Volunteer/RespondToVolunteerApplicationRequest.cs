using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Volunteer
{
    public class RespondToVolunteerApplicationRequest
    {
        public int VolunteerApplicationID { get; set; }
        public int AdminID { get; set; }
        public bool IsApproved { get; set; }
       

    }
}
