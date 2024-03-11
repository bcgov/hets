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

        private DateTime? _askedDateTime;
        public DateTime? AskedDateTime {
            get => _askedDateTime is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _askedDateTime = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        public bool? WasAsked { get; set; }
        public string OfferResponse { get; set; }
        public string OfferResponseNote { get; set; }
        public string OfferRefusalReason { get; set; }

        private DateTime? _offerResponseDatetime;
        public DateTime? OfferResponseDatetime {
            get => _offerResponseDatetime is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _offerResponseDatetime = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
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
