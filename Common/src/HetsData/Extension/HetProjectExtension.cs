using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    /// <summary>
    /// Rental Request Database Model Extension
    /// </summary>
    public sealed partial class HetProject
    {
        /// <summary>
        /// Status Codes
        /// </summary>
        public const string StatusActive = "Active";
        public const string StatusComplete = "Completed";

        [NotMapped]
        public bool CanEditStatus { get; set; }

        [NotMapped]
        public string Status { get; set; }
    }
}
