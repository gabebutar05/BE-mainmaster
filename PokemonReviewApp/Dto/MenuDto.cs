using System.ComponentModel.DataAnnotations.Schema;

namespace API_Dinamis.Dto
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}
