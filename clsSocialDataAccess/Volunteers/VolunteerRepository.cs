using clsSocialServicesDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Volunteers
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly AppDbContext _context;

        public VolunteerRepository(AppDbContext context)
        {
            _context = context;
        }



        public async Task<bool> AddVolunteer(VolunteerEntity volunteer)
        {
            try
            {

                _context.Volunteers.Add(volunteer);
                _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return false;
            }
        }

        public async Task<int> IssueVolunteerRequest(VolunteerApplicationEntity volunteerApplication)
        {
            try
            {
                _context.VolunteerApplications.Add(volunteerApplication);
                await _context.SaveChangesAsync();
                return volunteerApplication.VolunteerApplicationID;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return 0;
            }
        }

        public async Task<bool> AddVolunteerProofImage(VolunteerProofImage proofImage)
        {
            try
            {
                _context.VolunteerProofImages.Add(proofImage);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return false;
            }

        }

        public bool CanUserApplyToBeVolunteer(int userID)
        {

            try
            {
                if (_context.VolunteerApplications.Any(a => a.UserID == userID && a.Status == IVolunteerRepository.ApplicationStatus.Pending))
                    return false;
                return true;
            }

            catch
            {
                return false;
            }
          

        }
    }
}
