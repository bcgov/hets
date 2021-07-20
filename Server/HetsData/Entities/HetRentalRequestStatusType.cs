using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRentalRequestStatusType
    {
        public HetRentalRequestStatusType()
        {
            HetRentalRequests = new HashSet<HetRentalRequest>();
        }

        public int RentalRequestStatusTypeId { get; set; }
        public string RentalRequestStatusTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
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

        public virtual ICollection<HetRentalRequest> HetRentalRequests { get; set; }
    }
}
