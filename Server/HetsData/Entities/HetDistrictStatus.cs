using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetDistrictStatus
    {
        public int DistrictId { get; set; }
        public int? CurrentFiscalYear { get; set; }
        public int? NextFiscalYear { get; set; }

        private DateTime? _rolloverStartDate;
        public DateTime? RolloverStartDate {
            get => _rolloverStartDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _rolloverStartDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        private DateTime? _rolloverEndDate;
        public DateTime? RolloverEndDate {
            get => _rolloverEndDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _rolloverEndDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int? LocalAreaCount { get; set; }
        public int? DistrictEquipmentTypeCount { get; set; }
        public int? LocalAreaCompleteCount { get; set; }
        public int? DistrictEquipmentTypeCompleteCount { get; set; }
        public int? ProgressPercentage { get; set; }
        public bool DisplayRolloverMessage { get; set; }
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

        public virtual HetDistrict District { get; set; }
    }
}
