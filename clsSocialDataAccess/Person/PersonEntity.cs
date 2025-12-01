using clsSocialServicesDataAccess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clsSocialServicesDataAccess
{
    [Table("People")]
    public class PersonEntity // Represents the 'People' table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonID { get; set; }

        // Person Details

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; } // Based on your schema
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string ImagePath { get; set; }

        // Navigation property for the User linked to this Person
        public virtual UserEntity User { get; set; }
    }
}