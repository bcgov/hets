using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class BatchReportDto
    {
        [JsonProperty("Id")]
        public int ReportId { get; set; }

        public string ReportName { get; set; }
        public string ReportLink { get; set; }

        private DateTime? _startDate;
        public DateTime? StartDate {
            get => _startDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _startDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }

        private DateTime? _endDate;
        public DateTime? EndDate {
            get => _endDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _endDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public bool Complete { get; set; }
        public int DistrictId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
        public string Status { get => Complete ? "Complete" : "In Progress"; }

    }
}
