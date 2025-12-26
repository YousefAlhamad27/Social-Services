using clsSocialServicesDataAccess.Counties___Cities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTOs.Counties_Cities;

namespace SocialServices.Controllers
{

    [ApiController]
    [Route("CitiesCounties")]
    public class CitiesCountiesController : Controller
    {
        private readonly clsSocialServicesBussiness.clsCountiesCities _countiesCitiesService;
        public CitiesCountiesController(clsSocialServicesBussiness.clsCountiesCities countiesCitiesService)
        {
            _countiesCitiesService = countiesCitiesService;
        }

        [HttpGet("Get City Name")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles ="User,Admin")]
        public ActionResult GetCityNameByID(int cityID)
        {
            var cityName = _countiesCitiesService.GetCityNameByID(cityID);
            if (string.IsNullOrEmpty(cityName))
            {
                return NotFound("City not found");
            }
            return Ok(cityName);
        }

        [HttpGet("Get Counties In City")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult<List<CountyEntity>> GetCountiesInCity(int cityID)
        {
            var counties = _countiesCitiesService.GetCountiesInCity(cityID);
            if (counties == null || counties.Count == 0)
            {
                return NotFound("No counties found for the specified city");
            }
            var countyDTOs = counties.Select(c => new CountyDTO
            {
                CountyID = c.CountyID,
                CountyName = c.CountyName,
                CityID = c.CityID
            }
            ).ToList();
            return Ok(countyDTOs);
        }

        [HttpGet("Get All Counties")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User,Admin")]

        public ActionResult<List<CountyEntity>> GetAllCounties()
        {
            var counties = _countiesCitiesService.GetAllCounties();
            if (counties == null || counties.Count == 0)
            {
                return NotFound("No counties found");
            }
            var countyDTOs = counties.Select(c => new CountyDTO
            {
                CountyID = c.CountyID,
                CountyName = c.CountyName,
                CityID = c.CityID
            }
            ).ToList();
            return Ok(countyDTOs);
        }

        [HttpGet("Get All Cities")]
        [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status401Unauthorized), ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User,Admin")]
        public ActionResult<List<CityEntity>> GetAllCities()
        {
            var cities = _countiesCitiesService.GetAllCities();
            if (cities == null || cities.Count == 0)
            {
                return NotFound("No cities found");
            }

            return Ok(cities);
        }

    }
        }
