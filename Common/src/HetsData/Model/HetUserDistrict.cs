using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetUserDistrict
    {
        [JsonProperty("Id")]
        public int UserDistrictId { get; set; }

        public bool IsPrimary { get; set; }
        public int? UserId { get; set; }
        public int? DistrictId { get; set; }
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
        public HetUser User { get; set; }
    }
}
