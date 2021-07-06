using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class RatePeriodTypeDto
    {
        [JsonProperty("Id")]
        public int RatePeriodTypeId { get; set; }
        public string RatePeriodTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
