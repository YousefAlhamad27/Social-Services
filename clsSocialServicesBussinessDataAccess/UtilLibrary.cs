using clsSocialServicesDataAccess;
using DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Resources;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace clsSocialServicesBussiness
{
  public class UtilLibrary
    {
     public class FileOperations
        {
            public enum ImageType
            {
                UserImage,
                PostImage,
                AdminImage
                //admin image
            }

            private static readonly string usersImages = clsConfigurations.UserImagePath;
            private static readonly string postsImages = clsConfigurations.PostImagePath;
            private static readonly string adminsImages = clsConfigurations.AdminImagePath;

            public static  string saveImageTofile(string selectedImagePath,ImageType type)
            {




                if (string.IsNullOrEmpty(selectedImagePath))
                    return null!;

                string storage;

                if (type == ImageType.UserImage)
                {
                     storage=usersImages;
                }
                else if(type==ImageType.PostImage)
                {
                    storage = postsImages;
                }
                else
                {
                    storage = adminsImages;
                }



                    try
                    {
                        // Ensure storage directory exists
                        if (!Directory.Exists(storage))
                        {
                            Directory.CreateDirectory(storage);
                        }
                        // Generate unique filename to avoid conflicts

                        string fileName = Path.GetFileName(selectedImagePath);
                        string destinationPath = Path.Combine(storage, fileName);

                        // Handle duplicate names by adding a counter



                        Guid newGuid = Guid.NewGuid();
                        string newFileName = newGuid.ToString();


                        string extension = Path.GetExtension(fileName);
                        string fileNameWithExtension = newFileName + extension;
                        string guidPathName = storage + "\\" + fileNameWithExtension;
                        destinationPath = Path.Combine(storage, $"{fileNameWithExtension}");



                        // Copy the file to storage directory
                        File.Copy(selectedImagePath, destinationPath);

                        return guidPathName;






                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., log the error)
                        return null!;
                    }

            }
            public static bool removeImageFromFile(string ImagePath,ImageType type)
            {
                if (string.IsNullOrEmpty(ImagePath))
                    return false;

                string storage;

                if (type == ImageType.UserImage)
                {
                    storage = usersImages;
                }
                else if (type == ImageType.PostImage)
                {
                    storage = postsImages;
                }
                else
                {
                    storage = adminsImages;
                }

                try
                    {
                       // string fileName = Path.GetFileName(ImagePath);
                        File.Delete(ImagePath);
                    return true;

                    }
                    catch (Exception ex)
                    {

                    return false;
                      
                    }
                
            }
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // for admin 
       // static public string returnToken(AdminEntity admin)
       // {

       //     var claims = new List<Claim>
       // {
       //     new Claim(ClaimTypes.Name, admin.Username),
       //     new Claim(ClaimTypes.NameIdentifier, admin.AdminID.ToString()),
       //     new Claim(ClaimTypes.Role, "Admin")
       // };



       //     var token = new JwtSecurityToken(
       //issuer: clsConfigurations.returnIssuer(),
       //audience: clsConfigurations.returnAudience(),
       //claims: claims,
       //expires: DateTime.Now.AddHours(0.25),
       //signingCredentials: new SigningCredentials(clsConfigurations.getKeyValue(), SecurityAlgorithms.HmacSha256));


       //     return new JwtSecurityTokenHandler().WriteToken(token);
       // }

        static public string returnToken(UserEntity user)
        {
          
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
            new Claim(ClaimTypes.Role, "User")
        };



            var token = new JwtSecurityToken(
       issuer: clsConfigurations.returnIssuer(),
       audience: clsConfigurations.returnAudience(),
       claims: claims,
       expires: DateTime.Now.AddHours(1),
       signingCredentials: new SigningCredentials(clsConfigurations.getKeyValue(), SecurityAlgorithms.HmacSha256));

       
                return new JwtSecurityTokenHandler().WriteToken(token);
            }

        
        static public string returnHashedPassword(string password)
        {

            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);

        }
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
            }
            catch
            {
                return false;
            }
        }

        static public string ReturnSHA256(string String)
        {

            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(String));
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
