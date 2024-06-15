using API_Dinamis.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Dto
{
    public class LicenseDtoForm
    {
        public int ID { get; set; }
        public string LicenseCode { get; set; }
    }

    public class LicenseDto
    {
        public int ID { get; set; }
        public string LicenseCode { get; set; }
        public string UpdatedBy { get; set; }
        //public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class LicenseDtoPendingUpdate
    {
        public int ID { get; set; }
        public string LicenseCode { get; set; }
        public string UpdatedBy { get; set; }
        public string Remarks { get; set; }
        public string ActionRemarks { get; set; }
    }

    public class LicenseDtoPending
    {
        public int ID { get; set; }
        public int LicenseId { get; set; }
        public string LicenseCode { get; set; }
        public string UpdatedBy { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string Remarks { get; set; }
        public string ActionRemarks { get; set; }
    }
}
