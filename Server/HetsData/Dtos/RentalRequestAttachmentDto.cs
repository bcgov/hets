using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class RentalRequestAttachmentDto
    {
        [JsonProperty("Id")]
        public int RentalRequestAttachmentId { get; set; }
        public string Attachment { get; set; }
        public int? RentalRequestId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public RentalRequestDto RentalRequest { get; set; }
    }
}
