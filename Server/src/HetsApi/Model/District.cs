using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class District : HetDistrict
    {
        public int Id
        {
            get => DistrictId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                DistrictId = value;
            }
        }
    }
}
