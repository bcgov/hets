using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetConditionType
    {
        [NotMapped]
        public int Id
        {
            get => ConditionTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                ConditionTypeId = value;
            }
        }
    }
}
