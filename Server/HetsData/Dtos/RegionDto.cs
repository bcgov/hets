using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class RegionDto
    {
        [JsonProperty("Id")]
        public int RegionId { get; set; }
        public string Name { get; set; }
        public int? RegionNumber { get; set; }
        public int MinistryRegionId { get; set; }

        private DateTime _startDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime StartDate {
            get => DateTime.SpecifyKind(_startDate, DateTimeKind.Utc);
            set => _startDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        private DateTime? _endDate;
        public DateTime? EndDate {
            get => _endDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _endDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int ConcurrencyControlNumber { get; set; }
    }
}
