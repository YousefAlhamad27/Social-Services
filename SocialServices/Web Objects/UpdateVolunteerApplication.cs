using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Web_Objects
{
    public class UpdateVolunteerApplication
    {
        // [FromForm]
        public int ApplicationID { get; set; }
        public IFormFile IdImage {  get; set; }
        public bool IdImageChanged { get; set; }
        public List<IFormFile> ProofImages { get; set; }
        public bool ProofImagesChanged { get; set; }

    }
}
