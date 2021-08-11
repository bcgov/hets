using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRentalRequestRotationList
    {
        public HetRentalRequestRotationList()
        {
            HetRentalAgreements = new HashSet<HetRentalAgreement>();
        }

        public int RentalRequestRotationListId { get; set; }
        public int RotationListSortOrder { get; set; }
        public DateTime? AskedDateTime { get; set; }
        public bool? WasAsked { get; set; }
        public string OfferResponse { get; set; }
        public string OfferResponseNote { get; set; }
        public string OfferRefusalReason { get; set; }
        public DateTime? OfferResponseDatetime { get; set; }
        public bool? IsForceHire { get; set; }
        public string Note { get; set; }
        public int? EquipmentId { get; set; }
        public int? RentalAgreementId { get; set; }
        public int? RentalRequestId { get; set; }
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
        public int? BlockNumber { get; set; }

        public virtual HetEquipment Equipment { get; set; }
        public virtual HetRentalAgreement RentalAgreement { get; set; }
        public virtual HetRentalRequest RentalRequest { get; set; }
        public virtual ICollection<HetRentalAgreement> HetRentalAgreements { get; set; }
    }
}
