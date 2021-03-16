using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetHistory
    {
        [JsonProperty("Id")]
        public int HistoryId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string HistoryText { get; set; }
        public int? EquipmentId { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        public int? RentalRequestId { get; set; }
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

        [JsonIgnore]
        public HetEquipment Equipment { get; set; }

        [JsonIgnore]
        public HetOwner Owner { get; set; }

        [JsonIgnore]
        public HetProject Project { get; set; }

        [JsonIgnore]
        public HetRentalRequest RentalRequest { get; set; }
    }
}
