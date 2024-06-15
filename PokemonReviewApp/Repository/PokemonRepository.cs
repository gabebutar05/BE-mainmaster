using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;

namespace API_Dinamis.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;
        public PokemonRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p => p.Id).ToList();
        }
    }
}
