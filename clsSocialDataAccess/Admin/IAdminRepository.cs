using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Admin
{
    public interface IAdminRepository 
    {
            AdminEntity? GetAdminByUsername(string username);
    }
}
