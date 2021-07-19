using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Entities
{
    /// <summary>
    /// Rental Request Database Model Extension
    /// </summary>
    public partial class HetRentalRequest
    {
        /// <summary>
        /// Status Codes
        /// </summary>
        public const string StatusInProgress = "In Progress";
        public const string StatusComplete = "Complete";

        [NotMapped]
        public int YesCount { get; set; }

        [NotMapped]
        public int NumberOfBlocks { get; set; }

        [NotMapped]
        public string Status { get; set; }

        [NotMapped]
        public string LocalAreaName { get; set; }
    }
}
