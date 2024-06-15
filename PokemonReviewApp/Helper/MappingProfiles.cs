using API_Dinamis.Dto;
using API_Dinamis.Models;
using AutoMapper;

namespace API_Dinamis.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Branch
            CreateMap<Branch, BranchDto>();
            CreateMap<BranchDto, Branch>();
            CreateMap<BranchLog, BranchDto>();
            CreateMap<BranchDto, BranchLog>();
            CreateMap<BranchLogTemp, BranchDto>();
            CreateMap<BranchDto, BranchLogTemp>();
            CreateMap<BranchLog, BranchLogTemp>();
            CreateMap<BranchLogTemp, BranchLog>();
            CreateMap<BranchLogTemp, BranchDtoPending>();
            CreateMap<BranchDtoPending, BranchLogTemp>();
            CreateMap<Branch, BranchDtoForm>();
            CreateMap<BranchDtoForm, Branch>();
            CreateMap<BranchDtoListTableJoin, Branch>();
            CreateMap<Branch, BranchDtoListTableJoin>();
            CreateMap<BranchLogTemp, Branch>();
            CreateMap<Branch, BranchLogTemp>();
            CreateMap<BranchLog, Branch>();
            CreateMap<Branch, BranchLog>();

            CreateMap<Branch, BranchTestDto>();
            CreateMap<BranchTestDto, Branch>();

            //CreateMap<BranchLogTemp, BranchDtoPendingUpdate>();
            //CreateMap<BranchDtoPendingUpdate, BranchLogTemp>();
            //Branch

            //Department
            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
            CreateMap<DepartmentLog, DepartmentDto>();
            CreateMap<DepartmentDto, DepartmentLog>();
            CreateMap<DepartmentLogTemp, DepartmentDto>();
            CreateMap<DepartmentDto, DepartmentLogTemp>();
            CreateMap<DepartmentLog, DepartmentLogTemp>();
            CreateMap<DepartmentLogTemp, DepartmentLog>();
            CreateMap<DepartmentLogTemp, DepartmentDtoPending>();
            CreateMap<DepartmentDtoPending, DepartmentLogTemp>();
            CreateMap<Department, DepartmentDtoForm>();
            CreateMap<DepartmentDtoForm, Department>();
            CreateMap<DepartmentDtoListTableJoin, Department>();
            CreateMap<Department, DepartmentDtoListTableJoin>();
            CreateMap<DepartmentLogTemp, Department>();
            CreateMap<Department, DepartmentLogTemp>();
            CreateMap<DepartmentLog, Department>();
            CreateMap<Department, DepartmentLog>();
            //Department

            //Auth
            CreateMap<Authx, AuthDto>();
            CreateMap<AuthDto, Authx>();
            CreateMap<Key, KeyDto>();
            CreateMap<KeyDto, Key>();
            //Auth

            //City
            CreateMap<City, CityDto>();
            CreateMap<CityDto, City>();
            //City

            //ZipCode
            CreateMap<Zipcode, ZipCodeDto>();
            CreateMap<ZipCodeDto, Zipcode>();
            //ZipCode

            //Company
            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyDto, Company>();
            CreateMap<CompanyLogTemp, CompanyTempDto>();
            CreateMap<CompanyTempDto, CompanyLogTemp>();
            CreateMap<CompanyTempDto, CompanyLog>();
            CreateMap<CompanyLog, CompanyTempDto>();

            CreateMap<CompanyLogTemp, CompanyForm>();
            CreateMap<CompanyForm, CompanyLogTemp>();
            CreateMap<CompanyLog, CompanyForm>();
            CreateMap<CompanyForm, CompanyLog>();
            CreateMap<CompanyLog, CompanyLogTemp>();
            CreateMap<CompanyForm, Company>();
            CreateMap<Company, CompanyForm>();
            //Company

            //Holiday
            CreateMap<HolidayTempDto, HolidayLog>();
            CreateMap<HolidayLog, HolidayTempDto>();
            CreateMap<HolidayTempDto, HolidayLogTemp>();
            CreateMap<HolidayLogTemp, HolidayTempDto>();

            CreateMap<Holiday, HolidayDto>();
            CreateMap<HolidayDto, Holiday>();
            CreateMap<HolidayLogTemp, HolidayForm>();
            CreateMap<HolidayForm, HolidayLogTemp>();
            CreateMap<HolidayLog, HolidayForm>();
            CreateMap<HolidayForm, HolidayLog>();
            CreateMap<HolidayLog, HolidayLogTemp>();
            CreateMap<HolidayForm, Holiday>();
            CreateMap<Holiday, HolidayForm>();
            CreateMap<HolidayLogTemp, HolidayLog>();
            CreateMap<HolidayLog, HolidayLogTemp>();
            CreateMap<HolidayLogTemp, Holiday>();
            CreateMap<Holiday, HolidayLog>();
            CreateMap<HolidayLog, Holiday>();
            //Holiday

            //Module
            CreateMap<Module, ModuleDto>();
            CreateMap<ModuleDto, Module>();
            CreateMap<Holiday, HolidayLogTemp>();

            //Menu
            CreateMap<Menu, MenuDto>();
            CreateMap<MenuDto, Menu>();
            CreateMap<Holiday, HolidayLogTemp>();
        
            //Role
            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();

            CreateMap<RoleDetail, RoleDetailDto>();
            CreateMap<RoleDetailDto, RoleDetail>();

            CreateMap<Role, RoleResultDto>();
            CreateMap<RoleResultDto, Role>();
            CreateMap<RoleDetail, RoleResultDto>();
            CreateMap<RoleResultDto, RoleDetail>();

            CreateMap<RoleDto, RoleResultDto>();
            CreateMap<RoleResultDto, RoleDto>();
            CreateMap<RoleDetailDto, RoleResultDto>();
            CreateMap<RoleResultDto, RoleDetailDto>();

            CreateMap<RoleDetail, RoleResultDto>();
            CreateMap<RoleResultDto, RoleDetail >();

            CreateMap<RoleJoinDto, RoleResultJoinDto>();
            CreateMap<RoleResultJoinDto, RoleJoinDto>();
            CreateMap<RoleDetailJoinDto, RoleResultJoinDto>();
            CreateMap<RoleResultJoinDto, RoleDetailJoinDto>();

            CreateMap<RoleLogTemp, RoleResultDto>();
            CreateMap<RoleResultDto, RoleLogTemp>();
            CreateMap<RoleLog, RoleResultDto>();
            CreateMap<RoleResultDto, RoleLog>();

            CreateMap<RoleDetailLogTemp, RoleDetailDto>();
            CreateMap<RoleDetailDto, RoleDetailLogTemp>();
            CreateMap<RoleDetailLog, RoleDetailDto>();
            CreateMap<RoleDetailDto, RoleDetailLog>();

            CreateMap<RoleDto, RoleLog>();
            CreateMap<RoleLog, RoleDto>();

            CreateMap<RoleDetailLogTemp, RoleDetail>();
            CreateMap<RoleDetail, RoleDetailLogTemp>();

            CreateMap<RoleDetailLog, RoleDetail>();
            CreateMap<RoleDetail, RoleDetailLog>();

            CreateMap<RoleDetailLog, RoleDetailLogTemp>();
            CreateMap<RoleDetailLogTemp, RoleDetailLog>();

            CreateMap<RoleDetailJoinDto, RoleDetail>();
            CreateMap<RoleDetail, RoleDetailJoinDto>();

            CreateMap<RoleJoinDto, Role>();
            CreateMap<Role, RoleJoinDto>();

            CreateMap<RoleLogTemp, Role>();
            CreateMap<Role, RoleLogTemp>();

            CreateMap<RoleLogTemp, RoleLog>();
            CreateMap<RoleLog, RoleLogTemp>();
            CreateMap<RoleDetailLogTemp, RoleDetailLog>();
            CreateMap<RoleDetailLog, RoleDetailLogTemp>();

            CreateMap<RoleLog, Role>();
            CreateMap<Role, RoleLog>();

            CreateMap<RoleJoinDtoPending, RoleLogTemp>();
            CreateMap<RoleLogTemp, RoleJoinDtoPending>();

            CreateMap<RoleJoinDtoPending, RoleResultJoinDtoPending>();
            CreateMap<RoleResultJoinDtoPending, RoleJoinDtoPending>();
            CreateMap<RoleDetailJoinDtoPending, RoleResultJoinDtoPending>();
            CreateMap<RoleResultJoinDtoPending, RoleDetailJoinDtoPending>();

            CreateMap<RoleResultJoinDtoPendingUpdate, RoleLog>();
            CreateMap<RoleLog, RoleResultJoinDtoPendingUpdate>();

            CreateMap<RoleDetailLog,RoleDetailJoinDtoPending>();
            CreateMap<RoleDetailJoinDtoPending,RoleDetailLog>();

            CreateMap < RoleDtoForm,Role>();
            CreateMap<Role, RoleDtoForm>();
            //Role

            //License
            CreateMap<License, LicenseDto>();
            CreateMap<LicenseDto, License>();
            CreateMap<LicenseLog, LicenseDto>();
            CreateMap<LicenseDto, LicenseLog>();
            CreateMap<LicenseLogTemp, LicenseDto>();
            CreateMap<LicenseDto, LicenseLogTemp>();
            CreateMap<LicenseLog, LicenseLogTemp>();
            CreateMap<LicenseLogTemp, LicenseLog>();
            CreateMap<LicenseLogTemp, LicenseDtoPending>();
            CreateMap<LicenseDtoPending, LicenseLogTemp>();
            CreateMap<License, LicenseDtoForm>();
            CreateMap<LicenseDtoForm, License>();
            CreateMap<LicenseLogTemp, License>();
            CreateMap<License, LicenseLogTemp>();
            CreateMap<LicenseLog, License>();
            CreateMap<License, LicenseLog>();
            //License

            //Employee
            CreateMap<Employee, EmployeeResultDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Employee,EmployeeLogTemp>();
            CreateMap<Employee,EmployeeLog > ();

            CreateMap<EmployeeDetail,EmployeeDetailLogTemp>();
            CreateMap<EmployeeDetail,EmployeeDetailDto>();
            CreateMap<EmployeeDetail,EmployeeDetailLog>();

            CreateMap<EmployeeDto, Employee> ();
            CreateMap<EmployeeDto,EmployeeLog>();

            CreateMap<EmployeeResultDto, Employee>();
            CreateMap<EmployeeResultDto, EmployeeLogTemp>();
            CreateMap<EmployeeResultDto, EmployeeLog>();

            CreateMap<EmployeeLogTemp, EmployeeResultDto>();
            CreateMap<EmployeeLogTemp, Employee>();
            CreateMap<EmployeeLogTemp,EmployeeLog>();

            CreateMap<EmployeeDetailLogTemp, EmployeeDetailDto>();
            CreateMap<EmployeeDetailLogTemp, EmployeeDetail>();
            CreateMap<EmployeeDetailLogTemp,EmployeeDetailLog>();

            CreateMap<EmployeeLog, EmployeeResultDto>();
            CreateMap<EmployeeLog, EmployeeDto > ();
            CreateMap<EmployeeLog, EmployeeLogTemp>();
            CreateMap<EmployeeLog, Employee>();

            CreateMap<EmployeeDetailLog, EmployeeDetailDto>();
            CreateMap<EmployeeDetailLog, EmployeeDetailLogTemp>();
            CreateMap<EmployeeDetailLog, EmployeeDetail>();

            CreateMap<EmployeeDetailDto, EmployeeDetailLogTemp>();
            CreateMap<EmployeeDetailDto, EmployeeDetailLog>();
            CreateMap<EmployeeDetailDto, EmployeeDetail>();
            //Employee
        }
    }
}
