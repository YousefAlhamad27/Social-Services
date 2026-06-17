using DTOs.Login;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Web_Objects
{
    public class AddFullUserDetails
    {
        [FromForm] public  RegisterRequestDTO RegisterRequestDTO { get; set; }
        public IFormFile? postImage { get; set; }    
     

    }
}
