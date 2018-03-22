using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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

        /// <summary>
        /// Add or update equipment types onl
        /// </summary>
        /// <param name="context"></param>
        private void UpdateEquipmentTypes(DbAppContext context)
        {
            List<EquipmentType> seedEquipmentTypes = GetSeedEquipmentTypes();

            foreach (EquipmentType equipmentType in seedEquipmentTypes)
            {
                context.AddInitialEquipmentType(equipmentType);
            }

            AddInitialEquipmentTypes(context);            
        }

        private void AddInitialEquipmentTypes(DbAppContext context)
        {
            context.AddInitialEquipmentTypesFromFile(Configuration["EquipmentTypesInitializationFile"]);
        }

        private List<EquipmentType> GetSeedEquipmentTypes()
        {
            List<EquipmentType> equipmentTypes = new List<EquipmentType>(GetDefaultEquipmentTypes());

            if (IsDevelopmentEnvironment)
                equipmentTypes.AddRange(GetDevEquipmentTypes());

            if (IsTestEnvironment || IsStagingEnvironment)
                equipmentTypes.AddRange(GetTestEquipmentTypes());

            if (IsProductionEnvironment)
                equipmentTypes.AddRange(GetProdEquipmentTypes());

            return equipmentTypes;
        }

        /// <summary>
        /// Returns a list of equipmentTypes to be populated in all environments
        /// </summary>
        private List<EquipmentType> GetDefaultEquipmentTypes()
        {
            return new List<EquipmentType>();
        }

        /// <summary>
        /// Returns a list of equipmentTypes to be populated in the Development environment
        /// </summary>
        private List<EquipmentType> GetDevEquipmentTypes()
        {
            return new List<EquipmentType>();
        }

        /// <summary>
        /// Returns a list of equipmentTypes to be populated in the Test environment
        /// </summary>
        private List<EquipmentType> GetTestEquipmentTypes()
        {
            return new List<EquipmentType>();
        }

        /// <summary>
        /// Returns a list of equipmentTypes to be populated in the Production environment
        /// </summary>
        private List<EquipmentType> GetProdEquipmentTypes()
        {
            return new List<EquipmentType>();
        }
    }
}
