using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface ILicenseRepository
    {
        ICollection<License> GetLicenses(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "");
        int Getdatacount();
        ICollection<License> GetLicensesAdvance(string keyword);
        bool LicenseExists(int id);
        License GetLicense(int id);
        ICollection<License> GetLicenses_all();
        ICollection<LicenseLogTemp> GetPendingLicenses_all();
        bool CreateLicenseLogTemp(LicenseLogTemp licenselogtemp_);
        bool CreateLicenseLog(LicenseLog licenselog_);
        bool UpdateLicense(License license_);
        bool LicenseTempExists(int id);
        LicenseLogTemp GetLicenseLogTemp(int id);
        bool DeleteLicenseLogTemp(LicenseLogTemp licenselogtemp_);
        bool UpdateLicenseTemp(LicenseLogTemp licenselogtemp_);
        bool CreateLicense(License license_); 
        bool DeleteLicense(License license_);
        ICollection<LicenseLogTemp> GetPendingLicenses(int limit, int page, string sortby, string sortdesc, string keyword);
        int Getpendingdatacount();
        ICollection<LicenseLogTemp> GetLicensesAdvancePending(string keyword); 
    }
}
