using DTOs.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Posts
{
    public interface IPostRepository
    {
       public bool AddPost(PostEntity postEntity);

        bool UpdatePost(PostEntity postEntity);
        bool DeletePost(int postID);
        PostEntity? Find(int postID);
        List<PostListDTO> GetAllPosts(int userID);
        List<PostListDTO> GetAllPosts();
        List<PostListDTO> GetFilteredPosts(string? searchQuery, int? countyID, int? postTypeID, int? professionID);
         public Task<int> PostsCount();
        public int GetLastPostIdByUser(int userId);
        public Task<PostEntity> GetPostById(int postID);
        public bool CompletePost(int userID, int postID);
        public bool LockPost(int postID, int? userID);
        public bool UnlockPost(int postID);

    }
}
