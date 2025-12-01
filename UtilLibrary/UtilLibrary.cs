using System.Text;
using System.Security.Cryptography;


namespace UtilLibrary
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


    }
}
