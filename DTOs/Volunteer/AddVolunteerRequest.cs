using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Volunteer
{
    public class AddVolunteerRequest
    {
        public int UserID { get; set; }
        public string? IdImagePath { get; set; }
        public string? Description { get; set; }
         public List<string>? ProofImagePaths { get; set; }
         



    }
}
