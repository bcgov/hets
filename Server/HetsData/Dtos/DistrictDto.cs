using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class DistrictDto
    {
        [JsonProperty("Id")]
        public int DistrictId { get; set; }
        public int? DistrictNumber { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MinistryDistrictId { get; set; }
        public int? RegionId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public RegionDto Region { get; set; }
    }
}
