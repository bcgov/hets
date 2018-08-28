using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRentalAgreementStatusType
    {
        [NotMapped]
        public int Id
        {
            get => RentalAgreementStatusTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RentalAgreementStatusTypeId = value;
            }
        }
    }
}
