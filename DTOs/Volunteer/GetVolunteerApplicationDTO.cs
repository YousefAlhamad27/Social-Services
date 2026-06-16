using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Volunteer
{
    public class GetVolunteerApplicationDTO
    {
      

        public int VolunteerApplicationID { get; set; }
        public int UserID { get; set; }
        public DateTime ApplicationDateTime { get; set; }
        public string? Description { get; set; }
       
        public string ApplicationStatus { get; set; }
       public string IdImagePath { get; set; }
        public List<string> ProofImagePaths { get; set; }

        
    }
}
