using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Repository;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleRepository _modulepository;
        private readonly IMapper _mapper;
        public ModuleController(IModuleRepository moduleRepository, IMapper mapper)
        {
            _modulepository = moduleRepository;
            _mapper = mapper;
        }
        // GET: api/<ModuleController>
        [HttpGet("AllModule")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Module>))]
        public IActionResult GetModule_All()
        {
            var modules = _mapper.Map<List<ModuleDto>>(_modulepository.GetModules_all());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", modules, datacount: modules.Count);

            return Ok(response);
        }

        // GET api/<ModuleController>/5
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Module>))]
        public IActionResult GetModule([FromQuery] int limit, [FromQuery] string? keyword)
        {
            var modules = _mapper.Map<List<ModuleDto>>(_modulepository.GetModules(limit, keyword));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", modules, datacount: modules.Count);

            return Ok(response);
        }
    }
}
