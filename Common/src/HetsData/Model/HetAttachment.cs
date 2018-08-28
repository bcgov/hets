using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetAttachment
    {
        [JsonProperty("Id")]
        public int AttachmentId { get; set; }

        public string Description { get; set; }
        public int? EquipmentId { get; set; }
        public string FileName { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        [JsonIgnore]public DateTime DbCreateTimestamp { get; set; }
        [JsonIgnore]public string AppCreateUserDirectory { get; set; }
        public byte[] FileContents { get; set; }
        [JsonIgnore]public DateTime DbLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserDirectory { get; set; }
        public string Type { get; set; }
        public int? RentalRequestId { get; set; }
        public string MimeType { get; set; }
        [JsonIgnore]public DateTime AppCreateTimestamp { get; set; }
        [JsonIgnore]public string AppCreateUserGuid { get; set; }
        [JsonIgnore]public string AppCreateUserid { get; set; }
        [JsonIgnore]public DateTime AppLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserGuid { get; set; }
        [JsonIgnore]public string AppLastUpdateUserid { get; set; }
        [JsonIgnore]public string DbCreateUserId { get; set; }
        [JsonIgnore]public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetEquipment Equipment { get; set; }
        public HetOwner Owner { get; set; }
        public HetProject Project { get; set; }
        public HetRentalRequest RentalRequest { get; set; }
    }
}
