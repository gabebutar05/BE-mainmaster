using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Repository;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _menurepository;
        private readonly IMapper _mapper;

        public MenuController(IMenuRepository menuRepository, IMapper mapper)
        {
            _menurepository = menuRepository;
            _mapper = mapper;
        }

        [HttpGet("AllMenu")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Menu>))]
        public IActionResult GetMenu_All()
        {
            var menus = _mapper.Map<List<MenuDto>>(_menurepository.GetMenus_all());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", menus, datacount: menus.Count);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Menu>))]
        public IActionResult GetZipCode([FromQuery] int moduleid, [FromQuery] int? limit = 0, [FromQuery] int? page = 1, [FromQuery] string? sortby = "d", [FromQuery] string? sortdesc = "Id", [FromQuery] string? keyword = "")
        {
            var menus = _mapper.Map<List<MenuDto>>(_menurepository.GetMenus(moduleid, limit, page, sortby, sortdesc));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = ApiResponseHelper.StdApiResponse("Data load success", menus, datacount: menus.Count);

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
