using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetDigitalFile
    {
        public int DigitalFileId { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Type { get; set; }
        public int MimeTypeId { get; set; }
        public byte[] FileContents { get; set; }
        public int? EquipmentId { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        public int? RentalRequestId { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string DbCreateUserId { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public virtual HetEquipment Equipment { get; set; }
        public virtual HetMimeType MimeType { get; set; }
        public virtual HetOwner Owner { get; set; }
        public virtual HetProject Project { get; set; }
        public virtual HetRentalRequest RentalRequest { get; set; }
    }
}
