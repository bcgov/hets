using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetOwner
    {
        [NotMapped]
        public int Id
        {
            get => OwnerId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                OwnerId = value;
            }
        }
    }
}
