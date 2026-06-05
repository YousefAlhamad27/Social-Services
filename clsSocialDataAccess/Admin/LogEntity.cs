using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Admin
{
    public class LogEntity
    {
        [Key]
        public int LogId { get; set; }

        public string Action {  get; set; } = string.Empty;

        public int TargetId { get; set; }

        public string TargetType { get; set; } = string.Empty;

       public string TargetDescription { get; set; } = string.Empty;

        public int? AdminId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
