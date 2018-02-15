using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HETSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HETSAPI.Seeders
{
    public class RoleSeeder : Seeder<DbAppContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public RoleSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        public override Type InvokeAfter => typeof(PermissionSeeder);

        protected override void Invoke(DbAppContext context)
        {
            UpdateRoles(context);
            context.SaveChanges();
        }

        private void UpdateRoles(DbAppContext context)
        {
            var permissions = context.Permissions.ToList();

            // Load the roles
            var roles = new List<Role>
            {
                new Role
                {
                    Name = "HETS Clerk",
                    Description = "HETS District User",
                    RolePermissions = permissions.Where(p =>
                        new[] { Permission.Login }
                        .Contains(p.Code))
                        .Select(p => new RolePermission()
                        {
                            Permission = p
                        })
                        .ToList()
                },
                new Role
                {
                    Name = "HETS Manager",
                    Description = "HETS District User with additional permissions",
                    RolePermissions = permissions.Where(p =>
                            new[] { Permission.Login, Permission.DistrictCodeTableManagement }
                                .Contains(p.Code))
                        .Select(p => new RolePermission()
                        {
                            Permission = p
                        })
                        .ToList()
                },
                new Role
                {
                    Name = "HETS Application Administrator",
                    Description = "HETS User with administrative permissions",
                    RolePermissions = permissions.Where(p =>
                            new[]
                                {
                                    Permission.Login, Permission.DistrictCodeTableManagement,
                                    Permission.CodeTableManagement, Permission.UserManagement
                                }
                                .Contains(p.Code))
                        .Select(p => new RolePermission()
                        {
                            Permission = p
                        })
                        .ToList()
                },
                new Role
                {
                    Name = "Administrator",
                    Description = "System Administrator; full access to the whole system",
                    RolePermissions = permissions.Select(p => new RolePermission()
                    {
                        Permission = p
                    }).ToList()
                },
                new Role
                {
                    Name = "Data Conversion",
                    Description = "Can import data into HETS",
                    RolePermissions = permissions.Where(p =>
                            new[] { Permission.ImportData }
                                .Contains(p.Code))
                        .Select(p => new RolePermission()
                        {
                            Permission = p
                        })
                        .ToList()
                },
            };

            Logger.LogDebug("Updating roles ...");
            foreach (var role in roles)
            {
                var r = context.GetRole(role.Name);
                if (r == null)
                {
                    Logger.LogDebug($"Adding role; {role.Name} ...");
                    context.Roles.Add(role);
                }
                else
                {
                    Logger.LogDebug($"Updating role; {r.Name} ...");
                    r.Description = role.Description;
                }
            }
        }
    }
}
