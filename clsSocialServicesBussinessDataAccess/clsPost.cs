using clsSocialServicesDataAccess.Posts;
using DTOs;
using DTOs.Posts;
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
        public clsPost(PostRepository postRepository)
        {
            _postRepository = postRepository;
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
                Status=dto.Status
            };
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
                    Status=currentDetails.Status

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
                UtilLibrary.FileOperations.removeImageFromFile(currentDetails.ImagePath, UtilLibrary.FileOperations.ImageType.PostImage);
                dto.imagePath = UtilLibrary.FileOperations.saveImageTofile(dto.imagePath, UtilLibrary.FileOperations.ImageType.PostImage);
            }
            currentDetails.ImagePath= dto.imagePath!;
            currentDetails.Description= dto.Description!;
            currentDetails.PostTitle= dto.PostTitle;
            currentDetails.CountyID= dto.CountyID;


          //  PostEntity updatedPost = MapPostUpdateDTOToPostEntity(userID, dto,currentDetails);
            return _postRepository.UpdatePost(currentDetails);
        }

        public bool addPost(int userID, AddPostDTO dto)
        {
           dto.imagePath= UtilLibrary.FileOperations.saveImageTofile(dto.imagePath,UtilLibrary.FileOperations.ImageType.PostImage);

            return _postRepository.AddPost(MapPostDTOToPostEntity( userID,dto));
             

        }
        public bool deletePost(int postID)
        {
            PostEntity post=_postRepository.Find(postID)!;
            if(post==null)
            {
                return false;
            }
            UtilLibrary.FileOperations.removeImageFromFile( post.ImagePath!, UtilLibrary.FileOperations.ImageType.PostImage);

            return _postRepository.DeletePost(postID);
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
        public bool CompletePost(int userID,int postID)
        {
             
            return _postRepository.CompletePost(userID,postID);
        }
        public bool LockPost(int postID,int? userID)
        {

            return _postRepository.LockPost(postID,userID);
        }
        public bool UnlockPost(int postID,int? adminID)
        {
            return _postRepository.UnlockPost(postID);
        }

    }
}
