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
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;
        private readonly RepositoryUtils _repositoryUtility;

        public RoleRepository(DataContext context, RepositoryUtils repositoryUtility)
        {
            _context = context;
            _repositoryUtility = repositoryUtility;
        }

        public bool CreateRoleDetail(RoleDetail roledetail_)
        {
            _context.Add(roledetail_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateRoleHeaderLogTemp(RoleLogTemp role_)
        {
            _context.Add(role_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateRoleHeaderLog(RoleLog role_)
        {
            _context.Add(role_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateRoleHeader(Role role_)
        {
            _context.Add(role_);
            return _repositoryUtility.SaveChanges();
        }

        public RoleJoinDto GetRolesJoin(int id)
        {
            var result = _context.Roles.Join(
                _context.Modules,
                role => role.ModuleId,
                module => module.Id,
                (role, module) => new RoleJoinDto { Id = role.Id, Name = role.Name, ModuleId = module.Id, ModuleName = module.Name })
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return result;
        }

        public ICollection<RoleDetailJoinDto> GetRolesDetailJoin(int roleid)
        {
            var result = _context.RolesDetail.Join(
                _context.Menus,
                roledetail => roledetail.MenuId,
                menu => menu.Id,
                (roledetail, menu) => new RoleDetailJoinDto
                {
                    Id = roledetail.Id,
                    RoleId = roledetail.RoleId,
                    MenuId = menu.Id,
                    MenuName = menu.Name,
                    Run = roledetail.Run,
                    Add = roledetail.Add,
                    Update = roledetail.Update,
                    Delete = roledetail.Delete,
                })
                .Where(p => p.RoleId == roleid)
                .ToList();

            return result;
        }

        public ICollection<Role> GetRoles_all()
        {
            return _context.Roles.OrderBy(p => p.Name).ToList();
        }

        public bool CreateRoleDetailLogTemp(RoleDetailLogTemp roledetail_)
        {
            _context.Add(roledetail_);
            return _repositoryUtility.SaveChanges();
        }

        public bool CreateRoleDetailLog(RoleDetailLog roledetail_)
        {
            _context.Add(roledetail_);
            return _repositoryUtility.SaveChanges();
        }

        public bool RoleTempExists(int id)
        {
            return _context.RolesLogTemp.Any(p => p.Id == id);
        }

        public RoleLogTemp GetRoleLogTemp(int id)
        {
            return _context.RolesLogTemp.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool DeleteRoleLogTemp(RoleLogTemp rolelogtemp_)
        {
            _context.Remove(rolelogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public ICollection<RoleDetailLogTemp> GetRoleDetailLogTemp_roleid(int roleid)
        {
            return _context.RolesDetailLogTemp.Where(p => p.RoleTempId == roleid).ToList();
        }

        public ICollection<RoleDetailLogTemp> GetRoleDetailLogTemp_(int RoleTempId)
        {
            return _context.RolesDetailLogTemp.Where(p => p.RoleTempId == RoleTempId).ToList();
        }

        public RoleDetailLogTemp GetRoleDetailLogTemp(int id)
        {
            return _context.RolesDetailLogTemp.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool DeleteRoleDetailLogTemp(RoleDetailLogTemp roledetaillogtemp_)
        {
            _context.Remove(roledetaillogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool RoleExists(int id)
        {
            return _context.Roles.Any(p => p.Id == id);
        }

        public bool UpdateRole(Role role_)
        {
            _context.Update(role_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateRoleDetail(RoleDetail role_)
        {
            _context.Update(role_);
            return _repositoryUtility.SaveChanges();
        }

        public Role GetRole(int id)
        {
            return _context.Roles.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<RoleDetail> GetRolesDetail(int roleid)
        {
            return _context.RolesDetail.Where(p => p.RoleId == roleid).ToList();
        }

        public bool DeleteRole(Role role_)
        {
            _context.Remove(role_);
            return _repositoryUtility.SaveChanges();
        }

        public bool DeleteRoleDetail(RoleDetail roledetail_)
        {
            _context.Remove(roledetail_);
            return _repositoryUtility.SaveChanges();
        }

        public RoleDetail GetRoleDetail_roleid(int roleid)
        {
            return _context.RolesDetail.Where(p => p.RoleId == roleid).FirstOrDefault();
        }

        public RoleDetail GetRoleDetail(int id)
        {
            return _context.RolesDetail.Where(p => p.Id == id).FirstOrDefault();
        }

        //BACKUP
        //public ICollection<RoleLogTemp> GetPendingRoles(int limit, int page, string sort, string sortdesc, string keyword)
        //{
        //    int offset = 0;

        //    var key = "%" + keyword + "%";
        //    var dynamicOrder = sortdesc;

        //    var query = _context.RolesLogTemp
        //                        .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
        //                        .OrderBy(GetOrderByExpressionPending(dynamicOrder))
        //                        .Skip(offset)
        //                        .Take(limit)
        //                        .ToList();



        //    if ((limit <= 0 || limit == null) && (page <= 0 || page == null))
        //    {
        //        offset = 0; //default
        //        limit = 10; //default
        //    }
        //    else if (page > 1)
        //    {
        //        offset = (page - 1) * limit;
        //    }

        //    if (sort == "d")
        //    {
        //        query = _context.RolesLogTemp
        //                        .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
        //                        .OrderByDescending(GetOrderByExpressionPending(dynamicOrder))
        //                        .Skip(offset)
        //                        .Take(limit)
        //                        .ToList();
        //    }
        //    else if (sort == "a")
        //    {
        //        query = _context.RolesLogTemp
        //                        .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
        //                        .OrderBy(GetOrderByExpressionPending(dynamicOrder))
        //                        .Skip(offset)
        //                        .Take(limit)
        //                        .ToList();
        //    }

        //    return query;
        //}
        //BACKUP

        public ICollection<RoleLogTemp> GetPendingRoles(int limit, int page, string sort, string sortdesc, string keyword)
        {
            int offset = 0;

            var key = "%" + keyword + "%";
            var dynamicOrder = sortdesc;

            var query = _context.RolesLogTemp
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
                query = _context.RolesLogTemp
                                .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderByDescending(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }
            else if (sort == "a")
            {
                query = _context.RolesLogTemp
                                .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
                                .OrderBy(GetOrderByExpressionPending(dynamicOrder))
                                .Skip(offset)
                                .Take(limit)
                                .ToList();
            }

            return query;
        }

        public int Getpendingdatacount()
        {
            return _context.RolesLogTemp.Count();
        }

        public ICollection<RoleLogTemp> GetRolesAdvancePending(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.RolesLogTemp.Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper())).ToList();
        }

        private static Expression<Func<RoleLogTemp, object>> GetOrderByExpressionPending(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(RoleLogTemp), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<RoleLogTemp, object>>(Expression.Convert(property, typeof(object)), parameter);

            return lambda;
        }

        public RoleJoinDtoPending GetRolesJoinPending(int id)
        {
            var result = _context.RolesLogTemp.Join(
                _context.Modules,
                role => role.ModuleId,
                module => module.Id,
                (role, module) => new RoleJoinDtoPending { Id = role.Id, Name = role.Name, ModuleId = module.Id, ModuleName = module.Name })
                .Where(p => p.Id == id)
                .FirstOrDefault();

            return result;
        }

        public ICollection<RoleDetailJoinDtoPending> GetRolesDetailJoinPending(int id)
        {
            var result = _context.RolesDetailLogTemp.Join(
                _context.Menus,
                roledetail => roledetail.MenuId,
                menu => menu.Id,
                (roledetail, menu) => new RoleDetailJoinDtoPending
                {
                    Id = roledetail.Id,
                    RoleTempId = roledetail.RoleTempId,
                    MenuId = menu.Id,
                    MenuName = menu.Name,
                    Run = roledetail.Run,
                    Add = roledetail.Add,
                    Update = roledetail.Update,
                    Delete = roledetail.Delete,
                })
                .Where(p => p.RoleTempId == id)
                .ToList();

            return result;
        }

        public bool RoleDetailExists(int id)
        {
            return _context.RolesDetail.Any(p => p.Id == id);
        }

        public bool UpdateRoleDetailLogTemp(RoleDetailLogTemp roledetsaillogtemp_)
        {
            _context.Update(roledetsaillogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public bool UpdateRoleLogTemp(RoleLogTemp rolelogtemp_)
        {
            _context.Update(rolelogtemp_);
            return _repositoryUtility.SaveChanges();
        }

        public ICollection<Role> GetRoles(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "")
        {
            IQueryable<Role> query = _context.Roles;

            if (!string.IsNullOrEmpty(keyword))
            {
                var key = "%" + keyword + "%";
                query = query.Where(role => EF.Functions.Like(role.Name.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            if (sortby == "d")
            {
                query = query.OrderByDescending(RepositoryUtils.GetOrderByExpression<Role>(sortdesc));
            }
            else
            {
                query = query.OrderBy(RepositoryUtils.GetOrderByExpression<Role>(sortdesc));
            }


            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var result = query.ToList();
            return result;
        }

        public int Getdatacount()
        {
            return _context.Roles.Count();
        }

        public ICollection<Role> GetRolesAdvance(string keyword)
        {
            var key = "%" + keyword + "%";
            return _context.Roles.Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper())).ToList();
        }

        public ICollection<RoleJoinDtoPending> GetPendingRoles_Join(int? limit = 0, int? page = 1, string? sortby = "d", string? sortdesc = "id", string? keyword = "")
        {
            IQueryable<RoleJoinDtoPending> query = _context.RolesLogTemp
                .Join(_context.Modules,
                roles => roles.ModuleId,
                module => module.Id,
                (roles, module) => new RoleJoinDtoPending
                {
                    Id = roles.Id,
                    Name = roles.Name,
                    ModuleId = roles.ModuleId,
                    ModuleName = module.Name,
                    
                });

            if (!string.IsNullOrEmpty(keyword))
            {
                var key = "%" + keyword + "%";
                query = query.Where(role => EF.Functions.Like(role.Name.Trim().ToUpper(), key.Trim().ToUpper()));
            }

            if (sortby == "d")
            {
                query = query.OrderByDescending(RepositoryUtils.GetOrderByExpression<RoleJoinDtoPending>(sortdesc));
            }
            else
            {
                query = query.OrderBy(RepositoryUtils.GetOrderByExpression<RoleJoinDtoPending>(sortdesc));
            }


            if (limit.HasValue)
            {
                query = query.Take(limit.Value);
            }

            var result = query.ToList();
            return result;
        }


        //public ICollection<RoleDetailJoinDto> GetRoleDetail_Test(int roleid)
        //{
        //    var result = _context.RolesDetail.Join(
        //        _context.Menus,
        //        roledetail => roledetail.MenuId,
        //        menu => menu.Id,
        //        (roledetail, menu) => new RoleDetailJoinDto { 
        //            Id = roledetail.Id,
        //            RoleId = roledetail.RoleId,
        //            MenuId = menu.Id, 
        //            MenuName = menu.Name, 
        //            Run = roledetail.Run,
        //            Add = roledetail.Add,
        //            Update = roledetail.Update,
        //            Delete = roledetail.Delete,
        //        })
        //        .Where(p => p.RoleId == roleid)
        //        .ToList();

        //    return result;
        //}
    }
}
