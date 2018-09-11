using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalAgreement
    {
        [NotMapped]
        public string Status { get; set; }

        [NotMapped]
        public string RatePeriod { get; set; }
    }
}
