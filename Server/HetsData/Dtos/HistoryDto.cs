using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class HistoryDto
    {
        [JsonProperty("Id")]
        public int HistoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string HistoryText { get; set; }
        public int? EquipmentId { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        public int? RentalRequestId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
