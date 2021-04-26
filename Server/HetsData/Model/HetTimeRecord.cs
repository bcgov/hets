using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetTimeRecord
    {
        [JsonProperty("Id")]
        public int TimeRecordId { get; set; }

        public DateTime? EnteredDate { get; set; }
        public DateTime WorkedDate { get; set; }
        public int TimePeriodTypeId { get; set; }
        public float? Hours { get; set; }
        public int? RentalAgreementRateId { get; set; }
        public int? RentalAgreementId { get; set; }
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

        [JsonIgnore]
        public HetRentalAgreement RentalAgreement { get; set; }

        [JsonIgnore]
        public HetRentalAgreementRate RentalAgreementRate { get; set; }

        [JsonIgnore]
        public HetTimePeriodType TimePeriodType { get; set; }
    }
}
