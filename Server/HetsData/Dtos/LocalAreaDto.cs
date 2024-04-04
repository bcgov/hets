using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class LocalAreaDto
    {
        [JsonProperty("Id")]
        public int LocalAreaId { get; set; }
        public int LocalAreaNumber { get; set; }
        public string Name { get; set; }

        private DateTime? _endDate;
        public DateTime? EndDate {
            get => _endDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _endDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        private DateTime _startDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime StartDate {
            get => DateTime.SpecifyKind(_startDate, DateTimeKind.Utc);
            set => _startDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        public int? ServiceAreaId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public ServiceAreaDto ServiceArea { get; set; }
    }
}
