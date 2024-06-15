using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("License")]
    public class License
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string LicenseCode { get; set; } = null!;
        [Column(TypeName = "date")]
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
    }

    [Table("LicenseLogTemp")]
    public class LicenseLogTemp
    {
        public int Id { get; set; }
        public int LicenseId { get; set; } = 0;
        [Column(TypeName = "varchar(20)")]
        public string LicenseCode { get; set; } = null!;
        [Column(TypeName = "date")]
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
    }

    [Table("LicenseLog")]
    public class LicenseLog
    {
        public int Id { get; set; }
        public int LicenseId { get; set; } = 0;
        public int LicenseTempId { get; set; } = 0;
        [Column(TypeName = "varchar(20)")]
        public string LicenseCode { get; set; } = null!;
        [Column(TypeName = "date")]
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
    }
}
