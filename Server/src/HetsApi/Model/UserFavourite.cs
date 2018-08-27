using System;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class UserFavourite : HetUserFavourite
    {
        public int Id
        {
            get => UserFavouriteId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                UserFavouriteId = value;
            }
        }
    }
}
