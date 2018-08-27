using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class City : HetCity
    {
        public int Id
        {
            get => CityId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                CityId = value;
            }
        }
    }
}
