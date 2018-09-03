using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    /// <summary>
    /// Rental Agreement Rate Database Model Extension
    /// </summary>
    public sealed partial class HetRentalAgreementRate
    {
        [NotMapped]
        public string RateString { get; set; }
    }
}
