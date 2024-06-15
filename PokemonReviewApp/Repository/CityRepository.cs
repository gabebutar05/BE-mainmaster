using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using API_Dinamis.Interfaces;
using API_Dinamis.Data;
using API_Dinamis.Models;

namespace API_Dinamis.Repository
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _context;
        public CityRepository(DataContext context)
        {
            _context = context;
        }

        public bool CityExists(int cityId)
        {
            var result = _context.Cities.Where(city => city.Id == cityId).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public ICollection<City> GetCities(int limit, string keyword)
        {
            var key = "%" + keyword + "%";

            var query = _context.Cities
            .Where(p => EF.Functions.Like(p.CityName.Trim().ToUpper(), key.Trim().ToUpper()))
            .OrderBy(p => p.CityName)
            .Take(limit)
            .ToList();

            return query;
        }

        public ICollection<City> GetCities_all()
        {
            return _context.Cities.OrderBy(p => p.CityName).ToList();
        }

        public City GetCity(int id)
        {
            return _context.Cities.Where(p => p.Id == id).FirstOrDefault();
        }
    }
}
