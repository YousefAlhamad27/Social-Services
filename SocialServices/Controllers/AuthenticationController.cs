using clsSocialServicesBussiness;
using DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly clsUser _userService;
        private readonly clsPerson _personSerivce;

        // The Controller only depends on the Business Logic
        public AuthenticationController(clsUser userService, clsPerson personSerivce)
        {
            // The framework ensures UserService is NOT NULL
            _userService = userService;
            _personSerivce = personSerivce;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult RegisterUser(RegisterRequestDTO reqDto)
        {


            if (!UtilLibrary.checkReqDTOValues(reqDto))
                return BadRequest("Register Credentials are not valid");
            // code for checking username

            if (_userService.CheckUsernameExistence(reqDto.Username))
            {
                return BadRequest("Username Already Exists");
            }


            // add person first

            int personID = _personSerivce.AddPerson(reqDto);

            // if person was not added return server error
            if (personID == -1)
                return StatusCode(500, new { Message = "Error adding person" });


            //add new user then check whether addition was success and hash the password
            int userID = _userService.RegisterNewUser(new RegisterRequestDTO(
                 "", "", "", "", "", 0, "", reqDto.Username, UtilLibrary.returnHashedPassword(reqDto.PasswordHash), reqDto.IsActive, reqDto.CreationDate));

            if (userID == -1)
            {
                // delete added person
                return StatusCode(500, new { Message = "Error adding user" });
            }

            return Ok("User registered Sucessfully!");
        }

        //[HttpPost("Login")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]

        //public ActionResult Login(LoginRequest request) {
        
        
        //}

    }
}
