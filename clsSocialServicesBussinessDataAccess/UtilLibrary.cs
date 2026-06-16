using clsSocialServicesDataAccess;
using DTOs;
using DTOs.Login;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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


            public static string RootPath;

            public enum ImageType
            {
                UserImage,
                PostImage,
                AdminImage,
                VolunteerImage
            }

            private static readonly string usersImages = clsConfigurations.UserImagePath;
            private static readonly string postsImages = clsConfigurations.PostImagePath;
            private static readonly string adminsImages = clsConfigurations.AdminImagePath;
            private static readonly string volunteersImages = clsConfigurations.VolunteerImagePath;

            public static string saveImageTofile(byte[] fileBytes, string fileName, ImageType type)
            {
                if (fileBytes == null || fileBytes.Length == 0)
                    return null!;

                string relativePath;
                if (type == ImageType.UserImage)
                    relativePath = usersImages;
                else if (type == ImageType.PostImage)
                    relativePath = postsImages;
                else if (type == ImageType.AdminImage)
                    relativePath = adminsImages;
                else
                    relativePath = volunteersImages;

                try
                {
                    string fullStorageDirectory = Path.Combine(RootPath, relativePath);

                    if (!Directory.Exists(fullStorageDirectory))
                        Directory.CreateDirectory(fullStorageDirectory);

                    string extension = Path.GetExtension(fileName);
                    string newFileName = Guid.NewGuid().ToString() + extension;
                    string destinationPath = Path.Combine(fullStorageDirectory, newFileName);

                    File.WriteAllBytes(destinationPath, fileBytes);

                    return Path.Combine(relativePath, newFileName);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            public static string saveImageTofile(string selectedImagePath, ImageType type)
            {
                if (string.IsNullOrEmpty(selectedImagePath))
                    return null!;

                string relativePath;
                if (type == ImageType.UserImage)
                    relativePath = usersImages;
                else if (type == ImageType.PostImage)
                    relativePath = postsImages;
                else if (type == ImageType.AdminImage)
                    relativePath = adminsImages;
                else
                    relativePath = volunteersImages;

                try
                {
                    string fullStorageDirectory = Path.Combine(RootPath, relativePath);

                    if (!Directory.Exists(fullStorageDirectory))
                        Directory.CreateDirectory(fullStorageDirectory);

                    string fileName = Path.GetFileName(selectedImagePath);
                    string extension = Path.GetExtension(fileName);
                    string newFileName = Guid.NewGuid().ToString() + extension;

                    // ✅ FIX: use fullStorageDirectory instead of relativePath
                    string destinationPath = Path.Combine(fullStorageDirectory, newFileName);

                    File.Copy(selectedImagePath, destinationPath);

                    // Return the relative path for storing in DB
                    return Path.Combine(relativePath, newFileName);
                }
                catch (Exception ex)
                {
                    return null!;
                }
            }
            public static bool removeImageFromFile(string ImagePath)
            {
                if (string.IsNullOrEmpty(ImagePath))
                    return false;
                try
                {

                    string fullPath = Path.Combine(RootPath, ImagePath);
                    File.Delete(fullPath);
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
            static public string returnToken(AdminEntity admin)
            {


                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, admin.Username),
            new Claim(ClaimTypes.NameIdentifier, admin.AdminID.ToString()),
            new Claim(ClaimTypes.Role, "Admin")
        };


                var token = new JwtSecurityToken(
                issuer: clsConfigurations.returnIssuer(),
                audience: clsConfigurations.returnAudience(),
                claims: claims,
                expires: DateTime.Now.AddHours(0.25),
                signingCredentials: new SigningCredentials(clsConfigurations.getKeyValue(), SecurityAlgorithms.HmacSha256));


                return new JwtSecurityTokenHandler().WriteToken(token);
            }

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

