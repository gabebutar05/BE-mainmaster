using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IDepartmentRepository
    {
        ICollection<Department> GetDepartments(int limit, int page, string sort, string sortdesc, string keyword);
        ICollection<DepartmentLogTemp> GetPendingDepartments(int limit, int page, string sort, string sortdesc, string keyword);
        ICollection<Department> GetDepartments_all();
        ICollection<DepartmentLogTemp> GetPendingDepartments_all();
        ICollection<DepartmentDtoListTableJoin> DepartmentDtoListTableJoin_all();
        DepartmentDtoListTableJoin DepartmentDtoTableJoin(int id);
        DepartmentDtoListTableJoinPending DepartmentDtoTableJoinPending(int id);
        ICollection<Department> GetDepartmensAdvance(string keyword);
        ICollection<DepartmentLogTemp> GetDepartmentsAdvancePending(string keyword);
        Department GetDepartment(int id);
        DepartmentLogTemp GetDepartmentLogTemp(int id);
        bool CreateDepartment(Department department_);
        bool CreateDepartmentLog(DepartmentLog departmentlog_);
        bool CreateDepartmentLogTemp(DepartmentLogTemp departmentlogtemp_);
        bool UpdateDepartment(Department department_);
        bool UpdateDepartmentTemp(DepartmentLogTemp departmentlogtemp_);
        bool DeleteDepartment(Department department_);
        bool DeleteDepartmentLogTemp(DepartmentLogTemp departmentlogtemp_);
        bool DepartmentExists(int id);
        bool DepartmentTempExists(int id);
        bool Save();
        int Getdatacount();
        int Getpendingdatacount();
    }
}
