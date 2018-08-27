using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class DistrictEquipmentType : HetDistrictEquipmentType
    {
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
