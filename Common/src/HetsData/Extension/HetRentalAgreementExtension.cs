using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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

        [NotMapped]
        [JsonProperty("OvertimeRates")]
        public ICollection<HetRentalAgreementRate> HetRentalAgreementOvertimeRate { get; set; }
    }
}
