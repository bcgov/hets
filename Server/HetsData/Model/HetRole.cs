using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetRole
    {
        public HetRole()
        {
            HetRolePermission = new HashSet<HetRolePermission>();
            HetUserRole = new HashSet<HetUserRole>();
            HetBusinessUserRole = new HashSet<HetBusinessUserRole>();
        }

        [JsonProperty("Id")]
        public int RoleId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
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

        [JsonProperty("RolePermissions")]
        public ICollection<HetRolePermission> HetRolePermission { get; set; }

        [JsonIgnore]
        public ICollection<HetUserRole> HetUserRole { get; set; }

        [JsonIgnore]
        public ICollection<HetBusinessUserRole> HetBusinessUserRole { get; set; }
    }
}
