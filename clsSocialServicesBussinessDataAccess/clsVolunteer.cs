using clsSocialDataAccess.Volunteers;
using clsSocialServicesDataAccess;
using DTOs.Volunteer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsVolunteer
    {
        private readonly IVolunteerRepository repository;
        private readonly IUserRepository user_rep;


        public clsVolunteer(IVolunteerRepository context, IUserRepository user) { repository = context; user_rep = user; }

        public async Task<bool> AddVolunteer(VolunteerEntity volunteer)
        {
            return await repository.AddVolunteer(volunteer);
        }
        public async Task<bool> AddVolunteerProofImages(List<VolunteerProofImage> proofImages)
        {
            foreach (var proofImage in proofImages)
            {
                if (!await repository.AddVolunteerProofImage(proofImage))
                    return false;
            }
            return true;
        }
        public async Task<bool> IssueVolunteerRequest(AddVolunteerRequest addVolunteerRequest)
        {
            if (UtilLibrary.FileOperations.saveImageTofile(addVolunteerRequest.IdImagePath, UtilLibrary.FileOperations.ImageType.VolunteerImage) == null) return false;


            foreach (var proofImagePath in addVolunteerRequest.ProofImagePaths)
            {
                if (UtilLibrary.FileOperations.saveImageTofile(proofImagePath, UtilLibrary.FileOperations.ImageType.VolunteerImage) == null)
                {
                    return false;
                }
            }

            VolunteerApplicationEntity request = new VolunteerApplicationEntity
            {
                UserID = addVolunteerRequest.UserID,
                Description = addVolunteerRequest.Description,
                AdminID = null,
                CreationDate = DateTime.Now,
                IdImagePath = addVolunteerRequest.IdImagePath
               ,
                Status = IVolunteerRepository.ApplicationStatus.Pending
            };


            int RequestID = await repository.IssueVolunteerRequest(request);

            if (RequestID == 0)
            {
                return false;
            }

            List<VolunteerProofImage> proofImages = addVolunteerRequest.ProofImagePaths.Select(path => new VolunteerProofImage { VolunteerApplicationID = RequestID, ImagePath = path }).ToList();

            if (!await AddVolunteerProofImages(proofImages))
                return false;

            return true;

        }

        public bool CanUserBecomeVolunteer(int userID)
        {


            return repository.CanUserApplyToBeVolunteer(userID);
        }

        public async Task<bool> RespondToVolunteerApplication(RespondToVolunteerApplicationRequest request)
        {
            VolunteerApplicationEntity application = repository.GetVolunteerApplicationByID(request.VolunteerApplicationID);

            if (application == null || application.Status != IVolunteerRepository.ApplicationStatus.Pending)
            {
                return false; // Application not found or not pending
            }

            application.AdminID = request.AdminID;
            application.Status = request.IsApproved ? IVolunteerRepository.ApplicationStatus.Approved : IVolunteerRepository.ApplicationStatus.Rejected;
            bool updateResult = await repository.UpdateVolunteerApplication(application);
            if (!updateResult)
            {
                return false; // Failed to update the application
            }

            if (request.IsApproved)
            {
                VolunteerEntity newVolunteer = new VolunteerEntity
                {
                    UserID = application.UserID,
                    VolunteerApplicationID = application.VolunteerApplicationID,
                    Description = application.Description,
                    PointsCount = 0,
                    CreationDate = DateTime.Now,
                    AccomplishedServiceApplicationsCount = 0
                };

                bool addResult = await AddVolunteer(newVolunteer);
                return addResult;
            }
            return true;
        }
        public string GetAppStatusString(IVolunteerRepository.ApplicationStatus status)
        {
            switch (status)
            {
                case IVolunteerRepository.ApplicationStatus.Pending:
                    return "Pending";
                case IVolunteerRepository.ApplicationStatus.Approved:
                    return "Approved";
                case IVolunteerRepository.ApplicationStatus.Rejected:
                    return "Rejected";
                default:
                    return "Unknown";
            }
        }


        public async Task<GetVolunteerApplicationDTO> GetVolunteerApplicationByID(int applicationID)
        {



            VolunteerApplicationEntity entity = repository.GetVolunteerApplicationByID(applicationID);
            List<VolunteerProofImage> images=await repository.GetProofImagesByApplicationID(applicationID);


            if (entity == null)
            {
                return null;
            }
            string statusString = GetAppStatusString(entity.Status);

            GetVolunteerApplicationDTO dto = new GetVolunteerApplicationDTO
            {
                VolunteerApplicationID = entity.VolunteerApplicationID,
                UserID = entity.UserID,
                Description = entity.Description,
                IdImagePath = entity.IdImagePath,
                ApplicationStatus = statusString,
                ApplicationDateTime = entity.CreationDate,
                ProofImagePaths = images.Select(i => i.ImagePath).ToList()
            };

            return dto;

        }
        private async Task<bool> UpdateVolunteer(VolunteerEntity entity)
        {
            return await repository.UpdateVolunteer(entity);
        }
        public async Task<bool> AddPointsToVolunteer(int userID,int pointsCount)
        {
            VolunteerEntity volunteer = await GetVolunteer(userID);
            if (volunteer == null)
                return false;

            volunteer.PointsCount += pointsCount;
            return true;

        }
       
        private async Task<VolunteerEntity> GetVolunteer(int userID)
        {
            VolunteerEntity existingVolunteer =  await repository.GetVolunteerByUserID(userID);
            if (existingVolunteer == null)
            {
                return null; // Volunteer not found
            }
            return existingVolunteer;
        }
        public async Task<GetVolunteerDTO> GetVolunteerByUserID(int userID)
        {
            VolunteerEntity volunteer = await GetVolunteer(userID);
            if (volunteer == null)
                return null;
            VolunteerApplicationEntity application=repository.GetVolunteerApplicationByID(volunteer.VolunteerApplicationID);

            GetVolunteerDTO dto = new GetVolunteerDTO
            {VolunteerID=volunteer.VolunteerID,
                UserID = volunteer.UserID,
                Description = volunteer.Description,
                PointsCount = volunteer.PointsCount,
                AccomplishedServiceApplicationsCount = volunteer.AccomplishedServiceApplicationsCount,
                CreationDate=volunteer.CreationDate,
                IdImagePath=application.IdImagePath,
            };
            return dto;

        }

        public async Task<bool> UpdateVolunteerDescription(int userID,string description)
        {


            VolunteerEntity volunteer = await GetVolunteer(userID);
               volunteer.Description = description;

                return await UpdateVolunteer(volunteer);

        }
        public async Task<List<GetVolunteerApplicationDTO>> GetAllVolunteerApplications()
        {
            List<VolunteerApplicationEntity> applications = await repository.GetAllVolunteerApplications();

            List<GetVolunteerApplicationDTO> dtos = new List<GetVolunteerApplicationDTO>();
            foreach (var application in applications)
            {
                string statusString = GetAppStatusString(application.Status);
                List<VolunteerProofImage> images = await repository.GetProofImagesByApplicationID(application.VolunteerApplicationID);
                GetVolunteerApplicationDTO dto = new GetVolunteerApplicationDTO
                {
                    VolunteerApplicationID = application.VolunteerApplicationID,
                    UserID = application.UserID,
                    Description = application.Description,
                    IdImagePath = application.IdImagePath,
                    ApplicationStatus = statusString,
                    ApplicationDateTime = application.CreationDate,
                    ProofImagePaths = images.Select(i => i.ImagePath).ToList()
                   
                };
                dtos.Add(dto);
            }
            return dtos;

        }
    }
}
