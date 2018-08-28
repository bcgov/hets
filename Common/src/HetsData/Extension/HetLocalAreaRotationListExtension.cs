using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetLocalAreaRotationList
    {
        [NotMapped]
        public int Id
        {
            get => LocalAreaRotationListId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                LocalAreaRotationListId = value;
            }
        }
    }
}
