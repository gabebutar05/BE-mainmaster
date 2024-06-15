using API_Dinamis.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Dto
{
    public class ZipCodeDto
    {
        public int Id { get; set; }
        //public virtual City City { get; set; }
        public string ZipCode { get; set; } = null!;
    }
}
