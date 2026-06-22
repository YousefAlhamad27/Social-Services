using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class NotificationDTO
    {
        public int NotificationID { get; set; }
        public int UserID { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsViewed { get; set; } = false;

    }
}
