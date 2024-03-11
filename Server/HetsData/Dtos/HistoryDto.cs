using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class HistoryDto
    {
        [JsonProperty("Id")]
        public int HistoryId { get; set; }

        private DateTime? _createdDate;
        public DateTime? CreatedDate {
            get => _createdDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _createdDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string HistoryText { get; set; }
        public int? EquipmentId { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        public int? RentalRequestId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
