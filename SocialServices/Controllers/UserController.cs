using Azure.Core;
using clsSocialServicesBussiness;
using DTOs;
using DTOs.User_Person_DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


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
            if (userID == 0)
            {
                return BadRequest("Not Accepted");
            }


            UserDTO userDTO = _userService.Find(Username);

            if (userDTO == null)
                return BadRequest("Not Found");

            if (_userService.deleteUser(userDTO))
                if (_personSerivce.deletePerson(userDTO.PersonID))
                {
                    return Ok("User has been deleted successfully");
                }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Failed to Delete User" });
            ;
        }


        [HttpPatch("Update Personal Details"), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "User,Admin")]
        // [Authorize]
        public ActionResult updateUser(PersonDetailsUpdateDTO updateDTO)
        {

            UserDTO user = _userService.Find(updateDTO.Username);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (_personSerivce.updatePerson(user.PersonID, updateDTO))
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

        public ActionResult getUser(string username)
        {
            string currentUsername = User.Identity!.Name!; // Or specific claim
            string currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value!;
            int userID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (currentUserRole != "Admin" && currentUsername != username)
            {
                return Forbid();
            }
            if (userID == 0)
            {
                return BadRequest("Not Accepted");
            }

            
            UserDTO user = _userService.Find(username);
            if (user == null)
            {
                return BadRequest("User not found");
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
                    Username = username
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
