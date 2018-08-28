using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalRequest
    {
        [NotMapped]
        public int Id
        {
            get => RentalRequestId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RentalRequestId = value;
            }
        }
    }
}
