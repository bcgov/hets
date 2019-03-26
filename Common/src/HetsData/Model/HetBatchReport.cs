using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetBatchReport
    {
        [JsonProperty("Id")]
        public int ReportId { get; set; }

        public string ReportName { get; set; }
        public string ReportLink { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Complete { get; set; }
        public int DistrictId { get; set; }
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
