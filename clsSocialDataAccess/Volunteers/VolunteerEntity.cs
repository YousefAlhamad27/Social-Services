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
    public class VolunteerEntity
    {
        [Key]
        public int VolunteerID { get; set; }

        [ForeignKey("Users")]
        public int userID { get; set; }
        public string? Description { get; set; }

        public int PointsCount { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
