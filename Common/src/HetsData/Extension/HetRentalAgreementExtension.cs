using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalAgreement
    {
        [NotMapped]
        public int Id
        {
            get => RentalAgreementId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RentalAgreementId = value;
            }
        }
    }
}
