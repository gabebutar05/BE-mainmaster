using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetRoles_all();
        RoleJoinDto GetRolesJoin(int id);
        ICollection<RoleDetailJoinDto> GetRolesDetailJoin(int roleid);
        ICollection<RoleDetailLogTemp> GetRoleDetailLogTemp_roleid(int roleid);
        Role GetRole(int id);
        RoleDetail GetRoleDetail_roleid(int roleid);
        RoleDetail GetRoleDetail(int id);
        ICollection<RoleDetail> GetRolesDetail(int roleid);
        //bool RoleExists(int cityId);
        //ICollection<RoleDetailJoinDto> GetRoleDetail_Test(int roleid);
        bool CreateRoleHeader(Role role_);
        bool CreateRoleHeaderLogTemp(RoleLogTemp role_);
        bool CreateRoleHeaderLog(RoleLog role_);
        bool CreateRoleDetail(RoleDetail roledetail_);
        bool CreateRoleDetailLogTemp(RoleDetailLogTemp roledetail_);
        bool CreateRoleDetailLog(RoleDetailLog roledetail_);
        bool RoleExists(int id);
        bool RoleTempExists(int id);
        RoleLogTemp GetRoleLogTemp(int id);
        bool DeleteRole(Role role_);
        bool DeleteRoleDetail(RoleDetail roledetail_);
        bool DeleteRoleLogTemp(RoleLogTemp rolelogtemp_);
        bool DeleteRoleDetailLogTemp(RoleDetailLogTemp roledetaillogtemp_);
        bool UpdateRole(Role role_);
        bool UpdateRoleDetail(RoleDetail role_);
        ICollection<RoleLogTemp> GetPendingRoles(int limit, int page, string sort, string sortdesc, string keyword);
        ICollection<Role> GetRoles(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "");
        int Getpendingdatacount();
        ICollection<RoleLogTemp> GetRolesAdvancePending(string keyword);
        RoleJoinDtoPending GetRolesJoinPending(int id);
        ICollection<RoleDetailJoinDtoPending> GetRolesDetailJoinPending(int id);
        bool RoleDetailExists(int id);
        public RoleDetailLogTemp GetRoleDetailLogTemp(int id);
        ICollection<RoleDetailLogTemp> GetRoleDetailLogTemp_(int RoleTempId);
        bool UpdateRoleDetailLogTemp(RoleDetailLogTemp roledetsaillogtemp_);
        bool UpdateRoleLogTemp(RoleLogTemp rolelogtemp_);
        int Getdatacount();
        ICollection<Role> GetRolesAdvance(string keyword);
        ICollection<RoleJoinDtoPending> GetPendingRoles_Join(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "");
    }
}
