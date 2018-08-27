using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class ConditionType : HetConditionType
    {
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
