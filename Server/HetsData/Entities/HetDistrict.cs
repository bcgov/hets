using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetDistrict
    {
        public HetDistrict()
        {
            HetBatchReports = new HashSet<HetBatchReport>();
            HetConditionTypes = new HashSet<HetConditionType>();
            HetDistrictEquipmentTypes = new HashSet<HetDistrictEquipmentType>();
            HetProjects = new HashSet<HetProject>();
            HetRentalAgreements = new HashSet<HetRentalAgreement>();
            HetServiceAreas = new HashSet<HetServiceArea>();
            HetUserDistricts = new HashSet<HetUserDistrict>();
            HetUserFavourites = new HashSet<HetUserFavourite>();
            HetUsers = new HashSet<HetUser>();
        }

        public int DistrictId { get; set; }
        public int? DistrictNumber { get; set; }
        public string Name { get; set; }

        private DateTime _startDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime StartDate {
            get => DateTime.SpecifyKind(_startDate, DateTimeKind.Utc);
            set => _startDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        private DateTime? _endDate;
        public DateTime? EndDate {
            get => _endDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _endDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int MinistryDistrictId { get; set; }
        public int? RegionId { get; set; }
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

        public virtual HetRegion Region { get; set; }
        public virtual HetDistrictStatus HetDistrictStatus { get; set; }
        public virtual HetRolloverProgress HetRolloverProgress { get; set; }
        public virtual ICollection<HetBatchReport> HetBatchReports { get; set; }
        public virtual ICollection<HetConditionType> HetConditionTypes { get; set; }
        public virtual ICollection<HetDistrictEquipmentType> HetDistrictEquipmentTypes { get; set; }
        public virtual ICollection<HetProject> HetProjects { get; set; }
        public virtual ICollection<HetRentalAgreement> HetRentalAgreements { get; set; }
        public virtual ICollection<HetServiceArea> HetServiceAreas { get; set; }
        public virtual ICollection<HetUserDistrict> HetUserDistricts { get; set; }
        public virtual ICollection<HetUserFavourite> HetUserFavourites { get; set; }
        public virtual ICollection<HetUser> HetUsers { get; set; }
    }
}
