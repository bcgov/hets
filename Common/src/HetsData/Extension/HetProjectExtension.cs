using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    /// <summary>
    /// Rental Request Database Model Extension
    /// </summary>
    public sealed partial class HetProject
    {
        [NotMapped]
        public bool CanEditStatus { get; set; }
    }
}
