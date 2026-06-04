using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Posts;

namespace clsSocialServicesDataAccess.Posts
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _dbContext;
        public PostRepository(AppDbContext dbContext)
        {

            _dbContext = dbContext;
        }

        public bool AddPost(PostEntity post)
        {

            if (post.Latitude == 0 || post.Longitude == 0)
            {
                post.Latitude = null;
                post.Longitude = null;
            }
            if (post.Price == 0)
            {
                post.Price = null;
            }
            try
            {

                _dbContext.Posts.Add(post);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return false;
            }
        }
        public bool DeletePost(int postID)
        {
            try
            {
                var post = _dbContext.Posts.Find(postID);
                if (post == null)
                {
                    return false; // Post not found
                }
                _dbContext.Posts.Remove(post);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return false;
            }
        }
        public bool UpdatePost(PostEntity post)
        {
            if (post.Latitude == 0 || post.Longitude == 0)
            {
                post.Latitude = null;
                post.Longitude = null;
            }
            if (post.Price == 0)
            {
                post.Price = null;
            }

            try
            {
                _dbContext.Posts.Update(post);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return false;
            }
        }
        public PostEntity? Find(int postID)
        {
            try
            {
                return _dbContext.Posts.Find(postID);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return null;
            }
        }
        public List<PostListDTO> GetAllPosts(int userID)
        {

            try
            {

                IQueryable query = from p in _dbContext.Posts

                                       //  Get County Name
                                   join c in _dbContext.Counties
                                   on p.CountyID equals c.CountyID

                                   join prof in _dbContext.Professions
                                   on p.ProfessionID equals prof.ProfessionID
                                   //  Get Post Type Name 
                                   join pt in _dbContext.PostTypes
                            on p.PostTypeID equals pt.PostTypeID

                                   // Get User/Person Name (To show Author)
                                   // Assuming you have a Users table linked to a People table
                                   join u in _dbContext.Users on p.UserID equals u.UserID
                                   join per in _dbContext.People on u.PersonID equals per.PersonID
                                   where u.UserID == userID

                                   // SELECT: Build your final list
                                   select new PostListDTO
                                   {
                                       PostID = p.PostID,
                                       PostTitle = p.PostTitle,
                                       Description = p.Description,
                                       PublishDateTime = p.PublishDateTime,

                                       // HERE IS THE MAGIC:
                                       // We grab the name from the 'c' (County) variable, not 'p' (Post)
                                       CountyName = c.CountyName,
                                       ImagePath = p.ImagePath,
                                       IsComplete = p.IsComplete,
                                       ProfessionName = prof.ProfessionTitle, // Assuming ProfessionID is an int
                                       Status = p.Status,
                                       // Grab Type name from 'pt'
                                       PostTypeName = pt.TypeTitle,

                                       // Grab Author name from 'per'
                                       AuthorName = per.FirstName + " " + per.LastName,

                                       // We still pass the IDs for logic if needed
                                       UserID = p.UserID
                                   };
                return query.Cast<PostListDTO>().ToList();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return null!;
            }
        }
        public List<PostListDTO> GetAllPosts()
        {

            try
            {

                IQueryable query = from p in _dbContext.Posts

                                   //  Get County Name
                                   join c in _dbContext.Counties
                                   on p.CountyID equals c.CountyID

                                   join prof in _dbContext.Professions
                                   on p.ProfessionID equals prof.ProfessionID
                                   //  Get Post Type Name 
                                   join pt in _dbContext.PostTypes
                            on p.PostTypeID equals pt.PostTypeID

                                   // Get User/Person Name (To show Author)
                                   // Assuming you have a Users table linked to a People table
                                   join u in _dbContext.Users on p.UserID equals u.UserID
                                   join per in _dbContext.People on u.PersonID equals per.PersonID
                                   where p.LockDate == null

                                   // SELECT: Build your final list
                                   select new PostListDTO
                                   {
                                       PostID = p.PostID,
                                       PostTitle = p.PostTitle,
                                       Description = p.Description,
                                       PublishDateTime = p.PublishDateTime,

                                       // HERE IS THE MAGIC:
                                       // We grab the name from the 'c' (County) variable, not 'p' (Post)
                                       CountyName = c.CountyName,
                                       ImagePath = p.ImagePath,
                                       IsComplete = p.IsComplete,
                                       ProfessionName = prof.ProfessionTitle, // Assuming ProfessionID is an int
                                       Status = p.Status,
                                       // Grab Type name from 'pt'
                                       PostTypeName = pt.TypeTitle,

                                       // Grab Author name from 'per'
                                       AuthorName = per.FirstName + " " + per.LastName,

                                       // We still pass the IDs for logic if needed
                                       UserID = p.UserID
                                   };
                return query.Cast<PostListDTO>().ToList();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return null!;
            }
        }
        public List<PostListDTO> GetFilteredPosts(string? searchQuery, int? countyID, int? postTypeID, int? professionID)
        {

            try
            {
                IQueryable<PostEntity> posts = _dbContext.Posts;

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    posts = posts.Where(p => p.PostTitle.Contains(searchQuery) ||
                                             (p.Description != null && p.Description.Contains(searchQuery)));
                }

                // Filter by Dropdowns
                if (countyID.HasValue)
                {
                    posts = posts.Where(p => p.CountyID == countyID.Value);
                }

                if (postTypeID.HasValue)
                {
                    posts = posts.Where(p => p.PostTypeID == postTypeID.Value);
                }

                if (professionID.HasValue)
                {
                    posts = posts.Where(p => p.ProfessionID == professionID.Value);
                }

                // 3. NOW PERFORM THE JOINS
                // Notice: We use 'posts' (the filtered list) instead of '_dbContext.Posts'
                var query = from p in posts

                            join c in _dbContext.Counties on p.CountyID equals c.CountyID
                            join pt in _dbContext.PostTypes on p.PostTypeID equals pt.PostTypeID
                            join u in _dbContext.Users on p.UserID equals u.UserID
                            join per in _dbContext.People on u.PersonID equals per.PersonID

                            // Left Join for Profession
                            join prof in _dbContext.Professions
                            on p.ProfessionID equals prof.ProfessionID into profGroup
                            from prof in profGroup.DefaultIfEmpty()
                            where p.Status == 1
                            // 4. ORDERING (Optional but recommended)
                            // Show newest posts first
                            orderby p.PublishDateTime descending

                            select new PostListDTO
                            {
                                PostID = p.PostID,
                                PostTitle = p.PostTitle,
                                Description = p.Description,
                                PublishDateTime = p.PublishDateTime,
                                CountyName = c.CountyName,
                                ImagePath = p.ImagePath,
                                IsComplete = p.IsComplete,
                                Status = p.Status,
                                PostTypeName = pt.TypeTitle,
                                AuthorName = per.FirstName + " " + per.LastName,
                                UserID = p.UserID,
                                ProfessionName = (prof == null) ? "General" : prof.ProfessionTitle
                            };

                return query.ToList();
            }

            catch (Exception ex)
            {
                // Log error
                return new List<PostListDTO>();
            }



        }
        public bool CompletePost(int userID, int postID)
        {
            try
            {
                var post = _dbContext.Posts.Find(postID);
                if (post == null)
                {
                    return false; // Post not found
                }
                if (post.UserID != userID)
                {
                    return false; // Unauthorized
                }
                post.IsComplete = true;
                _dbContext.Posts.Update(post);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return false;
            }
        }

        public bool LockPost(int postID, int? userID)
        {
            try
            {
                var post = _dbContext.Posts.Find(postID);
                if (post == null)
                {
                    return false; // Post not found
                }
                // implement for admin later

                if (userID != null)
                    if (post.UserID != userID)
                    {
                        return false; // unauthorized
                    }
                post.LockDate = DateTime.Now;
                post.Status = 2; // post locked
                _dbContext.Posts.Update(post);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return false;
            }
        }
        public bool UnlockPost(int postID)
        {
            try
            {
                var post = _dbContext.Posts.Find(postID);
                if (post == null)
                {
                    return false; // Post not found
                }
                // confirm admin is authorized
                post.LockDate = null;
                post.Status = 1; // post unlocked
                _dbContext.Posts.Update(post);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                return false;
            }
        }

        public async Task<int> PostsCount()
        {
          
                try
                {
                    return await _dbContext.Posts.CountAsync();
                }
                catch (Exception ex)
                {
                    return 0;
                }
  
        }
    }
}
