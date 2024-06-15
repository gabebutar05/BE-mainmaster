using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("Branch")]
    public class Branch
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string BranchCode { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string BranchName { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Address1 { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Address2 { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string ContactPerson { get; set; } = null!;
        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; } = null!;
        [Column(TypeName = "varchar(20)")]
        public string Fax { get; set; } = null!;
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
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        // Foreign key
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        // Foreign key
    }

    [Table("BranchLogTemp")]
    public class BranchLogTemp
    {
        public int Id { get; set; }
        public int BranchId { get; set; } = 0;
        public string BranchCode { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string BranchName { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Address1 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Address2 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string ContactPerson { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Fax { get; set; }
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
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
        // Foreign key
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        // Foreign key
    }

    [Table("BranchLog")]
    public class BranchLog
    {
        public int Id { get; set; }
        public int BranchId { get; set; } = 0;
        public int BranchTempId { get; set; } = 0;
        [Column(TypeName = "varchar(6)")]
        public string BranchCode { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string BranchName { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Address1 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Address2 { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string ContactPerson { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string Fax { get; set; }

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
        [Column(TypeName = "varchar(50)")]
        public string Remarks { get; set; } = "";
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
        // Foreign key
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        // Foreign key
    }
}
