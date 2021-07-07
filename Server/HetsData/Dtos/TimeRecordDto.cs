using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class TimeRecordDto
    {
        [JsonProperty("Id")]
        public int TimeRecordId { get; set; }
        public DateTime? EnteredDate { get; set; }
        public DateTime WorkedDate { get; set; }
        public int TimePeriodTypeId { get; set; }
        public float? Hours { get; set; }
        public int? RentalAgreementRateId { get; set; }
        public int? RentalAgreementId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public string TimePeriod { get; set; }
    }
}
