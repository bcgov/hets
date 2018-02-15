using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using HETSAPI.Models;

namespace HETSAPI.Seeders
{
    public class DistrictSeeder : Seeder<DbAppContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public DistrictSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        protected override void Invoke(DbAppContext context)
        {
            UpdateDistricts(context);
            context.SaveChanges();
        }

        public override Type InvokeAfter => typeof(RegionSeeder);

        private void UpdateDistricts(DbAppContext context)
        {
            List<District> seedUsers = GetSeedDistricts();

            foreach (District district in seedUsers)
            {
                context.UpdateSeedDistrictInfo(district);
                context.SaveChanges();
            }

            AddInitialDistricts(context);            
        }

        private void AddInitialDistricts(DbAppContext context)
        {
            context.AddInitialDistrictsFromFile(Configuration["DistrictInitializationFile"]);
        }

        private List<District> GetSeedDistricts()
        {
            List<District> districts = new List<District>(GetDefaultDistricts());

            if (IsDevelopmentEnvironment)
                districts.AddRange(GetDevDistricts());

            if (IsTestEnvironment || IsStagingEnvironment)
                districts.AddRange(GetTestDistricts());

            if (IsProductionEnvironment)
                districts.AddRange(GetProdDistricts());

            return districts;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<District> GetDefaultDistricts()
        {
            return new List<District>();
        }

        /// <summary>
        /// Returns a list of users to be populated in the Development environment.
        /// </summary>
        private List<District> GetDevDistricts()
        {
            return new List<District>();            
        }

        /// <summary>
        /// Returns a list of users to be populated in the Test environment.
        /// </summary>
        private List<District> GetTestDistricts()
        {
            return new List<District>();
        }

        /// <summary>
        /// Returns a list of users to be populated in the Production environment.
        /// </summary>
        private List<District> GetProdDistricts()
        {
            return new List<District>();
        }
    }
}
