using API_Dinamis.Dto;
using API_Dinamis.Models;

namespace API_Dinamis.Interfaces
{
    public interface IHolidayRepository
    {
        ICollection<HolidayDto> GetHolidays(int? limit=0, int? page=1, string? sortby = "D", string? sortdesc = "Id", string? keyword="");
        ICollection<HolidayTempDto> GetHolidaysLogTemp(int? limit = 0, int? page = 1, string? sortby = "D", string? sortdesc = "Id", string? keyword = "");
        Holiday GetHoliday(int id);
        HolidayLogTemp GetHolidayLogTemp(int id);
        int GetHolidayCount(string? target);
        bool HolidayExists(int id, string? target);

        /*process data start*/
        bool CreateHoliday(Holiday data_);
        bool CreateHolidayLogTemp(HolidayLogTemp data_);
        bool CreateHolidayLog(HolidayLog data_);
        bool UpdateHoliday(Holiday data_);
        bool UpdateHolidayLogTemp(HolidayLogTemp data_);
        bool DeleteHoliday(Holiday data_);
        bool DeleteHolidayLogTemp(HolidayLogTemp data_);
        /*process data end*/

    }
}
