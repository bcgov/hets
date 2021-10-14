using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class EquipmentStatusTypeDto
    {
        [JsonProperty("Id")]
        public int EquipmentStatusTypeId { get; set; }
        public string EquipmentStatusTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
