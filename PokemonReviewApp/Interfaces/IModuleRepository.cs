using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IModuleRepository
    {
        ICollection<Module> GetModules_all();
        ICollection<Module> GetModules(int limit, string keyword);
        Module GetModule(int id);
        bool ModuleExists(int moduleId);
    }
}
