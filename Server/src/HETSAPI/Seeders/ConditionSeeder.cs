using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using HETSAPI.Models;

namespace HETSAPI.Seeders
{
    public class ConditionSeeder : Seeder<DbAppContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public ConditionSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(DbAppContext context)
        {
            UpdateConditions(context);
            context.SaveChangesForImport();
        }

        public override Type InvokeAfter => typeof(DistrictSeeder);

        /// <summary>
        /// Add new condition types only - users can edit online so we don't want to update again
        /// </summary>
        /// <param name="context"></param>
        private void UpdateConditions(DbAppContext context)
        {            
            AddInitialConditions(context);            
        }

        private void AddInitialConditions(DbAppContext context)
        {
            context.AddInitialConditionsFromFile(Configuration["ConditionInitializationFile"]);
        }        
    }
}
