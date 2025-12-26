using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace clsSocialServicesDataAccess.Counties___Cities
{
    public class CountyCityRepository
    {
        private readonly  AppDbContext  _context;

        public CountyCityRepository(AppDbContext context)
        {
            _context = context;
        }
        public List<CityEntity> GetAllCities()
        {
            try
            {
                var countiesAndCities = _context.Cities.ToList();
                return countiesAndCities;
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
                var counties = _context.Counties.ToList();
                return counties;
            }
            catch (Exception)
            {
                return null!;
            }
        }
        public List<CountyEntity> GetCountiesInCity(int cityID)
        {
                       try
            {
                var counties = _context.Counties
                    .Where(c => c.CityID == cityID)
                    .ToList();
                return counties;
            }
            catch (Exception)
            {
                return null!;
            }
        }
        public string GetCityNameByID(int cityID)
        {
            try
            {
                var city = _context.Cities
                    .FirstOrDefault(c => c.CityID == cityID);

                return city!.CityName;
            }
            catch (Exception)
            {
                return null!;
            }
        }
    }
}
