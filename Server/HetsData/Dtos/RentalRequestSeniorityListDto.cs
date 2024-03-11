using System;

namespace HetsData.Dtos
{
    public class RentalRequestSeniorityListDto
    {
        public int RentalRequestSeniorityListId { get; set; }
        public int RentalRequestId { get; set; }
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
        public bool WorkingNow { get; set; }
        public bool LastCalled { get; set; }
        public float YtdHours { get; set; }
    }
}
