namespace DTOs.Login
{
    public class RegisterRequestDTO
    {
      
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string? Imagepath { get; set; }
      
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }

        public RegisterRequestDTO() { }

        public RegisterRequestDTO( string firstName, string secondName, string lastName, string email, string phone, int age, string imagepath, string username, string passwordHash,
            bool isActive, DateTime creationDate)
        {
           
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Age = age;
            Imagepath = imagepath;
           
            Username = username;
            PasswordHash = passwordHash;
            IsActive = isActive;
            CreationDate = creationDate;
        }
    }
}
