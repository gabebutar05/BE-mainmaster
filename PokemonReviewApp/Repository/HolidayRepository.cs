using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Dinamis.Repository
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly RepositoryUtils _repositoryUtility;
        private readonly IAuthRepository _authrepository;

        public HolidayRepository(DataContext context, IMapper mapper, RepositoryUtils repositoryUtility, IAuthRepository authrepository)
        {
            _context = context;
            _mapper = mapper;
            _repositoryUtility = repositoryUtility;
            _authrepository = authrepository;
        }

        public bool CreateHoliday(Holiday data_)
        {
            _context.Add(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateHolidayLog(HolidayLog data_)
        {
            _context.Add(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateHolidayLogTemp(HolidayLogTemp data_)
        {
            _context.Add(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteHoliday(Holiday data_)
        {
            _context.Remove(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteHolidayLogTemp(HolidayLogTemp data_)
        {
            _context.Remove(data_);
            return _repositoryUtility.SaveChanges();
        }

        public Holiday GetHoliday(int id)
        {
            return _context.Holidays.Where(data => data.Id == id).FirstOrDefault();
        }

        public int GetHolidayCount(string? target)
        {
            return (target == "main") ? _context.Holidays.Count() : _context.HolidaysLogTemp.Count();
        }

        public HolidayLogTemp GetHolidayLogTemp(int id)
        {
            return _context.HolidaysLogTemp.Where(data => data.Id == id).FirstOrDefault();
        }

        public ICollection<HolidayDto> GetHolidays(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "Id", string? keyword = "")
        {
            IQueryable<Holiday> query = _context.Holidays;

            if (keyword != "")
            {
                var key = "%" + keyword + "%";
                query = query.Where(data => EF.Functions.Like(data.HolidayDate.ToString().Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(data.Description.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            query = (sortby.ToLower() == "d") ? query.OrderByDescending(RepositoryUtils.GetOrderByExpression<Holiday>(sortdesc)) : query.OrderBy(RepositoryUtils.GetOrderByExpression<Holiday>(sortdesc));

            if(limit > 0)
            {
                limit = (limit < 10) ? 10 : limit;
                int offset = (page > 1) ? (page.Value - 1) * limit.Value : 0;

                query = query.Skip(offset).Take(limit.Value);
            }

            return _mapper.Map<List<HolidayDto>>(query).ToList();
        }

        public ICollection<HolidayTempDto> GetHolidaysLogTemp(int? limit = 0, int? page = 1, string? sortby = "D", string? sortdesc = "Id", string? keyword = "")
        {
            IQueryable<HolidayLogTemp> query = _context.HolidaysLogTemp;

            if (keyword != "")
            {
                var key = "%" + keyword + "%";
                query = query.Where(data => EF.Functions.Like(data.HolidayDate.ToString().Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(data.Description.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            query = (sortby.ToLower() == "d") ? query.OrderByDescending(RepositoryUtils.GetOrderByExpression<HolidayLogTemp>(sortdesc)) : query.OrderBy(RepositoryUtils.GetOrderByExpression<HolidayLogTemp>(sortdesc));

            if (limit > 0)
            {
                limit = (limit < 10) ? 10 : limit;
                int offset = (page > 1) ? (page.Value - 1) * limit.Value : 0;

                query = query.Skip(offset).Take(limit.Value);
            }

            return _mapper.Map<List<HolidayTempDto>>(query).ToList();
        }

        public bool HolidayExists(int id, string? target="main")
        {
            IEnumerable<object> result = (target == "main") ? _context.Holidays : _context.HolidaysLogTemp;
                
            return result.Any(data => _repositoryUtility.GetIdPropertyValue(data) == id);
        }

        public bool UpdateHoliday(Holiday data_)
        {
            _context.Update(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateHolidayLogTemp(HolidayLogTemp data_)
        {
            _context.Update(data_);
            return _repositoryUtility.SaveChanges();
        }
    }
}
