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

        private DateTime? _archiveDate;
        public DateTime? ArchiveDate {
            get => _archiveDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _archiveDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string ArchiveCode { get; set; }
        public string ArchiveReason { get; set; }
        public int? LocalAreaId { get; set; }
        public int? PrimaryContactId { get; set; }
        public string CglCompany { get; set; }
        public string CglPolicyNumber { get; set; }

        private DateTime? _cglendDate;
        public DateTime? CglendDate {
            get => _cglendDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _cglendDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public string WorkSafeBcpolicyNumber { get; set; }

        private DateTime? _workSafeBcexpiryDate;
        public DateTime? WorkSafeBcexpiryDate {
            get => _workSafeBcexpiryDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _workSafeBcexpiryDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public bool? IsMaintenanceContractor { get; set; }
        public bool MeetsResidency { get; set; }
        public int? BusinessId { get; set; }
        public string SharedKey { get; set; }
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
