using clsSocialServicesBussiness;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/Notifications")]
    public class NotificationController : Controller
    {
        private readonly clsNotification _notification;

        public NotificationController(clsNotification notification)
        {
            _notification = notification;
        }


        [HttpGet("Get Notification")]
        [Authorize(Roles="User")]
        [ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult GetNotification(int notificationID)
        {
            NotificationDTO notificationDTO = _notification.GetNotification(notificationID);
            if (notificationDTO == null)
                return BadRequest("Notification Does not exist");
            return Ok(notificationDTO);
        }
        [HttpGet("Get Notifications")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult GetNotifications()
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            List<NotificationDTO> notificationDTO = _notification.GetNotificationList(currentUserID);

            if (notificationDTO == null)
                return BadRequest("User has no notifications");
            return Ok(notificationDTO);
        }

        [HttpDelete("Delete Notification")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult DeleteNotification(int notificationID)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

             bool isDeleted=_notification.DeleteNotification(notificationID);


            if(!isDeleted)
                return BadRequest("User has no notifications");

            return Ok("Notification has been deleted.");
        }

        [HttpPut("View Notification")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult ViewNotification(int notificationID)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            
            bool isDone=_notification.ViewNotificaiton(notificationID);

            if (!isDone)
                return BadRequest("Notification does not exist.");

            return Ok("Done.");
        }


    }
}
