using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Volunteer
{
    public class CertificateListDTO
    {

        public int CertificateID { get; set; }
        public int VolunteerID { get; set; }

        public DateTime CreationDate { get; set; }


        public int NumberOfAccomplishedServices { get; set; }

        public string Classifcation { get; set; }
    }
}
