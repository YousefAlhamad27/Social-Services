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

        public clsVolunteer(IVolunteerRepository context,IUserRepository user) { repository = context; user_rep = user; }

        public async Task<bool> AddVolunteer(VolunteerEntity volunteer)
        {
            return await repository.AddVolunteer(volunteer);
        }
        public async Task<bool> AddVolunteerProofImages( List<VolunteerProofImage> proofImages)
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
                    if(UtilLibrary.FileOperations.saveImageTofile(proofImagePath, UtilLibrary.FileOperations.ImageType.VolunteerImage) == null)
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
   

            int RequestID= await repository.IssueVolunteerRequest(request);

            if (RequestID == 0)
            {
                return false;
            }

           List<VolunteerProofImage> proofImages = addVolunteerRequest.ProofImagePaths.Select(path => new VolunteerProofImage { VolunteerApplicationID = RequestID, ImagePath = path }).ToList();
            
                if (!await AddVolunteerProofImages(proofImages))
                    return false;

            return true;
                
        }

        public bool CanUserBecomeVolunteer(int userID) {


            return repository.CanUserApplyToBeVolunteer(userID);
        }

    }
}
