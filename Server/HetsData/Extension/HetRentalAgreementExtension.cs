using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace HetsData.Entities
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
        public string LocalAreaName { get; set; }
    }
}
