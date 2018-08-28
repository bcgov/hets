using System;

namespace HetsData.Model
{
    public partial class HetRentalAgreementHist
    {
        public int RentalAgreementHistId { get; set; }
        public int RentalAgreementId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EquipmentId { get; set; }
        public int? ProjectId { get; set; }
        public string Status { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime? DatedOn { get; set; }
        public float? EquipmentRate { get; set; }
        public int? EstimateHours { get; set; }
        public DateTime? EstimateStartWork { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string Note { get; set; }
        public string Number { get; set; }
        public string RateComment { get; set; }
        public string RatePeriod { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
    }
}
