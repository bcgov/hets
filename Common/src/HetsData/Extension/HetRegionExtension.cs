using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRegion
    {
        [NotMapped]
        public int Id
        {
            get => RegionId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RegionId = value;
            }
        }
    }
}
