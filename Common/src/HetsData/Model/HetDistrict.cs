using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetDistrict
    {
        public HetDistrict()
        {
            HetConditionType = new HashSet<HetConditionType>();
            HetDistrictEquipmentType = new HashSet<HetDistrictEquipmentType>();
            HetProject = new HashSet<HetProject>();
            HetServiceArea = new HashSet<HetServiceArea>();
            HetUser = new HashSet<HetUser>();
            HetUserDistrict = new HashSet<HetUserDistrict>();
        }

        [JsonProperty("Id")]
        public int DistrictId { get; set; }

        public DateTime? EndDate { get; set; }
        public int MinistryDistrictId { get; set; }
        public string Name { get; set; }
        public int? RegionId { get; set; }
        public DateTime StartDate { get; set; }
        public int? DistrictNumber { get; set; }
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

        public HetRegion Region { get; set; }

        [JsonIgnore]
        public ICollection<HetConditionType> HetConditionType { get; set; }

        [JsonIgnore]
        public ICollection<HetDistrictEquipmentType> HetDistrictEquipmentType { get; set; }

        [JsonIgnore]
        public ICollection<HetProject> HetProject { get; set; }

        [JsonIgnore]
        public ICollection<HetServiceArea> HetServiceArea { get; set; }

        [JsonIgnore]
        public ICollection<HetUser> HetUser { get; set; }

        [JsonIgnore]
        public ICollection<HetUserDistrict> HetUserDistrict { get; set; }
    }
}
