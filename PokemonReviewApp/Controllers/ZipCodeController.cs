using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API_Dinamis.Repository;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipCodeController : Controller
    {
        private readonly IZipCodeRepository _zipcoderepository;
        private readonly IMapper _mapper;

        public ZipCodeController(IZipCodeRepository zipcodeRepository, IMapper mapper)
        {
            _zipcoderepository = zipcodeRepository;
            _mapper = mapper;
        }

        // GET: CityController
        [HttpGet("AllZipCode")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<City>))]
        public IActionResult GetZipCode_All()
        {
            var zipcodes = _mapper.Map<List<ZipCodeDto>>(_zipcoderepository.GetZipcodes_all());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponse("Data load success", zipcodes, datacount: zipcodes.Count);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<City>))]
        public IActionResult GetZipCode([FromQuery] int limit, [FromQuery] string? keyword, [FromQuery] int cityid)
        {
            var cities = _mapper.Map<List<ZipCodeDto>>(_zipcoderepository.GetZipcodes(limit, keyword, cityid));

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
