using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRole
    {
        public HetRole()
        {
            HetBusinessUserRoles = new HashSet<HetBusinessUserRole>();
            HetRolePermissions = new HashSet<HetRolePermission>();
            HetUserRoles = new HashSet<HetUserRole>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string DbCreateUserId { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public virtual ICollection<HetBusinessUserRole> HetBusinessUserRoles { get; set; }
        public virtual ICollection<HetRolePermission> HetRolePermissions { get; set; }
        public virtual ICollection<HetUserRole> HetUserRoles { get; set; }
    }
}
