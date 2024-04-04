using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRentalAgreementCondition
    {
        public int RentalAgreementConditionId { get; set; }
        public string Comment { get; set; }
        public string ConditionName { get; set; }
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

        public virtual HetRentalAgreement RentalAgreement { get; set; }
    }
}
