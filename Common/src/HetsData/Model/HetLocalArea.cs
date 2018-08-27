using System;
using System.Collections.Generic;

namespace HetsData.Model
{
    public partial class HetLocalArea
    {
        public HetLocalArea()
        {
            HetEquipment = new HashSet<HetEquipment>();
            HetLocalAreaRotationList = new HashSet<HetLocalAreaRotationList>();
            HetOwner = new HashSet<HetOwner>();
            HetRentalRequest = new HashSet<HetRentalRequest>();
            HetSeniorityAudit = new HashSet<HetSeniorityAudit>();
        }

        public int LocalAreaId { get; set; }
        public string Name { get; set; }
        public int? ServiceAreaId { get; set; }
        public DateTime? EndDate { get; set; }
        public int LocalAreaNumber { get; set; }
        public DateTime StartDate { get; set; }
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

        public HetServiceArea ServiceArea { get; set; }
        public ICollection<HetEquipment> HetEquipment { get; set; }
        public ICollection<HetLocalAreaRotationList> HetLocalAreaRotationList { get; set; }
        public ICollection<HetOwner> HetOwner { get; set; }
        public ICollection<HetRentalRequest> HetRentalRequest { get; set; }
        public ICollection<HetSeniorityAudit> HetSeniorityAudit { get; set; }
    }
}
