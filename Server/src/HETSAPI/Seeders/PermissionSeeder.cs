using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HETSAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace HETSAPI.Seeders
{
    internal class PermissionSeeder : Seeder<DbAppContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public PermissionSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(DbAppContext context)
        {
            UpdatePermissions(context);
            context.SaveChanges();

            Logger.LogDebug("Listing permissions ...");
            foreach (var p in context.Permissions.ToList())
            {
                Logger.LogDebug($"{p.Code}");
            }
        }

        private void UpdatePermissions(DbAppContext context)
        {
            var permissions = Permission.AllPermissions;

            Logger.LogDebug("Updating permissions ...");

            foreach (Permission permission in permissions)
            {
                Logger.LogDebug($"Looking up {permission.Code} ...");

                Permission p = context.Permissions.FirstOrDefault(x => x.Code == permission.Code);

                if (p == null)
                {
                    Logger.LogDebug($"{permission.Code} does not exist, adding it ...");
                    context.Permissions.Add(permission);
                }
                else
                {
                    Logger.LogDebug($"Updating the fields for {permission.Code} ...");
                    p.Description = permission.Description;
                    p.Name = permission.Name;
                }
            }            
        }
    }
}
