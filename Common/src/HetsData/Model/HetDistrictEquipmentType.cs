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

        public string DistrictEquipmentName { get; set; }
        public int? DistrictId { get; set; }
        public int? EquipmentTypeId { get; set; }
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
