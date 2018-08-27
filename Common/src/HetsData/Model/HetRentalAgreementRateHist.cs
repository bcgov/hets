using System;
using System.Collections.Generic;

namespace HetsData.Model
{
    public partial class HetRentalAgreementRateHist
    {
        public int RentalAgreementRateHistId { get; set; }
        public int RentalAgreementRateId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Comment { get; set; }
        public string ComponentName { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public bool? IsAttachment { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public float? PercentOfEquipmentRate { get; set; }
        public float? Rate { get; set; }
        public string RatePeriod { get; set; }
        public int? RentalAgreementId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public bool IsIncludedInTotal { get; set; }
    }
}
