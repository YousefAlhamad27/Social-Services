using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Volunteers
{
    public interface IVolunteerRepository
    {
        public enum ApplicationStatus : byte
        {
            Pending = 1,
            Approved = 2,
            Rejected = 3
        }


    }
}
