using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("Country")]
    public class Country
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string CountryCode { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string CountryName { get; set; } = null!;
    }
}
