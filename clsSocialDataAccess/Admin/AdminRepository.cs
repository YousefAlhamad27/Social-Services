using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Admin
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public AdminEntity? GetAdminByUsername(string username)
        {
            return _context.Admins.FirstOrDefault(a => a.Username == username);
        }

    }
}
