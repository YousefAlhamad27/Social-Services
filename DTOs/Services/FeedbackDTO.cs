using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Services
{
    public class AddFeedbackDTO
    {
       
        public int ServiceApplicationID { get; set; }
        public byte Rating { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
