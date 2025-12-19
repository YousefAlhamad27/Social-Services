using clsSocialServicesDataAccess;
using DTOs;
using DTOs.User_Person_DTOs;
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
        

        public int RegisterNewUser(RegisterRequestDTO reqDto,int personID)
        {

         
            UserEntity userEntity = new UserEntity 
            {
               PersonID= personID,
                Username = reqDto.Username,
                Password = reqDto.PasswordHash, 
                IsActive = true,
                CreationDate = DateTime.Now

                 
            };

          
            return _userRepo.AddUser(userEntity);
        }

         
        public int returnPersonID(UserDTO userDTO) {
            return _userRepo.getPersonID(MapUserDTOToUserEntity(userDTO));
        }
        public bool deleteUser(UserDTO userDTO) { 

        return _userRepo.DeleteUser(userDTO.Username);

        }
        public UserDTO Find(string username) {

            try
            {
                return MapToUserDTO(_userRepo.FindUserName(username));
            }
            catch (Exception)
            {
                return null;
            }
        }
        public  UserDTO Find(int userID) {

            try
            {
                return MapToUserDTO(_userRepo.Find(userID));
            }

            catch (Exception)
            {
                return null;
            }

        }
        public bool updatePassword(UserDTO dto) {

            UserEntity user = _userRepo.FindUserName( dto.Username);
            user.Password = dto.PasswordHash;

            return _userRepo.UpdateUser(user);
        }

        public bool revokeToken(RefreshTokenDTO dto,string newToken)
        {
           return  _userRepo.revokeRefreshToken(UtilLibrary.ReturnSHA256(dto.TokenString), UtilLibrary.ReturnSHA256(newToken));
     
        }
        public bool deleteToken(RefreshTokenDTO dto)
        {
            return _userRepo.DeleteRefreshToken(UtilLibrary.ReturnSHA256(dto.TokenString));

        }

        public string getAccessToken(string username)
        {
            UserEntity user = _userRepo.FindUserName(username);

            if (user != null)
            {
                return UtilLibrary.returnToken( user);
            }
            else
            {
                return null;
            }
        }
        public bool addRefreshToken(string refreshToken,string username)
        {

            return _userRepo.AddRefreshToken(UtilLibrary.ReturnSHA256(refreshToken), username);

        }
        public UserDTO returnUserForRefreshToken(string refreshToken)
        {

            UserDTO user;

            UserEntity userEntity = _userRepo.returnUserForToken(_userRepo.returnRefreshToken(refreshToken));

            if (userEntity != null)
            {
                user = MapToUserDTO(userEntity);
                return user;
            }
            return null;
        }
        public UserDTO isRefreshTokenValidForUse(RefreshTokenDTO dto)
        {

            RefreshToken token = _userRepo.returnRefreshToken(UtilLibrary.ReturnSHA256(dto.TokenString));




            if (token == null)
                return null;

            if (token.Expires < DateTime.Now)
                return null;

            


            UserDTO user =  returnUserForRefreshToken(UtilLibrary.ReturnSHA256(dto.TokenString));
            if(user== null) return null;

            return user;

                

                }
    }
}
