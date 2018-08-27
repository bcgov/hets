using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class LocalArea : HetLocalArea
    {
        public int Id
        {
            get => LocalAreaId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                LocalAreaId = value;
            }
        }
    }
}
