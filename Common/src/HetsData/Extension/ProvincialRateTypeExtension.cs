using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    /// <summary>
    /// Provincial Rate Type Database Model Extension
    /// </summary>
    public sealed partial class HetProvincialRateType
    {
        [NotMapped]
        public int Id { get; set; }
    }
}
