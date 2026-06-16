using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Volunteer
{
    public class GetVolunteerDTO
    {

        public int VolunteerID { get; set; }
        public int UserID { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Description { get; set; }
        public string IdImagePath { get; set; }

        public int PointsCount { get; set; }
        public int AccomplishedServiceApplicationsCount { get; set; }
    }
}
