using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess
{
    public class RefreshToken
    {
        public int Id { get; set; }          
        public string Token { get; set; }

        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }   
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string? JwtRole { get ; set; }
    }
}
