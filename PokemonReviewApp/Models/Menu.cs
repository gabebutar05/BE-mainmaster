using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("Menu")]
    public class Menu
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}
