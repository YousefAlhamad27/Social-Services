using DTOs.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Posts.Preferances
{
    public interface ILogViewRepository 
    {
        public Task AddLogView(int UserId,int ProfessionID);
        public Task<List<PostListDTO>> GetAllPreferancesPosts(int UserId);
        
    }
}
