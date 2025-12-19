using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess
{
    public class RefreshToken
    {
        public int Id { get; set; }          
        public string Token { get; set; }    
        public int UserId { get; set; }   
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string? JwtRole { get ; set; }
    }
}
