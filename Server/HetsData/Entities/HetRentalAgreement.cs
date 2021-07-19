using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRentalAgreement
    {
        public HetRentalAgreement()
        {
            HetRentalAgreementConditions = new HashSet<HetRentalAgreementCondition>();
            HetRentalAgreementRates = new HashSet<HetRentalAgreementRate>();
            HetRentalRequestRotationLists = new HashSet<HetRentalRequestRotationList>();
            HetTimeRecords = new HashSet<HetTimeRecord>();
        }

        public int RentalAgreementId { get; set; }
        public string Number { get; set; }
        public int? EstimateHours { get; set; }
        public DateTime? EstimateStartWork { get; set; }
        public string Note { get; set; }
        public float? EquipmentRate { get; set; }
        public string RateComment { get; set; }
        public int RatePeriodTypeId { get; set; }
        public DateTime? DatedOn { get; set; }
        public int RentalAgreementStatusTypeId { get; set; }
        public int? EquipmentId { get; set; }
        public int? ProjectId { get; set; }
        public int? DistrictId { get; set; }
        public int? RentalRequestId { get; set; }
        public int? RentalRequestRotationListId { get; set; }
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
        public string AgreementCity { get; set; }

        public virtual HetDistrict District { get; set; }
        public virtual HetEquipment Equipment { get; set; }
        public virtual HetProject Project { get; set; }
        public virtual HetRatePeriodType RatePeriodType { get; set; }
        public virtual HetRentalAgreementStatusType RentalAgreementStatusType { get; set; }
        public virtual HetRentalRequest RentalRequest { get; set; }
        public virtual HetRentalRequestRotationList RentalRequestRotationList { get; set; }
        public virtual ICollection<HetRentalAgreementCondition> HetRentalAgreementConditions { get; set; }
        public virtual ICollection<HetRentalAgreementRate> HetRentalAgreementRates { get; set; }
        public virtual ICollection<HetRentalRequestRotationList> HetRentalRequestRotationLists { get; set; }
        public virtual ICollection<HetTimeRecord> HetTimeRecords { get; set; }
    }
}
