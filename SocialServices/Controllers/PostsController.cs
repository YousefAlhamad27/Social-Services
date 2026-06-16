using clsSocialServicesBussiness;
using DTOs;
using DTOs.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialServices.Web_Objects;
using System.Security.Claims;
using static clsSocialServicesBussiness.UtilLibrary.FileOperations;
using static SocialServices.Controllers.GeneralClass;

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

        public ActionResult CreatePost(AddPost post)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            int postID = _postService.GetLastPostIdByUser(userID);

            UserDTO user = _userService.Find(userID);

            if (user == null)
            {
                return Unauthorized("Invalid User");
            }
            
               

                if (post.Image != null)
                {

                post.Data.imagePath = UtilLibrary.FileOperations.saveImageTofile(
                  FormFileHelper.ToByteArray(post.Image),
                  post.Image.FileName,
                  ImageType.PostImage);
}
                
            


            if (!_postService.addPost(userID, post.Data,postID))
            {
                return StatusCode(500, new { Message = "Error adding post" });
            }

            // Implementation to create a new post
            return Ok("Post Created Successfully");
        }

        [HttpDelete("Delete Post")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult DeletePost(int postID)
        {
            int userID = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (postID <= 0)
                return BadRequest("Post does not exist");

            if (userID == 0)
                return Unauthorized("Invalid User");

            bool isAdmin = User.IsInRole("Admin");

            int? adminId = isAdmin ? userID : (int?)null;
            int? ownerUserId = isAdmin ? (int?)null : userID;

            if (!isAdmin)
            {
                UserDTO user = _userService.Find(userID);
                if (user == null)
                    return Unauthorized("Invalid User");
            }

            if (!_postService.deletePost(postID, ownerUserId, adminId))
                return StatusCode(500, new { Message = "Error deleting post" });

            string message = isAdmin ? "Post Deleted Successfully by Admin" : "Post Deleted Successfully";
            return Ok(message);
        }

        [HttpPut("Update Post"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]

        public ActionResult UpdatePost(UpdatePost details)
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (details.Data.PostID <= 0)
                return BadRequest("Post Does not exist");

            if (userID == 0)
            {
                return Unauthorized("Invalid User");
            }

            UserDTO CurrentUser = _userService.Find(userID);
           
            var sentUser = _userService.Find(details.Data.UserID);

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

            if (details.ImageChanged)
            {
                PostDTO post=_postService.getPost(details.Data.PostID)!;
                if (post != null)
                {

                    if (details.Image != null)
                    {

                        details.Data.imagePath = UtilLibrary.FileOperations.saveImageTofile(
                      FormFileHelper.ToByteArray(details.Image),
                      details.Image.FileName,
                      ImageType.PostImage


                  );

                    }

                    UtilLibrary.FileOperations.removeImageFromFile(post.imagePath!);
                }
                else
                {
                    return BadRequest("Post does not exist");
                }
            }

            if (!_postService.updatePost(userID, details.Data))
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
                 bool isAdmin = User.IsInRole("Admin");
                if (!_postService.LockPost(postID, currentUserID,isAdmin?currentUserID:null))
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

        [HttpGet("GetPostById", Name = "GetPostById")
            ,ProducesResponseType(StatusCodes.Status200OK)
            ,ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetPostById(int postID)
        {
            var result = await _postService.GetPostById(postID);

            if(result == null)
            {
                return NotFound("No post found!");
            }
            return Ok(result);
        }
    }
}
