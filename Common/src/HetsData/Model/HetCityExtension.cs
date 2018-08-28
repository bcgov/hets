using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{    
    public partial class HetCity
    {
        [NotMapped]
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
