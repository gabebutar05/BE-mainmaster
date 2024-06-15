using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IStandardValueRepository
    {
        ICollection<StandardValueDto> GetStandardValues(int? limit = 0, int? page = 1, string? sortby = "D", string? sortdesc = "Id", string? keyword = "");
        ICollection<StandardValueLogTemp> GetStandardValuesLogTemp(int? limit = 0, int? page = 1, string? sortby = "D", string? sortdesc = "Id", string? keyword = "");
        StandardValue GetStandardValue(int id);
        StandardValueLogTemp GetStandardValueLogTemp(int id);
        bool StandardValueExists(int id, string? target);
        int GetStandardValueCount(string? target);
        /*process data start*/
        bool CreateStandardValueLog(StandardValueLog data_);
        bool CreateStandardValueLogTemp(StandardValueLogTemp data_);
        bool UpdateStandardValue(StandardValue data_);
        bool UpdateStandardValueLogTemp(StandardValueLogTemp data_);
        bool DeleteStandardValueLogTemp(StandardValueLogTemp data_);
        /*process data end*/
    }
}
