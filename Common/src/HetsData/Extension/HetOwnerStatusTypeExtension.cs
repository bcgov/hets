using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetOwnerStatusType
    {
        [NotMapped]
        public int Id
        {
            get => OwnerStatusTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                OwnerStatusTypeId = value;
            }
        }
    }
}
