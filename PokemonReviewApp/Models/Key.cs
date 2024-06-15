using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Models
{
    [Table("Key")]
    public class Key
    {
        public int id { get; set; }
        public string Keys { get; set; }
    }
}
