using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetUserDistrict
    {
        [NotMapped]
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
