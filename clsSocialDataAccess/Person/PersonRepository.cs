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
             return _dbContext.People.Find(personID)!;
            }
            catch
            {
             return null;
            }
        }
        public bool UpdateImage(int personID,string imagePath)
        {
           
           

            try
            {
                PersonEntity person = _dbContext.People.FirstOrDefault(u => u.PersonID == personID)!;
                if (person != null)
                {
                    if(imagePath==null||imagePath=="")
                        person.ImagePath = null!;
                    
                    else
                    {
                        person.ImagePath = imagePath!;
                    }
                        _dbContext.People.Update(person);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;

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
                
                
                PersonEntity personToDelete = _dbContext.People.Find(personID)!;

                if (personToDelete != null)
                {
                   
                    _dbContext.People.Remove(personToDelete);

                    
                    _dbContext.SaveChanges();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }
            //public PersonEntity GetPersonByID(int personID)
            //{
            //    //
            //}
        }
}
