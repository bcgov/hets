using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class EquipmentAttachmentDto
    {
        [JsonProperty("Id")]
        public int EquipmentAttachmentId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int? EquipmentId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public EquipmentDto Equipment { get; set; }
    }
}
