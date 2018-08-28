using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetUserRole
    {
        [NotMapped]
        public int Id
        {
            get => UserRoleId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                UserRoleId = value;
            }
        }
    }
}
