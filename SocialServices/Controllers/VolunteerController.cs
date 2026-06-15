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

        public VolunteerController(clsVolunteer volunteerService, clsUser user)
        {
            this.volunteerService = volunteerService;
            userService = user;
        }

        [HttpPost("Response To Volunteer Application")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RespondToVolunteerApplication(RespondToVolunteerApplicationRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            bool result = await volunteerService.RespondToVolunteerApplication(request);

            if (Convert.ToBoolean(result))
            {
                return Ok("Response to volunteer application processed successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while processing the response to the volunteer application.");
            }
        }


        [HttpPost("Issue Volunteer Request")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddVolunteerApplication(AddVolunteerRequest request)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            UserDTO user = userService.Find(currentUserID);

            if (user == null) return Unauthorized();

            if (!user.IsActive)
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
        [HttpGet("Get VolunteerApplication by ID")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetVolunteerApplicationByID(int appID)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            if (appID < 1)
                return BadRequest("Application ID cannot be less than 1");

            GetVolunteerApplicationDTO dto = await volunteerService.GetVolunteerApplicationByID(appID);

            if (dto != null)
                return Ok(dto);

            return StatusCode(500, "An error occured while fetching the Application");

        }
        [HttpGet("Get Volunteer by userID")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetVolunteerByUserID(int userID)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            if (userID < 1)
                return BadRequest("Application ID cannot be less than 1");

            GetVolunteerDTO dto =  volunteerService.GetVolunteerByUserID(userID);

            if (dto != null)
                return Ok(dto);

            return StatusCode(500, "An error occured while fetching the Application");

        }
        [HttpGet("Get all Volunteer Applications")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> GetAllVolunteerApplications()
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (currentUserID < 1)
                return Unauthorized();


            List<GetVolunteerApplicationDTO> applications = await volunteerService.GetAllVolunteerApplications();
            if (applications != null)
                return Ok(applications);
            return StatusCode(500, "An error occured while fetching the Volunteer Applications");

        }
        [HttpPut("Update Volunteer Description")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> UpdateVolunteerDescription(UpdateVolunteerDTO DTO)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (currentUserID < 1)

                return Unauthorized();

            if (DTO == null || string.IsNullOrEmpty(DTO.Description))
                return BadRequest("Invalid request data.");

            bool result = await volunteerService.UpdateVolunteerDescription(currentUserID, DTO.Description);

            if (Convert.ToBoolean(result))
            {
                return Ok("Volunteer description updated successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the volunteer description.");
            }

        }

        [HttpPut("Add Volunteer Points")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult AddPointsToVolunteer(int pointsCount)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

           
            UserDTO currentUser = userService.Find(currentUserID);
                if (currentUser == null)
                    return Unauthorized();
    
                if (!currentUser.IsActive)
                    return BadRequest("User is inactive and unallowed to become a volunteer");

            if (volunteerService.IsUserAVolunteer(currentUserID) == false)
                return BadRequest("Current user is not a volunteer.");






            
            bool result =  volunteerService.AddPointsToVolunteer(currentUserID, pointsCount);
            if (Convert.ToBoolean(result))
            {
                return Ok("Points added to volunteer successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding points to the volunteer.");
            }

        }

        }

    
}
