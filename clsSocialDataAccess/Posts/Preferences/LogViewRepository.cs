using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess.Posts;
using DTOs.Posts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Posts.Preferances
{
    public class LogViewRepository : ILogViewRepository
    {
        private readonly AppDbContext _dbContext;

        public LogViewRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddLogView(int UserId, int ProfessionID)
        {
            var log = new LogViewEntity
            {
                UserId = UserId,
                ProfessionID = ProfessionID,
                ViewDate = DateTime.Now
            };

            await _dbContext.LogViews.AddAsync(log);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<PostListDTO>> GetAllPreferancesPosts(int UserId)
        {
            var professionIds = await _dbContext.LogViews
                .Where(l => l.UserId == UserId)
                .GroupBy(l => l.ProfessionID)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .ToListAsync();

            var allPosts = await _dbContext.Posts
                .Join(_dbContext.Users, p => p.UserID, u => u.UserID, (p, u) => new { p, u })
                .Join(_dbContext.People, pu => pu.u.PersonID, per => per.PersonID, (pu, per) => new { pu.p, pu.u, per })
                .Join(_dbContext.Professions, pup => pup.p.ProfessionID, pr => pr.ProfessionID, (pup, pr) => new { pup.p, pup.per, pr })
                .Join(_dbContext.Counties, pupr => pupr.p.CountyID, c => c.CountyID, (pupr, c) => new { pupr.p, pupr.per, pupr.pr, c })
                .Join(_dbContext.PostTypes, puprc => puprc.p.PostTypeID, pt => pt.PostTypeID, (puprc, pt) => new
                {
                    PostID = puprc.p.PostID,
                    UserID = puprc.p.UserID,
                    ProfessionID = puprc.p.ProfessionID,
                    PostTitle = puprc.p.PostTitle,
                    Description = puprc.p.Description,
                    ImagePath = puprc.p.ImagePath,
                    AuthorName = puprc.per.FirstName + " " + puprc.per.LastName,
                    ProfessionName = puprc.pr.ProfessionTitle,
                    CountyName = puprc.c.CountyName,
                    PostTypeName = pt.TypeTitle,
                    Status = puprc.p.Status,
                    PublishDateTime = puprc.p.PublishDateTime,
                    IsComplete = puprc.p.IsComplete,
                    Price = puprc.p.Price,
                    Latitude = puprc.p.Latitude,
                    Longitude = puprc.p.Longitude,
                    RemainingServicesRequiredCount = puprc.p.RequiredServicesCount - puprc.p.AcceptedServiceApplicationsCount
                })
                .ToListAsync();

            var random = new Random();

            var result = allPosts
                .Select(p => new
                {
                    Post = p,
                    Weight = professionIds.Contains(p.ProfessionID)
                        ? (professionIds.Count - professionIds.IndexOf(p.ProfessionID)) * 10
                        : 1
                })
                .OrderByDescending(x => x.Weight * random.NextDouble())
                .Select(x => new PostListDTO
                {
                    PostID = x.Post.PostID,
                    UserID = x.Post.UserID,
                    PostTitle = x.Post.PostTitle,
                    Description = x.Post.Description,
                    ImagePath = x.Post.ImagePath,
                    AuthorName = x.Post.AuthorName,
                    ProfessionName = x.Post.ProfessionName,
                    CountyName = x.Post.CountyName,
                    PostTypeName = x.Post.PostTypeName,
                    Status = x.Post.Status,
                    PublishDateTime = x.Post.PublishDateTime,
                    IsComplete = x.Post.IsComplete,
                    Price = x.Post.Price,
                    Latitude = x.Post.Latitude,
                    Longitude = x.Post.Longitude,
                    RemainingServicesRequiredCount = x.Post.RemainingServicesRequiredCount
                })
                .ToList();

            return result;
        }
    }
}
