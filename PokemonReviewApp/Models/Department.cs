using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("Department")]
    public class Department
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string DepartmentCode { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string DepartmentName { get; set; } = null!;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
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
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        // Foreign key
        public int BranchId { get; set; }
        // Foreign key
    }

    [Table("DepartmentLogTemp")]
    public class DepartmentLogTemp
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; } = 0;

        [Column(TypeName = "varchar(6)")]
        public string DepartmentCode { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string DepartmentName { get; set; } = null!;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
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
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
        // Foreign key
        public int BranchId { get; set; }
        // Foreign key
    }

    [Table("DepartmentLog")]
    public class DepartmentLog
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; } = 0;
        public int DepartmentTempId { get; set; } = 0;
        [Column(TypeName = "varchar(6)")]
        public string DepartmentCode { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string DepartmentName { get; set; } = null!;
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(50)")]
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
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
        // Foreign key
        public int BranchId { get; set; }
        // Foreign key
    }
}
