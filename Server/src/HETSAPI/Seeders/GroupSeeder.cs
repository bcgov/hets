using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HETSAPI.Models;
using System.Collections.Generic;

namespace HETSAPI.Seeders
{
    public class GroupSeeder : Seeder<DbAppContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public GroupSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(DbAppContext context)
        {
            UpdateGroups(context);
            context.SaveChanges();
        }

        private void UpdateGroups(DbAppContext context)
        {
            List<Group> groups = new List<Group>()
            {
                new Group
                {
                    Name = "Other",
                    Description = "Users in the system not part of any other group(s)"
                },
                new Group
                {
                    Name = "HETS Application Administrators",
                    Description = "HETS Application Administrators Group"
                },
                new Group
                {
                    Name = "HETS Managers",
                    Description = "HETS Managers Group"
                },
                new Group
                {
                    Name = "HETS Clerks",
                    Description = "HETS Clerks Group"
                },
                new Group
                {
                    Name = "Administrators",
                    Description = "System Administrators Group"
                },
            };

            Logger.LogDebug("Updating groups ...");
            foreach (Group group in groups)
            {
                Group g = context.GetGroup(group.Name);
                if (g == null)
                {
                    Logger.LogDebug($"Adding group; {group.Name} ...");
                    context.Groups.Add(group);
                }
                else
                {
                    Logger.LogDebug($"Updating group; {g.Name} ...");
                    g.Description = group.Description;
                }
            }
        }
    }
}
