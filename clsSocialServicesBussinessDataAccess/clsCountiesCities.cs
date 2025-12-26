using clsSocialServicesDataAccess.Counties___Cities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsSocialServicesBussiness
{
    public class clsCountiesCities
    {
        private readonly clsSocialServicesDataAccess.Counties___Cities.CountyCityRepository _countyCityRepository;

        public clsCountiesCities(clsSocialServicesDataAccess.Counties___Cities.CountyCityRepository countyCityRepository)
        {
            _countyCityRepository = countyCityRepository;
        }
        public string GetCityNameByID(int cityID)
        {
            try
            {
                var city = _countyCityRepository.GetAllCities()
                    .FirstOrDefault(c => c.CityID == cityID);
                return city != null ? city.CityName : string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public List<CountyEntity> GetCountiesInCity(int cityID)
        {
            try
            {
                var counties = _countyCityRepository.GetCountiesInCity(cityID);
                return counties;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        public List<CountyEntity> GetAllCounties()
        {
            try
            {
                var counties = _countyCityRepository.GetAllCounties();
                return counties;
            }
            catch (Exception)
            {
                return null!;
            }
        }
        public List<CityEntity> GetAllCities()
        {
            try
            {
                var cities = _countyCityRepository.GetAllCities();
                return cities;
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
