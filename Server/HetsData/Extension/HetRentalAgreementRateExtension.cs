using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Entities
{
    public partial class HetRentalAgreementRate
    {
        [NotMapped]
        public string RatePeriod { get; set; }

        [NotMapped]
        public string RateString { get; set; }
    }
}
