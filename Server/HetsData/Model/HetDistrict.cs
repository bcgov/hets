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
            HetDistrictStatus = new HashSet<HetDistrictStatus>();
            HetDistrictEquipmentType = new HashSet<HetDistrictEquipmentType>();
            HetProject = new HashSet<HetProject>();
            HetServiceArea = new HashSet<HetServiceArea>();
            HetUser = new HashSet<HetUser>();
            HetUserDistrict = new HashSet<HetUserDistrict>();
            HetUserFavourite = new HashSet<HetUserFavourite>();
            HetRentalAgreement = new HashSet<HetRentalAgreement>();
        }

        [JsonProperty("Id")]
        public int DistrictId { get; set; }

        public int? DistrictNumber { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MinistryDistrictId { get; set; }
        public int? RegionId { get; set; }
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

        public HetRegion Region { get; set; }

        [JsonIgnore]
        public ICollection<HetConditionType> HetConditionType { get; set; }

        [JsonIgnore]
        public ICollection<HetDistrictStatus> HetDistrictStatus { get; set; }

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

        [JsonIgnore]
        public ICollection<HetUserFavourite> HetUserFavourite { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalAgreement> HetRentalAgreement { get; set; }
    }
}
