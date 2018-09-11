using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetProvincialRateType
    {
        public string RateType { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string PeriodType { get; set; }
        public float? Rate { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public bool IsPercentRate { get; set; }
        public bool IsRateEditable { get; set; }
        public bool IsInTotalEditable { get; set; }
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
    }
}
