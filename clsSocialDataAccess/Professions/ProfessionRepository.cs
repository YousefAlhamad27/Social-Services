using clsSocialServicesDataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Professions
{
    public class ProfessionRepository : IProfessionRepository
    {
        private readonly AppDbContext _context;

        public ProfessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetProfessionIdByProfessionName(string ProfessionTitle)
        {
           return await _context.Professions
                .Where(p=> p.ProfessionTitle == ProfessionTitle)
                .Select(p=> p.ProfessionID).
                FirstOrDefaultAsync();
        }

    }
}
