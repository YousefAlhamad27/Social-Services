using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Volunteers
{
    public class CertficateEntity
    {
        public int CertificateID { get; set; }
        public int VolunteerID { get; set; }

        public DateTime CreationDate { get; set; }

        public int NumberOfAccomplishedServices { get; set; }



    }
}
