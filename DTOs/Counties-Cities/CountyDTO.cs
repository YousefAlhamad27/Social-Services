using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Counties_Cities
{
    public class CountyDTO
    {
        public int CountyID { get; set; }
        public string CountyName { get; set; } = string.Empty;
        public int CityID { get; set; }
    }
}
