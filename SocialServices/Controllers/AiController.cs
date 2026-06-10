using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using clsSocialServicesBussiness;
namespace SocialServices.Controllers
{
    [Route("api/AI")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly clsAiRecommendationService _aiService;

        public AiController(clsAiRecommendationService aiService)
        {
            _aiService = aiService;
        }

        [HttpGet("GetRecommendations")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> GetRecommendations(string userMessage)
        {
            int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _aiService.GetRecommendations(userMessage, userId);
            return Ok(result);
        }
    }
}
