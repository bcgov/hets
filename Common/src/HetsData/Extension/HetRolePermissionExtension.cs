using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    public partial class HetRolePermission
    {
        [NotMapped]
        public int Id
        {
            get => RolePermissionId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                RolePermissionId = value;
            }
        }
    }
}
