using System;
using System.Collections.Generic;

namespace HetsData.Model
{
    public partial class HetEquipment
    {
        public HetEquipment()
        {
            HetAttachment = new HashSet<HetAttachment>();
            HetEquipmentAttachment = new HashSet<HetEquipmentAttachment>();
            HetHistory = new HashSet<HetHistory>();
            HetLocalAreaRotationListAskNextBlock1 = new HashSet<HetLocalAreaRotationList>();
            HetLocalAreaRotationListAskNextBlock2 = new HashSet<HetLocalAreaRotationList>();
            HetLocalAreaRotationListAskNextBlockOpen = new HashSet<HetLocalAreaRotationList>();
            HetNote = new HashSet<HetNote>();
            HetRentalAgreement = new HashSet<HetRentalAgreement>();
            HetRentalRequest = new HashSet<HetRentalRequest>();
            HetRentalRequestRotationList = new HashSet<HetRentalRequestRotationList>();
            HetSeniorityAudit = new HashSet<HetSeniorityAudit>();
        }

        public int EquipmentId { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public string ArchiveReason { get; set; }
        public int? BlockNumber { get; set; }
        public string ArchiveCode { get; set; }
        public int? DistrictEquipmentTypeId { get; set; }
        public string EquipmentCode { get; set; }
        public int? LocalAreaId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Operator { get; set; }
        public int? OwnerId { get; set; }
        public float? PayRate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string RefuseRate { get; set; }
        public string LicencePlate { get; set; }
        public float? Seniority { get; set; }
        public string SerialNumber { get; set; }
        public string Size { get; set; }
        public string Status { get; set; }
        public DateTime? ToDate { get; set; }
        public float? YearsOfService { get; set; }
        public float? ServiceHoursLastYear { get; set; }
        public float? ServiceHoursThreeYearsAgo { get; set; }
        public float? ServiceHoursTwoYearsAgo { get; set; }
        public string Year { get; set; }
        public DateTime LastVerifiedDate { get; set; }
        public string InformationUpdateNeededReason { get; set; }
        public bool? IsInformationUpdateNeeded { get; set; }
        public DateTime? SeniorityEffectiveDate { get; set; }
        public bool? IsSeniorityOverridden { get; set; }
        public string SeniorityOverrideReason { get; set; }
        public int? NumberInBlock { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public string Type { get; set; }
        public string StatusComment { get; set; }
        public string LegalCapacity { get; set; }
        public string LicencedGvw { get; set; }
        public string PupLegalCapacity { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetDistrictEquipmentType DistrictEquipmentType { get; set; }
        public HetLocalArea LocalArea { get; set; }
        public HetOwner Owner { get; set; }
        public ICollection<HetAttachment> HetAttachment { get; set; }
        public ICollection<HetEquipmentAttachment> HetEquipmentAttachment { get; set; }
        public ICollection<HetHistory> HetHistory { get; set; }
        public ICollection<HetLocalAreaRotationList> HetLocalAreaRotationListAskNextBlock1 { get; set; }
        public ICollection<HetLocalAreaRotationList> HetLocalAreaRotationListAskNextBlock2 { get; set; }
        public ICollection<HetLocalAreaRotationList> HetLocalAreaRotationListAskNextBlockOpen { get; set; }
        public ICollection<HetNote> HetNote { get; set; }
        public ICollection<HetRentalAgreement> HetRentalAgreement { get; set; }
        public ICollection<HetRentalRequest> HetRentalRequest { get; set; }
        public ICollection<HetRentalRequestRotationList> HetRentalRequestRotationList { get; set; }
        public ICollection<HetSeniorityAudit> HetSeniorityAudit { get; set; }
    }
}
