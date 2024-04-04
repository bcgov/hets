using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class DigitalFileDto
    {
        [JsonProperty("Id")]
        public int DigitalFileId { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public int MimeTypeId { get; set; }
        public byte[] FileContents { get; set; }
        public int? EquipmentId { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        public int? RentalRequestId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public MimeTypeDto MimeType { get; set; }
        public int? FileSize { get; set; }
        public string LastUpdateUserid { get; set; }

        private DateTime? _lastUpdateTimestamp;
        public DateTime? LastUpdateTimestamp {
            get => _lastUpdateTimestamp is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _lastUpdateTimestamp = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string UserName { get; set; }
    }
}
