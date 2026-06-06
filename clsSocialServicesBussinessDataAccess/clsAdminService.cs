using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess.Admin;
using clsSocialServicesDataAccess.Posts;
using DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsAdminService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly IPostRepository _postRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogRepository _logRepo;

        public clsAdminService(IAdminRepository adminRepository, IPostRepository postRepo, IUserRepository userRepo, ILogRepository logRepo)
        {
            _adminRepo = adminRepository;
            _postRepo = postRepo;
            _userRepo = userRepo;
            _logRepo = logRepo;
        }

        public async Task<AdminStatusDTO> GetDashBoardStatus()
        {
            return new AdminStatusDTO()
            {
                TotalPost = await _postRepo.PostsCount(),
                TotalUser = await _userRepo.GetUsersCount()

            };
        }

        public string? Login(string UserName, string Password)
        {
            var Admin = _adminRepo.GetAdminByUsername(UserName);

            if (Admin == null)
                return null;

            if (Admin.Password != Password)
                return null;

            return UtilLibrary.returnAdminToken(Admin);
        }

        public async Task<bool> BlockUser(int UserID,int AdminId)
        {
            if (await _userRepo.BlockUser(UserID))
            {
                await _logRepo.AddLog("Uesr Blocked",UserID,"User","User has been blocked",AdminId);
                return true;
            }
            return false;
        }

        public async Task<bool> UnBlockUser(int UserID,int AdminId)
        {
            if (await _userRepo.BlockUser(UserID))
            {
                await _logRepo.AddLog("Uesr Unblocked", UserID, "User", "User has been Unblocked", AdminId);
                return true;
            }
            return false;
        }

        public async Task<List<LogEntity>> GetLogs(string targetType)
        {
            return await _logRepo.GetLogs(targetType);
        }

        public Task<List<UserEntity>> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }
    }
}
