/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HETSAPI.Models;
using System.Collections.Generic;

namespace HETSAPI.Seeders
{
    public class GroupSeeder : Seeder<DbAppContext>
    {
        private string[] ProfileTriggers = { AllProfiles };

        public GroupSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles
        {
            get { return ProfileTriggers; }
        }

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
                    Description = "Users in the system not part of any other group(s)."
                },
                new Group
                {
                    Name = "Managers",
                    Description = "The Managers group."
                },
                new Group
                {
                    Name = "HETS Clerks",
                    Description = "The HETS Clerks group."
                },
            };

            _logger.LogDebug("Updating groups ...");
            foreach (Group group in groups)
            {
                Group g = context.GetGroup(group.Name);
                if (g == null)
                {
                    _logger.LogDebug($"Adding group; {group.Name} ...");
                    context.Groups.Add(group);
                }
                else
                {
                    _logger.LogDebug($"Updating group; {g.Name} ...");
                    g.Description = group.Description;
                }
            }
        }
    }
}
