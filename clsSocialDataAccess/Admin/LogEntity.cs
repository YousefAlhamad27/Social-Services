using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Admin
{
    public class LogEntity
    {
        [Key]
        public int LogID { get; set; }

        public string Action {  get; set; } = string.Empty;

        public int TargetID { get; set; }

        public string TargetType { get; set; } = string.Empty;

        public string TargetDescription { get; set; } = string.Empty;

        [Column("AdminID"), ForeignKey("AdminID")]
        public int? AdminID { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}