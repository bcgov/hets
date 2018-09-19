using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalAgreement
    {
        /// <summary>
        /// Status Codes
        /// </summary>
        public const string StatusActive = "Active";
        public const string StatusComplete = "Complete";

        [NotMapped]
        public string Status { get; set; }

        [NotMapped]
        public string RatePeriod { get; set; }
    }
}
