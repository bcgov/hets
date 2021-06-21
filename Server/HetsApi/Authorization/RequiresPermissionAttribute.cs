using Microsoft.AspNetCore.Mvc;

namespace HetsApi.Authorization
{
    /// <summary>
    /// Allows declarative claims based permissions to be applied to controller methods for authorization.
    /// </summary>
    public class RequiresPermissionAttribute : TypeFilterAttribute
    {
        public RequiresPermissionAttribute(params string[] permissions) : base(typeof(RequiresPermissionFilter))
        {
            Arguments = new object[] { new PermissionRequirement(permissions) };
        }
    }
}
