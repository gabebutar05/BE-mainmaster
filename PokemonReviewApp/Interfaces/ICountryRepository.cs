using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface ICountryRepository
    {
        ICollection<Country> GetCountries(int? limit = null, string? keyword = null);

        Country GetCountry(int id);

        int GetCountryCount();
        bool CountryExists(int Id);
    }
}
