using API_Dinamis.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models
{
    [Table("StandardValue")]
    [Index(nameof(DataName), IsUnique = true)]
    public class StandardValue
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(125)")]
        [Required]
        public string DataName { get; set; }
        public string? Description { get; set; } = "";

        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(ValueTypeEnum))]
        public string ValueType { get; set; } //N:Numeric, I:Integer, T:Text, O:Option, B: Bool, D: Date, DT: DateTime, P: Percentage
        public string? ValueOption { get; set; } = ""; //writing with ; as separator

        public string DataValue { get; set; }
        public bool? ValueInPercentage { get; set; } = false;
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

    [Table("StandardValueLogTemp")]
    [Index(nameof(DataName), IsUnique = true)]
    public class StandardValueLogTemp
    {
        public int Id { get; set; }
        public int DataId { get; set; } = 0;

        [Column(TypeName = "varchar(125)")]
        [Required]
        public string DataName { get; set; }
        public string? Description { get; set; } = "";

        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(ValueTypeEnum))]
        public string ValueType { get; set; } //N:Numeric, I:Integer, T:Text, O:Option, B: Bool, D: Date, DT: DateTime, P: Percentage
        public string? ValueOption { get; set; } = ""; //writing with ; as separator
        public string DataValue { get; set; }
        public bool? ValueInPercentage { get; set; } = false;
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

    [Table("StandardValueLog")]
    public class StandardValueLog
    {
        public int Id { get; set; }
        public int DataId { get; set; } = 0;
        public int DataTempId { get; set; } = 0;

        [Column(TypeName = "varchar(125)")]
        [Required]
        public string DataName { get; set; }
        public string? Description { get; set; } = "";

        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(ValueTypeEnum))]
        public string ValueType { get; set; } //N:Numeric, I:Integer, T:Text, O:Option, B: Bool, D: Date, DT: DateTime,  P: Percentage
        public string? ValueOption { get; set; } = ""; //writing with ; as separator
        public string DataValue { get; set; }
        public bool? ValueInPercentage { get; set; } = false;
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
