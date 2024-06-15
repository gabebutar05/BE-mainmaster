using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Repository;
using API_Dinamis.Utilities;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryrepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryrepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult GetCountry([FromQuery] int? limit = null, [FromQuery] string? keyword = null)
        {
            var countries = _mapper.Map<List<Country>>(_countryrepository.GetCountries(limit, keyword));
            int countriesCount = (limit != null && keyword != null) ? countries.Count() : _countryrepository.GetCountryCount();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", countries, datacount: countriesCount);

            return Ok(response);
        }
    }
}
