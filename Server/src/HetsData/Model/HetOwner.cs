using System;
using System.Collections.Generic;

namespace HetsData.Model
{
    public partial class HetOwner
    {
        public HetOwner()
        {
            HetAttachment = new HashSet<HetAttachment>();
            HetContact = new HashSet<HetContact>();
            HetEquipment = new HashSet<HetEquipment>();
            HetHistory = new HashSet<HetHistory>();
            HetNote = new HashSet<HetNote>();
            HetSeniorityAudit = new HashSet<HetSeniorityAudit>();
        }

        public int OwnerId { get; set; }
        public string ArchiveReason { get; set; }
        public DateTime? CglendDate { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public int? LocalAreaId { get; set; }
        public string ArchiveCode { get; set; }
        public int? PrimaryContactId { get; set; }
        public string Status { get; set; }
        public DateTime? WorkSafeBcexpiryDate { get; set; }
        public bool? IsMaintenanceContractor { get; set; }
        public string OrganizationName { get; set; }
        public string OwnerCode { get; set; }
        public string DoingBusinessAs { get; set; }
        public bool MeetsResidency { get; set; }
        public string RegisteredCompanyNumber { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string WorkSafeBcpolicyNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string StatusComment { get; set; }
        public string CglPolicyNumber { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetLocalArea LocalArea { get; set; }
        public HetContact PrimaryContact { get; set; }
        public ICollection<HetAttachment> HetAttachment { get; set; }
        public ICollection<HetContact> HetContact { get; set; }
        public ICollection<HetEquipment> HetEquipment { get; set; }
        public ICollection<HetHistory> HetHistory { get; set; }
        public ICollection<HetNote> HetNote { get; set; }
        public ICollection<HetSeniorityAudit> HetSeniorityAudit { get; set; }
    }
}
