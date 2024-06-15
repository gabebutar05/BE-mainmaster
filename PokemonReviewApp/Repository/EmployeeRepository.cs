using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Utilities;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Migrations;
using System.Data;
using System.Globalization;
using System.Linq.Expressions;

namespace API_Dinamis.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;
        private readonly RepositoryUtils _repositoryUtility;

        public EmployeeRepository(DataContext context, RepositoryUtils repositoryUtility)
        {
            _context = context;
            _repositoryUtility = repositoryUtility;
        }

        public bool CreateEmployeeDetail(EmployeeDetail employeedetail_)
        {
            _context.Add(employeedetail_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateEmployeeDetailLog(EmployeeDetailLog employeedetail_)
        {
            _context.Add(employeedetail_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateEmployeeDetailLogTemp(EmployeeDetailLogTemp employeedetail_)
        {
            _context.Add(employeedetail_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateEmployeeHeader(Employee employee_)
        {
            _context.Add(employee_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateEmployeeHeaderLog(EmployeeLog Employeelog_)
        {
            _context.Add(Employeelog_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateEmployeeHeaderLogTemp(EmployeeLogTemp employeelogtemp_)
        {
            _context.Add(employeelogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateEmployeeLog(EmployeeLog Employeelog_)
        {
            _context.Add(Employeelog_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateEmployeeLogTemp(EmployeeLogTemp employeelogtemp_)
        {
            _context.Add(employeelogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteEmployee(Employee employee_)
        {
            _context.Remove(employee_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteEmployeeDetail(EmployeeDetail employeedetail_)
        {
            _context.Remove(employeedetail_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteEmployeeDetailLogTemp(EmployeeDetailLogTemp employeedetaillogtemp_)
        {
            _context.Remove(employeedetaillogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteEmployeeLogTemp(EmployeeLogTemp employeelogtemp_)
        {
            _context.Remove(employeelogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool EmployeeExists(int id)
        {
            return _context.Employees.Any(p => p.Id == id);
        }

        public bool EmployeeTempExists(int id)
        {
            return _context.EmployeesLogTemp.Any(p => p.Id == id);
        }

        public int Getdatacount()
        {
            return _context.Employees.Count();
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees.Where(p => p.Id == id).FirstOrDefault();
        }

        public EmployeeDetail GetEmployeeDetail(int id)
        {
            return _context.EmployeesDetail.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<EmployeeDetailLogTemp> GetEmployeeDetailLogTemp_employeeid(int employeeid)
        {
            return _context.EmployeesDetailLogTemp.Where(p => p.EmployeeTempId == employeeid).ToList();
        }

        public EmployeeDetail GetEmployeeDetail_employeeid(int employeeid)
        {
            return _context.EmployeesDetail.Where(p => p.EmployeeId == employeeid).FirstOrDefault();
        }

        public EmployeeLogTemp GetEmployeeLogTemp(int id)
        {
            return _context.EmployeesLogTemp.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Employee> GetEmployees(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "")
        {
            IQueryable<Employee> query = _context.Employees;

            if (!string.IsNullOrEmpty(keyword))
            {
                var key = "%" + keyword + "%";
                query = query.Where(employee => EF.Functions.Like(employee.Name.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            if (sortby == "d")
            {
                query = query.OrderByDescending(RepositoryUtils.GetOrderByExpression<Employee>(sortdesc));
            }
            else
            {
                query = query.OrderBy(RepositoryUtils.GetOrderByExpression<Employee>(sortdesc));
            }


            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var result = query.ToList();
            return result;
        }

        public ICollection<Employee> GetEmployeesAdvance(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.Employees.Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper())).ToList();
        }

        public ICollection<EmployeeLogTemp> GetEmployeesAdvancePending(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.EmployeesLogTemp.Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper())).ToList();
        }

        public ICollection<EmployeeDetail> GetEmployeesDetail(int employeeid)
        {
            return _context.EmployeesDetail.Where(p => p.EmployeeId == employeeid).ToList();
        }

        public ICollection<Employee> GetEmployees_all()
        {
            return _context.Employees.OrderBy(p => p.Id).ToList();
        }

        public int Getpendingdatacount()
        {
            return _context.EmployeesLogTemp.Count();
        }

        public ICollection<EmployeeLogTemp> GetPendingEmployees(int limit, int page, string sort, string sortdesc, string keyword)
        {
            int offset = 0;

            var key = "%" + keyword + "%";
            var dynamicOrder = sortdesc;

            var query = _context.EmployeesLogTemp
                                .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();



            if ((limit <= 0 || limit == null) && (page <= 0 || page == null))
            {
                offset = 0; //default
                limit = 10; //default
            }
            else if (page > 1)
            {
                offset = (page - 1) * limit;
            }

            if (sort == "d")
            {
                query = _context.EmployeesLogTemp
                                .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderByDescending(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }
            else if (sort == "a")
            {
                query = _context.EmployeesLogTemp
                                .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }

            return query;
        }

        private static Expression<Func<EmployeeLogTemp, object>> GetOrderByExpressionPending(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(EmployeeLogTemp), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<EmployeeLogTemp, object>>(Expression.Convert(property, typeof(object)), parameter);

            return lambda;
        }

        public ICollection<EmployeeLogTemp> GetPendingemployees_all()
        {
            return _context.EmployeesLogTemp.OrderBy(p => p.Id).ToList();
        }

        public bool UpdateEmployee(Employee employee_)
        {
            _context.Update(employee_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateEmployeeDetail(EmployeeDetail employee_)
        {
            _context.Update(employee_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateEmployeeDetailLogTemp(EmployeeDetailLogTemp employeedetaillogtemp_)
        {
            _context.Update(employeedetaillogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateEmployeeHeaderTemp(EmployeeLogTemp employeelogtemp_)
        {
            _context.Update(employeelogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public EmployeeJoinDtoPending GetEmployeesJoinPending(int id)
        {
            //var result = _context.EmployeesLogTemp.Join(
            //    _context.Modules,
            //    employee => employee.ModuleId,
            //    module => module.Id,
            //    (employee, module) => new EmployeeJoinDtoPending { Id = employee.Id, Name = employee.Name, ModuleId = module.Id, ModuleName = module.Name })
            //    .Where(p => p.Id == id)
            //    .FirstOrDefault();

            //return result;

            var result = _context.EmployeesLogTemp.Join(
                _context.Menus,
                employee => employee.Id,
                license => license.Id,
                (employee, license) => new EmployeeJoinDtoPending { Id = employee.Id})
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return result;
        }

        public ICollection<EmployeeDetailJoinDtoPending> GetEmployeesDetailJoinPending(int id)
        {
            var result = _context.EmployeesDetailLogTemp.Join(
                _context.Licenses,
                employeedetail => employeedetail.LicenseId,
                license => license.Id,
                (employeedetail, license) => new EmployeeDetailJoinDtoPending
                {
                    Id = employeedetail.Id,
                    EmployeeTempId = employeedetail.EmployeeTempId,
                    LicenseId = license.Id,
                    LicenseNo = license.LicenseCode
                })
                .Where(p => p.EmployeeTempId == id)
                .ToList();

            return result;
        }

        public ICollection<EmployeeDetailLogTemp> GetEmployeeDetailLogTemp_(int EmployeeTempId)
        {
            return _context.EmployeesDetailLogTemp.Where(p => p.EmployeeTempId == EmployeeTempId).ToList();
        }

        public EmployeeDetailLogTemp GetEmployeeDetailLogTemp(int id)
        {
            return _context.EmployeesDetailLogTemp.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool EmployeeDetailExists(int id)
        {
            return _context.EmployeesDetail.Any(p => p.Id == id);
        }
    }
}
