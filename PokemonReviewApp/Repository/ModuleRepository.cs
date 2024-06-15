using API_Dinamis.Data;
using API_Dinamis.Interfaces;
using API_Dinamis.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Dinamis.Repository
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly DataContext _context;
        public ModuleRepository(DataContext context)
        {
            _context = context;
        }

        public Module GetModule(int id)
        {
            return _context.Modules.Where(p => p.Id == id).FirstOrDefault();
        }

        public ICollection<Module> GetModules(int limit, string keyword)
        {
            var key = "%" + keyword + "%";

            var query = _context.Modules
            .Where(p => EF.Functions.Like(p.Name.Trim().ToUpper(), key.Trim().ToUpper()))
            .OrderBy(p => p.Name)
            .Take(limit)
            .ToList();

            return query;
        }

        public ICollection<Module> GetModules_all()
        {
            return _context.Modules.OrderBy(p => p.Name).ToList();
        }

        public bool ModuleExists(int moduleId)
        {
            var result = _context.Modules.Where(module => module.Id == moduleId).FirstOrDefault();
            if (result == null)
            {
                return false;
            }
            return true;
        }
    }
}
