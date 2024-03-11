using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetTimeRecordHist
    {
        public int TimeRecordHistId { get; set; }
        public int TimeRecordId { get; set; }

        private DateTime _effectiveDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime EffectiveDate {
            get => DateTime.SpecifyKind(_effectiveDate, DateTimeKind.Utc);
            set => _effectiveDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        private DateTime? _endDate;
        public DateTime? EndDate {
            get => _endDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _endDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }

        private DateTime? _enteredDate;
        public DateTime? EnteredDate {
            get => _enteredDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _enteredDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }

        private DateTime _workedDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime WorkedDate {
            get => DateTime.SpecifyKind(_workedDate, DateTimeKind.Utc);
            set => _workedDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        public int TimePeriodTypeId { get; set; }
        public float? Hours { get; set; }
        public int? RentalAgreementRateId { get; set; }
        public int? RentalAgreementId { get; set; }
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
    }
}
