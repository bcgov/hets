using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class UserDistrict : HetUserDistrict
    {
        public int Id
        {
            get => UserDistrictId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                UserDistrictId = value;
            }
        }
    }
}
