using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PersonDTO
    {
        public int personID;
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string Imagepath { get; set; }


        public PersonDTO(int personID,string firstName, string secondName, string lastName, string email, string phone, int age, string imagepath)
        {
            this.personID= personID;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.LastName = lastName;
            this.Email = email;
            this.Phone = phone;
            this.Age = age;
            this.Imagepath = imagepath;

          
        }
    }
}
