using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetRentalRequestRotationList
    {
        [JsonProperty("Id")]
        public int RentalRequestRotationListId { get; set; }

        public DateTime? AskedDateTime { get; set; }
        public int? EquipmentId { get; set; }
        public bool? IsForceHire { get; set; }
        public string Note { get; set; }
        public string OfferResponse { get; set; }
        public string OfferResponseNote { get; set; }
        public int? RentalAgreementId { get; set; }
        public int? RentalRequestId { get; set; }
        public int RotationListSortOrder { get; set; }
        public bool? WasAsked { get; set; }
        public string OfferRefusalReason { get; set; }
        public DateTime? OfferResponseDatetime { get; set; }
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
        public int ConcurrencyControlNumber { get; set; }

        public HetEquipment Equipment { get; set; }
        public HetRentalAgreement RentalAgreement { get; set; }
        public HetRentalRequest RentalRequest { get; set; }
    }
}
