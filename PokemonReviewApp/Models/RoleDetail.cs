using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models
{
    [Table("RoleDetail")]
    public class RoleDetail
    {
        public int Id { get; set; }
        public bool Run { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string UpdatedBy { get; set; } = "";
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        [EnumDataType(typeof(ActionEnum))]
        public string ActionDetail { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        // Foreign key
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        // Foreign key
    }

    [Table("RoleDetailLogTemp")]
    public class RoleDetailLogTemp
    {
        public int Id { get; set; }
        public bool Run { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string UpdatedBy { get; set; } = "";
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        [EnumDataType(typeof(ActionEnum))]
        public string ActionDetail { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        // Foreign key
        public int RoleId { get; set; }
        public int RoleTempId { get; set; }
        public int RoleDetailId { get; set; }
        public int MenuId { get; set; }
        // Foreign key
    }

    [Table("RoleDetailLog")]
    public class RoleDetailLog
    {
        public int Id { get; set; }
        public bool Run { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string UpdatedBy { get; set; } = "";
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        [EnumDataType(typeof(ActionEnum))]
        public string ActionDetail { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        // Foreign key
        public int RoleId { get; set; }
        public int RoleTempId { get; set; }
        public int RoleDetailId { get; set; }
        public int RoleDetailTempId { get; set; }
        public int MenuId { get; set; }
        // Foreign key
    }
}
