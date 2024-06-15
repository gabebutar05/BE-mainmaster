namespace API_Dinamis.Dto
{
    public class AuthorizerList
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
    }

    public class AuthorizerTempList : AuthorizerList
    {
        public string ActionRemarks { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
    }

    public class AuthorizerInfo
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
