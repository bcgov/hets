using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetOwner
    {
        public HetOwner()
        {
            HetContact = new HashSet<HetContact>();
            HetDigitalFile = new HashSet<HetDigitalFile>();
            HetEquipment = new HashSet<HetEquipment>();
            HetHistory = new HashSet<HetHistory>();
            HetNote = new HashSet<HetNote>();
            HetSeniorityAudit = new HashSet<HetSeniorityAudit>();
        }

        [JsonProperty("Id")]
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
        public string CglPolicyNumber { get; set; }
        public DateTime? CglendDate { get; set; }
        public string WorkSafeBcpolicyNumber { get; set; }
        public DateTime? WorkSafeBcexpiryDate { get; set; }
        public bool? IsMaintenanceContractor { get; set; }
        public bool MeetsResidency { get; set; }
        [JsonIgnore]public string AppCreateUserDirectory { get; set; }
        [JsonIgnore]public string AppCreateUserGuid { get; set; }
        [JsonIgnore]public string AppCreateUserid { get; set; }
        [JsonIgnore]public DateTime AppCreateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserDirectory { get; set; }
        [JsonIgnore]public string AppLastUpdateUserGuid { get; set; }
        [JsonIgnore]public string AppLastUpdateUserid { get; set; }
        [JsonIgnore]public DateTime AppLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string DbCreateUserId { get; set; }
        [JsonIgnore]public DateTime DbCreateTimestamp { get; set; }
        [JsonIgnore]public DateTime DbLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public HetLocalArea LocalArea { get; set; }
        public HetOwnerStatusType OwnerStatusType { get; set; }
        public HetContact PrimaryContact { get; set; }

        [JsonProperty("Contacts")]
        public ICollection<HetContact> HetContact { get; set; }

        [JsonProperty("Attachments")]
        public ICollection<HetDigitalFile> HetDigitalFile { get; set; }

        [JsonProperty("Equipment")]
        public ICollection<HetEquipment> HetEquipment { get; set; }

        [JsonProperty("History")]
        public ICollection<HetHistory> HetHistory { get; set; }

        [JsonProperty("Notes")]
        public ICollection<HetNote> HetNote { get; set; }

        [JsonIgnore]
        public ICollection<HetSeniorityAudit> HetSeniorityAudit { get; set; }
    }
}
