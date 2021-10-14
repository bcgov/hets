using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class MimeTypeDto
    {
        [JsonProperty("Id")]
        public int MimeTypeId { get; set; }
        public string MimeTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
