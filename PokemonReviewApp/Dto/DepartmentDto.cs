using API_Dinamis.Models;

namespace API_Dinamis.Dto
{
    public class DepartmentDtoForm
    {
        public int ID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; } = null!;
        public string BranchName { get; set; } = null!;
    }
    public class DepartmentDtoListTableJoin
    {
        public int ID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; } = null!;
        public int BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public string UpdatedBy { get; set; }
        public string Remarks { get; set; }
    }

    public class DepartmentDtoListTableJoinPending
    {
        public int ID { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; } = null!;
        public int BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public string UpdatedBy { get; set; }
        public string Remarks { get; set; }
    }
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int BranchId { get; set; }
        public string UpdatedBy { get; set; }
        public string Remarks { get; set; }
    }
    public class DepartmentDtoPendingUpdate
    {
        public int ID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int BranchId { get; set; }
        public string UpdatedBy { get; set; }
        public string Remarks { get; set; }
        public string ActionRemarks { get; set; }
    }

    public class DepartmentDtoPending
    {
        public int ID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int BranchId { get; set; }
        public string UpdatedBy { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string Remarks { get; set; }
        public string ActionRemarks { get; set; }
    }
}
