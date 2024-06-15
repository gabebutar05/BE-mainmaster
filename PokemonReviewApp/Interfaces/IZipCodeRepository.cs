using API_Dinamis.Models;
using API_Dinamis.Dto;

namespace API_Dinamis.Interfaces
{
    public interface IZipCodeRepository
    {
        ICollection<Zipcode> GetZipcodes_all();
        ICollection<Zipcode> GetZipcodes(int limit, string keyword, int cityid);
        Zipcode GetZipCode(int id);
        bool ZipCodeExists(int zipCodeId, int cityId);
    }
}
