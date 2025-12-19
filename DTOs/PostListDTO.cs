using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PostListDTO
    {
      
        public int PostID { get; set; }

      
        public int UserID { get; set; }

     
        public string PostTitle { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string AuthorName { get; set; }
        public string ProfessionName { get; set; }
        public string CountyName { get; set; }
        public string PostTypeName { get; set; }
          

        
        public int Status { get; set; } 
        public DateTime PublishDateTime { get; set; }
        public bool IsComplete { get; set; }
        // You can usually skip LockDate and Status for the public list, 
        // unless you use them for styling (e.g. Grey out closed posts).
    }
}
