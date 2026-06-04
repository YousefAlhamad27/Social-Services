using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Admin
{
    public class AdminEntity
    {

        

        public int AdminID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; } 
        
        public string Password { get; set; } 
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
