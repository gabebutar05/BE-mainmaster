namespace API_Dinamis.Dto
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string Remarks { get; set; }
    }

    public class RoleDetailDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public bool Run { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public string ActionDetail { get; set; }
    }

    public class RoleResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string Remarks { get; set; }
        public string UpdatedBy { get; set; }
        public virtual ICollection<RoleDetailDto> detail { get; set; }
    }

    public class RoleJoinDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }

    public class RoleDetailJoinDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public bool Run { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }

    }

    public class RoleResultJoinDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public virtual ICollection<RoleDetailJoinDto> detail { get; set; }
    }

    public class RoleJoinDtoPending
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
    }

    public class RoleDetailJoinDtoPending
    {
        public int Id { get; set; }
        public int RoleTempId { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public bool Run { get; set; }
        public bool Add { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public string Action { get; set; }
        public string ActionRemarks { get; set; }
        public string Remarks { get; set; }
        public string ActionDetail { get; set; }
        public int RoleId { get; set; }
        public int RoleDetailId { get; set; }
        public string UpdatedBy { get; set; }
    }

    public class RoleResultJoinDtoPending
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public virtual ICollection<RoleDetailJoinDtoPending> detail { get; set; }
    }

    public class RoleResultJoinDtoPendingUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public string Remarks { get; set; }
        public string ActionRemarks { get; set; }
        public virtual ICollection<RoleDetailJoinDtoPending> detail { get; set; }
    }

    public class RoleDtoForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public string Remarks { get; set; }
    }
}
