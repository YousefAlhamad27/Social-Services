using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Volunteer
{
    public class UpdateVolunteerDTO
    {
                public int VolunteerID { get; set; }
        public string Description { get; set; } = string.Empty;

    }
}
