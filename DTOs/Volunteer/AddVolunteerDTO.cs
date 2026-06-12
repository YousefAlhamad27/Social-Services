using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Volunteer
{
    public class AddVolunteerDTO
    {
        public int UserID { get; set; }
        public int VolunteerApplicationID { get; set; }
        public string? Description { get; set; }
          


    }
}
