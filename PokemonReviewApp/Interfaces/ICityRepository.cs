using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface ICityRepository
    {
        ICollection<City> GetCities_all();
        ICollection<City> GetCities(int limit, string keyword);
        City GetCity(int id);
        bool CityExists(int cityId);
    }
}
