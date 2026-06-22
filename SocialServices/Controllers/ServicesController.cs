using clsSocialServicesBussiness;
using DTOs.Posts;
using DTOs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/Services")]
    public class ServicesController : Controller
    {

        private readonly clsServiceApplication _serviceApplication;
        private readonly clsUser _user;
        private readonly clsPost _post;
        public ServicesController(clsServiceApplication serviceApplication, clsUser user, clsPost post)
        {
            _serviceApplication = serviceApplication;
            _user = user;
            _post = post;
        }

        [HttpPost("Create Service Application")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status400BadRequest),
     ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User")]

        public async Task<IActionResult> CreateService(AddServiceDTO dto)
        {
            if (dto.PostID <= 0)
            {
                return BadRequest("Invalid Post ID");
            }
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var userExists = _user.Find(userID);
            if (userExists == null)
            {
                return Unauthorized("User not found");
            }
            bool isUserAllowedToCreateService = await _serviceApplication.isVolunteerAllowedToCreateService(userID,dto.PostID);
            if (!isUserAllowedToCreateService)
                return BadRequest("Volunteer cannot issue a service application for this post");

            bool isCreated = _serviceApplication.addService(dto, userID);
            if (isCreated)
            {
                return Ok("Service Application Created Successfully");
            }
            else
            {
                return StatusCode(500, "An error occurred while creating the service application");
            }

        }

        [HttpGet("Get Service Applications for Post")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status400BadRequest),
        ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult GetServicesForPost(int postID)
        {
            if (postID <= 0)
            {
                return BadRequest("Invalid Post ID");
            }

            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (!User.IsInRole("Admin"))
            {


                var userexists = _user.Find(userID);



                if (userexists == null)
                {
                    return Unauthorized("user not found");
                }
                PostDTO? postDetails = _post.getPost(postID);
                if (postDetails == null || postDetails.UserID != userID)
                {
                    return BadRequest("Forbidden");
                }


            }

            var services = _serviceApplication.getServicesForPost(postID);
            return Ok(services);

        }
        [HttpGet("Get Service Applications for User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status400BadRequest),
   ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult GetServicesForUser(string username)
        {


            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (!User.IsInRole("Admin"))
            {


                var userexists = _user.Find(currentUserID);
                var userexistsByName = _user.Find(username);

                if (userexists == null || userexistsByName == null)
                {
                    return Unauthorized("user not found");
                }

                if (userexists.Username != userexistsByName.Username)
                    return Unauthorized("forbidden");




            }


            int userID = _user.getUserID(username);

            var services = _serviceApplication.getServicesForUser(userID);
            return Ok(services);

        }

        [HttpPost("Respond To Service Application")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status400BadRequest),
ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User")]
        public ActionResult AcceptServiceApplication(int serviceApplicationID,bool IsAccepted,string? AcceptanceMessage)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID <= 0)
                return Unauthorized("Not allowed");

            if ( _serviceApplication.AcceptSerivceApplication(currentUserID, serviceApplicationID,IsAccepted ,AcceptanceMessage))
            {
                return Ok("Response Sent Successfully!");
            }
            return StatusCode(StatusCodes.Status500InternalServerError);

        }


        [HttpDelete("Delete Service Application")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status400BadRequest),
            ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult DeleteServiceApplication(int serviceApplicationID)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (currentUserID <= 0)
                return Unauthorized("Not allowed");
            if (!User.IsInRole("Admin"))
            {
                var serviceApp = _serviceApplication.Find(serviceApplicationID);
                if (serviceApp == null || serviceApp.UserID != currentUserID)
                {
                    return Unauthorized("Not allowed to delete this service application");
                }

            }
            // confirm admin exists

            if (_serviceApplication.DeleteServiceApplication(serviceApplicationID))
            {
                return Ok("Service Application Deleted Successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


    }
}
