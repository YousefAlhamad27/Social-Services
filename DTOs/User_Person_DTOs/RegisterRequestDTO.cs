namespace DTOs
{
    public class RegisterRequestDTO
    {
      
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string Imagepath { get; set; }
      
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }


        public RegisterRequestDTO( string firstName, string secondName, string lastName, string email, string phone, int age, string imagepath, string username, string passwordHash,
            bool isActive, DateTime creationDate)
        {
           
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.LastName = lastName;
            this.Email = email;
            this.Phone = phone;
            this.Age = age;
            this.Imagepath = imagepath;
           
            this.Username = username;
            this.PasswordHash = passwordHash;
            this.IsActive = isActive;
            this.CreationDate = creationDate;
        }
    }
}
