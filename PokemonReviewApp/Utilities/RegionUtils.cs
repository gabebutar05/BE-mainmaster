using API_Dinamis.Interfaces;
using API_Dinamis.Repository;

namespace API_Dinamis.Utilities
{
    public class RegionUtils
    {
        private readonly ICityRepository _cityRepository;
        private readonly IZipCodeRepository _zipCodeRepository;
        private readonly ICountryRepository _countryRepository;
        public RegionUtils(ICityRepository cityRepository, IZipCodeRepository zipCodeRepository, ICountryRepository countryRepository)
        {
            _cityRepository = cityRepository;
            _zipCodeRepository = zipCodeRepository;
            _countryRepository = countryRepository;
        }
        public bool CityExists(int cityId)
        {
            return _cityRepository.CityExists(cityId);
        }

        public bool ZipCodeExists(int zipCodeId, int cityId)
        {
            return _zipCodeRepository.ZipCodeExists(zipCodeId,cityId);
        }
        public bool CountryExists(int countryId)
        {
            return _countryRepository.CountryExists(countryId);
        }
    }
}
