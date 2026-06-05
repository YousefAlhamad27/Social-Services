using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Admin
{
    public class LogRepository : ILogRepository
    {
        private readonly AppDbContext _dbContext;

        public LogRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddLog(string action, int targetId, string targetType, string targetDescription, int? adminId)
        {
            var log = new LogEntity
            {
                Action = action,
                TargetId = targetId,
                TargetType = targetType,
                TargetDescription = targetDescription,
                AdminId = adminId,
                CreatedAt = DateTime.Now
            };

            await _dbContext.Logs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<LogEntity>> GetLogs(string targetType)
        {
            return await _dbContext.Logs.Where(l => l.TargetType.ToLower() == targetType.ToLower())
                .ToListAsync();
        }

    }
}

