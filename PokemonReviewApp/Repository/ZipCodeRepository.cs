using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API_Dinamis.Repository
{
    public class ZipCodeRepository : IZipCodeRepository
    {
        private readonly DataContext _context;
        public ZipCodeRepository(DataContext context)
        {
            _context = context;
        }

        public Zipcode GetZipCode(int id)
        {
            return _context.Zipcodes.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Zipcode> GetZipcodes(int limit, string keyword, int cityid)
        {
            var key = "%" + keyword + "%";

            var query = _context.Zipcodes
            .Where(p => EF.Functions.Like(p.ZipCode.Trim().ToUpper(), key.Trim().ToUpper()) && p.CityId == cityid)
            .OrderBy(p => p.ZipCode)
            .Take(limit)
            .ToList();

            return query;
        }

        public ICollection<Zipcode> GetZipcodes_all()
        {
            return _context.Zipcodes.OrderBy(p => p.ZipCode).ToList();
        }

        public bool ZipCodeExists(int zipCodeId, int cityId)
        {
            var result = _context.Zipcodes.Where(zipcode => zipcode.CityId == cityId && zipcode.Id == zipCodeId).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}
