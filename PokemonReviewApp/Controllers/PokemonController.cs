using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Dinamis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonrepository;
        public PokemonController(IPokemonRepository pokemonRepository)
        {
            _pokemonrepository = pokemonRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonrepository.GetPokemons();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }
    }
}
