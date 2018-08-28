using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalRequestRotationList
    {
        [NotMapped]
        public int Id
        {
            get => RentalRequestRotationListId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RentalRequestRotationListId = value;
            }
        }
    }
}
