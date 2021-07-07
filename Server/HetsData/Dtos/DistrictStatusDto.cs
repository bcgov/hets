using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class DistrictStatusDto
    {
        [JsonProperty("Id")]
        public int DistrictId { get; set; }
        public int? CurrentFiscalYear { get; set; }
        public int? NextFiscalYear { get; set; }
        public DateTime RolloverStartDate { get; set; }
        public DateTime RolloverEndDate { get; set; }
        public int? LocalAreaCount { get; set; }
        public int? DistrictEquipmentTypeCount { get; set; }
        public int? LocalAreaCompleteCount { get; set; }
        public int? DistrictEquipmentTypeCompleteCount { get; set; }
        public int? ProgressPercentage { get; set; }
        public bool? DisplayRolloverMessage { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
    }
}
