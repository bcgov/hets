using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalRequestStatusType
    {
        [NotMapped]
        public int Id
        {
            get => RentalRequestStatusTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RentalRequestStatusTypeId = value;
            }
        }
    }
}
