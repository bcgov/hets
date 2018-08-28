using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetRentalAgreement
    {
        public HetRentalAgreement()
        {
            HetRentalAgreementCondition = new HashSet<HetRentalAgreementCondition>();
            HetRentalAgreementRate = new HashSet<HetRentalAgreementRate>();
            HetRentalRequestRotationList = new HashSet<HetRentalRequestRotationList>();
            HetTimeRecord = new HashSet<HetTimeRecord>();
        }

        [JsonProperty("Id")]
        public int RentalAgreementId { get; set; }

        public int? EquipmentId { get; set; }
        public int? ProjectId { get; set; }
        public string Status { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime? DatedOn { get; set; }
        public float? EquipmentRate { get; set; }
        public int? EstimateHours { get; set; }
        public DateTime? EstimateStartWork { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string Note { get; set; }
        public string Number { get; set; }
        public string RateComment { get; set; }
        public string RatePeriod { get; set; }
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
        public HetProject Project { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalAgreementCondition> HetRentalAgreementCondition { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalAgreementRate> HetRentalAgreementRate { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalRequestRotationList> HetRentalRequestRotationList { get; set; }

        [JsonIgnore]
        public ICollection<HetTimeRecord> HetTimeRecord { get; set; }
    }
}
