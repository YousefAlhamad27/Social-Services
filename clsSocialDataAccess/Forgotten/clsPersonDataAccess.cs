//using Microsoft.Data.SqlClient;


//using clsSocialServicesDataAccess;

//namespace clsSocialDataAccess
//{
//    public class clsPersonDataAccess
//    {

//        static public int addNewPerson(string firstName, string secondName, string lastName, string email, string phone, int age, string imagepath) {
//            int personID = -1;
          
//            string connectionString = clsConfigurations.ConnectionString;
//            string query = "insert into People" +
//                " (FirstName,SecondName,LastName,Email,Phone,Age,imagepath) values" +
//                " (@firstname,@secondName,@lastName,@email,@phone,@age,@imagepath); select SCOPE_IDENTITY();";
//           using (SqlConnection connection = new SqlConnection(connectionString))
//            {
//                using(SqlCommand command=new SqlCommand(query,connection))
//                {
//                    command.Parameters.AddWithValue("@firstname",firstName);
//                    command.Parameters.AddWithValue("@secondName", secondName);
//                    command.Parameters.AddWithValue("@lastName", lastName);
//                    command.Parameters.AddWithValue("@email", email);
//                    command.Parameters.AddWithValue("@phone", phone);
//                    command.Parameters.AddWithValue("@age", age);
//                    command.Parameters.AddWithValue("@imagepath", imagepath);

//                    try
//                    {
//                        connection.Open();


//                        personID= Convert.ToInt32(command.ExecuteScalar());
//                        return personID;
//                    }
//                    catch
//                    {
//                        return -1;
//                    }
//                }


//            }
             


//        }

//    }
//}
