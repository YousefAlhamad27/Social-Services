using clsSocialServicesDataAccess.Admin;
using clsSocialServicesDataAccess.Posts;
using DTOs.Posts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsPost
    {
        private readonly PostRepository _postRepository;
        private readonly LogRepository _logRepo;
        public clsPost(PostRepository postRepository,LogRepository LogRepo)
        {
            _postRepository = postRepository;
            _logRepo = LogRepo;
        }

        private PostEntity MapPostDTOToPostEntity(int userID,AddPostDTO dto)
        {
            return new PostEntity
            {
                UserID = userID,
                Description = dto.Description,
                PostTitle = dto.PostTitle,
                CountyID = dto.CountyID,
                PostTypeID = dto.TypeID,
                PublishDateTime = dto.PublishDate,
                LockDate = null,
                ImagePath=dto.imagePath,
                Status=dto.Status,
                ProfessionID=dto.ProfessionID
                ,Price=dto.Price,
                Latitude=dto.Latitude,
                Longitude=dto.Longitude
            };
        }
        private PostDTO MapPostEntityToPostDTO(PostEntity dto)
        {
            if (dto != null)
            {
                return new PostDTO
                {
                    UserID = dto.UserID,
                    Description = dto.Description,
                    PostTitle = dto.PostTitle,
                    CountyID = dto.CountyID,

                    PublishDate = dto.PublishDateTime,
                    PostID = dto.PostID,
                    TypeID = dto.PostTypeID,
                    imagePath = dto.ImagePath,
                    Status = dto.Status,
                    Price=dto.Price,
                    ProfessionID=dto.ProfessionID,
                     Latitude=dto.Latitude,
                        Longitude=dto.Longitude

                };
            }
            else
                return null!;
        }
        private PostEntity MapPostUpdateDTOToPostEntity(int userID, PostUpdateDTO dto,PostEntity currentDetails)
        {
         
            if (currentDetails != null)

                return new PostEntity
                {
                    PostID = dto.PostID,
                    UserID = userID,
                    Description = dto.Description!,
                    PostTitle = dto.PostTitle,
                    CountyID = dto.CountyID,
                    LockDate = null,
                    ImagePath = dto.imagePath!,
                    PostTypeID=currentDetails.PostTypeID,
                    IsComplete=currentDetails.IsComplete,
                    PublishDateTime=currentDetails.PublishDateTime,
                    Status=currentDetails.Status,
                    Price=currentDetails.Price,
                    ProfessionID=currentDetails.ProfessionID,
                    Latitude=currentDetails.Latitude,
                    Longitude=currentDetails.Longitude


                };
            else return null!;
        }
        public bool updatePost(int userID, PostUpdateDTO dto)
        {
            PostEntity currentDetails = _postRepository.Find(dto.PostID)!;
            if (currentDetails == null)
            {
                return false;
            }

            if(dto.imagePath!=null && dto.imagePath!=currentDetails.ImagePath)
            {
                UtilLibrary.FileOperations.removeImageFromFile(currentDetails.ImagePath!, UtilLibrary.FileOperations.ImageType.PostImage);
                dto.imagePath = UtilLibrary.FileOperations.saveImageTofile(dto.imagePath, UtilLibrary.FileOperations.ImageType.PostImage);
            }
            currentDetails.ImagePath= dto.imagePath!;
            currentDetails.Description= dto.Description!;
            currentDetails.PostTitle= dto.PostTitle;
            currentDetails.CountyID= dto.CountyID;
            currentDetails.Price = dto.Price;
            currentDetails.Latitude = dto.Latitude;
            currentDetails.Longitude = dto.Longitude;


            //  PostEntity updatedPost = MapPostUpdateDTOToPostEntity(userID, dto,currentDetails);
            if(_postRepository.UpdatePost(currentDetails))
            {
                int postId = _postRepository.GetLastPostIdByUser(userID);
                _logRepo.AddLog("Update Post", postId, "Post", $"User {userID} Updated Post{postId}", null);
                return true;
            }
            return false; 
        }

        public bool addPost(int userID, AddPostDTO dto,int postID)
        {
           dto.imagePath= UtilLibrary.FileOperations.saveImageTofile(dto.imagePath,UtilLibrary.FileOperations.ImageType.PostImage);

            if( _postRepository.AddPost(MapPostDTOToPostEntity( userID,dto)))
            {
                _logRepo.AddLog("Add Post", postID, "Post", $"User {userID} add new post.", null);
                return true;
            }
            return false;
        }
        public bool deletePost(int postID,int? userId, int? adminId)
        {
            PostEntity post=_postRepository.Find(postID)!;
            if(post==null)
            {
                return false;
            }
            UtilLibrary.FileOperations.removeImageFromFile( post.ImagePath!, UtilLibrary.FileOperations.ImageType.PostImage);

            if (_postRepository.DeletePost(postID))
            {
                string logMessage = adminId.HasValue
                    ? $"Post {postID} has been deleted by Admin {adminId}."
                    : $"Post {postID} has been deleted by User {userId}.";

                _logRepo.AddLog("Delete Post", postID, "Post", logMessage, adminId);
                return true;
            }
            return false;
        }
        public List<PostListDTO> getAllPosts()
        {

            return _postRepository.GetAllPosts();
        }
        public List<PostListDTO> getAllPosts(int userID)
        {

            return _postRepository.GetAllPosts(userID);
        }

        public List<PostListDTO> FilteredList(string? title, int? countyID, int? typeID, int? professionID)
        {
            return _postRepository.GetFilteredPosts(title, countyID, typeID, professionID);
        }

        public bool postExists(int postID)
        {
            PostEntity? post = _postRepository.Find(postID);
            return post != null;
        }
        public PostDTO? getPost(int postID)
        {
            return MapPostEntityToPostDTO(_postRepository.Find(postID)!);
        }
        public bool CompletePost(int userID,int postID)
        {
             
            return _postRepository.CompletePost(userID,postID);
        }
        public bool LockPost(int postID, int? userID, int? adminId = null)
        {
            // إذا Admin بنبعث null للـ userID
            int? repoUserId = adminId != null ? null : userID;

            if (_postRepository.LockPost(postID, repoUserId))
            {
                _logRepo.AddLog("LockPost", postID, "Post", "Post locked", adminId);
                return true;
            }
            return false;
        }
        public bool UnlockPost(int postID,int adminID)
        {
            if(_postRepository.UnlockPost(postID))
            {
                _logRepo.AddLog("Unlock Post", postID, "Post", "Post Unlocked", adminID);
                return true;
            }
            return false;
        }

        public int GetLastPostIdByUser(int userId)
        {
            return _postRepository.GetLastPostIdByUser(userId);
        }

        public async Task<PostEntity> GetPostById(int postId)
        {
            return await _postRepository.GetPostById(postId);
        }
    }
}
