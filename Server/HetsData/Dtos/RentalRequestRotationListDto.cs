using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class RentalRequestRotationListDto
    {
        public RentalRequestRotationListDto()
        {
            RentalAgreements = new List<RentalAgreementDto>();
        }

        [JsonProperty("Id")]
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
        public int ConcurrencyControlNumber { get; set; }
        public EquipmentDto Equipment { get; set; }
        public RentalAgreementDto RentalAgreement { get; set; }
        public RentalRequestDto RentalRequest { get; set; }
        public List<RentalAgreementDto> RentalAgreements { get; set; }

        public float? SeniorityFloat { get; set; }
        public int? BlockNumber { get; set; }

    }
}
