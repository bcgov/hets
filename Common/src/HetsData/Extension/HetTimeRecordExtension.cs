using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetTimeRecord
    {
        [NotMapped]
        public string TimePeriod { get; set; }
    }
}
