using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalAgreementCondition
    {
        [NotMapped]
        public int Id
        {
            get => RentalAgreementConditionId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RentalAgreementConditionId = value;
            }
        }
    }
}
