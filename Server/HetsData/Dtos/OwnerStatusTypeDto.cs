using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class OwnerStatusTypeDto
    {
        [JsonProperty("Id")]
        public int OwnerStatusTypeId { get; set; }
        public string OwnerStatusTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
