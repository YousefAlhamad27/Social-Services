using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess
{
    public interface IPersonRespository
    {
        public int AddPerson(PersonEntity personEntity);
        public bool UpdateImage(int personID, string imagePath);
        public PersonEntity Find(int personID);


        public bool UpdatePerson(PersonEntity personEntity);
        public bool DeletePerson(int personID);

        //public PersonEntity GetPersonById(int personId);
    }
}
