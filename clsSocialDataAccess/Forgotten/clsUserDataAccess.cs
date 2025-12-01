//using Azure;
//using Microsoft.Data.SqlClient;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;

//namespace clsSocialServicesDataAccess
//{
//    public class clsUserDataAccess
//    {
      
//        static public int addNewUser(int personID, string username, string passwordHash, bool isActive, DateTime creationDate)
//        {
//            int userID = -1;
//            string connectionString = clsConfigurations.ConnectionString;
//            string query = "insert into Users" +
//                " (personID,Username,Password,IsActive,CreationDate) values" +
//                " (@personID,@username,@passwordHash,@isActive,@creationDate); select SCOPE_IDENTITY();";

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                using (SqlCommand command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.AddWithValue("@personID", personID);
//                    command.Parameters.AddWithValue("@username", username);
//                    command.Parameters.AddWithValue("@passwordHash", passwordHash);
//                    command.Parameters.AddWithValue("@isActive", isActive);
//                    command.Parameters.AddWithValue("@creationDate", creationDate);

//                    try
//                    {
//                        connection.Open();


//                       userID=Convert.ToInt32(command.ExecuteScalar());
//                        return userID;
//                    }
//                    catch
//                    {
//                        return -1;
//                    }
//                }
//            }

//        }

//        static public bool doesUsernameExist(string username)
//        {
//            string connectionString = clsConfigurations.ConnectionString;
            
         

//            using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                using (SqlCommand command = new SqlCommand("Select username from users where username=@username", connection))
//                {
                    
//                    command.Parameters.AddWithValue("@username", username);
        

//                    try
//                    {
//                        connection.Open();
//                        using (SqlDataReader reader = command.ExecuteReader()) {

//                            if (reader.Read())
//                            {
//                                return true;
//                            }
//                            return false;
//                        }
                        
                      
//                    }
//                    catch
//                    {
//                        return true;
//                    }
//                }
//            }
//        }

//    }
//}
