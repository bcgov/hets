using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class BcbidRotationDoc
    {        
        [JsonProperty("Id")]
        public int NoteId { get; set; }

        public string NoteType { get; set; }
        public string Reason { get; set; }
        public DateTime AskedDate { get; set; }
        public bool? WasAsked { get; set; }
        public string OfferRefusalReason { get; set; }
        public bool? IsForceHire { get; set; }
        public int? EquipmentId { get; set; }
        public int? ProjectId { get; set; }
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
    }
}
