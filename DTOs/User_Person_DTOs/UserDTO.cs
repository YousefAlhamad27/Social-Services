using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class UserDTO
    {

       
     
  
       public int PersonID {  get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }


        public UserDTO( int personID,string username, string passwordHash,
            bool isActive, DateTime creationDate)
        {



            this.PersonID = personID;
            this.Username = username;
            this.PasswordHash = passwordHash;
            this.IsActive = isActive;
            this.CreationDate = creationDate;
        }
    }
}
