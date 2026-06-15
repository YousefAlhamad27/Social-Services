using clsSocialServicesBussiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/General")]
    public class GeneralController : Controller
    {

        [Authorize(Roles ="User")]
        [HttpPost("Upload Image")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UploadImage(IFormFile image,UtilLibrary.FileOperations.ImageType type)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image uploaded.");
            }

             UtilLibrary.FileOperations.saveImageTofile(image, type);


            return Ok("Image uploaded successfully.");
        }

    }
}
