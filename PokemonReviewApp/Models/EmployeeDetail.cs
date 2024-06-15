using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models
{
    [Table("EmployeeDetail")]
    public class EmployeeDetail
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LicenseId { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string LicenseNo { get; set; }
        [Column(TypeName = "date")]
        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
        [Column(TypeName = "varchar(2)")]
        public string Status { get; set; } = "NA";
        //NA=need approve
        //A=approve
        //R=reject
        [Column(TypeName = "varchar(1)")]
        public string Action { get; set; } = "N";
        //N=no action
        //C=create
        //U=update
        //D=delete
        public string ActionDetail { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
    }

    [Table("EmployeeDetailLogTemp")]
    public class EmployeeDetailLogTemp
    {
        public int Id { get; set; }
        public int EmployeeDetailId { get; set; }
        public int EmployeeTempId { get; set; }
        public int EmployeeId { get; set; }
        public int LicenseId { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string LicenseNo { get; set; }
        [Column(TypeName = "date")]
        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
        [Column(TypeName = "varchar(1)")]
        public string Action { get; set; } = "N";
        //N=no action
        //C=create
        //U=update
        //D=delete
        [EnumDataType(typeof(ActionEnum))]
        public string ActionDetail { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        public string ActionRemarks { get; set; } = "";
    }

    [Table("EmployeeDetailLog")]
    public class EmployeeDetailLog
    {
        public int Id { get; set; }
        public int EmployeeDetailId { get; set; }
        public int EmployeeDetailTempId { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeTempId { get; set; }
        public int LicenseId { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string LicenseNo { get; set; }
        [Column(TypeName = "date")]
        public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        [Column(TypeName = "date")]
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
        [EnumDataType(typeof(ActionEnum))]
        public string ActionDetail { get; set; } = "N"; //N=no action //C=create //U=update //D=delete
        [Column(TypeName = "varchar(1)")]
        public string Action { get; set; } = "N";
        //N=no action
        //C=create
        //U=update
        //D=delete
        public string ActionRemarks { get; set; } = "";
    }
}
