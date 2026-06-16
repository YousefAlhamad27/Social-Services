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

            // جيب كل البوستات
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

            var preferredGroups = professionIds
                .Select(profId => allPosts
                    .Where(p => p.ProfessionID == profId)
                    .OrderBy(_ => random.Next())
                    .ToList())
                .ToList();

            var otherPosts = allPosts
                .Where(p => !professionIds.Contains(p.ProfessionID))
                .OrderBy(_ => random.Next())
                .ToList();

            var result = new List<PostListDTO>();
            int maxCount = preferredGroups.Any() ? preferredGroups.Max(g => g.Count) : 0;

            for (int i = 0; i < maxCount; i++)
            {
                foreach (var group in preferredGroups)
                {
                    if (i < group.Count)
                    {
                        var p = group[i];
                        result.Add(new PostListDTO
                        {
                            PostID = p.PostID,
                            UserID = p.UserID,
                            PostTitle = p.PostTitle,
                            Description = p.Description,
                            ImagePath = p.ImagePath,
                            AuthorName = p.AuthorName,
                            ProfessionName = p.ProfessionName,
                            CountyName = p.CountyName,
                            PostTypeName = p.PostTypeName,
                            Status = p.Status,
                            PublishDateTime = p.PublishDateTime,
                            IsComplete = p.IsComplete,
                            Price = p.Price,
                            Latitude = p.Latitude,
                            Longitude = p.Longitude,
                            RemainingServicesRequiredCount = p.RemainingServicesRequiredCount
                        });
                    }
                }
            }
            result.AddRange(otherPosts.Select(p => new PostListDTO
            {
                PostID = p.PostID,
                UserID = p.UserID,
                PostTitle = p.PostTitle,
                Description = p.Description,
                ImagePath = p.ImagePath,
                AuthorName = p.AuthorName,
                ProfessionName = p.ProfessionName,
                CountyName = p.CountyName,
                PostTypeName = p.PostTypeName,
                Status = p.Status,
                PublishDateTime = p.PublishDateTime,
                IsComplete = p.IsComplete,
                Price = p.Price,
                Latitude = p.Latitude,
                Longitude = p.Longitude,
                RemainingServicesRequiredCount = p.RemainingServicesRequiredCount
            }));

            return result;
        }
    }
}
