using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IMenuRepository
    {
        ICollection<Menu> GetMenus_all();
        ICollection<Menu> GetMenus(int moduleId, int? limit = 0, int? page = 1, string sortby = "D", string? sortdesc = "Id", string? keyword = "");
        Menu GetMenu(int id);
        bool MenuExists(int moduleId, int menuId);
    }
}
