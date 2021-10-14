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
