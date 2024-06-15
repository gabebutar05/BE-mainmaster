using API_Dinamis.Data;
using API_Dinamis.Dto;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using API_Dinamis.Utilities;
using Microsoft.EntityFrameworkCore;

namespace API_Dinamis.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DataContext _context;
        public MenuRepository(DataContext context)
        {
            _context = context;
        }

        public Menu GetMenu(int id)
        {
            return _context.Menus.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Menu> GetMenus(int moduleId, int? limit = 0, int? page = 1, string sortby = "D", string? sortdesc = "Id", string? keyword = "")
        {
            var key = "%" + keyword + "%";

            var query = _context.Menus
                .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()) && p.ModuleId == moduleId);

            // Menambahkan pengurutan
            if (sortby.ToLower() == "d")
            {
                if (sortdesc.ToLower() == "id")
                {
                    query = query.OrderByDescending(p => p.Id);
                }
                else if (sortdesc.ToLower() == "name")
                {
                    query = query.OrderByDescending(p => p.Name);
                }
            }
            else
            {
                if (sortdesc.ToLower() == "id")
                {
                    query = query.OrderBy(p => p.Id);
                }
                else if (sortdesc.ToLower() == "name")
                {
                    query = query.OrderBy(p => p.Name);
                }
            }

            // Mengambil halaman
            if (limit > 0)
            {
                limit = (limit < 10) ? 10 : limit;
                int offset = (page > 1) ? (page.Value - 1) * limit.Value : 0;

                query = query.Skip(offset).Take(limit.Value);
            }

            return query.ToList();
        }

        public ICollection<Menu> GetMenus_all()
        {
            return _context.Menus.OrderBy(p => p.Name).ToList();
        }

        public bool MenuExists(int moduleId, int menuId)
        {
            var result = _context.Menus.Where(menu => menu.ModuleId == moduleId && menu.Id == menuId).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}
