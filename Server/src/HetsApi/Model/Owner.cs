using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class Owner : HetOwner
    {
        public int Id
        {
            get => OwnerId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                OwnerId = value;
            }
        }
    }
}
