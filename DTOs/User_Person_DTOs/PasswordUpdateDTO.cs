using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.User_Person_DTOs
{
    public class PasswordUpdateDTO
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
