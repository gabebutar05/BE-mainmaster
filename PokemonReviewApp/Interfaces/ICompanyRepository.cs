using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface ICompanyRepository
    {
        CompanyDto GetCompany();
        CompanyTempDto GetCompanyTemp();
        Company GetBasicCompany();
        bool CreateCompany(Company data_);
        bool UpdateCompany(Company data_);
        bool CreateCompanyLogTemp(CompanyLogTemp datalogtemp_);
        bool UpdateCompanyLogTemp(CompanyLogTemp datalogtemp_);
        bool DeleteCompanyLogTemp(CompanyLogTemp datalogtemp_);
        bool CreateCompanyLog(CompanyLog datalog_);
    }
}
