using clsSocialServicesDataAccess;
using clsSocialServicesDataAccess.Admin;
using DTOs;
using DTOs.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsPerson
    {

        private readonly IPersonRespository _personRepository;
        private readonly ILogRepository _logRepo;

        public clsPerson(IPersonRespository personRepository, ILogRepository logRepo)
        {
            _personRepository = personRepository;
            _logRepo = logRepo;
        }
    
        private static PersonEntity MapToPersonEntity(RegisterRequestDTO reqDto)
        {
            return new PersonEntity
            {
                FirstName = reqDto.FirstName,
                SecondName = reqDto.SecondName,
                LastName=reqDto.LastName,
                // Assuming ThirdName is derived or nullable. If not, map it here.
                Email = reqDto.Email,
                Phone = reqDto.Phone,
                Age = reqDto.Age,
                ImagePath = reqDto.Imagepath
            };
        }
        private static PersonEntity MapPersonDTOToPersonEntity(PersonDTO personDTO)
        {
            return new PersonEntity
            {
                PersonID=personDTO.personID,
                FirstName = personDTO.FirstName,
                SecondName = personDTO.SecondName,
                LastName = personDTO.LastName,
                // Assuming ThirdName is derived or nullable. If not, map it here.
                Email = personDTO.Email,
                Phone = personDTO.Phone,
                Age = personDTO.Age,
                ImagePath = personDTO.Imagepath
            };
        }

        private static PersonEntity MapUpdateDTOToPersonEntity(int personID, PersonDetailsUpdateDTO updateDTO)
        {
            return new PersonEntity
            {
                PersonID = personID,
                FirstName = updateDTO.FirstName,
                SecondName = updateDTO.SecondName,
                LastName = updateDTO.LastName,
                Email = updateDTO.Email,
                Phone = updateDTO.Phone,
                Age = updateDTO.Age,
                ImagePath = updateDTO.Imagepath
            };
        }
        private static PersonDTO MapToPersonDTO(PersonEntity personEntity)
        {

            return new PersonDTO(personEntity.PersonID,personEntity.FirstName, personEntity.SecondName, personEntity.LastName, personEntity.Email, personEntity.Phone, personEntity.Age,
                personEntity.ImagePath);

        }


        public  int AddPerson( RegisterRequestDTO reqDto)
        {
            if (reqDto.Imagepath == null || reqDto.Imagepath == "")
                reqDto.Imagepath = null!;


            string newImagePath = UtilLibrary.FileOperations.saveImageTofile(reqDto.Imagepath, UtilLibrary.FileOperations.ImageType.UserImage);
            reqDto.Imagepath = newImagePath;

            return _personRepository.AddPerson(MapToPersonEntity(reqDto));
        }
        //public bool updateImage(int personID,string newPath) {
        
        
        
        //}
        public bool updatePerson(int personID,PersonDetailsUpdateDTO updateDTO , int UserId)
        {
            string newImagePath = UtilLibrary.FileOperations.saveImageTofile(updateDTO.Imagepath, UtilLibrary.FileOperations.ImageType.UserImage);
            updateDTO.Imagepath = newImagePath;

            if( _personRepository.UpdatePerson(MapUpdateDTOToPersonEntity(personID,updateDTO)))

            {
                _logRepo.AddLog("Update User",UserId, "User", "User been updated", null);
                return true;
            }

            return false;
        }
        public bool deletePerson(int personID) {
        
        return  _personRepository.DeletePerson(personID);
        }
        public PersonDTO Find(int personID)
        {
            return MapToPersonDTO(_personRepository.Find(personID));

        }
        //static public int addPerson(RegisterRequestDTO dTO)
        //{

        //    // return clsPersonDataAccess.addNewPerson(dTO.firstName, dTO.secondName, dTO.lastName, dTO.email, dTO.phone, dTO.age, dTO.imagepath);
        //    try
        //    {
        //        _dbContext.People.Add(new clsPerson(dTO));
        //        return dTO.personID;
        //    }
        //    catch
        //    {
        //        return -1;
        //    }

        //}

    }
}
