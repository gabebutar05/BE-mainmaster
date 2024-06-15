using API_Dinamis.Models;

namespace API_Dinamis.Dto
{
    public class HolidayDto
    {
        public int ID { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Description { get; set; }
    }

    public class HolidayTempDto : HolidayDto
    {
        public int HolidayId { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string ActionRemarks { get; set; }
    }

    public class HolidayForm
    {
        public int ID { get; set;}
        public int? HolidayID { get; set; }
        public DateTime HolidayDate { get; set;}
        public string Description { get; set; }
        public string? Remarks { get; set; } = "";
        public string? ActionRemarks { get; set; } = "";
        public string UpdatedBy { get; set; } = null!;
    }
}
