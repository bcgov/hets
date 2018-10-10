using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetDistrictStatus
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
        [JsonIgnore]public string AppCreateUserDirectory { get; set; }
        [JsonIgnore]public string AppCreateUserGuid { get; set; }
        [JsonIgnore]public string AppCreateUserid { get; set; }
        [JsonIgnore]public DateTime AppCreateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserDirectory { get; set; }
        [JsonIgnore]public string AppLastUpdateUserGuid { get; set; }
        [JsonIgnore]public string AppLastUpdateUserid { get; set; }
        [JsonIgnore]public DateTime AppLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string DbCreateUserId { get; set; }
        [JsonIgnore]public DateTime DbCreateTimestamp { get; set; }
        [JsonIgnore]public DateTime DbLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetDistrict District { get; set; }        
    }
}
