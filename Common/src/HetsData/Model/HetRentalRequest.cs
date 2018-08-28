using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetRentalRequest
    {
        public HetRentalRequest()
        {
            HetAttachment = new HashSet<HetAttachment>();
            HetHistory = new HashSet<HetHistory>();
            HetNote = new HashSet<HetNote>();
            HetRentalRequestAttachment = new HashSet<HetRentalRequestAttachment>();
            HetRentalRequestRotationList = new HashSet<HetRentalRequestRotationList>();
        }

        [JsonProperty("Id")]
        public int RentalRequestId { get; set; }

        public int EquipmentCount { get; set; }
        public int? DistrictEquipmentTypeId { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public int? ExpectedHours { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public int? FirstOnRotationListId { get; set; }
        public int? LocalAreaId { get; set; }
        public int? ProjectId { get; set; }
        public string Status { get; set; }
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

        public HetDistrictEquipmentType DistrictEquipmentType { get; set; }
        public HetEquipment FirstOnRotationList { get; set; }
        public HetLocalArea LocalArea { get; set; }
        public HetProject Project { get; set; }

        [JsonIgnore]
        public ICollection<HetAttachment> HetAttachment { get; set; }

        [JsonIgnore]
        public ICollection<HetHistory> HetHistory { get; set; }

        [JsonIgnore]
        public ICollection<HetNote> HetNote { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalRequestAttachment> HetRentalRequestAttachment { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalRequestRotationList> HetRentalRequestRotationList { get; set; }
    }
}
