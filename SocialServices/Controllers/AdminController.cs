using clsSocialServicesBussiness;
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

        public async Task<ActionResult> GetDashBoardStatus ()
        {
            try
            {
                var status = await _clsAdminPostService.GetDashBoardStatus();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"Internal server error : {ex.Message}");
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

            return Ok("Token = "+token);
        }
    }
}
