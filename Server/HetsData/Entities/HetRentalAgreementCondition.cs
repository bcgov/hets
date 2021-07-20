using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetRentalAgreementCondition
    {
        public int RentalAgreementConditionId { get; set; }
        public string Comment { get; set; }
        public string ConditionName { get; set; }
        public int? RentalAgreementId { get; set; }
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

        public virtual HetRentalAgreement RentalAgreement { get; set; }
    }
}
