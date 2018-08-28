using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalAgreementRate
    {
        [NotMapped]
        public int Id
        {
            get => RentalAgreementRateId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RentalAgreementRateId = value;
            }
        }
    }
}
