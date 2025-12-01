using clsSocialServicesDataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace clsSocialServicesDataAccess
{
    // Implementation of the Repository
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        // DBContext is injected here in the Data Layer
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool DoesUsernameExist(string username)
        {
            return _dbContext.Users.Any(u => u.Username == username);
        }
        public int AddUser(UserEntity userEntity)
        {
            try
            {
                _dbContext.Users.Add(userEntity);
                _dbContext.SaveChanges();
                return userEntity.UserID;
            }
            catch (Exception e)
            {
                return -1;

            }
            
        }
        public int getPersonID(UserEntity userEntity)
        {
            int personID = -1;
            _dbContext.Users.FirstOrDefault(userEntity);
            personID=userEntity.PersonID;
            return personID;
        }
        public UserEntity FindUserName(string username)
        {
            try
            {
                return _dbContext.Users.FirstOrDefault(u => u.Username == username);
            }
            catch
            {
                return null;
            }
        }
        public UserEntity Find(int userID)
        {
           
            try
            {
             return  _dbContext.Users.Find(userID);
                
              

            }
            catch
            {
                return null;
            }

        }
        public bool UpdateUser(UserEntity userEntity)
        {
            try
            {
                _dbContext.Users.Update(userEntity);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteUser(string username) {

            try
            {
                // 2. Find the existing, tracked entity using its permanent ID.
                // Find() is the fastest way to check the database and load the entity.
                UserEntity userToDelete = FindUserName(username);
              //  UserEntity userToDelete = _dbContext.Users.Find(userID);

                if (userToDelete != null)
                {
                    // 3. Remove the tracked entity instance.
                    _dbContext.Users.Remove(userToDelete);

                    // 4. Commit the deletion to the database.
                    _dbContext.SaveChanges();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the deletion failure (e.g., if there are cascade constraints preventing deletion)
                // Console.WriteLine(ex.Message); 
                return false;
            }
        }
    }
}