using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetBusinessUser
    {
        public HetBusinessUser()
        {
            HetBusinessUserRoles = new HashSet<HetBusinessUserRole>();
        }

        public int BusinessUserId { get; set; }
        public string BceidUserId { get; set; }
        public string BceidGuid { get; set; }
        public string BceidDisplayName { get; set; }
        public string BceidFirstName { get; set; }
        public string BceidLastName { get; set; }
        public string BceidEmail { get; set; }
        public string BceidTelephone { get; set; }
        public int? BusinessId { get; set; }
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

        public virtual HetBusiness Business { get; set; }
        public virtual ICollection<HetBusinessUserRole> HetBusinessUserRoles { get; set; }
    }
}
