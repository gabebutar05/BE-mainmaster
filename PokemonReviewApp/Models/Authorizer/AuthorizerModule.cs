using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static API_Dinamis.Utilities.EnumUtils;

namespace API_Dinamis.Models.Authorizer
{
    [Table("AuthorizerModule")]
    public class AuthorizerModule
    {
        public int Id { get; set; }

        [Required]
        public int AuthorizerId { get; set; }

        [Required]
        public int ModuleId { get; set; }

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

    [Table("AuthorizerModuleLogTemp")]
    public class AuthorizerModuleLogTemp
    {
        public int Id { get; set; }
        public int AuthorizerModuleId { get; set; }

        [Required]
        public int AuthorizerId { get; set; }

        [Required]
        public int ModuleId { get; set; }

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

    [Table("AuthorizerModuleLog")]
    public class AuthorizerModuleLog
    {
        public int Id { get; set; }
        public int AuthorizerModuleId { get; set; }
        public int AuthorizerModuleTempId { get; set; }

        [Required]
        public int AuthorizerId { get; set; }

        [Required]
        public int ModuleId { get; set; }

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
