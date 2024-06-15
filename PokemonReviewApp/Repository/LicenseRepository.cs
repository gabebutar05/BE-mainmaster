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
using System.Globalization;

namespace API_Dinamis.Repository
{
    public class LicenseRepository : ILicenseRepository
    {
        private readonly DataContext _context;
        private readonly RepositoryUtils _repositoryUtility;
        public LicenseRepository(DataContext context, RepositoryUtils repositoryUtility)
        {
            _context = context;
            _repositoryUtility = repositoryUtility;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateLicenseLog(LicenseLog licenselog_)
        {
            _context.Add(licenselog_);
            return _repositoryUtility.SaveChanges();
            //return Save();
        }

        public bool CreateLicenseLogTemp(LicenseLogTemp licenselogtemp_)
        {
            _context.Add(licenselogtemp_);
            return _repositoryUtility.SaveChanges();
            //return Save();
        }

        public int Getdatacount()
        {
            return _context.Licenses.Count();
        }

        public ICollection<License> GetLicenses(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "")
        {
            IQueryable<License> query = _context.Licenses;

            if (!string.IsNullOrEmpty(keyword))
            {
                var key = "%" + keyword + "%";
                query = query.Where(license => EF.Functions.Like(license.LicenseCode.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            if (sortby == "d")
            {
                query = query.OrderByDescending(RepositoryUtils.GetOrderByExpression<License>(sortdesc));
            }
            else
            {
                query = query.OrderBy(RepositoryUtils.GetOrderByExpression<License>(sortdesc));
            }


            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var result = query.ToList();
            return result;
        }

        public ICollection<License> GetLicensesAdvance(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.Licenses.Where(p => EF.Functions.Like(p.LicenseCode.Trim().ToUpper(), key.Trim().ToUpper())).ToList();

        }

        public ICollection<License> GetLicenses_all()
        {
            return _context.Licenses.OrderBy(p => p.Id).ToList();
        }

        public License GetLicense(int id)
        {
            return _context.Licenses.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<LicenseLogTemp> GetPendingLicenses_all()
        {
            return _context.LicensesLogTemp.OrderBy(p => p.Id).ToList();
        }

        public bool LicenseExists(int id)
        {
            return _context.Licenses.Any(p => p.Id == id);
        }

        public bool UpdateLicense(License license_)
        {
            _context.Update(license_);
            return _repositoryUtility.SaveChanges();
        }

        public bool LicenseTempExists(int id)
        {
            return _context.LicensesLogTemp.Any(p => p.Id == id);
        }

        public LicenseLogTemp GetLicenseLogTemp(int id)
        {
            return _context.LicensesLogTemp.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool DeleteLicenseLogTemp(LicenseLogTemp licenselogtemp_)
        {
            _context.Remove(licenselogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateLicenseTemp(LicenseLogTemp licenselogtemp_)
        {
            _context.Update(licenselogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateLicense(License license_)
        {
            _context.Add(license_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteLicense(License license_)
        {
            _context.Remove(license_);
            return _repositoryUtility.SaveChanges();
        }

        public ICollection<LicenseLogTemp> GetPendingLicenses(int limit, int page, string sortby, string sortdesc, string keyword)
        {
            int offset = 0;

            var key = "%" + keyword + "%";
            var dynamicOrder = sortdesc;

            var query = _context.LicensesLogTemp
                                .Where(p => EF.Functions.Like(p.LicenseCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(RepositoryUtils.GetOrderByExpression<LicenseLogTemp>(dynamicOrder))
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
                query = _context.LicensesLogTemp
                                .Where(p => EF.Functions.Like(p.LicenseCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderByDescending(RepositoryUtils.GetOrderByExpression<LicenseLogTemp>(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }
            else if (sortby == "a")
            {
                query = _context.LicensesLogTemp
                                .Where(p => EF.Functions.Like(p.LicenseCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(RepositoryUtils.GetOrderByExpression<LicenseLogTemp>(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }

            return query;
        }

        public int Getpendingdatacount()
        {
            return _context.LicensesLogTemp.Count();
        }

        public ICollection<LicenseLogTemp> GetLicensesAdvancePending(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.LicensesLogTemp.Where(p => EF.Functions.Like(p.LicenseCode.Trim().ToUpper(), key.Trim().ToUpper())).ToList();
        }
    }
}
