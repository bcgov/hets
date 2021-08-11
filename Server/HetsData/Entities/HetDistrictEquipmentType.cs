using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetDistrictEquipmentType
    {
        public HetDistrictEquipmentType()
        {
            HetEquipments = new HashSet<HetEquipment>();
            HetRentalRequestSeniorityLists = new HashSet<HetRentalRequestSeniorityList>();
            HetRentalRequests = new HashSet<HetRentalRequest>();
        }

        public int DistrictEquipmentTypeId { get; set; }
        public string DistrictEquipmentName { get; set; }
        public int? DistrictId { get; set; }
        public int? EquipmentTypeId { get; set; }
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
        public bool Deleted { get; set; }
        public int? ServiceAreaId { get; set; }

        public virtual HetDistrict District { get; set; }
        public virtual HetEquipmentType EquipmentType { get; set; }
        public virtual ICollection<HetEquipment> HetEquipments { get; set; }
        public virtual ICollection<HetRentalRequestSeniorityList> HetRentalRequestSeniorityLists { get; set; }
        public virtual ICollection<HetRentalRequest> HetRentalRequests { get; set; }
    }
}
