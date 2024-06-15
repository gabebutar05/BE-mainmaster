using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Expressions;

namespace API_Dinamis.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _context;
        private readonly RepositoryUtils _repositoryUtility;
        public DepartmentRepository(DataContext context, RepositoryUtils repositoryUtility)
        {
            _context = context;
            _repositoryUtility = repositoryUtility;
        }

        public ICollection<Department> GetDepartments(int limit, int page, string sortby, string sortdesc, string keyword)
        {
            int offset = 0;

            var key = "%" + keyword + "%";
            var dynamicOrder = sortdesc;
            var query = _context.Departments
                                .Where(p => EF.Functions.Like(p.DepartmentName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.DepartmentCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpression(dynamicOrder))
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

            if (sortby == "d")
            {
                query = _context.Departments
                                .Where(p => EF.Functions.Like(p.DepartmentName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.DepartmentCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderByDescending(GetOrderByExpression(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }
            else if (sortby == "a")
            {
                query = _context.Departments
                                .Where(p => EF.Functions.Like(p.DepartmentName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.DepartmentCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpression(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }


            return query;
        }

        public ICollection<DepartmentLogTemp> GetPendingDepartments(int limit, int page, string sortby, string sortdesc, string keyword)
        {
            int offset = 0;

            var key = "%" + keyword + "%";
            var dynamicOrder = sortdesc;

            var query = _context.DepartmentsLogTemp
                                .Where(p => EF.Functions.Like(p.DepartmentName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.DepartmentCode.Trim().ToUpper(), key.Trim().ToUpper()))
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

            if (sortby == "d")
            {
                query = _context.DepartmentsLogTemp
                                .Where(p => EF.Functions.Like(p.DepartmentName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.DepartmentCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderByDescending(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }
            else if (sortby == "a")
            {
                query = _context.DepartmentsLogTemp
                                .Where(p => EF.Functions.Like(p.DepartmentName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.DepartmentCode.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }

            return query;
        }

        private static Expression<Func<Department, object>> GetOrderByExpression(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(Department), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<Department, object>>(Expression.Convert(property, typeof(object)), parameter);

            return lambda;
        }
        private static Expression<Func<DepartmentLogTemp, object>> GetOrderByExpressionPending(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(DepartmentLogTemp), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<DepartmentLogTemp, object>>(Expression.Convert(property, typeof(object)), parameter);

            return lambda;
        }

        public ICollection<Department> GetDepartments_all()
        {
            return _context.Departments.OrderBy(p => p.Id).ToList();
        }

        public ICollection<DepartmentLogTemp> GetPendingDepartments_all()
        {
            return _context.DepartmentsLogTemp.OrderBy(p => p.Id).ToList();
        }

        public ICollection<Department> GetDepartmensAdvance(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.Departments.Where(p => EF.Functions.Like(p.DepartmentName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.DepartmentCode.Trim().ToUpper(), key.Trim().ToUpper())).ToList();

        }

        public ICollection<DepartmentLogTemp> GetDepartmentsAdvancePending(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.DepartmentsLogTemp.Where(p => EF.Functions.Like(p.DepartmentName.Trim().ToUpper(), key.Trim().ToUpper()) || EF.Functions.Like(p.DepartmentCode.Trim().ToUpper(), key.Trim().ToUpper())).ToList();

        }

        public Department GetDepartment(int id)
        {
            return _context.Departments.Where(p => p.Id == id).FirstOrDefault();
        }

        public DepartmentLogTemp GetDepartmentLogTemp(int id)
        {
            return _context.DepartmentsLogTemp.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool CreateDepartment(Department department_)
        {
            _context.Add(department_);
            //return Save();
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateDepartmentLog(DepartmentLog departmentlog_)
        {
            _context.Add(departmentlog_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateDepartmentLogTemp(DepartmentLogTemp departmentlogtemp_)
        {
            _context.Add(departmentlogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateDepartment(Department department_)
        {
            _context.Update(department_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateDepartmentTemp(DepartmentLogTemp departmentlogtemp_)
        {
            _context.Update(departmentlogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteDepartment(Department department_)
        {
            _context.Remove(department_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteDepartmentLogTemp(DepartmentLogTemp departmentlogtemp_)
        {
            _context.Remove(departmentlogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DepartmentExists(int id)
        {
            return _context.Departments.Any(p => p.Id == id);
        }

        public bool DepartmentTempExists(int id)
        {
            return _context.DepartmentsLogTemp.Any(p => p.Id == id);
        }

        public int Getdatacount()
        {
            return _context.Departments.Count();
        }

        public int Getpendingdatacount()
        {
            return _context.DepartmentsLogTemp.Count();
        }

        public ICollection<DepartmentDtoListTableJoin> DepartmentDtoListTableJoin_all()
        {
            var result = _context.Departments
                .Join(
                _context.Branches,
                department => department.BranchId,
                branch => branch.Id,
                (department, branch) => new DepartmentDtoListTableJoin
                {
                    ID = department.Id,
                    DepartmentCode = department.DepartmentCode,
                    DepartmentName = department.DepartmentName,
                    BranchId = department.BranchId,
                    BranchName = branch.BranchName,
                    UpdatedBy = department.UpdatedBy,
                    Remarks = department.Remarks,
                })
                .ToList();

            return result;
        }

        public DepartmentDtoListTableJoin DepartmentDtoTableJoin(int id)
        {
            var result = _context.Departments
                .Join(
                _context.Branches,
                department => department.BranchId,
                branch => branch.Id,
                (department, branch) => new DepartmentDtoListTableJoin
                {
                    ID = department.Id,
                    DepartmentCode = department.DepartmentCode,
                    DepartmentName = department.DepartmentName,
                    BranchId = department.BranchId,
                    BranchName = branch.BranchName,
                    UpdatedBy = department.UpdatedBy,
                    Remarks = department.Remarks,
                })
                .Where(p => p.ID == id)
                .FirstOrDefault();

            return result;
        }

        public DepartmentDtoListTableJoinPending DepartmentDtoTableJoinPending(int id)
        {
            var result = _context.DepartmentsLogTemp
                .Join(
                _context.Branches,
                department => department.BranchId,
                branch => branch.Id,
                (department, branch) => new DepartmentDtoListTableJoinPending
                {
                    ID = department.Id,
                    DepartmentId = department.DepartmentId,
                    DepartmentCode = department.DepartmentCode,
                    DepartmentName = department.DepartmentName,
                    BranchId = branch.Id,
                    BranchName = branch.BranchName,
                    UpdatedBy = department.UpdatedBy,
                    Remarks = department.Remarks,
                })
                .Where(p => p.ID == id)
                .FirstOrDefault();

            return result;
        }
    }
}
