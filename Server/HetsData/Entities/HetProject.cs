using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetProject
    {
        public HetProject()
        {
            HetContacts = new HashSet<HetContact>();
            HetDigitalFiles = new HashSet<HetDigitalFile>();
            HetHistories = new HashSet<HetHistory>();
            HetNotes = new HashSet<HetNote>();
            HetRentalAgreements = new HashSet<HetRentalAgreement>();
            HetRentalRequests = new HashSet<HetRentalRequest>();
        }

        public int ProjectId { get; set; }
        public string ProvincialProjectNumber { get; set; }
        public string Name { get; set; }
        public int ProjectStatusTypeId { get; set; }
        public string Information { get; set; }
        public int? DistrictId { get; set; }
        public int? PrimaryContactId { get; set; }
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
        public string FiscalYear { get; set; }
        public string ResponsibilityCentre { get; set; }
        public string ServiceLine { get; set; }
        public string Stob { get; set; }
        public string Product { get; set; }
        public string BusinessFunction { get; set; }
        public string WorkActivity { get; set; }
        public string CostType { get; set; }

        public virtual HetDistrict District { get; set; }
        public virtual HetContact PrimaryContact { get; set; }
        public virtual HetProjectStatusType ProjectStatusType { get; set; }
        public virtual ICollection<HetContact> HetContacts { get; set; }
        public virtual ICollection<HetDigitalFile> HetDigitalFiles { get; set; }
        public virtual ICollection<HetHistory> HetHistories { get; set; }
        public virtual ICollection<HetNote> HetNotes { get; set; }
        public virtual ICollection<HetRentalAgreement> HetRentalAgreements { get; set; }
        public virtual ICollection<HetRentalRequest> HetRentalRequests { get; set; }
    }
}
