using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HETSAPI.Models;
using System;
using System.Collections.Generic;

namespace HETSAPI.Seeders
{
    public class UserSeeder : Seeder<DbAppContext>
    {
        private readonly string[] _profileTriggers = { AllProfiles };

        public UserSeeder(IConfiguration configuration, IHostingEnvironment env, ILoggerFactory loggerFactory) 
            : base(configuration, env, loggerFactory)
        { }

        protected override IEnumerable<string> TriggerProfiles => _profileTriggers;

        public override Type InvokeAfter => typeof(RoleSeeder);

        protected override void Invoke(DbAppContext context)
        {
            UpdateUsers(context);
            context.SaveChangesForImport();
        }

        private void UpdateUsers(DbAppContext context)
        {
            List<User> seedUsers = GetSeedUsers();

            foreach (User user in seedUsers)
            {
                context.UpdateSeedUserInfo(user);                
            }

            AddInitialUsers(context);           
        }

        private void AddInitialUsers(DbAppContext context)
        {
            context.AddInitialUsersFromFile(Configuration["UserInitializationFile"]);
        }

        private List<User> GetSeedUsers()
        {
            List<User> users = new List<User>(GetDefaultUsers());

            if (IsDevelopmentEnvironment)
                users.AddRange(GetDevUsers());

            if (IsTestEnvironment || IsStagingEnvironment)
                users.AddRange(GetTestUsers());

            if (IsProductionEnvironment)
                users.AddRange(GetProdUsers());

            return users;
        }

        /// <summary>
        /// Returns a list of users to be populated in all environments.
        /// </summary>
        private List<User> GetDefaultUsers()
        {
            return new List<User>();
        }

        /// <summary>
        /// Returns a list of users to be populated in the Development environment.
        /// </summary>
        private List<User> GetDevUsers()
        {
            return new List<User>();
        }

        /// <summary>
        /// Returns a list of users to be populated in the Test environment.
        /// </summary>
        private List<User> GetTestUsers()
        {
            return new List<User>();
        }

        /// <summary>
        /// Returns a list of users to be populated in the Production environment.
        /// </summary>
        private List<User> GetProdUsers()
        {
            return new List<User>();
        }
    }
}
