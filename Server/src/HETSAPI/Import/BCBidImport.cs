using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using HETSAPI.Models;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Import
{
    /// <summary>
    /// BC Bid Data Import Functions
    /// </summary>
    public static class BCBidImport
    {
        private const string SystemId = "SYSTEM_HETS";

        /// <summary>
        /// Returns current date in a standard format for all conversion routines
        /// </summary>
        public static string TodayDate => DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>
        /// Returns the common "SigId" for all conversion routines
        /// </summary>
        public static int SigId => 388888;

        /// <summary>
        /// Hangfire job to do the Annual Rollover tasks.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        /// <param name="fileLocation"></param>
        /// <param name="configuration"></param>
        public static void ImportJob(PerformContext context, string connectionstring, string fileLocation, IConfiguration configuration)
        {
            // open a connection to the database.
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionstring);

            DbAppContext dbContext = new DbAppContext(null, options.Options, configuration);
            context.WriteLine("Starting Data Import Job");

            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(dbContext, SystemId);            
  
            //*** Importing the Local Areas from the file of Area.xml to the table of HET_LOCAL_AREA
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportLocalArea.Import(context, dbContext, fileLocation, SystemId);

            //*** Users from User_HETS.xml. This has effects on Table HET_USER and HET_USER_ROLE  
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportUser.Import(context, dbContext, fileLocation, SystemId);

            //*** Owners: This has effects on Table HETS_OWNER and HETS_Contact
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportOwner.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Dump_Truck  from Dump_Truck.xml   
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportProject.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equiptment type from EquipType.xml This has effects on Table HET_USER and HET_EQUIPMENT_TYPE  
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportDisEquipType.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equiptment  from Equip.xml  This has effects on Table HET_USER and HET_EQUIP
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportEquip.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Dump_Truck  from Dump_Truck.xml   
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportDumpTruck.Import(context, dbContext, fileLocation,  SystemId);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            //*** Import Equipment_Attached  from Equip_Attach.xml   
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportEquipAttach.Import(context, dbContext, fileLocation,  SystemId);

            //*** Import the table of "HET_DISTRICT_EQUIPMENT_TYPE"  from Block.xml   
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportBlock.Import(context, dbContext, fileLocation,  SystemId);
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs.ToString());

            //*** Import the table of  "HET_RENTAL_AGREEMENT" and "HET_TIME_RECORD";  from Equip_Usage.xml   
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportEquipUsage.Import(context, dbContext, fileLocation, SystemId);

            //*** Import the table of  "HET_RENTAL_AGREEMENT" and "HET_TIME_RECORD";  from Equip_Usage.xml   
            dbContext = new DbAppContext(null, options.Options, configuration);
            ImportRotation.Import(context, dbContext, fileLocation, SystemId);
        }
    }
}
