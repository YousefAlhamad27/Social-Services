using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialDataAccess.Posts.Preferances
{
    public class LogViewEntity
    {
       public int Id { get; set; }
       public int UserId { get; set; }
       public int ProfessionID { get; set; }
       public DateTime ViewDate { get; set; }
    }
}
