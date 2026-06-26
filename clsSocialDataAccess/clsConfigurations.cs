using dotenv.net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace clsSocialServicesDataAccess
{
    public class clsConfigurations
    {
       static private  IConfiguration _config;

      public  clsConfigurations(IConfiguration config)
        {
            _config = config;
        }

        static public string returnAudience()
        {
            return _config["Jwt:Audience"]!;
        }
        public static string PostImagePath { get {return _config["Paths:PostImages"]!;
            }
        } 
        public static string UserImagePath
        {
            get
            {
                return _config["Paths:UserImages"]!;
            }
        }
        public static string AdminImagePath
        {
            get
            {
                return _config["Paths:AdminImages"]!;
            }
        }
        public static string VolunteerImagePath
        {
                        get
            {
                return _config["Paths:VolunteerImages"]!;
            }
        }
        static public string returnIssuer()
        {
            return _config["Jwt:Issuer"]!;
        }
        static public SymmetricSecurityKey getKeyValue()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config["Jwt:Key"]!));

            return key;
        }
        static public string ConnectionString
        {
            get
            {
                 return $"Data Source={_config["DB_HOST"]};Database={_config["db_name"]};Integrated Security=True;TrustServerCertificate=True;";
                // return $"Server={_config["DB_HOST"]};Integrated Security=False;Database={_config["db_name"]};User Id={_config["db_user"]};Password={_config["db_password"]};TrustServerCertificate=True;";
               // return _config["ConnectionString:DefaultConnection"]!;
            }
        }
    
    }
}
