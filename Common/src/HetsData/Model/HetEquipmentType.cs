using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetEquipmentType
    {
        public HetEquipmentType()
        {
            HetDistrictEquipmentType = new HashSet<HetDistrictEquipmentType>();
        }

        [JsonProperty("Id")]
        public int EquipmentTypeId { get; set; }

        public string Name { get; set; }
        public float? BlueBookRateNumber { get; set; }
        public float? BlueBookSection { get; set; }
        public float? ExtendHours { get; set; }
        public int NumberOfBlocks { get; set; }
        public float? MaximumHours { get; set; }
        public float? MaxHoursSub { get; set; }
        [JsonIgnore]public DateTime DbCreateTimestamp { get; set; }
        [JsonIgnore]public string AppCreateUserDirectory { get; set; }
        [JsonIgnore]public DateTime DbLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserDirectory { get; set; }
        public bool IsDumpTruck { get; set; }
        [JsonIgnore]public DateTime AppCreateTimestamp { get; set; }
        [JsonIgnore]public string AppCreateUserGuid { get; set; }
        [JsonIgnore]public string AppCreateUserid { get; set; }
        [JsonIgnore]public DateTime AppLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserGuid { get; set; }
        [JsonIgnore]public string AppLastUpdateUserid { get; set; }
        [JsonIgnore]public string DbCreateUserId { get; set; }
        [JsonIgnore]public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        [JsonIgnore]
        public ICollection<HetDistrictEquipmentType> HetDistrictEquipmentType { get; set; }
    }
}
