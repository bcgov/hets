using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    /// <summary>
    /// Rental Request Rotation List Database Model Extension
    /// </summary>
    public partial class HetRentalRequestRotationList
    {
        [NotMapped]
        public float? SeniorityFloat { get; set; }

        [NotMapped]
        public int? BlockNumber { get; set; }
    }
}
