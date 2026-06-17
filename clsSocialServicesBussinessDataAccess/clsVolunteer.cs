using clsSocialDataAccess.Migrations;
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
            //if (UtilLibrary.FileOperations.saveImageTofile(addVolunteerRequest.IdImagePath, UtilLibrary.FileOperations.ImageType.VolunteerImage) == null) return false;


            //foreach (var proofImagePath in addVolunteerRequest.ProofImagePaths)
            //{
            //    if (UtilLibrary.FileOperations.saveImageTofile(proofImagePath, UtilLibrary.FileOperations.ImageType.VolunteerImage) == null)
            //    {
            //        return false;
            //    }
            //}

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
        private string GetAppStatusString(IVolunteerRepository.ApplicationStatus status)
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
        public bool AddPointsToVolunteer(int userID,int pointsCount)
        {
            VolunteerEntity volunteer =  GetVolunteer(userID);
            if (volunteer == null)
                return false;

            volunteer.PointsCount += pointsCount;
              repository.UpdateVolunteer(volunteer);
            return true;

        }
       
        private VolunteerEntity GetVolunteer(int userID)
        {
            VolunteerEntity existingVolunteer =   repository.GetVolunteerByUserID(userID);
            if (existingVolunteer == null)
            {
                return null; // Volunteer not found
            }
            return existingVolunteer;
        }
        public GetVolunteerDTO GetVolunteerByUserID(int userID)
        {
            VolunteerEntity volunteer =  GetVolunteer(userID);
            if (volunteer == null)
                return null!;
            VolunteerApplicationEntity application=repository.GetVolunteerApplicationByID(volunteer.VolunteerApplicationID);

            GetVolunteerDTO dto = new GetVolunteerDTO
            {
                VolunteerID=volunteer.VolunteerID,
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


            VolunteerEntity volunteer =  GetVolunteer(userID);
               volunteer.Description = description;

                return await UpdateVolunteer(volunteer);

        }
         public async Task<bool> UpdateVolunteerApplicationImages(UpdateVolunteerApplicationImagesDTO dto)
        {
            VolunteerApplicationEntity volunteerApplication =  repository.GetVolunteerApplicationByID(dto.ApplicationID);

            volunteerApplication.IdImagePath=dto.IdImagePath;
           await repository.DeleteVolunteerApplicationProofImages(volunteerApplication.VolunteerApplicationID);


            List<VolunteerProofImage> images=new List<VolunteerProofImage>();
            foreach(var image in dto.ProofImages)
            {

                if (!await repository.AddVolunteerProofImage(new VolunteerProofImage { VolunteerApplicationID = volunteerApplication.VolunteerApplicationID, ImagePath = image })) {
                    return false;
                }
;
                
            }
           

           return await repository.UpdateVolunteerApplication(volunteerApplication);

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

        public bool IsUserAVolunteer(int userID)
        {
            VolunteerEntity volunteer =  GetVolunteer(userID);
            return volunteer != null;
        }

        public  int IsUserAllowedToIssueACertificate(int userID)
        {
            VolunteerEntity entity = GetVolunteer(userID);
            if (entity == null)
                return 0 ;

            List<CertficateEntity> certficates =  repository.GetCertificatesForVolunteer(entity.VolunteerID);
            List<CertificateClassification> classifications = repository.GetAllCertificateClassifications();

            if (entity.AccomplishedServiceApplicationsCount >= Convert.ToInt32(CertficateEntity.CertficateClass.Bronze))
            {
                if (certficates.Count ==0) {
                    return classifications.FirstOrDefault(c => c.RequiredAccomplishedServices == Convert.ToInt32(CertficateEntity.CertficateClass.Bronze)).ClassifcationID;
                }

                if (entity.AccomplishedServiceApplicationsCount >= Convert.ToInt32(CertficateEntity.CertficateClass.silver))
                {
                    if (certficates.Count == 1)
                    {
                        return classifications.FirstOrDefault(c => c.RequiredAccomplishedServices == Convert.ToInt32(CertficateEntity.CertficateClass.silver)).ClassifcationID;
                    }
                }
                else
                {
                    return 0;
                }
                if (entity.AccomplishedServiceApplicationsCount >= Convert.ToInt32(CertficateEntity.CertficateClass.Gold))
                {
                    if (certficates.Count == 2)
                    {
                        return classifications.FirstOrDefault(c => c.RequiredAccomplishedServices == Convert.ToInt32(CertficateEntity.CertficateClass.Gold)).ClassifcationID;
                    }
                }
                else return 0;

                    if (entity.AccomplishedServiceApplicationsCount >= Convert.ToInt32(CertficateEntity.CertficateClass.Platinum))
                {

                    if (certficates.Count == 3)
                    {
                        return classifications.FirstOrDefault(c => c.RequiredAccomplishedServices == Convert.ToInt32(CertficateEntity.CertficateClass.Platinum)).ClassifcationID;
                    }
                }
                else
                {
                    return 0;
                }
                if (entity.AccomplishedServiceApplicationsCount >= Convert.ToInt32(CertficateEntity.CertficateClass.Diamond))
                {

                    if (certficates.Count == 4)
                    {
                        return classifications.FirstOrDefault(c => c.RequiredAccomplishedServices == Convert.ToInt32(CertficateEntity.CertficateClass.Diamond)).ClassifcationID;
                    }
                }
                else
                {
                    return 0;
                }
                if (entity.AccomplishedServiceApplicationsCount >= Convert.ToInt32(CertficateEntity.CertficateClass.Elite))
                {

                    if (certficates.Count == 5)
                    {
                        return classifications.FirstOrDefault(c => c.RequiredAccomplishedServices == Convert.ToInt32(CertficateEntity.CertficateClass.Elite)).ClassifcationID;
                    }
                }
                else
                {
                    return 0;
                }

            }


                else
                {
                    return 0;
                }
            return 0;

            }
        public async Task<bool> IssueCertificate(int userID,int classID)
        {
            VolunteerEntity volunteer=repository.GetVolunteerByUserID(userID);

          return await  repository.IssueCertificate(new CertficateEntity {ClassifcationID=classID,VolunteerID=volunteer.VolunteerID,
                NumberOfAccomplishedServices=volunteer.AccomplishedServiceApplicationsCount,CreationDate=DateTime.Now });
        }    
            
        public List<CertificateListDTO> GetVolunteerCertificates(int userID)
        {
            VolunteerEntity volunteer=GetVolunteer(userID);

            List<CertficateEntity> list=repository.GetCertificatesForVolunteer(volunteer.VolunteerID);
            List<CertificateListDTO> DTOlist = new List<CertificateListDTO>();

            foreach(CertficateEntity certficate in list)
            {
                List<CertificateClassification> certificateClassifications = repository.GetAllCertificateClassifications();
                DTOlist.Add(new CertificateListDTO {VolunteerID=certficate.VolunteerID,CertificateID=certficate.CertificateID
                ,Classifcation = certificateClassifications.FirstOrDefault(c=>c.ClassifcationID==certficate.ClassifcationID)!.Title,CreationDate=certficate.CreationDate,NumberOfAccomplishedServices=certficate.NumberOfAccomplishedServices      
                }
                
                );
            }
            return DTOlist;

        }
        public CertificateListDTO GetCertificate(int certificateID,int userID) {

            VolunteerEntity volunteer = GetVolunteer(userID);
            if (volunteer == null)
                return null;
            List<CertificateClassification> certificateClassifications = repository.GetAllCertificateClassifications();

            CertficateEntity entity = repository.GetCertificate(certificateID);

           return new CertificateListDTO {CertificateID=entity.CertificateID, 
               Classifcation = certificateClassifications.FirstOrDefault(c => c.ClassifcationID == entity.ClassifcationID)!.Title,CreationDate=DateTime.Now,
               NumberOfAccomplishedServices=entity.NumberOfAccomplishedServices,VolunteerID=entity.VolunteerID };
        }

        }
}
