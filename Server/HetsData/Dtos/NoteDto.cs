using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class NoteDto
    {
        [JsonProperty("Id")]
        public int NoteId { get; set; }
        public string Text { get; set; }
        public bool? IsNoLongerRelevant { get; set; }
        public int? EquipmentId { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        public int? RentalRequestId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        private DateTime _dbCreateTimeStamp = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        
        [JsonProperty("createDate")]
        public DateTime DbCreateTimeStamp {
            get => DateTime.SpecifyKind(_dbCreateTimeStamp, DateTimeKind.Utc);
            set => _dbCreateTimeStamp = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
}
