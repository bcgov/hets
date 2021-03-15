using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetBusinessUser
    {
        public HetBusinessUser()
        {
            HetBusinessUserRole = new HashSet<HetBusinessUserRole>();
        }

        [JsonProperty("Id")]
        public int BusinessUserId { get; set; }

        public string BceidUserId { get; set; }
        public string BceidGuid { get; set; }
        public string BceidDisplayName { get; set; }
        public string BceidFirstName { get; set; }
        public string BceidLastName { get; set; }
        public string BceidEmail { get; set; }
        public string BceidTelephone { get; set; }
        public int? BusinessId { get; set; }
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

        public HetBusiness Business { get; set; }

        [JsonProperty("UserRoles")]
        public ICollection<HetBusinessUserRole> HetBusinessUserRole { get; set; }
    }
}
