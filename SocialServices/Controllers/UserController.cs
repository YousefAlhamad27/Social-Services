using clsSocialServicesBussiness;
using Microsoft.AspNetCore.Mvc;
using DTOs;
using System.Security.Cryptography;
using System.Text;


namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly clsUser _userService;
        private readonly clsPerson _personSerivce;

        // The Controller only depends on the Business Logic
        public UserController(clsUser userService,clsPerson personSerivce)
        {
            // The framework ensures UserService is NOT NULL
            _userService = userService;
            _personSerivce = personSerivce;
        }

      

        [HttpDelete("Delete User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult deleteUser(string Username)
        {
            UserDTO userDTO = _userService.Find(Username);
           
            if (userDTO == null)
                return BadRequest("Not Found");

            if (_userService.deleteUser(userDTO))
                if (_personSerivce.deletePerson(userDTO.PersonID))
            {
                return Ok("User has been deleted successfully");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new  { Message= "Failed to Delete User" });
;        }


        [HttpPatch("Update Personal Details")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult updateUser(PersonDetailsUpdateDTO updateDTO)
        {
            UserDTO user = _userService.Find(updateDTO.Username);

            if(user == null)
            {
                return BadRequest("User not found");
            }

            if (_personSerivce.updatePerson(user.PersonID,updateDTO))
            {
                return Ok("User updated Sucessfully!");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Error Updating Person Details" });
            }

        }

    }


}
