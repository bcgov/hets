using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class RentalAgreementConditionDto
    {
        [JsonProperty("Id")]
        public int RentalAgreementConditionId { get; set; }
        public string Comment { get; set; }
        public string ConditionName { get; set; }
        public int? RentalAgreementId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public RentalAgreementDto RentalAgreement { get; set; }
    }
}
