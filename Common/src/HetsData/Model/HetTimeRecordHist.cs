using System;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetTimeRecordHist
    {
        [JsonProperty("Id")]
        public int TimeRecordHistId { get; set; }

        public int TimeRecordId { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EnteredDate { get; set; }
        public float? Hours { get; set; }
        public int? RentalAgreementRateId { get; set; }
        public DateTime WorkedDate { get; set; }
        public string TimePeriod { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public string AppCreateUserDirectory { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public int? RentalAgreementId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public string DbCreateUserId { get; set; }
        public string DbLastUpdateUserId { get; set; }
    }
}
