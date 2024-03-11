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
        public int? BlockNumber { get; set; }

        public virtual HetEquipment Equipment { get; set; }
        public virtual HetRentalAgreement RentalAgreement { get; set; }
        public virtual HetRentalRequest RentalRequest { get; set; }
        public virtual ICollection<HetRentalAgreement> HetRentalAgreements { get; set; }
    }
}
