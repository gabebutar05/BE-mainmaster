using Microsoft.EntityFrameworkCore;
using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;

namespace API_Dinamis.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;
        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public bool CountryExists(int Id)
        {
            var result = _context.Countries.Where(country => country.Id == Id).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            return true;
        }

        public ICollection<Country> GetCountries(int? limit = null, string? keyword = null)
        {
            IQueryable<Country> query = _context.Countries;

            if (!string.IsNullOrEmpty(keyword))
            {
                var key = "%" + keyword + "%";
                query = query.Where(country => EF.Functions.Like(country.CountryName.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            query = query.OrderBy(country => country.CountryName);

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var result = query.ToList();
            return result;
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(country => country.Id == id).FirstOrDefault();
        }

        public int GetCountryCount()
        {
            return _context.Countries.Count();
        }
    }
}
