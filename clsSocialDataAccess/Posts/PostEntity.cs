using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Posts
{
    public class PostEntity
    {


        public int PostID { get; set; }
        public int PostTypeID { get; set; }

        [Column("ProfessionID"),ForeignKey("ProfessionID")]
        public int ProfessionID { get; set; }
        public int CountyID { get; set; }
        public int UserID { get; set; }
        public string? ImagePath { get; set; }
        public string PostTitle { get; set; } 
        public string? Description { get; set; }
        public DateTime PublishDateTime { get; set; }
        public DateTime? LockDate { get; set; }
        public bool IsComplete { get; set; }
        public int RequiredServicesCount { get; set; }
        public int AcceptedServiceApplicationsCount { get; set; }
        public byte Status { get; set; }
        public decimal? Price { get; set; }

        [Column(TypeName = "decimal(12,6)")]
        public decimal? Latitude { get; set; }
        [Column(TypeName = "decimal(12,6)")]
        public decimal? Longitude { get; set; }

    }
}
