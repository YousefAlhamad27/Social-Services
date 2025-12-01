using dotenv.net;
using Microsoft.Extensions.Configuration;

namespace clsSocialServicesDataAccess
{
    public class clsConfigurations
    {
       static private  IConfiguration _config;

      public  clsConfigurations(IConfiguration config)
        {
            _config = config;
        }
        static public string ConnectionString
        {
            get
            {
                return $"Data Source={_config["DB_HOST"]};Database={_config["db_name"]};Integrated Security=True;TrustServerCertificate=True;";
            }
        }
    
    }
}
