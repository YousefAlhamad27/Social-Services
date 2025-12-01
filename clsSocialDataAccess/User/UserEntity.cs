using clsSocialServicesDataAccess;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clsSocialServicesDataAccess
{
    [Table("Users")]
    public class UserEntity 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

     
        [ForeignKey("Person")]
        public int PersonID { get; set; }

        // User Credentials
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }

       
        public virtual PersonEntity Person { get; set; }
    }
}