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
        public bool doesTokenExist(string token)
        {

            try
            {

                return _dbContext.Tokens.Any(u => u.Token == token);
                
                
            }
            catch
            {
                return false;
            }

        }
        public UserEntity returnUserForToken(RefreshToken token)
        {

            try
            {
                UserEntity user = _dbContext.Users.FirstOrDefault(u => u.UserID == token.UserId)!;

                if (user != null)
                {
                    return user;
                }
                return null;
            }
            catch  {
                return null;
            }
        }
        public int getUserID(string username)
        {
                       try
            {
                UserEntity user = _dbContext.Users.FirstOrDefault(u => u.Username == username)!;
                if (user != null)
                {
                    return user.UserID;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }
        public RefreshToken returnRefreshToken(string token)
        {

            try
            {

              RefreshToken refreshToken=  _dbContext.Tokens.FirstOrDefault(u => u.Token == token )!;
                if (refreshToken != null)
                    return refreshToken;
                return null;
            }
            catch
            {
                return null;
            }

        }

        public bool DeleteRefreshToken(string token)
        {
            try
            {
                _dbContext.Tokens.Remove(_dbContext.Tokens.FirstOrDefault(u => u.Token == token)!) ;
                _dbContext.SaveChanges();
                    return true;
                
                
            }
            catch
            {
                return false;
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

        public bool revokeRefreshToken(string token,string newToken)
        {
            try
            {
                RefreshToken refreshToken = _dbContext.Tokens.FirstOrDefault(u => u.Token == token)!;
                refreshToken.Token = newToken;  

                _dbContext.Tokens.Update(refreshToken);
                _dbContext.SaveChanges();
                return true;


            }
            catch
            {
                return false;
            }
        }
        public bool AddRefreshToken(string refreshToken, string username)
        {
            try
            {
                UserEntity user = FindUserName(username);

                if (user == null)
                    return false;

                RefreshToken token = new RefreshToken
                {
                    Token = refreshToken,
                    Expires = DateTime.Now.AddDays(7),
                    UserId = user.UserID,
                    Created = DateTime.Now,
                    JwtRole = "User"
                };
                
                _dbContext.Tokens.Add(token);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public RefreshToken returnRefreshToken(RefreshToken token)
        //{

        //    try
        //    {

        //        RefreshToken refreshToken = _dbContext.Tokens.FirstOrDefault(u => u.Token == token.Token && u.UserId == token.UserId);
        //        if(refreshToken != null) 
        //        return refreshToken;
        //        return null;
        //    }
        //    catch
        //    {
        //        return null;
        //    }

        //}

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
                return _dbContext.Users.FirstOrDefault(u => u.Username == username)!;
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
             return  _dbContext.Users.Find(userID)!;
                
              

            }
            catch
            {
                return null;
            }

        }
      public bool DeleteAllRefreshTokensForUser(int userID)
        {
            try
            {
                var tokens = _dbContext.Tokens.Where(t => t.UserId == userID).ToList();
                if (tokens.Count == 0)
                    return false; // No tokens to delete
                _dbContext.Tokens.RemoveRange(tokens);
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
                    // 3. Remove the tracked entity instance
                     DeleteAllRefreshTokensForUser(userToDelete.UserID);
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