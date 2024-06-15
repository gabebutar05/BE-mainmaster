using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("Module")]
    public class Module
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
    }
}
