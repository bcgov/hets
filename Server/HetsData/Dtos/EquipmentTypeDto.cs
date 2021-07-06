using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class EquipmentTypeDto
    {
        [JsonProperty("Id")]
        public int EquipmentTypeId { get; set; }
        public string Name { get; set; }
        public float? BlueBookRateNumber { get; set; }
        public float? BlueBookSection { get; set; }
        public bool IsDumpTruck { get; set; }
        public int NumberOfBlocks { get; set; }
        public float? ExtendHours { get; set; }
        public float? MaximumHours { get; set; }
        public float? MaxHoursSub { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
