using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using API_Dinamis.Repository;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Collections.Generic;
using System.Globalization;
using API_Dinamis.Interfaces;
using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly ICityRepository _cityrepository;
        private readonly IMapper _mapper;
        public CityController(ICityRepository cityRepository, IMapper mapper)
        {
            _cityrepository = cityRepository;
            _mapper = mapper;
        }
        // GET: CityController
        [HttpGet("AllCity")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<City>))]
        public IActionResult GetCity_All()
        {
            var cities = _mapper.Map<List<CityDto>>(_cityrepository.GetCities_all());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponse("Data load success", cities, datacount: cities.Count);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<City>))]
        public IActionResult GetCity([FromQuery] int limit, [FromQuery] string? keyword)
        {
            var cities = _mapper.Map<List<CityDto>>(_cityrepository.GetCities(limit, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponse("Data load success", cities, datacount: cities.Count);

            return Ok(response);
        }

        private object ApiResponse(string message, object data, int datacount)
        {
            var response = new
            {
                datacount,
                message,
                data
            };

            return response;
        }
    }
}
