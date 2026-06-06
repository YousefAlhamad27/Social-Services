using clsSocialServicesBussiness;
using clsSocialServicesDataAccess;
using DTOs.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace SocialServices.Controllers
{
    [Route("api/Admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly clsAdminService _clsAdminPostService;

        public AdminController(clsAdminService clsAdminPostService)
        {
            _clsAdminPostService = clsAdminPostService;
        }

        [HttpGet("GetDashBoardStatus", Name = "GetDashBoardStatus"),
        ProducesResponseType(StatusCodes.Status200OK),
        ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin")]

        public async Task<ActionResult> GetDashBoardStatus()
        {
            try
            {
                var status = await _clsAdminPostService.GetDashBoardStatus();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error : {ex.Message}");
            }
        }

        [HttpPost("Login", Name = "Login"),
            ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login([FromBody] LoginAdminDTO DTO)
        {
            var token = _clsAdminPostService.Login(DTO.UserName, DTO.Password);

            if (token == null)
                return Unauthorized("Invalid username or password.");

            return Ok("Token = " + token);
        }

        [HttpPut("BlockUser", Name = "BlockUser"), ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status401Unauthorized),
            ProducesResponseType(StatusCodes.Status500InternalServerError),
            ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> BlockUser(int UserID)
        {
            try
            {
                int adminId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                bool result = await _clsAdminPostService.BlockUser(UserID,adminId);

                if (result)
                    return Ok("User has been blocked successfully");
                else
                    return BadRequest("Failed to block user");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("UnBlockUser", Name = "UnBlockUser"), ProducesResponseType(StatusCodes.Status200OK),
            ProducesResponseType(StatusCodes.Status401Unauthorized),
            ProducesResponseType(StatusCodes.Status500InternalServerError),
            ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UnBlockUser(int UserID)
        {
            try
            {
                int adminId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                var result = await _clsAdminPostService.UnBlockUser(UserID , adminId);

                if (result)
                {
                    return Ok("User has been unblocked successfully");

                }
                else
                {
                    return BadRequest("Failed to unblock user");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("GetLoginOrRegisterLog",Name = "GetLoginOrRegisterLog"),ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetLoginRegisterLog()
        {
            return Ok(await _clsAdminPostService.GetLogs("Login or Register"));
        }

        [HttpGet("GetUserLogs",Name = "GetUsersLogs"),ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetUserLogs()
        {
            return Ok(await _clsAdminPostService.GetLogs("User"));
        }

        [HttpGet("GetPostLogs", Name = "GetPostLogs"), ProducesResponseType(StatusCodes.Status200OK),ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetPostLogs()
        {
            return Ok(await _clsAdminPostService.GetLogs("Post"));
        }

        [HttpGet("GetAllUsres",Name ="GetAllUsers")
            ,ProducesResponseType(StatusCodes.Status200OK)
            ,ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserEntity>>> GetAllUsers()
        {
            var result = await _clsAdminPostService.GetAllUsers();

            if(result == null)
            {
                return NotFound("No Users Founded!");
            }

            return Ok(result);
        }
    }
}
