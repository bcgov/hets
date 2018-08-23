using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using HETSAPI.Models;

namespace HetsApi.Seeders
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
        /// Add new equipment types only - users can edit online so we don't want to update again
        /// </summary>
        /// <param name="context"></param>
        private void UpdateConditions(DbAppContext context)
        {
            List<ConditionType> seedConditionTypes = GetSeedConditionTypes();

            foreach (ConditionType conditionType in seedConditionTypes)
            {
                context.AddInitialCondition(conditionType);
            }

            AddInitialConditions(context);            
        }

        private void AddInitialConditions(DbAppContext context)
        {
            context.AddInitialConditionsFromFile(Configuration["ConditionsInitializationFile"]);
        }

        private List<ConditionType> GetSeedConditionTypes()
        {
            List<ConditionType> conditionTypes = new List<ConditionType>(GetDefaultConditionTypes());

            if (IsDevelopmentEnvironment)
                conditionTypes.AddRange(GetDevConditionTypes());

            if (IsTestEnvironment || IsStagingEnvironment)
                conditionTypes.AddRange(GetTestConditionTypes());

            if (IsProductionEnvironment)
                conditionTypes.AddRange(GetProdConditionTypes());

            return conditionTypes;
        }

        /// <summary>
        /// Returns a list of conditionTypes to be populated in all environments
        /// </summary>
        private List<ConditionType> GetDefaultConditionTypes()
        {
            return new List<ConditionType>();
        }

        /// <summary>
        /// Returns a list of conditionTypes to be populated in the Development environment
        /// </summary>
        private List<ConditionType> GetDevConditionTypes()
        {
            return new List<ConditionType>();
        }

        /// <summary>
        /// Returns a list of conditionTypes to be populated in the Test environment
        /// </summary>
        private List<ConditionType> GetTestConditionTypes()
        {
            return new List<ConditionType>();
        }

        /// <summary>
        /// Returns a list of conditionTypes to be populated in the Production environment
        /// </summary>
        private List<ConditionType> GetProdConditionTypes()
        {
            return new List<ConditionType>();
        }
    }
}
