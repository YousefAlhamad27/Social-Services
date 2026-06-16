using System.Linq;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess
{
    // Interface for the UserRepository
    public interface IUserRepository
    {
        // Define methods that perform database operations, NOT EF Core specific methods
        bool DoesUsernameExist(string username);
        int AddUser(UserEntity userEntity);
        bool UpdateUser(UserEntity userEntity);
        public bool doesTokenExist(string token);
        public bool revokeRefreshToken(string token, string newToken);
        public bool AddRefreshToken(string refreshToken, string username);
        public int getPersonID(UserEntity userEntity);
        public UserEntity Find(int userID);
        public bool DeleteAllRefreshTokensForUser(int userID);
        public UserEntity FindUserName(string username);
        public RefreshToken returnRefreshToken(string token);
        public int getUserID(string username);

        public UserEntity returnUserForToken(RefreshToken token);
        public bool DeleteRefreshToken(string token);
        public bool DeleteUser(string username);
        public Task<int> GetUsersCount();
        public Task<bool> BlockUser(int UserID);
        public Task<bool> UnBlockUser(int UserID);
        public Task<List<UserEntity>> GetAllUsers();
        public UserEntity GetUserByVolunteerID(int volunteerID);
    }
}