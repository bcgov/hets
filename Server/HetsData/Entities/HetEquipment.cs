using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetEquipment
    {
        public HetEquipment()
        {
            HetDigitalFiles = new HashSet<HetDigitalFile>();
            HetEquipmentAttachments = new HashSet<HetEquipmentAttachment>();
            HetHistories = new HashSet<HetHistory>();
            HetNotes = new HashSet<HetNote>();
            HetRentalAgreements = new HashSet<HetRentalAgreement>();
            HetRentalRequestRotationLists = new HashSet<HetRentalRequestRotationList>();
            HetRentalRequestSeniorityLists = new HashSet<HetRentalRequestSeniorityList>();
            HetRentalRequests = new HashSet<HetRentalRequest>();
            HetSeniorityAudits = new HashSet<HetSeniorityAudit>();
        }

        public int EquipmentId { get; set; }
        public string Type { get; set; }
        public string EquipmentCode { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }

        private DateTime _receivedDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime ReceivedDate {
            get => DateTime.SpecifyKind(_receivedDate, DateTimeKind.Utc);
            set => _receivedDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        public float? YearsOfService { get; set; }
        public string LicencePlate { get; set; }
        public string SerialNumber { get; set; }
        public string Size { get; set; }
        public float? Seniority { get; set; }

        private DateTime? _seniorityEffectiveDate;
        public DateTime? SeniorityEffectiveDate {
            get => _seniorityEffectiveDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _seniorityEffectiveDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }

        private DateTime? _toDate;
        public DateTime? ToDate {
            get => _toDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _toDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int? NumberInBlock { get; set; }
        public int? BlockNumber { get; set; }
        public float? ServiceHoursLastYear { get; set; }
        public float? ServiceHoursThreeYearsAgo { get; set; }
        public float? ServiceHoursTwoYearsAgo { get; set; }
        public bool? IsSeniorityOverridden { get; set; }
        public string SeniorityOverrideReason { get; set; }

        private DateTime? _approvedDate;
        public DateTime? ApprovedDate {
            get => _approvedDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _approvedDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public int EquipmentStatusTypeId { get; set; }
        public string StatusComment { get; set; }

        private DateTime? _archiveDate;
        public DateTime? ArchiveDate {
            get => _archiveDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _archiveDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string ArchiveCode { get; set; }
        public string ArchiveReason { get; set; }

        private DateTime _lastVerifiedDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime LastVerifiedDate {
            get => DateTime.SpecifyKind(_lastVerifiedDate, DateTimeKind.Utc);
            set => _lastVerifiedDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        public string InformationUpdateNeededReason { get; set; }
        public bool? IsInformationUpdateNeeded { get; set; }
        public int? DistrictEquipmentTypeId { get; set; }
        public int? LocalAreaId { get; set; }
        public string Operator { get; set; }
        public int? OwnerId { get; set; }
        public float? PayRate { get; set; }
        public string RefuseRate { get; set; }
        public string LegalCapacity { get; set; }
        public string LicencedGvw { get; set; }
        public string PupLegalCapacity { get; set; }
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

        public virtual HetDistrictEquipmentType DistrictEquipmentType { get; set; }
        public virtual HetEquipmentStatusType EquipmentStatusType { get; set; }
        public virtual HetLocalArea LocalArea { get; set; }
        public virtual HetOwner Owner { get; set; }
        public virtual ICollection<HetDigitalFile> HetDigitalFiles { get; set; }
        public virtual ICollection<HetEquipmentAttachment> HetEquipmentAttachments { get; set; }
        public virtual ICollection<HetHistory> HetHistories { get; set; }
        public virtual ICollection<HetNote> HetNotes { get; set; }
        public virtual ICollection<HetRentalAgreement> HetRentalAgreements { get; set; }
        public virtual ICollection<HetRentalRequestRotationList> HetRentalRequestRotationLists { get; set; }
        public virtual ICollection<HetRentalRequestSeniorityList> HetRentalRequestSeniorityLists { get; set; }
        public virtual ICollection<HetRentalRequest> HetRentalRequests { get; set; }
        public virtual ICollection<HetSeniorityAudit> HetSeniorityAudits { get; set; }
    }
}
