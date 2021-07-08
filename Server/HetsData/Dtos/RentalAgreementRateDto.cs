using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class RentalAgreementRateDto
    {
        [JsonProperty("Id")]
        public int RentalAgreementRateId { get; set; }
        public string Comment { get; set; }
        [JsonIgnore] public string ComponentName { get; set; }
        public float? Rate { get; set; }
        public int? RatePeriodTypeId { get; set; }
        public bool Overtime { get; set; }
        public bool Active { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public bool Set { get; set; }
        public int? RentalAgreementId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public RentalAgreementDto RentalAgreement { get; set; }
        public RatePeriodTypeDto RatePeriodType { get; set; }
        
        public string RatePeriod { get; set; }
        public string RateString { get; set; }
    }
}
