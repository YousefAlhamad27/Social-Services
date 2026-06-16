using Microsoft.AspNetCore.Mvc;
using DTOs;

namespace SocialServices.Web_Objects
{
    public class UpdatePersonalDetails
    {
        [FromForm] public  PersonDetailsUpdateDTO data { get; set; }
        public IFormFile? Image {  get; set; }
        public bool imageChanged { get; set; }

    }
}
