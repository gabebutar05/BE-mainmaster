using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models.Authorizer
{
    [Table("AuthorizerDownline")]
    public class AuthorizerDownline
    {
        public int Id { get; set; }

        [Required]
        public int AuthorizerModuleId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        /* field for technical flag start */
        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; // NA=need approve, A=approve, R=reject

        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; // N=no action, C=create, U=update, D=delete

        /* field for technical flag end */
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
    }

    [Table("AuthorizerDownlineLogTemp")]
    public class AuthorizerDownlineLogTemp
    {
        public int Id { get; set; }

        public int AuthorizerDownlineId { get; set; }

        [Required]
        public int AuthorizerModuleId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        /* field for technical flag start */
        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; // NA=need approve, A=approve, R=reject

        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; // N=no action, C=create, U=update, D=delete

        /* field for technical flag end */
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
    }

    [Table("AuthorizerDownlineLog")]
    public class AuthorizerDownlineLog
    {
        public int Id { get; set; }

        public int AuthorizerDownlineId { get; set; }
        public int AuthorizerDownlineTempId { get; set; }

        [Required]
        public int AuthorizerModuleId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        /* field for technical flag start */
        [Column(TypeName = "varchar(2)")]
        [EnumDataType(typeof(StatusEnum))]
        public string Status { get; set; } = "NA"; // NA=need approve, A=approve, R=reject

        [Column(TypeName = "varchar(1)")]
        [EnumDataType(typeof(ActionEnum))]
        public string Action { get; set; } = "N"; // N=no action, C=create, U=update, D=delete

        /* field for technical flag end */
        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = null!;
    }
}
