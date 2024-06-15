using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("ZipCode")]
    public class Zipcode
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        [Column(TypeName = "varchar(10)")]
        public string ZipCode { get; set; } = null!;
    }
}
