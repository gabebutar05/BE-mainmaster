namespace API_Dinamis.Models
{
    public class StatusIdList
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Approval { get; set; }
        public string ActionRemarks { get; set; }
        public List<string> IdList { get; set; }
    }

    public class SingleDataApproval
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Approval { get; set; }
        public string ActionRemarks { get; set; }
    }
}
