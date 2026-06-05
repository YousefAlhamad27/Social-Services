using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess.Admin
{
    public interface ILogRepository
    {
        Task AddLog(string action, int targetId, string targetType, string targetDescription, int? adminId);

        public Task<List<LogEntity>> GetLogs(string targetType);
    }
}
