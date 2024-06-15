using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IEmployeeRepository
    {
        ICollection<Employee> GetEmployees_all();
        ICollection<Employee> GetEmployees(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "");
        int Getdatacount();
        ICollection<Employee> GetEmployeesAdvance(string keyword);
        Employee GetEmployee(int id);
        bool EmployeeExists(int id);
        ICollection<EmployeeLogTemp> GetPendingemployees_all();
        bool CreateEmployeeHeaderLogTemp(EmployeeLogTemp employeelogtemp_);
        bool CreateEmployeeHeaderLog(EmployeeLog Employeelog_);
        bool CreateEmployeeDetailLogTemp(EmployeeDetailLogTemp employeedetaillogtemp_);
        bool CreateEmployeeDetailLog(EmployeeDetailLog employeedetail_);
        bool UpdateEmployee(Employee employee_);
        bool UpdateEmployeeDetail(EmployeeDetail employee_);
        ICollection<EmployeeDetail> GetEmployeesDetail(int employeeid);
        EmployeeDetail GetEmployeeDetail (int id);
        bool EmployeeTempExists(int id);
        EmployeeLogTemp GetEmployeeLogTemp (int id);
        ICollection<EmployeeDetailLogTemp> GetEmployeeDetailLogTemp_employeeid(int employeeid);
        bool CreateEmployeeHeader(Employee employee_);
        bool CreateEmployeeDetail(EmployeeDetail employeedetail_);
        EmployeeDetail GetEmployeeDetail_employeeid(int employeeid);
        bool DeleteEmployeeDetail (EmployeeDetail employeedetail_);
        bool DeleteEmployee (Employee employee_);
        bool DeleteEmployeeDetailLogTemp (EmployeeDetailLogTemp employeedetaillogtemp_);
        bool DeleteEmployeeLogTemp (EmployeeLogTemp employeelogtemp_);
        bool UpdateEmployeeHeaderTemp (EmployeeLogTemp employeelogtemp_);
        bool UpdateEmployeeDetailLogTemp(EmployeeDetailLogTemp employeedetaillogtemp_);
        ICollection<EmployeeLogTemp> GetPendingEmployees(int limit, int page, string sort, string sortdesc, string keyword);
        int Getpendingdatacount();
        ICollection<EmployeeLogTemp> GetEmployeesAdvancePending(string keyword);
        EmployeeJoinDtoPending GetEmployeesJoinPending(int id);
        ICollection<EmployeeDetailJoinDtoPending> GetEmployeesDetailJoinPending(int id);
        ICollection<EmployeeDetailLogTemp> GetEmployeeDetailLogTemp_(int EmployeeTempId); 
        public EmployeeDetailLogTemp GetEmployeeDetailLogTemp(int id);
        bool EmployeeDetailExists(int id);
    }
}
