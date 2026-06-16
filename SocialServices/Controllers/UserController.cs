using Azure.Core;
using clsSocialServicesBussiness;
using DTOs;
using DTOs.User_Person_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialServices.Web_Objects;
using static clsSocialServicesBussiness.UtilLibrary.FileOperations;
using static SocialServices.Controllers.GeneralClass;


namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly clsUser _userService;
        private readonly clsPerson _personSerivce;

        // The Controller only depends on the Business Logic
        public UserController(clsUser userService, clsPerson personSerivce)
        {
            // The framework ensures UserService is NOT NULL
            _userService = userService;
            _personSerivce = personSerivce;
        }



        [HttpDelete("Delete User"), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult deleteUser(string Username)
        {

            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
           // string currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value!;
            bool isAdmin = User.IsInRole("Admin");

            UserDTO user1= null;
            if (userID == 0)
            {
                return BadRequest("Not Accepted");
            }
            if (!isAdmin)
            {



                 user1 = _userService.Find(Username);
                UserDTO user2 = _userService.Find(userID);

                if (user1 != null && user2 != null)
                    if (user1.Username != user2.Username && user1.CreationDate != user2.CreationDate)
                        return Unauthorized("You are not authorized to delete this user");

                if (user2 == null)
                    return BadRequest("Not Found");
            }
            else {
                 user1 = _userService.Find(Username);
                if (user1 == null)
                    return BadRequest("Not Found");
            }
            if (_userService.deleteUser(user1!, userID, isAdmin ? userID : null))
                if (_personSerivce.deletePerson(user1!.PersonID))
                {
                    return Ok("User has been deleted successfully");
                }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Failed to Delete User" });
            
        }

        [HttpDelete("Logout Everywhere"), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> logoutEverywhere()
        {
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (userID == 0)
            {
                return BadRequest("User not found");
            }
            if (await _userService.logoutEverywhere(userID))
            {

                return Ok("User logged out from all devices Sucessfully!");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error Logging Out" });
            }
        }

        [HttpPatch("Update Personal Details"), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "User,Admin")]
        // [Authorize]
        public ActionResult updateUser(UpdatePersonalDetails details)
        {

            UserDTO user = _userService.Find(details.data.Username);
          

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (user.IsActive == false)
            {
                return BadRequest("User is not active");
            }

            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

           
            if (details.imageChanged)
            {
                PersonDTO person = _personSerivce.Find(user.PersonID);

                if (details.Image != null)
                {

                    details.data.Imagepath = UtilLibrary.FileOperations.saveImageTofile(
                  FormFileHelper.ToByteArray(details.Image),
                  details.Image.FileName,
                  ImageType.UserImage


              );

                }
                UtilLibrary.FileOperations.removeImageFromFile(person.Imagepath);
            }




            if (_personSerivce.updatePerson(user.PersonID, details.data,userID))
            {
                return Ok("User updated Sucessfully!");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error Updating Person Details" });
            }

        }
        [HttpPatch("Update User Password"), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User")]
        public ActionResult updateUserPassword(PasswordUpdateDTO dto)
        {

            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (userID == 0)
            {
                return BadRequest("User not found");
            }

            UserDTO user = _userService.Find(userID);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (user.IsActive == false)
            {
                return BadRequest("User is not active");
            }

            if (!UtilLibrary.VerifyPassword(dto.CurrentPassword, user.PasswordHash))
                return Unauthorized("Invalid Username or Password");

            user.PasswordHash = UtilLibrary.returnHashedPassword(dto.NewPassword);

            if (!_userService.updatePassword(user))
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error Updating Password" });
            }


            return Ok("Password updated Sucessfully!");


        }
        [HttpGet("Get User"), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "User,Admin")]

        public ActionResult getUser(string username="", int userID = -1)
        {
            string currentUsername = User.Identity!.Name!; // Or specific claim
            string currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value!;
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if(userID==-1 && string.IsNullOrEmpty(username))
            {
                return BadRequest("Please provide either username or userID");
            }
            UserDTO user;

            if (userID < 1)
            {
                if (currentUserRole != "Admin" && currentUsername != username)
                {
                    return Forbid();
                }
                if (currentUserID == 0)
                {
                    return BadRequest("Not Accepted");
                }
                 user = _userService.Find(username);

                if (user == null)
                {
                    return BadRequest("User not found");
                }
             
                if (user.IsActive == false)
                {
                    return BadRequest("User is not active");
                }
            }
            else {
                 user = _userService.Find(userID);
                if (user == null)
                {
                    return BadRequest("User not found");
                }
            }


                PersonDTO person = _personSerivce.Find(user.PersonID);
            if (person != null )
            {
                GetUserDTO getUserDTO = new GetUserDTO
                {
                     
                    CreationDate = user.CreationDate,
                    IsActive = user.IsActive,
                    Imagepath = person.Imagepath,
                    SecondName = person.SecondName,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Email = person.Email,
                    Phone = person.Phone,
                    Age = person.Age,
                    Username = user.Username
                };
                return Ok(getUserDTO);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound, new { Message = "User Not Found" });
            }

            
        }


    }


}
