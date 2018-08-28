using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetDistrictEquipmentType
    {
        [NotMapped]
        public int Id
        {
            get => DistrictEquipmentTypeId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                DistrictEquipmentTypeId = value;
            }
        }
    }
}
