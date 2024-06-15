using API_Dinamis.Dto;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models.Authorizer
{
    [Table("Authorizer")]
    [Index(nameof(EmployeeId), IsUnique = true)]
    public class Authorizer
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }
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

    [Table("AuthorizerLogTemp")]
    public class AuthorizerLogTemp
    {
        public int Id { get; set; }
        public int AuthorizerId { get; set; }
        public int EmployeeId { get; set; }
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

    [Table("AuthorizerLog")]
    public class AuthorizerLog
    {
        public int Id { get; set; }
        public int AuthorizerId { get; set; }
        public int AuthorizerTempId { get; set; }
        public int EmployeeId { get; set; }
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
