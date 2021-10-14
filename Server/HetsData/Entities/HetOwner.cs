using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetOwner
    {
        public HetOwner()
        {
            HetContacts = new HashSet<HetContact>();
            HetDigitalFiles = new HashSet<HetDigitalFile>();
            HetEquipments = new HashSet<HetEquipment>();
            HetHistories = new HashSet<HetHistory>();
            HetNotes = new HashSet<HetNote>();
            HetRentalRequestSeniorityLists = new HashSet<HetRentalRequestSeniorityList>();
            HetSeniorityAudits = new HashSet<HetSeniorityAudit>();
        }

        public int OwnerId { get; set; }
        public string OrganizationName { get; set; }
        public string OwnerCode { get; set; }
        public string DoingBusinessAs { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string RegisteredCompanyNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public int OwnerStatusTypeId { get; set; }
        public string StatusComment { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public string ArchiveCode { get; set; }
        public string ArchiveReason { get; set; }
        public int? LocalAreaId { get; set; }
        public int? PrimaryContactId { get; set; }
        public string CglCompany { get; set; }
        public string CglPolicyNumber { get; set; }
        public DateTime? CglendDate { get; set; }
        public string WorkSafeBcpolicyNumber { get; set; }
        public DateTime? WorkSafeBcexpiryDate { get; set; }
        public bool? IsMaintenanceContractor { get; set; }
        public bool MeetsResidency { get; set; }
        public int? BusinessId { get; set; }
        public string SharedKey { get; set; }
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

        public virtual HetBusiness Business { get; set; }
        public virtual HetLocalArea LocalArea { get; set; }
        public virtual HetOwnerStatusType OwnerStatusType { get; set; }
        public virtual HetContact PrimaryContact { get; set; }
        public virtual ICollection<HetContact> HetContacts { get; set; }
        public virtual ICollection<HetDigitalFile> HetDigitalFiles { get; set; }
        public virtual ICollection<HetEquipment> HetEquipments { get; set; }
        public virtual ICollection<HetHistory> HetHistories { get; set; }
        public virtual ICollection<HetNote> HetNotes { get; set; }
        public virtual ICollection<HetRentalRequestSeniorityList> HetRentalRequestSeniorityLists { get; set; }
        public virtual ICollection<HetSeniorityAudit> HetSeniorityAudits { get; set; }
    }
}
