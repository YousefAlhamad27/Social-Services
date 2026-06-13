using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Volunteers
{
    public interface IVolunteerRepository
    {
        public enum ApplicationStatus : byte
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }

        public Task<bool> AddVolunteer(VolunteerEntity volunteer);
        public Task<int> IssueVolunteerRequest(VolunteerApplicationEntity volunteerApplication);
        public Task<bool> AddVolunteerProofImage(VolunteerProofImage proofImage);
        public bool CanUserApplyToBeVolunteer(int userID);
        public Task<bool> RespondToVolunteerApplication(VolunteerApplicationEntity application);
        public VolunteerApplicationEntity GetVolunteerApplicationByID(int applicationID);

        public Task<bool> UpdateVolunteerApplication(VolunteerApplicationEntity application);
        public Task<bool> DeleteApplication(int applicationID);

        public Task<bool> DeleteVolunteer(int volunteerID);
        public Task<bool> UpdateVolunteer(VolunteerEntity volunteer);
        public Task<VolunteerEntity> GetVolunteerByUserID(int userID);
        public Task<VolunteerEntity> GetVolunteerByID(int volunteerID);
        public Task<List<VolunteerEntity>> GetAllVolunteers();
        public Task<List<VolunteerApplicationEntity>> GetAllVolunteerApplications();
        public Task<List<VolunteerApplicationEntity>> GetAllVolunteerApplicationsByStatus(ApplicationStatus status);
        public Task<List<VolunteerApplicationEntity>> GetAllVolunteerApplicationsByUserID(int userID);
        public Task<List<VolunteerProofImage>> GetProofImagesByApplicationID(int applicationID);


    }
}
