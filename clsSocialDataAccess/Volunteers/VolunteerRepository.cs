using clsSocialServicesDataAccess;
using Microsoft.EntityFrameworkCore;
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
        public async Task<bool> UpdateVolunteerProofImages(List<VolunteerProofImage> proofImages) {
            try
            {
               
                  _context.VolunteerProofImages.UpdateRange(proofImages);
              await  _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        
        }
        public async Task<bool> DeleteVolunteerApplicationProofImages(int applicationID)
        {

            try
            {

                List<VolunteerProofImage> images = _context.VolunteerProofImages.Where(app => app.VolunteerApplicationID == applicationID).ToList();
                _context.VolunteerProofImages.
                 RemoveRange(images);
                await _context.SaveChangesAsync();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public bool CanUserApplyToBeVolunteer(int userID)
        {

            try
            {
                if (_context.VolunteerApplications.Any(a => a.UserID == userID && (a.Status == IVolunteerRepository.ApplicationStatus.Pending||a.Status==IVolunteerRepository.ApplicationStatus.Approved)))
                    return false;
                return true;
            }

            catch
            {
                return false;
            }


        }

        public async Task<bool> RespondToVolunteerApplication(VolunteerApplicationEntity application)
        {
            try
            {
                var existingApplication = await _context.VolunteerApplications.FindAsync(application.VolunteerApplicationID);
                if (existingApplication == null)
                {
                    return false; // Application not found
                }
                existingApplication.Status = application.Status;
                existingApplication.AdminID = application.AdminID;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return false;
            }
        }

        public VolunteerApplicationEntity GetVolunteerApplicationByID(int applicationID)
        {
            try
            {
                return _context.VolunteerApplications.FirstOrDefault(a => a.VolunteerApplicationID == applicationID)!;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return null;
            }
        }

        public async Task<bool> UpdateVolunteerApplication(VolunteerApplicationEntity application)
        {
            try
            {
                //var existingApplication = await _context.VolunteerApplications.FindAsync(application.VolunteerApplicationID);
                //if (existingApplication == null)
                //{
                //    return false; // Application not found
                //}

                //existingApplication.Status = application.Status;
                //existingApplication.AdminID = application.AdminID;
                _context.VolunteerApplications.Update(application);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return false;
            }
        }

        public async Task<bool> DeleteApplication(int applicationID)
        {
            try
            {
                var application = await _context.VolunteerApplications.FindAsync(applicationID);
                if (application == null)
                {
                    return false; // Application not found
                }
                _context.VolunteerApplications.Remove(application);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return false;
            }
        }

        public async Task<bool> DeleteVolunteer(int volunteerID)
        {
            try
            {
                var volunteer = await _context.Volunteers.FindAsync(volunteerID);
                if (volunteer == null)
                {
                    return false; // Volunteer not found
                }
                _context.Volunteers.Remove(volunteer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return false;
            }
        }

        public async Task<bool> UpdateVolunteer(VolunteerEntity volunteer)
        {
            try
            {
                var existingVolunteer = await _context.Volunteers.FindAsync(volunteer.VolunteerID);
                if (existingVolunteer == null)
                {
                    return false; // Volunteer not found
                }
                existingVolunteer.Description = volunteer.Description;
                existingVolunteer.PointsCount = volunteer.PointsCount;
                existingVolunteer.AccomplishedServiceApplicationsCount = volunteer.AccomplishedServiceApplicationsCount;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return false;
            }

        }
        public VolunteerEntity GetVolunteerByUserID(int userID)
        {
            try
            {
                return  _context.Volunteers.FirstOrDefault(v => v.UserID == userID)!;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return null;
            }
        }

        public async Task<VolunteerEntity> GetVolunteerByID(int volunteerID)
        {
            try
            {
                return await _context.Volunteers.FindAsync(volunteerID);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return null;
            }
        }

        public async Task<List<VolunteerEntity>> GetAllVolunteers()
        {
            try
            {
                return await _context.Volunteers.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return new List<VolunteerEntity>();
            }
        }

        public async Task<List<VolunteerApplicationEntity>> GetAllVolunteerApplications()
        {
            try
            {
                return await _context.VolunteerApplications.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return new List<VolunteerApplicationEntity>();
            }
        }

        public async Task<List<VolunteerApplicationEntity>> GetAllVolunteerApplicationsByStatus(IVolunteerRepository.ApplicationStatus status)
        {
            try
            {
                return await _context.VolunteerApplications.Where(a => a.Status == status).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return new List<VolunteerApplicationEntity>();
            }
        }

        public async Task<List<VolunteerApplicationEntity>> GetAllVolunteerApplicationsByUserID(int userID)
        {
            try
            {
                return await _context.VolunteerApplications.Where(a => a.UserID == userID).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return new List<VolunteerApplicationEntity>();
            }
        }

        public async Task<List<VolunteerProofImage>> GetProofImagesByApplicationID(int applicationID)
        {
                        try
            {
                return await _context.VolunteerProofImages.Where(i => i.VolunteerApplicationID == applicationID).ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return new List<VolunteerProofImage>();
            }
        }
        public async Task<bool> IssueCertificate(CertficateEntity certficate)
        {
            try
            {
                _context.Certificates.Add(certficate);
                 await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public List<CertificateClassification> GetAllCertificateClassifications()
        {
            try
            {
              return  _context.CertificateClassifications.ToList();
            }
            catch
            {
                return null!;
            }

        }
        
        public  List<CertficateEntity> GetCertificatesForVolunteer(int volunteerID)
        {
            try
            {
                return  _context.Certificates.Where(c => c.VolunteerID == volunteerID).ToList();
            }
            catch
            {
                return null!;
            }

        }


    }
}
