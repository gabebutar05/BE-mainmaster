namespace API_Dinamis.Dto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public int Nationality { get; set; }
        public int IDType { get; set; }
        public string Remarks { get; set; }
    }

    public class EmployeeDtoForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public int Nationality { get; set; }
        public int IDType { get; set; }
        public string Remarks { get; set; }
    }

    public class EmployeeResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public int Nationality { get; set; }
        public int IDType { get; set; }
        public string IDNo { get; set; }
        public DateTime IDExpiredDate { get; set; }
        public string NPWP { get; set; }
        public string BirthPlace { get; set; }
        public DateTime BirthDate { get; set; }
        public int Sex { get; set; }
        public string Education { get; set; }
        public int MaritalStatus { get; set; }
        public string MailAddress1 { get; set; }
        public string MailAddress2 { get; set; }
        public int MailCityId { get; set; }
        public int MailZipCodeId { get; set; }
        public string HomeAddress1 { get; set; }
        public string HomeAddress2 { get; set; }
        public int HomeCityId { get; set; }
        public int HomeZipCodeId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string OtherEmail { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int MarketingPosition { get; set; }
        public string Position { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime ResignDate { get; set; }
        public string UserName { get; set; }
        public int AllowSaveToExcel { get; set; }
        public int PresDirectorXN1Signer { get; set; }
        public int WARPERDType { get; set; }
        public int WARPERDStatus { get; set; }
        public int HasPMPILicense { get; set; }
        public string PMPILicenseNo { get; set; }
        public DateTime PMPILicenseIssuedDate { get; set; }
        public int HasPMIPWMILicense { get; set; }
        public string PMIPWMILicenseNo { get; set; }
        public DateTime PMIPWMILicenseIssuedDate { get; set; }
        public int HasPMWMILicense { get; set; }
        public string PMWMILicenseNo { get; set; }
        public DateTime PMWMILicenseIssuedDate { get; set; }
        public int HasBLWMILicense { get; set; }
        public string BLWMILicenseNo { get; set; }
        public DateTime BLWMILicenseIssuedDate { get; set; }
        public int HasPMIPPEELicense { get; set; }
        public string PMIPPEELicenseNo { get; set; }
        public DateTime PMIPPEELicenseIssuedDate { get; set; }
        public int HasPMWPEELicense { get; set; }
        public string PMWPEELicenseNo { get; set; }
        public DateTime PMWPEELicenseIssuedDate { get; set; }
        public int HasBLWPEELicense { get; set; }
        public string BLWPEELicenseNo { get; set; }
        public DateTime BLWPEELicenseIssuedDate { get; set; }
        public int HasPMWPPELicense { get; set; }
        public string PMWPPELicenseNo { get; set; }
        public DateTime PMWPPELicenseIssuedDate { get; set; }
        public int HasBLWPPELicense { get; set; }
        public string BLWPPELicenseNo { get; set; }
        public DateTime BLWPPELicenseIssuedDate { get; set; }
        public int HasPMWAPERDLicense { get; set; }
        public string PMWAPERDLicenseNo { get; set; }
        public DateTime PMWAPERDLicenseIssuedDate { get; set; }
        public int HasBLWAPERDLicense { get; set; }
        public string BLWAPERDLicenseNo { get; set; }
        public DateTime BLWAPERDLicenseIssuedDate { get; set; }
        public int HasPPLWAPERDLicense { get; set; }
        public string PPLWAPERDLicenseNo { get; set; }
        public DateTime PPLWAPERDLicenseIssuedDate { get; set; }
        public string Remarks { get; set; }
        public virtual ICollection<EmployeeDetailDto> detail { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class EmployeeDetailDto
    {
        public int Id { get; set; }
        public int EmployeId { get; set; }
        public int LicenseId { get; set; }
        public string LicenseNo { get; set; }
        public DateTime IssuedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string ActionDetail { get; set; }
    }

    public class EmployeeJoinDtoPending
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public int Nationality { get; set; }
        public int IDType { get; set; }
    }

    public class EmployeeDetailJoinDtoPending
    {
        public int Id { get; set; }
        public int EmployeeTempId { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeDetailId { get; set; }
        public int LicenseId { get; set; }
        public string LicenseNo { get; set; }
        public string Action { get; set; }
        public string ActionRemarks { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class EmployeeResultJoinDtoPending
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public virtual ICollection<EmployeeDetailJoinDtoPending> detail { get; set; }
    }

    public class EmployeeResultJoinDtoPendingUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public string Remarks { get; set; }
        public string ActionRemarks { get; set; }
        public string Action { get; set; }
        public virtual ICollection<EmployeeDetailJoinDtoPending> detail { get; set; }
    }
}
