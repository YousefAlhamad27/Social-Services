using DTOs;
using System.Security.Cryptography;
using System.Text;


namespace clsSocialServicesBussiness
{
  public class UtilLibrary
    {
       static public string returnHashedPassword(string password)
        {

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }

        }
       static public bool checkReqDTOValues(RegisterRequestDTO reqDto)
        {

            if (string.IsNullOrEmpty(reqDto.FirstName) || string.IsNullOrEmpty(reqDto.LastName) || string.IsNullOrEmpty(reqDto.Phone)
                || string.IsNullOrEmpty(reqDto.Email) || string.IsNullOrEmpty(reqDto.Username) || string.IsNullOrEmpty(reqDto.PasswordHash) ||
               reqDto.Age < 16)
            {
                return false;
            }
            return true;
        }

    }
}
