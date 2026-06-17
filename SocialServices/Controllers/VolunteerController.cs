using clsSocialDataAccess.Volunteers;
using clsSocialServicesBussiness;
using DTOs;
using DTOs.Login;
using DTOs.Posts;
using DTOs.Volunteer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialServices.Web_Objects;
using static clsSocialServicesBussiness.UtilLibrary.FileOperations;
using static SocialServices.Classes.GeneralClass;

namespace SocialServices.Controllers
{
    [ApiController]
    [Route("api/Volunteer")]
    public class VolunteerController : Controller
    {

        private readonly clsVolunteer volunteerService;
        private readonly clsUser userService;

        public VolunteerController(clsVolunteer volunteerService, clsUser user)
        {
            this.volunteerService = volunteerService;
            userService = user;
        }

        [HttpPost("Response To Volunteer Application")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RespondToVolunteerApplication(RespondToVolunteerApplicationRequest request)
        {
            int currentUserID=Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            bool result = await volunteerService.RespondToVolunteerApplication(request);

            if (Convert.ToBoolean(result))
            {
                return Ok("Response to volunteer application processed successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while processing the response to the volunteer application.");
            }
        }


        [HttpPost("Issue Volunteer Request")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddVolunteerApplication([FromForm] AddVolunteerFormRequest form)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            form.Data.IdImagePath = UtilLibrary.FileOperations.saveImageTofile(
                FormFileHelper.ToByteArray(form.GovernmentID),
                form.GovernmentID.FileName,
                ImageType.VolunteerImage
            );

            form.Data.ProofImagePaths = new List<string>();
            foreach (var image in form.VolunteerImages)
            {
                string path = UtilLibrary.FileOperations.saveImageTofile(
                    FormFileHelper.ToByteArray(image),
                    image.FileName,
                    ImageType.VolunteerImage
                );
                form.Data.ProofImagePaths.Add(path);
            }


            if (currentUserID < 1)
                return Unauthorized();

            UserDTO user = userService.Find(currentUserID);

            if (user == null) return Unauthorized();

            if (!user.IsActive)
                return BadRequest("User is inactive and unallowed to become a volunteer");

            if (!volunteerService.CanUserBecomeVolunteer(currentUserID))
                return BadRequest("User already has a pending request to become a volunteer.");

            AddVolunteerRequest request= new AddVolunteerRequest();
            request.UserID = currentUserID;
            request.Description = form.Data.Description;

            request.IdImagePath = form.Data.IdImagePath;
            request.ProofImagePaths = form.Data.ProofImagePaths;
            bool result = await volunteerService.IssueVolunteerRequest(request);
            if (Convert.ToBoolean(result))
            {
                return Ok("Volunteer request issued successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while issuing the volunteer request.");
            }

        }
        [HttpGet("Get VolunteerApplication by ID")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetVolunteerApplicationByID(int appID)
        {
          
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            if (appID < 1)
                return BadRequest("Application ID cannot be less than 1");

            GetVolunteerApplicationDTO dto = await volunteerService.GetVolunteerApplicationByID(appID);

            if (dto != null)
                return Ok(dto);

            return StatusCode(500, "An error occured while fetching the Application");

        }
        [HttpGet("Get Volunteer by userID")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public  IActionResult GetVolunteerByUserID(int userID)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            if (userID < 1)
                return BadRequest("Application ID cannot be less than 1");

            GetVolunteerDTO dto =  volunteerService.GetVolunteerByUserID(userID);

            if (dto != null)
                return Ok(dto);

            return StatusCode(500, "An error occured while fetching the Application");

        }
        [HttpGet("Get all Volunteer Applications")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> GetAllVolunteerApplications()
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (currentUserID < 1)
                return Unauthorized();


            List<GetVolunteerApplicationDTO> applications = await volunteerService.GetAllVolunteerApplications();
            if (applications != null)
                return Ok(applications);
            return StatusCode(500, "An error occured while fetching the Volunteer Applications");

        }
        [HttpPut("Update Volunteer Description")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> UpdateVolunteerDescription(UpdateVolunteerDTO DTO)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            if (currentUserID < 1)

                return Unauthorized();

            if (DTO == null || string.IsNullOrEmpty(DTO.Description))
                return BadRequest("Invalid request data.");

            bool result = await volunteerService.UpdateVolunteerDescription(currentUserID, DTO.Description);

            if (Convert.ToBoolean(result))
            {
                return Ok("Volunteer description updated successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the volunteer description.");
            }

        }

        [HttpPut("Add Volunteer Points")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public IActionResult AddPointsToVolunteer(int pointsCount)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();


           
            UserDTO currentUser = userService.Find(currentUserID);
                if (currentUser == null)
                    return Unauthorized();
    
                if (!currentUser.IsActive)
                    return BadRequest("User is inactive and unallowed to become a volunteer");

            if (volunteerService.IsUserAVolunteer(currentUserID) == false)
                return BadRequest("Current user is not a volunteer.");






            
            bool result =  volunteerService.AddPointsToVolunteer(currentUserID, pointsCount);
            if (Convert.ToBoolean(result))
            {
                return Ok("Points added to volunteer successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while adding points to the volunteer.");
            }

        }


        [HttpPut("Update Volunteer Application Images")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateVolunteerApplicationImages(UpdateVolunteerApplication details)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

         GetVolunteerApplicationDTO application= await    volunteerService.GetVolunteerApplicationByID(details.ApplicationID);
            if (currentUserID != application.UserID)
                return Unauthorized();
            UpdateVolunteerApplicationImagesDTO dto=new UpdateVolunteerApplicationImagesDTO();
            dto.ApplicationID = application.VolunteerApplicationID;

            if (details.IdImageChanged)
            {
                

                if (details.IdImage != null)
                {

                    dto.IdImagePath = UtilLibrary.FileOperations.saveImageTofile(
                  FormFileHelper.ToByteArray(details.IdImage),
                  details.IdImage.FileName,
                  ImageType.VolunteerImage


              );

                }

                UtilLibrary.FileOperations.removeImageFromFile(application.IdImagePath);
            }
            if (details.ProofImagesChanged)
            {

                if (details.ProofImages != null)
                    foreach (var image in details.ProofImages) {
                   
                    

                            dto.ProofImages.Add(
                                UtilLibrary.FileOperations.saveImageTofile(
                          FormFileHelper.ToByteArray(image),
                          image.FileName,
                          ImageType.VolunteerImage));

                    }

                        foreach (var imagePath in application.ProofImagePaths) {
                            UtilLibrary.FileOperations.removeImageFromFile(imagePath);
                        }

                
            }

            if(await volunteerService.UpdateVolunteerApplicationImages(dto))
            {
                return Ok("Images Updated Successfully!");
            }

            return StatusCode(500, "Error occured while Updating images.");

        }


        [HttpPost("Issue Certificate")]
        [Authorize(Roles="User")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> CreateCertificate()
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            int ClassID=volunteerService.IsUserAllowedToIssueACertificate(currentUserID);
            if (ClassID == 0)
                return BadRequest("User isn't allowed to issue a certificate.");

            if(await volunteerService.IssueCertificate(currentUserID, ClassID)){
                return Ok("Certificate has been created successfully!");
            }
            return StatusCode(500, "Error occured while creating new certificate.");

        }
        [HttpGet("Get Certificates")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCertificates()
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            var list=volunteerService.GetVolunteerCertificates(currentUserID);
            if (list == null)
                return BadRequest("Volunteer has no Certificates.");

            return Ok(list);
        }

        [HttpGet("Get Certificate")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCertificate(int certficateID)
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            CertificateListDTO certListDTO = volunteerService.GetCertificate(certficateID,currentUserID);

            if (certListDTO == null)
                return BadRequest("Certificate doesn't exist.");
            return Ok(certListDTO);
        }

        [HttpGet("Is User allowed to Issue A Certificate")]
        [Authorize(Roles = "User,Admin")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError), ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> IsUserAllowedToIssueCertificate()
        {
            int currentUserID = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (currentUserID < 1)
                return Unauthorized();

            if (volunteerService.IsUserAllowedToIssueACertificate(currentUserID) == 0)
                return BadRequest("User isn't allowed to issue a certificate.");

            return Ok(true);
        }

    }


}
