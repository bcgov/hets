using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Entities
{
    public partial class HetTimeRecord
    {
        [NotMapped]
        public string TimePeriod { get; set; }
    }
}
