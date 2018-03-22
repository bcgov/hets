using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using HETSAPI.Models;

namespace HETSAPI.Seeders
{
    public class EquipmentTypeSeeder : Seeder<DbAppContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public EquipmentTypeSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(DbAppContext context)
        {
            UpdateEquipmentTypes(context);
            context.SaveChangesForImport();
        }

        public override Type InvokeAfter => typeof(DistrictSeeder);

        /// <summary>
        /// Add or update equipment types onl
        /// </summary>
        /// <param name="context"></param>
        private void UpdateEquipmentTypes(DbAppContext context)
        {            
            AddInitialEquipmentTypes(context);            
        }

        private void AddInitialEquipmentTypes(DbAppContext context)
        {
            context.AddInitialEquipmentTypesFromFile(Configuration["EquipmentTypesInitializationFile"]);
        }        
    }
}
