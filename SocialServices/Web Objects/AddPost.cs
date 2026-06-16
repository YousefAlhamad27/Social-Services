using DTOs.Posts;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Web_Objects
{
    public class AddPost
    {
     [FromForm] public AddPostDTO Data { get; set; }
       public IFormFile? Image { get; set; }

    }
}
