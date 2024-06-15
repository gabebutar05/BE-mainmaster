using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> GetPokemons();
    }
}
