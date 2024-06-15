using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models
{
    [Table("Role")]
    public class Role
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; //NA=need approve //A=approve //R=reject
        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        [Column(TypeName = "varchar(50)")]
        public string remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string UpdatedBy { get; set; } = null!;
        // Foreign key
        public int ModuleId { get; set; }
        // Foreign key
    }

    [Table("RoleLogTemp")]
    public class RoleLogTemp
    {
        public int Id { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = null!;
        
        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; //NA=need approve //A=approve //R=reject
        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string UpdatedBy { get; set; } = null!;
        // Foreign key
        public int ModuleId { get; set; }
        public int RoleId { get; set; }
        // Foreign key
    }

    [Table("RoleLog")]
    public class RoleLog
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = null!;

        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; //NA=need approve //A=approve //R=reject
        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string UpdatedBy { get; set; } = null!;
        // Foreign key
        public int ModuleId { get; set; }
        public int RoleId { get; set; }
        public int RoleTempId { get; set; }
        // Foreign key
    }
}
