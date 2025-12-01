using clsSocialServicesDataAccess;
using DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsUser
    {
    
        private readonly UserRepository _userRepo;

        // You would also need an IPersonRepository injected here (not shown for brevity)

      
      
        public clsUser(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        private static UserEntity MapToUserEntity(RegisterRequestDTO reqDto)
        {
            return new UserEntity
            {
                Username = reqDto.Username,
                Password = reqDto.PasswordHash,
                CreationDate = reqDto.CreationDate,
                IsActive = reqDto.IsActive,
            };
        }
        private  static UserEntity MapUserDTOToUserEntity(UserDTO userDTO)
        {
            return new UserEntity
            {
                Username = userDTO.Username,
                Password = userDTO.PasswordHash,
                CreationDate = userDTO.CreationDate,
                IsActive = userDTO.IsActive,
            };
        }
        private static UserDTO MapToUserDTO(UserEntity userEntity) {

            return new UserDTO(userEntity.PersonID,userEntity.Username, userEntity.Password, userEntity.IsActive, userEntity.CreationDate);
                              
        }                      
        public bool CheckUsernameExistence(string username)
        {
             
            return _userRepo.DoesUsernameExist(username);
        }

        public int RegisterNewUser(RegisterRequestDTO reqDto)
        {
         
            UserEntity userEntity = new UserEntity 
            {
               
                Username = reqDto.Username,
                Password = reqDto.PasswordHash, 
                IsActive = true,
                CreationDate = DateTime.Now

                 
            };

          
            return _userRepo.AddUser(userEntity);
        }
        public bool updateUser(RegisterRequestDTO registerRequestDTO) {
        
          return  _userRepo.UpdateUser(MapToUserEntity(registerRequestDTO));

        }
        public int returnPersonID(UserDTO userDTO) {
            return _userRepo.getPersonID(MapUserDTOToUserEntity(userDTO));
        }
        public bool deleteUser(UserDTO userDTO) { 

        return _userRepo.DeleteUser(userDTO.Username);

        }
        public UserDTO Find(string username) {
            return MapToUserDTO(_userRepo.FindUserName(username));
        }
        public  UserDTO Find(int userID) {
       return MapToUserDTO(_userRepo.Find(userID));
        
        }
    }
}
