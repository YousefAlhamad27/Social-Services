using clsSocialServicesBussiness;
using DTOs;
using DTOs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/Feedback")]
    public class FeedbackController : Controller
    {

        private readonly clsServiceApplication _serviceApplicationService;
        private readonly clsUser _userService;
        private readonly clsFeedBack _feedbackService;


        public FeedbackController(clsServiceApplication serviceApplicationService, clsFeedBack feedbackService, clsUser user)
        {
            _serviceApplicationService = serviceApplicationService;
            _feedbackService = feedbackService;
            _userService = user;

        }

        [HttpPost("Create Feedback")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)
            , ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]
        public ActionResult Create(AddFeedbackDTO dto)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (userID <= 0)
            {
                return Unauthorized("Not Authorized");

            }



            if (!_serviceApplicationService.doesServiceBelongToUser(dto.ServiceApplicationID, userID))
            {
                if (!_feedbackService.IsUserEligibleForPostingFeedback(dto.ServiceApplicationID, userID))
                {
                    return Unauthorized("Not Authorized to add feedback for this service application");
                }

                bool result = _feedbackService.CreateFeedback(dto, userID);
                if (result)
                {
                    return Ok("Feedback Added Successfully");
                }
                else
                {
                    return StatusCode(500, "An error occurred while adding feedback");
                }
            }
            else
            {
                return Unauthorized("Not Authorized to add feedback for this service application");
            }
        }

        [HttpPost("Get Feedbacks Applied By User")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)
            , ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]
        public ActionResult GetFeedbacksAppliedByUser(string username)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (userID <= 0)
            {
                return Unauthorized("Not Authorized");
            }
            UserDTO user = _userService.Find(username);
            UserDTO currentUser = _userService.Find(userID);

            if (user == null || currentUser == null || user.PersonID != currentUser.PersonID)
            {
                return Unauthorized("Not Authorized to view feedbacks for this user");
            }

            var feedbacks = _feedbackService.GetFeedbacksAppliedByUser(userID);
            return Ok(feedbacks);
        }


        [HttpPost("Get Feedbacks For User")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)
            , ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult GetFeedbacksForUser(string username)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (userID <= 0)
            {
                return Unauthorized("Not Authorized");
            }
            if (!User.Identities.Any
                (identity => identity.IsAuthenticated && identity.HasClaim(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "Admin")))
            {
                UserDTO user = _userService.Find(username);
                UserDTO currentUser = _userService.Find(userID);
                if (user == null || currentUser == null)
                {
                    return Unauthorized("Not Authorized to view feedbacks for this user");
                }
            }
            //else admin implementation, to confirm that admin exists
            var feedbacks = _feedbackService.GetFeedbacksForUser(username);
            return Ok(feedbacks);
        }

        [HttpPost("Get User Average Rating")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)
            , ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult GetUserAverageRating(string username)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (userID <= 0)
            {
                return Unauthorized("Not Authorized");
            }
            if (!User.Identities.Any
                (identity => identity.IsAuthenticated && identity.HasClaim(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "Admin")))
            {
                UserDTO user = _userService.Find(username);
                UserDTO currentUser = _userService.Find(userID);
                if (user == null || currentUser == null)
                {
                    return Unauthorized("Not Authorized to view feedbacks for this user");
                }
            }
            //else admin implementation, to confirm that admin exists
            double averageRating = _feedbackService.GetAverageRatingForUser(userID);
            return Ok(averageRating);

        }


    }
}
