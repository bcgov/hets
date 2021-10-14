using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class EquipmentDto
    {
        public EquipmentDto()
        {
            EquipmentAttachments = new List<EquipmentAttachmentDto>();
        }

        [JsonProperty("Id")]
        public int EquipmentId { get; set; }
        public string Type { get; set; }
        public string EquipmentCode { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public DateTime ReceivedDate { get; set; }
        public float? YearsOfService { get; set; }
        public string LicencePlate { get; set; }
        public string SerialNumber { get; set; }
        public string Size { get; set; }
        public float? Seniority { get; set; }
        public DateTime? SeniorityEffectiveDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? NumberInBlock { get; set; }
        public int? BlockNumber { get; set; }
        public float? ServiceHoursLastYear { get; set; }
        public float? ServiceHoursThreeYearsAgo { get; set; }
        public float? ServiceHoursTwoYearsAgo { get; set; }
        public bool? IsSeniorityOverridden { get; set; }
        public string SeniorityOverrideReason { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int EquipmentStatusTypeId { get; set; }
        public string StatusComment { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public string ArchiveCode { get; set; }
        public string ArchiveReason { get; set; }
        public DateTime LastVerifiedDate { get; set; }
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
        public int ConcurrencyControlNumber { get; set; }
        public DistrictEquipmentTypeDto DistrictEquipmentType { get; set; }
        public EquipmentStatusTypeDto EquipmentStatusType { get; set; }
        public LocalAreaDto LocalArea { get; set; }
        public OwnerDto Owner { get; set; }
        public List<EquipmentAttachmentDto> EquipmentAttachments { get; set; }

        public bool ActiveRentalRequest { get; set; }
        public int EquipmentNumber { get; set; }
        public string Status { get; set; }
        public int SenioritySortOrder { get; set; }
        public bool IsHired { get; set; }
        public float? HoursYtd { get; set; }
        public int NumberOfBlocks { get; set; }
        public int MaximumHours { get; set; }
        public string SeniorityString { get; set; }
        public string OwnerName { get; set; }
        public string EquipmentType { get; set; }
        public int AttachmentCount { get; set; }
        public string YearMinus1 { get; set; }
        public string YearMinus2 { get; set; }
        public string YearMinus3 { get; set; }
        public string EquipmentDetails { get; set; }
    }
}
