using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class User : HetUser
    {
        public int Id
        {
            get => UserId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                UserId = value;
            }
        }
    }
}
