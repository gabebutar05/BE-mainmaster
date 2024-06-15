using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API_Dinamis.Helper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.AspNetCore.Mvc;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Utilities;

namespace API_Dinamis.Repository
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DataContext _context;
        private readonly RepositoryUtils _repositoryUtility;
        public BranchRepository(DataContext context, RepositoryUtils repositoryUtility)
        {
            _context = context;
            _repositoryUtility = repositoryUtility;
        }

        public ICollection<Branch> GetBranches2(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "")
        {
            IQueryable<Branch> query = _context.Branches;

            if (!string.IsNullOrEmpty(keyword))
            {
                var key = "%" + keyword + "%";
                query = query.Where(branch => EF.Functions.Like(branch.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(branch.BranchCode.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            if (sortby == "d")
            {
                //query = query.OrderByDescending(GetOrderByExpression(sortdesc));
                query = query.OrderByDescending(RepositoryUtils.GetOrderByExpression<Branch>(sortdesc));
            }
            else
            {
                query = query.OrderBy(GetOrderByExpression(sortdesc));
            }
            

            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var result = query.ToList();
            return result;
        }

        public ICollection<Branch> GetBranches(int limit, int page, string sortby, string sortdesc, string keyword)
        {
            //return _context.Branches.OrderBy(p => p.Id).ToList();

            int offset = 0;

            //if (keyword == "-99")
            //{
            //    keyword = "";
            //}

            var key = "%" + keyword + "%";
            var dynamicOrder = sortdesc;
            var query = _context.Branches
                                .Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpression(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();

            if ((limit <= 0 || limit == null) && (page <= 0 || page == null))
            {
                offset = 0; //default
                limit = 10; //default
            }
            else if (page > 1)
            {
                offset = (page - 1) * limit;
            }

            if (sortby == "d")
            {
                query = _context.Branches
                                .Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderByDescending(GetOrderByExpression(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }
            else if (sortby == "a")
            {
                query = _context.Branches
                                .Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpression(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }


            return query;
        }

        //private Expression<Func<Branch, object>> GetOrderByExpression()
        //{
        //    throw new NotImplementedException();
        //}

        public Branch GetBranch(int id)
        {
            return _context.Branches.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Branch> GetBranchesAdvance(string keyword)
        {
            var key = "%" + keyword + "%";
            //return _context.Branches.Where(p => p.BranchName.Trim().ToUpper() == keyword.Trim().ToUpper()).ToList();
            return _context.Branches.Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper())).ToList();
        }

        // || p.BranchNo == Convert.ToDecimal(keyword)

        public bool CreateBranch(Branch branch_)
        {
            _context.Add(branch_);
            return _repositoryUtility.SaveChanges();
            //return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool BranchExists(int id)
        {
            return _context.Branches.Any(p => p.Id == id);
        }

        public bool BranchTempExists(int id)
        {
            return _context.BranchesLogTemp.Any(p => p.Id == id);
        }

        public bool UpdateBranch(Branch branch_)
        {
            _context.Update(branch_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteBranch(Branch branch_)
        {
            _context.Remove(branch_);
            return _repositoryUtility.SaveChanges();
        }

        public int Getdatacount()
        {
            return _context.Branches.Count();
        }

        public ICollection<Branch> GetBranches_all()
        {
            return _context.Branches.OrderBy(p => p.Id).ToList();
        }

        public bool CreateBranchLog(BranchLog branchlog_)
        {
            _context.Add(branchlog_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateBranchLogTemp(BranchLogTemp branchlogtemp_)
        {
            _context.Add(branchlogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteBranchLogTemp(BranchLogTemp branchlogtemp_)
        {
            _context.Remove(branchlogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public BranchLogTemp GetBranchLogTemp(int id)
        {
            return _context.BranchesLogTemp.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<BranchLogTemp> GetPendingBranches_all()
        {
            return _context.BranchesLogTemp.OrderBy(p => p.Id).ToList();
        }
        public ICollection<BranchLogTemp> GetPendingBranches(int limit, int page, string sortby, string sortdesc, string keyword)
        {
            int offset = 0;

            //if (keyword == "-99")
            //{
            //    keyword = "";
            //}

            var key = "%" + keyword + "%";
            var dynamicOrder = sortdesc;

            //COMMENT DL MAO JOIN TABLE
            //ini yang asli
            //var query = _context.BranchesLogTemp
            //                    .Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper()))
            //                    .OrderBy(GetOrderByExpressionPending(dynamicOrder))
            //                    .Skip(offset)
            //                    .Take(limit)
            //                    .ToList();
            //COMMENT DL MAO JOIN TABLE

            var query = _context.BranchesLogTemp
                                .Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();



            if ((limit <= 0 || limit == null) && (page <= 0 || page == null))
            {
                offset = 0; //default
                limit = 10; //default
            }
            else if (page > 1)
            {
                offset = (page - 1) * limit;
            }

            if (sortby == "d")
            {
                query = _context.BranchesLogTemp
                                .Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderByDescending(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }
            else if (sortby == "a")
            {
                query = _context.BranchesLogTemp
                                .Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }

            return query;
            //return _context.BranchesLogTemp.OrderBy(p => p.Id).ToList();
        }

        public int Getpendingdatacount()
        {
            return _context.BranchesLogTemp.Count();
        }

        public bool UpdateBranchTemp(BranchLogTemp branchlogtemp_)
        {
            _context.Update(branchlogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        private static Expression<Func<Branch, object>> GetOrderByExpression(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(Branch), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<Branch, object>>(Expression.Convert(property, typeof(object)), parameter);

            return lambda;
        }

        private static Expression<Func<BranchLogTemp, object>> GetOrderByExpressionPending(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(BranchLogTemp), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<BranchLogTemp, object>>(Expression.Convert(property, typeof(object)), parameter);

            return lambda;
        }

        //private static Expression<Func<BranchDtoListTableJoin, object>> GetOrderByExpressionPending(string propertyName)
        //{
        //    var parameter = Expression.Parameter(typeof(BranchDtoListTableJoin), "x");
        //    var property = Expression.Property(parameter, propertyName);
        //    var lambda = Expression.Lambda<Func<BranchDtoListTableJoin, object>>(Expression.Convert(property, typeof(object)), parameter);

        //    return lambda;
        //}

        public ICollection<BranchLogTemp> GetBranchesAdvancePending(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.BranchesLogTemp.Where(p => EF.Functions.Like(p.BranchName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.BranchCode.Trim().ToUpper(), key.Trim().ToUpper())).ToList();

        }

        public bool CreateBranchTest(Branch branch_)
        {
            _context.Add(branch_);
            return _repositoryUtility.SaveChanges();
        }

        public ICollection<BranchDtoListTableJoin> BranchDtoListTableJoin_all()
        {
            var result = _context.Branches
            .Join(
                _context.Cities,
                branch => branch.CityId, city => city.Id,
                (branch, city) => new { branch, city = city.Id, cityname = city.CityName })
            .Join(
                _context.Zipcodes,
                combined => combined.branch.ZipCodeId,
                zipcode => zipcode.Id,
                (combined, zipcode) => new BranchDtoListTableJoin
                {
                    ID = combined.branch.Id,
                    BranchCode = combined.branch.BranchCode,
                    BranchName = combined.branch.BranchName,
                    Address1 = combined.branch.Address1,
                    Address2 = combined.branch.Address2,
                    CityId = combined.branch.CityId,
                    CityName = combined.cityname,
                    ZipCodeId = combined.branch.Id,
                    ZipCode = zipcode.ZipCode,
                    ContactPerson = combined.branch.ContactPerson,
                    Phone = combined.branch.Phone,
                    Fax = combined.branch.Fax,
                    UpdatedBy = combined.branch.UpdatedBy,
                    Remarks = combined.branch.Remarks,
                }
                )
            .ToList();

            //return _context.Branches.OrderBy(p => p.Id).ToList();
            return result;
        }

        public BranchDtoListTableJoin BranchDtoTableJoin(int id)
        {
            var result = _context.Branches
            .Join(
                _context.Cities,
                branch => branch.CityId, city => city.Id,
                (branch, city) => new { branch, city = city.Id, cityname = city.CityName })
            .Join(
                _context.Zipcodes,
                combined => combined.branch.ZipCodeId,
                zipcode => zipcode.Id,
                (combined, zipcode) => new BranchDtoListTableJoin
                {
                    ID = combined.branch.Id,
                    BranchCode = combined.branch.BranchCode,
                    BranchName = combined.branch.BranchName,
                    Address1 = combined.branch.Address1,
                    Address2 = combined.branch.Address2,
                    CityId = combined.branch.CityId,
                    CityName = combined.cityname,
                    ZipCodeId = combined.branch.Id,
                    ZipCode = zipcode.ZipCode,
                    ContactPerson = combined.branch.ContactPerson,
                    Phone = combined.branch.Phone,
                    Fax = combined.branch.Fax,
                    UpdatedBy = combined.branch.UpdatedBy,
                    Remarks = combined.branch.Remarks,
                }
                )
            .Where(p => p.ID == id)
            .FirstOrDefault();

            return result;
        }

        public BranchDtoListTableJoinPending BranchDtoTableJoinPending(int id)
        {
            var result = _context.BranchesLogTemp
            .Join(
                _context.Cities,
                branch => branch.CityId, city => city.Id,
                (branch, city) => new { branch, city = city.Id, cityname = city.CityName })
            .Join(
                _context.Zipcodes,
                combined => combined.branch.ZipCodeId,
                zipcode => zipcode.Id,
                (combined, zipcode) => new BranchDtoListTableJoinPending
                {
                    ID = combined.branch.Id,
                    BranchId = combined.branch.BranchId,
                    BranchCode = combined.branch.BranchCode,
                    BranchName = combined.branch.BranchName,
                    Address1 = combined.branch.Address1,
                    Address2 = combined.branch.Address2,
                    CityId = combined.branch.CityId,
                    CityName = combined.cityname,
                    ZipCodeId = combined.branch.Id,
                    ZipCode = zipcode.ZipCode,
                    ContactPerson = combined.branch.ContactPerson,
                    Phone = combined.branch.Phone,
                    Fax = combined.branch.Fax,
                    UpdatedBy = combined.branch.UpdatedBy,
                    Remarks = combined.branch.Remarks,
                }
                )
            .Where(p => p.ID == id)
            .FirstOrDefault();

            return result;
        }
    }
}