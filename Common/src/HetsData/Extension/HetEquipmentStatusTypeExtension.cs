using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetEquipmentStatusType
    {
        [NotMapped]
        public int Id
        {
            get => EquipmentStatusTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                EquipmentStatusTypeId = value;
            }
        }
    }
}
