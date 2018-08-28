using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetUserFavourite
    {
        [NotMapped]
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
