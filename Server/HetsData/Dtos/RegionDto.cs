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
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
