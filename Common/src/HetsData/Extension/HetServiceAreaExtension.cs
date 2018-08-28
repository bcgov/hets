using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetServiceArea
    {
        [NotMapped]
        public int Id
        {
            get => ServiceAreaId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                ServiceAreaId = value;
            }
        }
    }
}
