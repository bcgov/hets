using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Entities
{
    /// <summary>
    /// Rental Request Rotation List Database Model Extension
    /// </summary>
    public partial class HetRentalRequestRotationList
    {
        [NotMapped]
        public float? SeniorityFloat { get; set; }
    }
}
