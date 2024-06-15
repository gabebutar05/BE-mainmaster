using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models
{
    [Table("Holiday")]
    [Index(nameof(HolidayDate), IsUnique = true)]
    public class Holiday
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        [Required]
        public DateTime HolidayDate { get; set; }

        [Column(TypeName = "varchar(125)")]
        [Required]
        public string Description { get; set; }

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

    [Table("HolidayLogTemp")]
    [Index(nameof(HolidayDate), IsUnique = true)]
    public class HolidayLogTemp
    {
        public int Id { get; set; }
        public int HolidayId { get; set; } = 0;

        [Column(TypeName = "date")]
        [Required]
        public DateTime HolidayDate { get; set; }

        [Column(TypeName = "varchar(125)")]
        [Required]
        public string Description { get; set; }

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

    [Table("HolidayLog")]
    public class HolidayLog
    {
        public int Id { get; set; }
        public int HolidayId { get; set; } = 0;
        public int HolidayTempId { get; set; } = 0;

        [Column(TypeName = "date")]
        [Required]
        public DateTime HolidayDate { get; set; }

        [Column(TypeName = "varchar(125)")]
        [Required]
        public string Description { get; set; }

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
