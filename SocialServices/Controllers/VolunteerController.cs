using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using clsSocialServicesBussiness;
using DTOs.Volunteer;
using Microsoft.AspNetCore.Authorization;
using DTOs;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/Volunteer")]
    public class VolunteerController : Controller
    {
        
        private readonly clsVolunteer volunteerService;
        private readonly clsUser userService;

        public VolunteerController(clsVolunteer volunteerService,clsUser user)
        {
            this.volunteerService = volunteerService;
            userService = user;
        }

        [HttpPost("Issue Volunteer Request")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddVolunteerApplication(AddVolunteerRequest request)
        {
            int currentUserID= Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            UserDTO user = userService.Find(currentUserID);

            if(user == null) return Unauthorized();

            if(!user.IsActive)
                return BadRequest("User is inactive and unallowed to become a volunteer");

            if (!volunteerService.CanUserBecomeVolunteer(currentUserID))
                return BadRequest("User already has a pending request to become a volunteer.");

            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            bool result = await volunteerService.IssueVolunteerRequest(request);
            if (Convert.ToBoolean(result))
            {
                return Ok("Volunteer request issued successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while issuing the volunteer request.");
            }

        }

    }
    
}
