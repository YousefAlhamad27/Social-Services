using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Posts
{
    public class PostUpdateDTO
    {
        public int PostID { get; set; }
        public int UserID { get; set; }
        public string? Description { get; set; }
        public string PostTitle { get; set; }
        public int CountyID { get; set; }
        public byte ServicesRequiredCount { get; set; }
         
        public string? imagePath { get; set; }
        public decimal? Price { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

    }
}
