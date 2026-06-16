using DTOs.Posts;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Web_Objects
{
    public class UpdatePost
    {
        [FromForm] public PostUpdateDTO Data {  get; set; }
        public IFormFile? Image {  get; set; }
        public bool ImageChanged { get; set; }

    }
}
