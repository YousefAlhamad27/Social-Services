using clsSocialDataAccess.Professions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsProfession
    {
        private readonly IProfessionRepository _professionRepo;

        public clsProfession(IProfessionRepository professionRepo)
        {
            _professionRepo = professionRepo;
        }

        public async Task<int> GetProfessionIdByProfessionName(string ProfessionTitle)
        { 
            return await _professionRepo.GetProfessionIdByProfessionName(ProfessionTitle);
        }

    }
}
