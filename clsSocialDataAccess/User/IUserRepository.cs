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
        public bool DeleteUser(string username);
        public Task<int> GetUsersCount();

        // Add other methods like GetUserByID, UpdateUser, etc.
    }
}