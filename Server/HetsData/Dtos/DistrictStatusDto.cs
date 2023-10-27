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

        private DateTime _rolloverStartDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime RolloverStartDate {
            get => DateTime.SpecifyKind(_rolloverStartDate, DateTimeKind.Utc);
            set => _rolloverStartDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        private DateTime _rolloverEndDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime RolloverEndDate {
            get => DateTime.SpecifyKind(_rolloverEndDate, DateTimeKind.Utc);
            set => _rolloverEndDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
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
