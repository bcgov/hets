using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetDistrict
    {
        [NotMapped]
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
