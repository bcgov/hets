using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetDistrictEquipmentType
    {
        public HetDistrictEquipmentType()
        {
            HetEquipment = new HashSet<HetEquipment>();
            HetLocalAreaRotationList = new HashSet<HetLocalAreaRotationList>();
            HetRentalRequest = new HashSet<HetRentalRequest>();
        }

        [JsonProperty("Id")]
        public int DistrictEquipmentTypeId { get; set; }

        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public string DistrictEquipmentName { get; set; }
        public int? DistrictId { get; set; }
        public int? EquipmentTypeId { get; set; }
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

        public HetDistrict District { get; set; }
        public HetEquipmentType EquipmentType { get; set; }

        [JsonIgnore]
        public ICollection<HetEquipment> HetEquipment { get; set; }

        [JsonIgnore]
        public ICollection<HetLocalAreaRotationList> HetLocalAreaRotationList { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalRequest> HetRentalRequest { get; set; }
    }
}
