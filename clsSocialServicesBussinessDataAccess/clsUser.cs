using Azure.Core;
using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess.Admin;
using DTOs;
using DTOs.Login;
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
    
        private readonly IUserRepository _userRepo;
        private readonly ILogRepository _logRepo;

        // You would also need an IPersonRepository injected here (not shown for brevity)

        public clsUser(IUserRepository userRepo, ILogRepository logRepo)
        {
            _userRepo = userRepo;
            _logRepo = logRepo;
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
        private  UserDTO MapToUserDTO(UserEntity userEntity) {

            if (userEntity == null)
                return null!;
            return new UserDTO(userEntity.PersonID,userEntity.Username, userEntity.Password, userEntity.IsActive, userEntity.CreationDate);
                              
        }    
        public int getUserID(string username)
        {
            return _userRepo.getUserID(username);
        }   
        public bool CheckUsernameExistence(string username)
        {
             
            return _userRepo.DoesUsernameExist(username);
        }
        public async Task<bool> logoutEverywhere(int userID)
        {
          if( _userRepo.DeleteAllRefreshTokensForUser(userID))
            {
                await _logRepo.AddLog("Logout",userID, "User", "User logout", null);
                return true;
            }
            return false;
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

            var userId = _userRepo.AddUser(userEntity);
            if (userId != -1)
            {
                _logRepo.AddLog("Register", userId, "Login or Register", "New User Registerd!", null);
                return userId;
            }
            return -1;
        }
      
        public int returnPersonID(UserDTO userDTO) {
            return _userRepo.getPersonID(MapUserDTOToUserEntity(userDTO));
        }
        public bool deleteUser(UserDTO userDTO , int userId=0,int? AdminID = null) { 

        if( _userRepo.DeleteUser(userDTO.Username))
            {
                _logRepo.AddLog("Delete User", userId, "User", "User Deleted", AdminID);
                return true;
            }
            return false;

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

            if( _userRepo.UpdateUser(user))
            {
                _logRepo.AddLog("Update password!", user.UserID,"User",$"User {user.UserID} change his password!", null);
                return true;
            }
            return false;
        }

        public bool revokeToken(RefreshTokenDTO dto,string newToken)
        {
           return  _userRepo.revokeRefreshToken(UtilLibrary.ReturnSHA256(dto.TokenString), UtilLibrary.ReturnSHA256(newToken));
     
        }
        public bool deleteToken(RefreshTokenDTO dto)
        {
            return _userRepo.DeleteRefreshToken(UtilLibrary.ReturnSHA256(dto.TokenString));

        }

        public string? getAccessToken(string username)
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
        public UserDTO? returnUserForRefreshToken(string refreshToken)
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
        public UserDTO GetUserByVolunteerID(int volunteerID)
        {
            var userEntity = _userRepo.GetUserByVolunteerID(volunteerID);
            return MapToUserDTO(userEntity);

        }
        }
}
