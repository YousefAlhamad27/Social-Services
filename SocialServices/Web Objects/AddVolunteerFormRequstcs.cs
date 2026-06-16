using DTOs.Volunteer;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Web_Objects
{
    // In your Web/API project
    public class AddVolunteerFormRequest
    {
        [FromForm] public AddVolunteerRequest Data { get; set; }
        public IFormFile GovernmentID { get; set; }
        public List<IFormFile> VolunteerImages { get; set; }
    }
}
