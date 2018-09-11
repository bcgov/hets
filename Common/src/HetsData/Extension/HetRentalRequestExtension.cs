using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    /// <summary>
    /// Rental Request Database Model Extension
    /// </summary>
    public sealed partial class HetRentalRequest
    {
        [NotMapped]
        public int YesCount { get; set; }

        [NotMapped]
        public int NumberOfBlocks { get; set; }

        [NotMapped]
        public string Status { get; set; }
    }
}
