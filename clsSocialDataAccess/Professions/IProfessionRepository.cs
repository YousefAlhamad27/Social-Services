using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Professions
{
    public interface IProfessionRepository
    {
        public Task<int> GetProfessionIdByProfessionName(string ProfessionTitle);
    }
}
