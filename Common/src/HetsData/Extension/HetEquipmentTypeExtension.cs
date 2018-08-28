using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetEquipmentType
    {
        [NotMapped]
        public int Id
        {
            get => EquipmentTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                EquipmentTypeId = value;
            }
        }
    }
}
