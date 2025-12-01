using dotenv.net;
using Microsoft.Extensions.Configuration;

namespace clsSocialServicesBussiness
{
    public class clsConfiguration 
    {
        static private IConfiguration _config;

        public clsConfiguration(IConfiguration config)
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
