using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("City")]
    public class City
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(6)")]
        public string CityCode { get; set; } = null!;
        [Column(TypeName = "varchar(50)")]
        public string CityName { get; set; } = null!;
    }
}
