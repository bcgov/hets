using HetsData.Entities;

namespace HetsData.Helpers
{
    #region Permission Models

    public class PermissionLite
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }                
    }    

    #endregion

    public static class PermissionHelper
    {
        #region Convert full permision record to a "Lite" version

        /// <summary>
        /// Convert to Permission Lite Model
        /// </summary>
        /// <param name="permission"></param>
        public static PermissionLite ToLiteModel(HetPermission permission)
        {
            PermissionLite permissionLite = new PermissionLite();

            if (permission != null)
            {
                permissionLite.Id = permission.PermissionId;
                permissionLite.Code = permission.Code;
                permissionLite.Name = permission.Name;
                permissionLite.Description = permission.Description;
            }

            return permissionLite;
        }

        #endregion
    }
}
