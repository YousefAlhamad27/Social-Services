using clsSocialServicesDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesDataAccess
{
    public class PersonRepository:IPersonRespository
    {
        private readonly AppDbContext _dbContext;

        // DBContext is injected here in the Data Layer
        public PersonRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public PersonEntity Find(int personID)
        {
           
            try
            {
             return _dbContext.People.Find(personID);
            }
            catch
            {
             return null;
            }
        }
        public int AddPerson(PersonEntity personEntity)
        {
            try
            {
                _dbContext.People.Add(personEntity);
                _dbContext.SaveChanges();
                return personEntity.PersonID;
            }
            catch
            {
                return -1;
            }
        }
        public bool UpdatePerson(PersonEntity personEntity)
        {
            try
            {
                _dbContext.People.Update(personEntity);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public bool DeletePerson(int personID)
        {

            try
            {
                // 2. Find the existing, tracked entity using its permanent ID.
                // Find() is the fastest way to check the database and load the entity.
                PersonEntity personToDelete = _dbContext.People.Find(personID);

                if (personToDelete != null)
                {
                    // 3. Remove the tracked entity instance.
                    _dbContext.People.Remove(personToDelete);

                    // 4. Commit the deletion to the database.
                    _dbContext.SaveChanges();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log the deletion failure (e.g., if there are cascade constraints preventing deletion)
                // Console.WriteLine(ex.Message); 
                return false;
            }
        }
            //public PersonEntity GetPersonByID(int personID)
            //{
            //    //
            //}
        }
}
