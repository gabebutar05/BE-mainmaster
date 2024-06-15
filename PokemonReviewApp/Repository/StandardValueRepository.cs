using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Dinamis.Repository
{
    public class StandardValueRepository : IStandardValueRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly RepositoryUtils _repositoryUtility;
        private readonly IAuthRepository _authrepository;

        public StandardValueRepository(DataContext context, IMapper mapper, RepositoryUtils repositoryUtility, IAuthRepository authrepository)
        {
            _context = context;
            _mapper = mapper;
            _repositoryUtility = repositoryUtility;
            _authrepository = authrepository;
        }

        public bool CreateStandardValueLog(StandardValueLog data_)
        {
            _context.Add(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateStandardValueLogTemp(StandardValueLogTemp data_)
        {
            _context.Add(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteStandardValueLogTemp(StandardValueLogTemp data_)
        {
            _context.Remove(data_);
            return _repositoryUtility.SaveChanges();
        }

        public StandardValue GetStandardValue(int id)
        {
            return _context.StandardValues.Where(data => data.Id == id).FirstOrDefault();
        }

        public int GetStandardValueCount(string? target)
        {
            return (target == "main") ? _context.StandardValues.Count() : _context.StandardValuesLogTemp.Count();
        }

        public StandardValueLogTemp GetStandardValueLogTemp(int id)
        {
            return _context.StandardValuesLogTemp.Where(data => data.Id == id).FirstOrDefault();
        }

        public ICollection<StandardValueDto> GetStandardValues(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "Id", string? keyword = "")
        {
            IQueryable<StandardValue> query = _context.StandardValues;

            if (keyword != "")
            {
                var key = "%" + keyword + "%";
                query = query.Where(data => EF.Functions.Like(data.DataName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(data.Description.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            query = (sortby.ToLower() == "d") ? query.OrderByDescending(RepositoryUtils.GetOrderByExpression<StandardValue>(sortdesc)) : query.OrderBy(RepositoryUtils.GetOrderByExpression<StandardValue>(sortdesc));

            if (limit > 0)
            {
                limit = (limit < 10) ? 10 : limit;
                int offset = (page > 1) ? (page.Value - 1) * limit.Value : 0;

                query = query.Skip(offset).Take(limit.Value);
            }

            return _mapper.Map<List<StandardValueDto>>(query).ToList();
        }

        public ICollection<StandardValueLogTemp> GetStandardValuesLogTemp(int? limit = 0, int? page = 1, string? sortby = "D", string? sortdesc = "Id", string? keyword = "")
        {
            IQueryable<StandardValueLogTemp> query = _context.StandardValuesLogTemp;

            if (keyword != "")
            {
                var key = "%" + keyword + "%";
                query = query.Where(data => EF.Functions.Like(data.DataName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(data.Description.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            query = (sortby.ToLower() == "d") ? query.OrderByDescending(RepositoryUtils.GetOrderByExpression<StandardValueLogTemp>(sortdesc)) : query.OrderBy(RepositoryUtils.GetOrderByExpression<StandardValueLogTemp>(sortdesc));

            if (limit > 0)
            {
                limit = (limit < 10) ? 10 : limit;
                int offset = (page > 1) ? (page.Value - 1) * limit.Value : 0;

                query = query.Skip(offset).Take(limit.Value);
            }

            return _mapper.Map<List<StandardValueLogTemp>>(query).ToList();
        }

        public bool StandardValueExists(int id, string? target)
        {
            IEnumerable<object> result = (target == "main") ? _context.StandardValues : _context.StandardValuesLogTemp;

            return result.Any(data => _repositoryUtility.GetIdPropertyValue(data) == id);
        }

        public bool UpdateStandardValue(StandardValue data_)
        {
            _context.Update(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateStandardValueLogTemp(StandardValueLogTemp data_)
        {
            _context.Update(data_);
            return _repositoryUtility.SaveChanges();
        }
    }
}
