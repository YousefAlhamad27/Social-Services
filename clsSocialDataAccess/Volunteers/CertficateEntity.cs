using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Volunteers
{
    public class CertficateEntity
    {
        public enum CertficateClass
        {
            Bronze = 10 ,
            silver = 25,
            Gold = 50,
            Platinum =100,
            Diamond = 200,
            Elite=500
        }

        public int CertificateID { get; set; }
        public int VolunteerID { get; set; }

        public DateTime CreationDate { get; set; }


        public int NumberOfAccomplishedServices { get; set; }

        public int ClassifcationID { get; set; }

       
    }
    public class CertificateClassification
    {

        public int ClassifcationID
        {
            get; set;

        }
        public string Title { get; set; }
        public int RequiredAccomplishedServices { get; set; }

    }
}
