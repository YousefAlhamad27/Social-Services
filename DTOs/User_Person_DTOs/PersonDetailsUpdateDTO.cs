using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PersonDetailsUpdateDTO
    {
        public string Username {  get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string? Imagepath { get; set; }

        public PersonDetailsUpdateDTO() { }

        public PersonDetailsUpdateDTO(string username, string firstName, string secondName, string lastName, string email, string phone, int age, string imagepath)
        {
            Username = username;
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Age = age;
            Imagepath = imagepath;
        }
    }
}
