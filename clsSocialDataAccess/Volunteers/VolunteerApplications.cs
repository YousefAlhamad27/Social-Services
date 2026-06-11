using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Volunteers
{
    public class VolunteerApplicationEntity
    {

        [Key]
        public int VolunteerApplicationID { get; set; }

        [ForeignKey("Users")]
        public int UserID { get; set; }
        [ForeignKey("Admins")]
        public int AdminID { get; set; }
        public string IdImagePath { get; set; }
        public string? Description { get; set; }
        public IVolunteerRepository.ApplicationStatus  Status { get; set; } = IVolunteerRepository.ApplicationStatus.Pending;


        public ICollection<VolunteerProofImage> ProofImages { get; set; }
    }

    public class VolunteerProofImage
    {
        public int ImageID { get; set; }
        public int VolunteerApplicationID { get; set; }
        public string ImagePath { get; set; }
    }
}
