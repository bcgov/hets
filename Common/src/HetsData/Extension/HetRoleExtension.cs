using System.Linq;

namespace HetsData.Model
{
    /// <summary>
    /// Role Database Model Extension
    /// </summary>
    public sealed partial class HetRole
    {
        /// <summary>
        /// Adds a permission to this role instance.
        /// </summary>
        /// <param name="permission">The permission to add.</param>
        public void AddPermission(HetPermission permission)
        {
            HetRolePermission rolePermission = new HetRolePermission
            {
                Permission = permission,
                Role = this
            };

            HetRolePermission.Add(rolePermission);
        }

        /// <summary>
        /// Removes a permission from this role instance.
        /// </summary>
        /// <param name="permission">The permission to remove.</param>
        public void RemovePermission(HetPermission permission)
        {
            HetRolePermission permissionToRemove = HetRolePermission.FirstOrDefault(x => x.Permission.Code == permission.Code);

            if (permissionToRemove != null)
            {
                HetRolePermission.Remove(permissionToRemove);
            }
        }
    }
}
