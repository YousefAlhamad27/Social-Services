using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTOs.Posts;
using clsSocialServicesBussiness;
using DTOs;

namespace SocialServices.Controllers
{

    [ApiController]
    [Route("api/Posts")]
    public class PostsController : Controller
    {
        private readonly clsPost _postService;
        private readonly clsUser _userService;

        public PostsController(clsPost postService, clsUser userService)
        {
            _postService = postService;
            _userService = userService;
        }

        [HttpGet("Get All Posts")]
        [Authorize(Roles = "User,Admin")]
        public ActionResult GetAllPosts()
        {
            int currentUserID= Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID == 0)
            {
                return Unauthorized("Invalid User");
            }
            if(!User.IsInRole("Admin"))
            {
                UserDTO user = _userService.Find(currentUserID);
                if (user == null)
                {
                    return Unauthorized("Invalid User");
                }
            }
            //else   implementation for admin 
            //{
                
            //}

            List<PostListDTO> posts = _postService.getAllPosts();
            if (posts == null)
            {
                return StatusCode(500, new { Message = "Error retrieving posts" });
            }
            return Ok(posts);

        }

        [HttpPost("Create Post"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]

        public ActionResult CreatePost(AddPostDTO dto)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            UserDTO user = _userService.Find(userID);

            if (user == null)
            {
                return Unauthorized("Invalid User");
            }

            if (!_postService.addPost(userID, dto))
            {
                return StatusCode(500, new { Message = "Error adding post" });
            }



            // Implementation to create a new post
            return Ok("Post Created Successfully");
        }

        [HttpDelete("Delete Post"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult DeletePost(int postID)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (postID <= 0)
                return BadRequest("Post Does not exist");

            if (userID == 0)
            {
                return Unauthorized("Invalid User");
            }

            if (User.IsInRole("Admin"))
            {
                // Admin can delete any post
                if (!_postService.deletePost(postID))
                {
                    return StatusCode(500, new { Message = "Error deleting post" });
                }
                return Ok("Post Deleted Successfully by Admin");
            }

            else
            {
                UserDTO user = _userService.Find(userID);

                if (user == null)
                {
                    return Unauthorized("Invalid User");
                }
                if (!_postService.deletePost(postID))
                {
                    return StatusCode(500, new { Message = "Error deleting post" });
                }
                // Implementation to delete a post
                return Ok("Post Deleted Successfully");
            }
        }

        [HttpPut("Update Post"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]

        public ActionResult UpdatePost(PostUpdateDTO dto)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (dto.PostID <= 0)
                return BadRequest("Post Does not exist");

            if (userID == 0)
            {
                return Unauthorized("Invalid User");
            }

            UserDTO CurrentUser = _userService.Find(userID);
           
            var sentUser = _userService.Find(dto.UserID);

            if (sentUser != null && CurrentUser != null)
            {
                if (sentUser.Username == CurrentUser.Username)
                {
                    if (sentUser == null)
                    {
                        return Unauthorized("user not found");
                    }
                }
                else
                {
                    return Unauthorized("Forbidden");
                }
            }
            else
            {
                return Unauthorized("Invalid User");
            }
           

            if (!_postService.updatePost(userID, dto))
            {
                return StatusCode(500, new { Message = "Error updating post" });
            }
            // Implementation to update a post
            return Ok("Post Updated Successfully");
        }

        [HttpGet("Get User Posts Post"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult GetUserPosts(string username)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (currentUserID == 0)
            {
                return Unauthorized("Invalid User");
            }
            
            if (!User.IsInRole("Admin"))
            {
                UserDTO user = _userService.Find(username);
                if (user == null)
                {
                    return Unauthorized("Invalid User");
                }
            }
            int userID = _userService.getUserID(username);
            
            List<PostListDTO> posts = _postService.getAllPosts(userID);

            if (posts == null)
            {
                return StatusCode(500, new { Message = "Error retrieving posts" });
            }
            return Ok(posts);
        }
        [HttpGet("Get Filtered Posts"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult GetFilteredPosts(string? searchQuery, int? countyID, int? postTypeID, int? professionID)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (userID == 0)
            {
                return Unauthorized("Invalid User");
            }
            UserDTO user = _userService.Find(userID);
            if (user == null)
            {
                return Unauthorized("Invalid User");
            }

            List<PostListDTO> posts = _postService.FilteredList(searchQuery, countyID, postTypeID, professionID);
            if (posts == null)
            {
                return StatusCode(500, new { Message = "Error retrieving posts" });
            }
            return Ok(posts);
        }

        [HttpPost("Complete Post"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]
        public ActionResult CompletePost(int postID)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (postID <= 0)
                return BadRequest("Post Does not exist");

            if (userID == 0)
            {
                return Unauthorized("Invalid User");
            }
            UserDTO user = _userService.Find(userID);
            if (user == null)
            {
                return Unauthorized("Invalid User");
            }
            if (!_postService.CompletePost(userID, postID))
            {
                return StatusCode(500, new { Message = "Error completing post" });
            }
            // Implementation to complete a post
            return Ok("Post Completed Successfully");
        }
        [HttpPost("Lock Post"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User,Admin")]

        public ActionResult LockPost(int postID)
        {

            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (postID <= 0)
                return BadRequest("Post Does not exist");

            if (currentUserID == 0)
            {
                return Unauthorized("Invalid User");
            }
            if (User.Identities.Any
                (identity => identity.IsAuthenticated && identity.HasClaim(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "Admin")))
            {
                // Admin can lock any post
                // we can also confirm admin by checking variable "currentUserID"
                // inside the bussiness logic
                if (!_postService.LockPost(postID, null))
                {
                    return StatusCode(500, new { Message = "Error locking post" });
                }
                return Ok("Post Locked Successfully by Admin");
            }

            UserDTO user = _userService.Find(currentUserID);
            if (user == null)
            {
                return Unauthorized("Invalid User");
            }
            // we will ensure that the currentUserID belongs to the user who has the access token of this request
            if (!_postService.CompletePost(currentUserID, postID))
            {
                return StatusCode(500, new { Message = "Error completing post" });
            }
            // Implementation to complete a post
            return Ok("Post Locked Successfully");
        }

        [HttpPost("Unlock Post"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin")]
        public ActionResult UnlockPost(int postID)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (postID <= 0)
                return BadRequest("Post Does not exist");
            if (currentUserID == 0)
            {
                return Unauthorized("Invalid User");
            }
            // Admin can unlock any post
            // we can also confirm admin by checking variable "currentUserID"
            // inside the bussiness logic
            if (!_postService.UnlockPost(postID, currentUserID))
            {
                return StatusCode(500, new { Message = "Error unlocking post" });
            }
            return Ok("Post Unlocked Successfully by Admin");
        }

    }
}
