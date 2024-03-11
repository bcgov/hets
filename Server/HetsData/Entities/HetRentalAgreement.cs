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

        private DateTime? _estimateStartWork;
        public DateTime? EstimateStartWork {
            get => _estimateStartWork is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _estimateStartWork = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string Note { get; set; }
        public float? EquipmentRate { get; set; }
        public string RateComment { get; set; }
        public int RatePeriodTypeId { get; set; }

        private DateTime? _datedOn;
        public DateTime? DatedOn {
            get => _datedOn is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _datedOn = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int RentalAgreementStatusTypeId { get; set; }
        public int? EquipmentId { get; set; }
        public int? ProjectId { get; set; }
        public int? DistrictId { get; set; }
        public int? RentalRequestId { get; set; }
        public int? RentalRequestRotationListId { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }

        private DateTime _appCreateTimestamp = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime AppCreateTimestamp {
            get => DateTime.SpecifyKind(_appCreateTimestamp, DateTimeKind.Utc);
            set => _appCreateTimestamp = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        public string AppLastUpdateUserDirectory { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }

        private DateTime _appLastUpdateTimestamp = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime AppLastUpdateTimestamp {
            get => DateTime.SpecifyKind(_appLastUpdateTimestamp, DateTimeKind.Utc);
            set => _appLastUpdateTimestamp = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        public string DbCreateUserId { get; set; }

        private DateTime _dbCreateTimestamp = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime DbCreateTimestamp {
            get => DateTime.SpecifyKind(_dbCreateTimestamp, DateTimeKind.Utc);
            set => _dbCreateTimestamp = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime _dbLastUpdateTimestamp = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime DbLastUpdateTimestamp {
            get => DateTime.SpecifyKind(_dbLastUpdateTimestamp, DateTimeKind.Utc);
            set => _dbLastUpdateTimestamp = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
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
