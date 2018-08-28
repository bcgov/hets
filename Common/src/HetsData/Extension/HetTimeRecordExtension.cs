using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetTimeRecord
    {
        [NotMapped]
        public int Id
        {
            get => TimeRecordId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                TimeRecordId = value;
            }
        }
    }
}
