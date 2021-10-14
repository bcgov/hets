using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetLocalArea
    {
        public HetLocalArea()
        {
            HetEquipments = new HashSet<HetEquipment>();
            HetOwners = new HashSet<HetOwner>();
            HetRentalRequestSeniorityLists = new HashSet<HetRentalRequestSeniorityList>();
            HetRentalRequests = new HashSet<HetRentalRequest>();
            HetSeniorityAudits = new HashSet<HetSeniorityAudit>();
        }

        public int LocalAreaId { get; set; }
        public int LocalAreaNumber { get; set; }
        public string Name { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime StartDate { get; set; }
        public int? ServiceAreaId { get; set; }
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

        public virtual HetServiceArea ServiceArea { get; set; }
        public virtual ICollection<HetEquipment> HetEquipments { get; set; }
        public virtual ICollection<HetOwner> HetOwners { get; set; }
        public virtual ICollection<HetRentalRequestSeniorityList> HetRentalRequestSeniorityLists { get; set; }
        public virtual ICollection<HetRentalRequest> HetRentalRequests { get; set; }
        public virtual ICollection<HetSeniorityAudit> HetSeniorityAudits { get; set; }
    }
}
