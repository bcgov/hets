using System;
using System.Collections.Generic;

namespace HetsData.Model
{
    public partial class HetAttachment
    {
        public int AttachmentId { get; set; }
        public string Description { get; set; }
        public int? EquipmentId { get; set; }
        public string FileName { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public byte[] FileContents { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string Type { get; set; }
        public int? RentalRequestId { get; set; }
        public string MimeType { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetEquipment Equipment { get; set; }
        public HetOwner Owner { get; set; }
        public HetProject Project { get; set; }
        public HetRentalRequest RentalRequest { get; set; }
    }
}
