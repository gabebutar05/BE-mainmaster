using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IBranchRepository
    {
        ICollection<Branch> GetBranches(int limit, int page, string sort, string sortdesc, string keyword);
        ICollection<Branch> GetBranches2(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "");
        ICollection<BranchLogTemp> GetPendingBranches(int limit, int page, string sort, string sortdesc, string keyword);
        ICollection<BranchLogTemp> GetPendingBranches_all();
        ICollection<Branch> GetBranches_all();
        ICollection<BranchDtoListTableJoin> BranchDtoListTableJoin_all();
        BranchDtoListTableJoin BranchDtoTableJoin(int id);
        BranchDtoListTableJoinPending BranchDtoTableJoinPending(int id);
        ICollection<Branch> GetBranchesAdvance(string keyword);
        ICollection<BranchLogTemp> GetBranchesAdvancePending(string keyword);
        Branch GetBranch(int id);
        BranchLogTemp GetBranchLogTemp(int id);
        bool CreateBranch(Branch branch_);
        bool CreateBranchTest(Branch branch_);
        bool CreateBranchLog(BranchLog branchlog_);
        bool CreateBranchLogTemp(BranchLogTemp branchlogtemp_);
        bool UpdateBranch(Branch branch_);
        bool UpdateBranchTemp(BranchLogTemp branchlogtemp_);
        bool DeleteBranch(Branch branch_);
        bool DeleteBranchLogTemp(BranchLogTemp branchlogtemp_);
        bool BranchExists(int id);
        bool BranchTempExists(int id);
        bool Save();
        int Getdatacount();
        int Getpendingdatacount();
    }
}
