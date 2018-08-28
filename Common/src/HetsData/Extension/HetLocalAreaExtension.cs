using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetLocalArea
    {
        [NotMapped]
        public int Id
        {
            get => LocalAreaId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                LocalAreaId = value;
            }
        }
    }
}
