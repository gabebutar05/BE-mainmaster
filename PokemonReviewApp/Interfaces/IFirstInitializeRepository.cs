using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IFirstInitializeRepository
    {
        Task InjectDefaultStandardValue(List<StandardValue> data_);
        Task InjectDefaultCountry(List<Country> data_);
        Task InjectDefaultCity(List<City> data_);
        Task InjectDefaultZipCode(List<Zipcode> data_);
        Task InjectDefaultModule(List<Module> data_);
        Task InjectDefaultMenu(List<Menu> data_);
    }
}
