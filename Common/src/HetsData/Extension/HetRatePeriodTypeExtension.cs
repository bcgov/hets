using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRatePeriodType
    {
        [NotMapped]
        public int Id
        {
            get => RatePeriodTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RatePeriodTypeId = value;
            }
        }
    }
}
