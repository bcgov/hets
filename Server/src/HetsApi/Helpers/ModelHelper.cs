using System;
using System.Reflection;
using System.Linq;
using Microsoft.AspNetCore.Http;
using HetsData.Model;

namespace HetsApi.Helpers
{
    public static class ModelHelper
    {
        public static string GetUserId(HttpContext httpContext)
        {
            string userId = httpContext.User.Identity.Name;
            return userId;
        }

        public static HetUser GetUser(DbAppContext context, HttpContext httpContext)
        {
            string userId = GetUserId(httpContext);
            HetUser user = context.HetUser.FirstOrDefault(x => x.SmUserId == userId);
            return user;
        }

        public static int? GetUsersDistrictId(DbAppContext context, HttpContext httpContext)
        {
            string userId = GetUserId(httpContext);
            int? districtId = context.HetUser.FirstOrDefault(x => x.SmUserId == userId)?.DistrictId;
            return districtId;
        }

        public static object CopyProperties(object source, object destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source Object is null");
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(source), "Destination Object is null");
            }

            // get the object types
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // iterate the properties of the source instance and  
            // populate them from their destination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();

            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }

                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);

                if (targetProperty == null)
                {
                    continue;
                }

                if (!targetProperty.CanWrite)
                {
                    continue;
                }

                if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate)
                {
                    continue;
                }

                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }

                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }

                // passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }

            return destination;
        }
    }
}
