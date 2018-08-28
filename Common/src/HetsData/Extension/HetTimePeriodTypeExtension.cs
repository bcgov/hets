using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetTimePeriodType
    {
        [NotMapped]
        public int Id
        {
            get => TimePeriodTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                TimePeriodTypeId = value;
            }
        }
    }
}
