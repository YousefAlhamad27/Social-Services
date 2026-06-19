using Azure.Core;
using clsSocialServicesBussiness;
using clsSocialServicesDataAccess.Admin;
using DTOs;
using DTOs.Login;
using DTOs.User_Person_DTOs;
using Microsoft.AspNetCore.Mvc;
using SocialServices.Web_Objects;
using System.Text;
using static clsSocialServicesBussiness.UtilLibrary.FileOperations;
using static SocialServices.Classes.GeneralClass;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly clsUser _userService;
        private readonly clsPerson _personSerivce;
        private readonly ILogRepository _logRepo;

        // The Controller only depends on the Business Logic
        public AuthenticationController(clsUser userService, clsPerson personSerivce, ILogRepository LogRepo)
        {
            // The framework ensures UserService is NOT NULL
            _userService = userService;
            _personSerivce = personSerivce;
            _logRepo = LogRepo;

        }

       
       
        [HttpPost("Register User"), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status202Accepted), ProducesResponseType(StatusCodes.Status400BadRequest)]
       
        public ActionResult RegisterUser([FromForm] AddFullUserDetails userDetails)
        {
            



            // if no data sent return bad request
            if (!UtilLibrary.checkReqDTOValues(userDetails.RegisterRequestDTO))
                return BadRequest("Register Credentials are not valid");

            // check if username already exists
            if (_userService.CheckUsernameExistence(userDetails.RegisterRequestDTO.Username))
            {
                return BadRequest("Username Already Exists");
            }

            if(userDetails.postImage!=null)
            userDetails.RegisterRequestDTO.Imagepath = UtilLibrary.FileOperations.saveImageTofile
                (FormFileHelper.ToByteArray(userDetails.postImage),userDetails.postImage.FileName,ImageType.UserImage);
            
            

            int personID = _personSerivce.AddPerson(userDetails.RegisterRequestDTO);

            // if person was not added return server error
            if (personID == -1)
                return StatusCode(500, new { Message = "Error adding person" });


            // add new user then check whether addition was success and hash the password
            int userID = _userService.RegisterNewUser(new RegisterRequestDTO(
                 "", "", "", "", "", 0, "", userDetails.RegisterRequestDTO.Username,
                 UtilLibrary.returnHashedPassword(userDetails.RegisterRequestDTO.PasswordHash), userDetails.RegisterRequestDTO.IsActive, userDetails.RegisterRequestDTO.CreationDate),personID);

            if (userID == -1)
            {
                
                return StatusCode(500, new { Message = "Error adding user" });
            }

            // if used was added successfully return ok
            return Ok("User registered Sucessfully!");
        }



        [HttpPost("Login User"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError),ProducesResponseType(StatusCodes.Status401Unauthorized)]
      

        public async Task<ActionResult> Login(UserLoginRequest request)
        {

            UserDTO userDTO = _userService.Find(request.Username);

            if (userDTO == null)
                return Unauthorized("Invalid Username or Password");

            if(userDTO.IsActive == false)
            {
                return Unauthorized("User has been blocked.");
            }

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

            await _logRepo.AddLog("Login" ,_userService.getUserID(request.Username) , "Login or Register", "User logged in", null);
            return Ok(new { AccessToken = token, RefreshToken = refreshToken });

        }

        [HttpPost("Login Admin"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status400BadRequest), ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> Login(AdminLoginRequest request)
        {

            UserDTO userDTO = _userService.Find(request.Username);

            if (userDTO == null)
                return Unauthorized("Invalid Username or Password");

            string hashedPassword = UtilLibrary.returnHashedPassword(request.Password);



            if (!UtilLibrary.VerifyPassword(request.Password, userDTO.PasswordHash))
                return Unauthorized("Invalid Username or Password");

            string token = _userService.getAccessToken(request.Username);
            string refreshToken = UtilLibrary.GenerateRefreshToken();

            bool isRefreshTokenAdded = _userService.addRefreshToken(refreshToken, request.Username);

            if (!isRefreshTokenAdded)
                return StatusCode(500, new { Message = "Error generating refresh token" });

            if (token == null)
                return StatusCode(500, new { Message = "Error generating token" });

            await _logRepo.AddLog("Login", _userService.getUserID(request.Username), "Login or Register", "User logged in", null);
            return Ok(new { AccessToken = token, RefreshToken = refreshToken });

        }

        [HttpPost("Refresh Token"), ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]

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
