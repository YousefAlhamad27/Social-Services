using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.User_Person_DTOs
{
    public class GetUserDTO
    {

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string? Imagepath { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }
       
    }
}
