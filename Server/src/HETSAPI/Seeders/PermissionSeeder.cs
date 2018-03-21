using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HETSAPI.Models;
using System.Collections.Generic;
using System.Linq;
using HETSAPI.Import;

namespace HETSAPI.Seeders
{
    internal class PermissionSeeder : Seeder<DbAppContext>
    {
        private const string SystemId = "SYSTEM_HETS";

        private readonly string[] _profileTriggers = { AllProfiles };

        public PermissionSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(DbAppContext context)
        {
            UpdatePermissions(context);
            context.SaveChangesForImport();

            Logger.LogDebug("Listing permissions ...");

            foreach (Permission p in context.Permissions.ToList())
            {
                Logger.LogDebug($"{p.Code}");
            }
        }

        private void UpdatePermissions(DbAppContext context)
        {
            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(context, SystemId);

            IEnumerable<Permission> permissions = Permission.AllPermissions;

            Logger.LogDebug("Updating permissions ...");

            foreach (Permission permission in permissions)
            {
                Logger.LogDebug($"Looking up {permission.Code} ...");

                Permission p = context.Permissions.FirstOrDefault(x => x.Code == permission.Code);

                if (p == null)
                {
                    Logger.LogDebug($"{permission.Code} does not exist, adding it ...");

                    permission.AppCreateUserid = SystemId;
                    permission.AppCreateTimestamp = DateTime.UtcNow;
                    permission.AppLastUpdateUserid = SystemId;
                    permission.AppLastUpdateTimestamp = DateTime.UtcNow;

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
