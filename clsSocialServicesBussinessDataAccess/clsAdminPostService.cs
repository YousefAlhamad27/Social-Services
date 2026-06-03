using clsSocialServicesDataAccess.Posts;
using DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsAdminPostService
    {
        private  readonly IPostRepository _postRepo;

        public  clsAdminPostService(IPostRepository postRepo)
        {
            _postRepo = postRepo;
        }

        public  AdminStatusDTO GetStatus()
        {
            return new AdminStatusDTO()
            {
                TotalPost = _postRepo.PostsCount
            };
        }

        public  bool DeletePost(int postID)
        {
            if (postID <= 0)
                return false;

            return _postRepo.DeletePost(postID);
        }

    }
}
