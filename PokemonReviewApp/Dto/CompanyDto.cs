using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Dto
{
    public class CompanyDto
    {
        public int ID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Npwp { get; set; }
        public DateTime NpwpDate { get; set; }
        public string KseiCode { get; set; }
        public string SinvestSaCode { get; set; }
        public string SinvestMiCode { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }

        /*foreign key regional start*/
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ZipCodeId { get; set; }
        public string ZipCode { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        /*foreign key regional end*/
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string Remarks { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class CompanyTempDto : CompanyDto
    {
        public int CompanyId { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string ActionRemarks { get; set; }
    }

    public class CompanyForm
    {
        public int ID { get; set; }
        public int? CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; } = null!;
        public string Npwp { get; set; } = null!;
        public DateTime NpwpDate { get; set; }
        public string KseiCode { get; set; } = null!;
        public string SinvestSaCode { get; set; } = null!;
        public string SinvestMiCode { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Address2 { get; set; } = "";

        /*foreign key regional start*/
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        public int CountryId { get; set; }
        /*foreign key regional end*/
        public string Phone { get; set; } = null!;
        public string? Fax { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? ContactPerson { get; set; } = "";
        public string? Remarks { get; set; } = "";
        public string UpdatedBy { get; set; } = null!;
        public string? Target { get; set; } = "main";
    }
}
