using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class ConditionTypeDto
    {
        [JsonProperty("Id")]
        public int ConditionTypeId { get; set; }
        public int? DistrictId { get; set; }
        public string ConditionTypeCode { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
    }
}
