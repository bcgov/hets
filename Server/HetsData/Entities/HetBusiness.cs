using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetBusiness
    {
        public HetBusiness()
        {
            HetBusinessUsers = new HashSet<HetBusinessUser>();
            HetOwners = new HashSet<HetOwner>();
        }

        public int BusinessId { get; set; }
        public string BceidLegalName { get; set; }
        public string BceidDoingBusinessAs { get; set; }
        public string BceidBusinessNumber { get; set; }
        public string BceidBusinessGuid { get; set; }
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

        public virtual ICollection<HetBusinessUser> HetBusinessUsers { get; set; }
        public virtual ICollection<HetOwner> HetOwners { get; set; }
    }
}
