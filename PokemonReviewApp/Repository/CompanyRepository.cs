using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API_Dinamis.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly RepositoryUtils _repositoryUtility;
        public CompanyRepository(DataContext context, IMapper mapper, RepositoryUtils repositoryUtility)
        {
            _context = context;
            _mapper = mapper;
            _repositoryUtility = repositoryUtility;
        }

        public bool CreateCompany(Company data_)
        {
            _context.Add(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateCompanyLog(CompanyLog datalog_)
        {
            _context.Add(datalog_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateCompanyLogTemp(CompanyLogTemp datalogtemp_)
        {
            _context.Add(datalogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteCompanyLogTemp(CompanyLogTemp datalogtemp_)
        {
            _context.Remove(datalogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public Company GetBasicCompany()
        {
            return _context.Companies.FirstOrDefault();
        }

        public CompanyDto GetCompany()
        {
            var company = _context.Companies
                .OrderByDescending(company => company.Id)
                .Join(_context.Cities, company => company.CityId, city => city.Id, (company, city) => new { company, city = city.Id, cityname = city.CityName })
                .Join(_context.Zipcodes, combined => combined.company.ZipCodeId, zipcode => zipcode.Id, (combined, zipcode) => new { combined, zipcode })
                .Join(
                    _context.Countries,
                    combinedZip => combinedZip.combined.company.CountryId,
                    country => country.Id,
                    (combinedZip, country) => new CompanyDto
                    {
                        ID = combinedZip.combined.company.Id,
                        CompanyCode = combinedZip.combined.company.CompanyCode,
                        CompanyName = combinedZip.combined.company.CompanyName,
                        Npwp = combinedZip.combined.company.Npwp,
                        NpwpDate = combinedZip.combined.company.NpwpDate,
                        KseiCode = combinedZip.combined.company.KseiCode,
                        SinvestSaCode = combinedZip.combined.company.SinvestSaCode,
                        SinvestMiCode = combinedZip.combined.company.SinvestMiCode,
                        Address = combinedZip.combined.company.Address,
                        Address2 = combinedZip.combined.company.Address2,
                        CityId = combinedZip.combined.company.CityId,
                        CityName = combinedZip.combined.cityname,
                        ZipCodeId = combinedZip.combined.company.ZipCodeId,
                        ZipCode = combinedZip.zipcode.ZipCode,
                        CountryId = combinedZip.combined.company.CountryId,
                        CountryCode = country.CountryCode,
                        CountryName = country.CountryName,
                        Phone = combinedZip.combined.company.Phone,
                        Fax = combinedZip.combined.company.Fax,
                        Email = combinedZip.combined.company.Email,
                        ContactPerson = combinedZip.combined.company.ContactPerson,
                        LastUpdate = combinedZip.combined.company.LastUpdate,
                        UpdatedBy = combinedZip.combined.company.UpdatedBy,
                    })
                .FirstOrDefault();
            return company;
        }

        public CompanyTempDto GetCompanyTemp()
        {
            var company = _context.CompaniesLogTemp
                .OrderByDescending(company => company.Id)
                .Join(_context.Cities, company => company.CityId, city => city.Id, (company, city) => new { company, city = city.Id, cityname = city.CityName })
                .Join(_context.Zipcodes, combined => combined.company.ZipCodeId, zipcode => zipcode.Id, (combined, zipcode) => new { combined, zipcode })
                .Join(
                    _context.Countries,
                    combinedZip => combinedZip.combined.company.CountryId,
                    country => country.Id,
                    (combinedZip, country) => new CompanyTempDto
                    {
                        ID = combinedZip.combined.company.Id,
                        CompanyId = combinedZip.combined.company.CompanyId,
                        CompanyCode = combinedZip.combined.company.CompanyCode,
                        CompanyName = combinedZip.combined.company.CompanyName,
                        Npwp = combinedZip.combined.company.Npwp,
                        NpwpDate = combinedZip.combined.company.NpwpDate,
                        KseiCode = combinedZip.combined.company.KseiCode,
                        SinvestSaCode = combinedZip.combined.company.SinvestSaCode,
                        SinvestMiCode = combinedZip.combined.company.SinvestMiCode,
                        Address = combinedZip.combined.company.Address,
                        Address2 = combinedZip.combined.company.Address2,
                        CityId = combinedZip.combined.company.CityId,
                        CityName = combinedZip.combined.cityname,
                        ZipCodeId = combinedZip.combined.company.ZipCodeId,
                        ZipCode = combinedZip.zipcode.ZipCode,
                        CountryId = combinedZip.combined.company.CountryId,
                        CountryCode = country.CountryCode,
                        CountryName = country.CountryName,
                        Phone = combinedZip.combined.company.Phone,
                        Fax = combinedZip.combined.company.Fax,
                        Email = combinedZip.combined.company.Email,
                        ContactPerson = combinedZip.combined.company.ContactPerson,
                        LastUpdate = combinedZip.combined.company.LastUpdate,
                        UpdatedBy = combinedZip.combined.company.UpdatedBy,
                        Status = combinedZip.combined.company.Status,
                        Action = combinedZip.combined.company.Action,
                    })
                .FirstOrDefault();
            return company;
        }

        public bool UpdateCompany(Company data_)
        {
            _context.Update(data_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateCompanyLogTemp(CompanyLogTemp datalogtemp_)
        {
            _context.Update(datalogtemp_);
            return _repositoryUtility.SaveChanges();
        }
    }
}
