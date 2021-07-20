using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRentalAgreementRate
    {
        public HetRentalAgreementRate()
        {
            HetTimeRecords = new HashSet<HetTimeRecord>();
        }

        public int RentalAgreementRateId { get; set; }
        public string Comment { get; set; }
        public string ComponentName { get; set; }
        public float? Rate { get; set; }
        public bool? Overtime { get; set; }
        public bool? Active { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public int? RentalAgreementId { get; set; }
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
        public bool? Set { get; set; }
        public int? RatePeriodTypeId { get; set; }

        public virtual HetRatePeriodType RatePeriodType { get; set; }
        public virtual HetRentalAgreement RentalAgreement { get; set; }
        public virtual ICollection<HetTimeRecord> HetTimeRecords { get; set; }
    }
}
