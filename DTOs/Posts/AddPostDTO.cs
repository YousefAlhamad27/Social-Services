using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Posts
{
    public class AddPostDTO
    {
        public int TypeID { get; set; }
        public DateTime PublishDate { get; set; }

        public string? Description { get; set; }
        public int ProfessionID { get; set; }
        public string PostTitle { get; set; }
        public int CountyID { get; set; }
        public string imagePath {get; set; }
        public byte Status { get; set; }
        public bool isComplete { get; set; }
        public decimal ? Price { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
