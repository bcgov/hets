using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class RentalRequestStatusTypeDto
    {
        [JsonProperty("Id")]
        public int RentalRequestStatusTypeId { get; set; }
        public string RentalRequestStatusTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
