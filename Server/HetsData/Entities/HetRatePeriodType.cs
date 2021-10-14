using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRatePeriodType
    {
        public HetRatePeriodType()
        {
            HetRentalAgreementRates = new HashSet<HetRentalAgreementRate>();
            HetRentalAgreements = new HashSet<HetRentalAgreement>();
        }

        public int RatePeriodTypeId { get; set; }
        public string RatePeriodTypeCode { get; set; }
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

        public virtual ICollection<HetRentalAgreementRate> HetRentalAgreementRates { get; set; }
        public virtual ICollection<HetRentalAgreement> HetRentalAgreements { get; set; }
    }
}
