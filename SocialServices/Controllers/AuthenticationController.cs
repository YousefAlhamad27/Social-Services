using Azure.Core;
using clsSocialServicesBussiness;
using DTOs;
using DTOs.User_Person_DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly clsUser _userService;
        private readonly clsPerson _personSerivce;
       

        // The Controller only depends on the Business Logic
        public AuthenticationController(clsUser userService, clsPerson personSerivce )
        {
            // The framework ensures UserService is NOT NULL
            _userService = userService;
            _personSerivce = personSerivce;
           
        }

       
       
        [HttpPost("Register"), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status202Accepted), ProducesResponseType(StatusCodes.Status400BadRequest)]
       
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
                 "", "", "", "", "", 0, "", reqDto.Username, UtilLibrary.returnHashedPassword(reqDto.PasswordHash), reqDto.IsActive, reqDto.CreationDate),personID);

            if (userID == -1)
            {
                // delete added person
                return StatusCode(500, new { Message = "Error adding user" });
            }

            return Ok("User registered Sucessfully!");
        }



        [HttpPost("Login"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]
      

        public ActionResult Login(LoginRequest request)
        {

            UserDTO userDTO = _userService.Find(request.Username);

            if (userDTO == null)
                return Unauthorized("Invalid Username or Password");

            string hashedPassword = UtilLibrary.returnHashedPassword(request.Password);

   

            if (!UtilLibrary.VerifyPassword(request.Password,userDTO.PasswordHash))
                return Unauthorized("Invalid Username or Password");

            string token = _userService.getAccessToken(request.Username);
            string refreshToken = UtilLibrary.GenerateRefreshToken();

            bool isRefreshTokenAdded = _userService.addRefreshToken(refreshToken, request.Username);

            if (!isRefreshTokenAdded) 
                return StatusCode(500, new { Message = "Error generating refresh token" });

            if (token == null)
                return StatusCode(500, new { Message = "Error generating token" });

            return Ok(new {AccessToken= token,RefreshToken=refreshToken});

        }


        [HttpPost("RefreshToken"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult RefreshToken(RefreshTokenDTO dto)
        {

            UserDTO userDTO = _userService.isRefreshTokenValidForUse(dto);
            if (userDTO==null)
                return Unauthorized("unauthorized");

             //_userService.returnUserForRefreshToken(dto.TokenString);

           
            
            string token = _userService.getAccessToken(userDTO.Username);
            string refreshToken = UtilLibrary.GenerateRefreshToken();




            if (_userService.revokeToken(dto,refreshToken) == false)
                return StatusCode(500, new { Message = "Error generating refresh token" });

         
                

             

            if (token == null)
                return StatusCode(500, new { Message = "Error generating token" });

            return Ok(new { Token = token, RefreshToken=refreshToken });

        }
    }
}
