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
        private  readonly IPostRepository _postRepo;
        private readonly IAdminRepository _adminRepo;

        public clsAdminService(IAdminRepository adminRepository, IPostRepository postRepo)
        {
            _adminRepo = adminRepository;
            _postRepo = postRepo;
        }

        public  AdminStatusDTO GetStatus()
        {
            return new AdminStatusDTO()
            {
                TotalPost = _postRepo.PostsCount
            };
        }

        public string? Login(string UserName , string Password)
        {
            var Admin = _adminRepo.GetAdminByUsername(UserName);

            if (Admin == null)
                return null;

            if(Admin.Password != Password)
                return null;

            return UtilLibrary.returnAdminToken(Admin);
        }

    }
}
