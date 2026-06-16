using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Volunteer
{
    public class UpdateVolunteerApplicationImagesDTO
    {
        public int ApplicationID { get; set; }
        public string IdImagePath { get; set; }
        public List<string> ProofImages { get; set; }=new List<string>();

    }
}
