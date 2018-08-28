using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetHistory
    {
        [NotMapped]
        public int Id
        {
            get => HistoryId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                HistoryId = value;
            }
        }
    }
}
