using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetLocalAreaRotationList
    {
        [JsonProperty("Id")]
        public int LocalAreaRotationListId { get; set; }

        public int? AskNextBlock1Id { get; set; }
        public float? AskNextBlock1Seniority { get; set; }
        public int? AskNextBlock2Id { get; set; }
        public float? AskNextBlock2Seniority { get; set; }
        public int? AskNextBlockOpenId { get; set; }
        [JsonIgnore]public DateTime DbCreateTimestamp { get; set; }
        [JsonIgnore]public string AppCreateUserDirectory { get; set; }
        public int? DistrictEquipmentTypeId { get; set; }
        [JsonIgnore]public DateTime DbLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserDirectory { get; set; }
        public int? LocalAreaId { get; set; }
        [JsonIgnore]public DateTime AppCreateTimestamp { get; set; }
        [JsonIgnore]public string AppCreateUserGuid { get; set; }
        [JsonIgnore]public string AppCreateUserid { get; set; }
        [JsonIgnore]public DateTime AppLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserGuid { get; set; }
        [JsonIgnore]public string AppLastUpdateUserid { get; set; }
        [JsonIgnore]public string DbCreateUserId { get; set; }
        [JsonIgnore]public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetEquipment AskNextBlock1 { get; set; }
        public HetEquipment AskNextBlock2 { get; set; }
        public HetEquipment AskNextBlockOpen { get; set; }
        public HetDistrictEquipmentType DistrictEquipmentType { get; set; }
        public HetLocalArea LocalArea { get; set; }
    }
}
