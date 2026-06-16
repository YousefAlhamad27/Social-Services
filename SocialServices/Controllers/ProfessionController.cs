using clsSocialServicesBussiness;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Controllers
{
    [Route("api/Profession")]
    [ApiController]
    public class ProfessionController : ControllerBase
    {
        private clsProfession _profession;

        public ProfessionController(clsProfession profession)
        {
            _profession = profession;
        }

        [HttpGet("GetProfessionIdByProfessionName", Name = "GetProfessionIdByProfessionName")
            ,ProducesResponseType(StatusCodes.Status200OK)
            ,ProducesResponseType(StatusCodes.Status400BadRequest)
            ,ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> GetProfessionId(string ProfessionTitle)
        {
            if (ProfessionTitle == null)
            {
                return BadRequest("Invalid value.");
            }

            var professionId = await _profession.GetProfessionIdByProfessionName(ProfessionTitle);

            if(professionId == 0)
            {
                return NotFound("No Profession founded.");
            }

            return Ok(professionId);
        }

        [HttpGet("GetAllProfessions",Name = "GetAllProfessions")
            ,ProducesResponseType(StatusCodes.Status200OK)
            ,ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> GetAllProfession()
        {
            var Profession = await _profession.GetAllProfessions();

            if(Profession == null)
            {
                return NotFound("No profession found.");
            }

            return Ok(Profession);
        }
    }
}
