using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetBusiness
    {
        public HetBusiness()
        {
            HetOwner = new HashSet<HetOwner>();
            HetBusinessUser = new HashSet<HetBusinessUser>();
        }

        [JsonProperty("Id")]
        public int BusinessId { get; set; }

        public string BceidLegalName { get; set; }
        public string BceidDoingBusinessAs { get; set; }
        public string BceidBusinessNumber { get; set; }
        public string BceidBusinessGuid { get; set; }
        [JsonIgnore] public string AppCreateUserDirectory { get; set; }
        [JsonIgnore] public string AppCreateUserGuid { get; set; }
        [JsonIgnore] public string AppCreateUserid { get; set; }
        [JsonIgnore] public DateTime AppCreateTimestamp { get; set; }
        [JsonIgnore] public string AppLastUpdateUserDirectory { get; set; }
        [JsonIgnore] public string AppLastUpdateUserGuid { get; set; }
        [JsonIgnore] public string AppLastUpdateUserid { get; set; }
        [JsonIgnore] public DateTime AppLastUpdateTimestamp { get; set; }
        [JsonIgnore] public string DbCreateUserId { get; set; }
        [JsonIgnore] public DateTime DbCreateTimestamp { get; set; }
        [JsonIgnore] public DateTime DbLastUpdateTimestamp { get; set; }
        [JsonIgnore] public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        [JsonProperty("Owners")]
        public ICollection<HetOwner> HetOwner { get; set; }

        [JsonProperty("BusinessUsers")]
        public ICollection<HetBusinessUser> HetBusinessUser { get; set; }
    }
}
