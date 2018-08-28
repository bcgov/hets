using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetRentalRequestAttachment
    {
        [JsonProperty("Id")]
        public int RentalRequestAttachmentId { get; set; }

        public string Attachment { get; set; }
        public int? RentalRequestId { get; set; }
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

        public HetRentalRequest RentalRequest { get; set; }
    }
}
