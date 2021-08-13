using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRentalRequest
    {
        public HetRentalRequest()
        {
            HetDigitalFiles = new HashSet<HetDigitalFile>();
            HetHistories = new HashSet<HetHistory>();
            HetNotes = new HashSet<HetNote>();
            HetRentalAgreements = new HashSet<HetRentalAgreement>();
            HetRentalRequestAttachments = new HashSet<HetRentalRequestAttachment>();
            HetRentalRequestRotationLists = new HashSet<HetRentalRequestRotationList>();
            HetRentalRequestSeniorityLists = new HashSet<HetRentalRequestSeniorityList>();
        }

        public int RentalRequestId { get; set; }
        public int EquipmentCount { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public int? ExpectedHours { get; set; }
        public int? FirstOnRotationListId { get; set; }
        public int RentalRequestStatusTypeId { get; set; }
        public int? DistrictEquipmentTypeId { get; set; }
        public int? LocalAreaId { get; set; }
        public int? ProjectId { get; set; }
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
        public int FiscalYear { get; set; }

        public virtual HetDistrictEquipmentType DistrictEquipmentType { get; set; }
        public virtual HetEquipment FirstOnRotationList { get; set; }
        public virtual HetLocalArea LocalArea { get; set; }
        public virtual HetProject Project { get; set; }
        public virtual HetRentalRequestStatusType RentalRequestStatusType { get; set; }
        public virtual ICollection<HetDigitalFile> HetDigitalFiles { get; set; }
        public virtual ICollection<HetHistory> HetHistories { get; set; }
        public virtual ICollection<HetNote> HetNotes { get; set; }
        public virtual ICollection<HetRentalAgreement> HetRentalAgreements { get; set; }
        public virtual ICollection<HetRentalRequestAttachment> HetRentalRequestAttachments { get; set; }
        public virtual ICollection<HetRentalRequestRotationList> HetRentalRequestRotationLists { get; set; }
        public virtual ICollection<HetRentalRequestSeniorityList> HetRentalRequestSeniorityLists { get; set; }
    }
}
