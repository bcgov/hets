using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetNoteHist
    {
        public int NoteHistId { get; set; }
        public int NoteId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Text { get; set; }
        public bool? IsNoLongerRelevant { get; set; }
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
    }
}
