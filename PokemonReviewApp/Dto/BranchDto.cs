using API_Dinamis.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Dto
{
    public class BranchDtoForm
    {
        public int ID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; } = null!;
        public string Address1 { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
    public class BranchDtoListTableJoin
    {
        public int ID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; } = null!;
        public string Address1 { get; set; } = null!;
        public string Address2 { get; set; } = null!;
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ZipCodeId { get; set; }
        public string ZipCode { get; set; }
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Fax { get; set; } = null!;
        public string UpdatedBy { get; set; }
        //public string Status { get; set; }
        public string Remarks { get; set; }
    }
    public class BranchDtoListTableJoinPending
    {
        public int ID { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; } = null!;
        public string Address1 { get; set; } = null!;
        public string Address2 { get; set; } = null!;
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int ZipCodeId { get; set; }
        public string ZipCode { get; set; }
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Fax { get; set; } = null!;
        public string UpdatedBy { get; set; }
        //public string Status { get; set; }
        public string Remarks { get; set; }
    }
    public class BranchDto
    {
        public int ID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; } = null!;
        public string Address1 { get; set; } = null!;
        public string Address2 { get; set; } = null!;
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Fax { get; set; } = null!;
        public string UpdatedBy { get; set; }
        //public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class BranchTestDto
    {
        public int ID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; } = null!;
        public string Address1 { get; set; } = null!;
        public string Address2 { get; set; } = null!;
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Fax { get; set; } = null!;
        public string UpdatedBy { get; set; }
        //public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class BranchDtoPending
    {
        public int ID { get; set; }
        public int BranchId { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Address1 { get; set; } = null!;
        public string Address2 { get; set; } = null!;
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Fax { get; set; } = null!;
        public string UpdatedBy { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string Remarks { get; set; }
        public string ActionRemarks { get; set; }
    }

    public class BranchDtoPendingUpdate
    {
        public int ID { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string Address1 { get; set; } = null!;
        public string Address2 { get; set; } = null!;
        public int CityId { get; set; }
        public int ZipCodeId { get; set; }
        public string ContactPerson { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Fax { get; set; } = null!;
        public string UpdatedBy { get; set; }
        public string Remarks { get; set; }
        public string ActionRemarks { get; set; }
    }
}
