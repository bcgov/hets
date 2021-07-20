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
        public DateTime? EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int? ServiceAreaId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public ServiceAreaDto ServiceArea { get; set; }
    }
}
