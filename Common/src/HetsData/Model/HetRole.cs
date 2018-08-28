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
        }

        [JsonProperty("Id")]
        public int RoleId { get; set; }

        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        [JsonIgnore]
        public ICollection<HetRolePermission> HetRolePermission { get; set; }

        [JsonIgnore]
        public ICollection<HetUserRole> HetUserRole { get; set; }
    }
}
