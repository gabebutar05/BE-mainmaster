using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("Employee")]
    public class Employee
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Initial { get; set; } = null!;
        public int Nationality { get; set; }
        public int IDType { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string IDNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime IDExpiredDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(16)")]
        public string NPWP { get; set; } = null!;
        [Column(TypeName = "varchar(16)")]
        public string BirthPlace { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public int Sex { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string Education { get; set; } = null!;
        public int MaritalStatus { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string MailAddress1 { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string MailAddress2 { get; set; } = null!;
        // Foreign key
        public int MailCityId { get; set; }
        public int MailZipCodeId { get; set; }
        // Foreign key
        [Column(TypeName = "varchar(50)")]
        public string HomeAddress1 { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string HomeAddress2 { get; set; } = null!;
        // Foreign key
        public int HomeCityId { get; set; }
        public int HomeZipCodeId { get; set; }
        // Foreign key
        [Column(TypeName = "varchar(15)")]
        public string Phone { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string OtherEmail { get; set; } = null!;
        // Foreign key
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        // Foreign key
        public int MarketingPosition { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Position { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "date")]
        public DateTime ResignDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(20)")]
        public string UserName { get; set; } = null!;
        public int AllowSaveToExcel { get; set; }
        public int PresDirectorXN1Signer { get; set; }
        public int WARPERDType { get; set; }
        public int WARPERDStatus { get; set; }
        public int HasPMPILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMPILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMPILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMIPWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMIPWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMIPWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMIPPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMIPPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMIPPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWPPELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWPPELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWPPELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWPPELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWPPELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWPPELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPPLWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PPLWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PPLWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
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
    }

    [Table("EmployeeLogTemp")]
    public class EmployeeLogTemp
    {
        public int Id { get; set; }
        public int EmployeeId {  get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Initial { get; set; } = null!;
        public int Nationality { get; set; }
        public int IDType { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string IDNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime IDExpiredDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(16)")]
        public string NPWP { get; set; } = null!;
        [Column(TypeName = "varchar(16)")]
        public string BirthPlace { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public int Sex { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string Education { get; set; } = null!;
        public int MaritalStatus { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string MailAddress1 { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string MailAddress2 { get; set; } = null!;
        // Foreign key
        public int MailCityId { get; set; }
        public int MailZipCodeId { get; set; }
        // Foreign key
        [Column(TypeName = "varchar(50)")]
        public string HomeAddress1 { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string HomeAddress2 { get; set; } = null!;
        // Foreign key
        public int HomeCityId { get; set; }
        public int HomeZipCodeId { get; set; }
        // Foreign key
        [Column(TypeName = "varchar(15)")]
        public string Phone { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string OtherEmail { get; set; } = null!;
        // Foreign key
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        // Foreign key
        public int MarketingPosition { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Position { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "date")]
        public DateTime ResignDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(20)")]
        public string UserName { get; set; } = null!;
        public int AllowSaveToExcel { get; set; }
        public int PresDirectorXN1Signer { get; set; }
        public int WARPERDType { get; set; }
        public int WARPERDStatus { get; set; }
        public int HasPMPILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMPILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMPILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMIPWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMIPWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMIPWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMIPPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMIPPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMIPPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWPPELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWPPELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWPPELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWPPELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWPPELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWPPELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPPLWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PPLWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PPLWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
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
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
    }

    [Table("EmployeeLog")]
    public class EmployeeLog
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeTempId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Initial { get; set; } = null!;
        public int Nationality { get; set; }
        public int IDType { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string IDNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime IDExpiredDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(16)")]
        public string NPWP { get; set; } = null!;
        [Column(TypeName = "varchar(16)")]
        public string BirthPlace { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;
        public int Sex { get; set; }
        [Column(TypeName = "varchar(16)")]
        public string Education { get; set; } = null!;
        public int MaritalStatus { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string MailAddress1 { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string MailAddress2 { get; set; } = null!;
        // Foreign key
        public int MailCityId { get; set; }
        public int MailZipCodeId { get; set; }
        // Foreign key
        [Column(TypeName = "varchar(50)")]
        public string HomeAddress1 { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string HomeAddress2 { get; set; } = null!;
        // Foreign key
        public int HomeCityId { get; set; }
        public int HomeZipCodeId { get; set; }
        // Foreign key
        [Column(TypeName = "varchar(15)")]
        public string Phone { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string OtherEmail { get; set; } = null!;
        // Foreign key
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        // Foreign key
        public int MarketingPosition { get; set; }
        [Column(TypeName = "varchar(25)")]
        public string Position { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime JoinDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "date")]
        public DateTime ResignDate { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "varchar(20)")]
        public string UserName { get; set; } = null!;
        public int AllowSaveToExcel { get; set; }
        public int PresDirectorXN1Signer { get; set; }
        public int WARPERDType { get; set; }
        public int WARPERDStatus { get; set; }
        public int HasPMPILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMPILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMPILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMIPWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMIPWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMIPWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWMILicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWMILicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWMILicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMIPPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMIPPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMIPPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWPEELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWPEELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWPEELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWPPELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWPPELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWPPELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWPPELicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWPPELicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWPPELicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPMWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PMWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PMWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasBLWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string BLWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime BLWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
        public int HasPPLWAPERDLicense { get; set; }
        [Column(TypeName = "varchar(20)")]
        public string PPLWAPERDLicenseNo { get; set; } = null!;
        [Column(TypeName = "date")]
        public DateTime PPLWAPERDLicenseIssuedDate { get; set; } = DateTime.UtcNow;
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
        [Column(TypeName = "varchar(50)")]
        public string ActionRemarks { get; set; } = "";
    }
}
