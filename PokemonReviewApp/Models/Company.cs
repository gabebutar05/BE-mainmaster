using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models
{
    [Table("Company")]
    [Index(nameof(CompanyCode), IsUnique = true)]
    public class Company
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(6)")]
        public string CompanyCode { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string CompanyName { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string Npwp { get; set; } = null!;

        [Column(TypeName = "date")]
        [Required]
        public DateTime NpwpDate { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string KseiCode {  get; set; } = null!;

        [Column(TypeName = "varchar(15)")]
        public string SinvestSaCode { get; set; } = null!;

        [Column(TypeName = "varchar(15)")]
        public string SinvestMiCode { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string Address { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string? Address2 { get; set; } = "";

        /*foreign key regional start*/
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        public int CountryId { get; set; }
        /*foreign key regional end*/

        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? Fax { get; set; } = "";

        [Column(TypeName = "varchar(150)")]
        public string? Email { get; set; } = "";

        [Column(TypeName = "varchar(50)")]
        public string? ContactPerson { get; set; } = "";
        public string? Remarks { get; set; } = "";

        /* field for technical flag start */
        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; // NA=need approve, A=approve, R=reject

        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; // N=no action, C=create, U=update, D=delete

        /* field for technical flag end */
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
    }

    [Table("CompanyLogTemp")]
    [Index(nameof(CompanyCode), IsUnique = true)]
    public class CompanyLogTemp
    {
        public int Id { get; set; }
        public int CompanyId { get; set; } = 0;

        [Column(TypeName = "varchar(6)")]
        public string CompanyCode { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string CompanyName { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string Npwp { get; set; } = null!;

        [Column(TypeName = "date")]
        [Required]
        public DateTime NpwpDate { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string KseiCode { get; set; } = null!;

        [Column(TypeName = "varchar(15)")]
        public string SinvestSaCode { get; set; } = null!;

        [Column(TypeName = "varchar(15)")]
        public string SinvestMiCode { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string Address { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string? Address2 { get; set; } = "";

        /*foreign key regional start*/
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        public int CountryId { get; set; }
        /*foreign key regional end*/

        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? Fax { get; set; } = "";

        [Column(TypeName = "varchar(150)")]
        public string? Email { get; set; } = "";

        [Column(TypeName = "varchar(50)")]
        public string? ContactPerson { get; set; } = "";
        public string? Remarks { get; set; } = "";
        public string? ActionRemarks { get; set; } = "";

        /* field for technical flag start */
        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; // NA=need approve, A=approve, R=reject

        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; // N=no action, C=create, U=update, D=delete

        /* field for technical flag end */
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
    }

    [Table("CompanyLog")]
    public class CompanyLog
    {
        public int Id { get; set; }
        public int CompanyId { get; set; } = 0;
        public int CompanyTempId { get; set; } = 0;

        [Column(TypeName = "varchar(6)")]
        public string CompanyCode { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string CompanyName { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string Npwp { get; set; } = null!;

        [Column(TypeName = "date")]
        [Required]
        public DateTime NpwpDate { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string KseiCode { get; set; } = null!;

        [Column(TypeName = "varchar(15)")]
        public string SinvestSaCode { get; set; } = null!;

        [Column(TypeName = "varchar(15)")]
        public string SinvestMiCode { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string Address { get; set; } = null!;

        [Column(TypeName = "varchar(255)")]
        public string? Address2 { get; set; } = "";

        /*foreign key regional start*/
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        public int CountryId { get; set; }
        /*foreign key regional end*/

        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? Fax { get; set; } = "";

        [Column(TypeName = "varchar(150)")]
        public string? Email { get; set; } = "";

        [Column(TypeName = "varchar(50)")]
        public string? ContactPerson { get; set; } = "";
        public string? Remarks { get; set; } = "";
        public string? ActionRemarks { get; set; } = "";

        /* field for technical flag start */
        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; // NA=need approve, A=approve, R=reject

        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; // N=no action, C=create, U=update, D=delete

        /* field for technical flag end */
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
    }
}
