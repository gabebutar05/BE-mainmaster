using static API_Dinamis.Utilities.DtoUtils;

namespace API_Dinamis.Dto
{
    public class StandardValueForm
    {
        public int ID { get; set; }
        public int? StandardValueID { get; set; }
        public string DataValue { get; set; }
        public bool? ValueInPercentage { get; set; } = false;
        public string? Remarks { get; set; } = "";
        public string? ActionRemarks { get; set; } = "";
        public string UpdatedBy { get; set; } = null!;
        [SpecificStringValue("main", "temp")]
        public string? Target { get; set; } = "main";

    }
    public class StandardValueDto
    {
        public int ID { get; set; }
        public string DataName { get; set; }
        public bool ValueInPercentage { get; set; }
        public string Description { get; set; }
        public string DataValue { get; set; }
        public string ValueType { get; set; }
    }
}
