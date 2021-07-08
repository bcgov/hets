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
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Complete { get; set; }
        public int DistrictId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
        public string Status { get => Complete ? "Complete" : "In Progress"; }

    }
}
