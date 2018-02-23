using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using HETSAPI.Models;

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
        /// Hangfire job to do the data import tasks.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        /// <param name="fileLocation"></param>
        public static void ImportJob(PerformContext context, string connectionstring, string fileLocation)
        {
            // open a connection to the database.
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionstring);

            DbAppContext dbContext = new DbAppContext(null, options.Options);
            context.WriteLine("Starting Data Import Job");

            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(dbContext, SystemId);

            dbContext = new DbAppContext(null, options.Options);
            ImportServiceArea.Import(context, dbContext, fileLocation, SystemId);

            //*** Importing the Local Areas from the file of Area.xml to the table of HET_LOCAL_AREA
            dbContext = new DbAppContext(null, options.Options);
            ImportLocalArea.Import(context, dbContext, fileLocation, SystemId);            

            //*** Users from User_HETS.xml. This has effects on Table HET_USER and HET_USER_ROLE  
            dbContext = new DbAppContext(null, options.Options);
            ImportUser.Import(context, dbContext, fileLocation, SystemId);

            //*** Owners: This has effects on Table HETS_OWNER and HETS_Contact
            dbContext = new DbAppContext(null, options.Options);
            ImportOwner.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Dump_Truck  from Dump_Truck.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportProject.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equiptment type from EquipType.xml This has effects on Table HET_USER and HET_EQUIPMENT_TYPE  
            dbContext = new DbAppContext(null, options.Options);
            ImportDisEquipType.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equiptment  from Equip.xml  This has effects on Table HET_USER and HET_EQUIP
            dbContext = new DbAppContext(null, options.Options);
            ImportEquip.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Dump_Truck  from Dump_Truck.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportDumpTruck.Import(context, dbContext, fileLocation,  SystemId);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            //*** Import Equipment_Attached  from Equip_Attach.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportEquipAttach.Import(context, dbContext, fileLocation,  SystemId);

            //*** Import the table of "HET_DISTRICT_EQUIPMENT_TYPE"  from Block.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportBlock.Import(context, dbContext, fileLocation,  SystemId);
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs.ToString());

            //*** Import the table of  "HET_RENTAL_AGREEMENT" and "HET_TIME_RECORD";  from Equip_Usage.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportEquipUsage.Import(context, dbContext, fileLocation, SystemId);

            //*** Import the table of  "HET_RENTAL_AGREEMENT" and "HET_TIME_RECORD";  from Equip_Usage.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportRotation.Import(context, dbContext, fileLocation, SystemId);
        }


        /// <summary>
        /// Hangfire job to do the data import tasks.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        /// <param name="fileLocation"></param>
        public static void ObfuscationJob(PerformContext context, string connectionstring, string sourceFileLocation, string destinationFileLocation)
        {
            // open a connection to the database.
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionstring);

            DbAppContext dbContext = new DbAppContext(null, options.Options);
            context.WriteLine("Starting Data Import Job");


            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(dbContext, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            // Process local areas.
            ImportLocalArea.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            ImportServiceArea.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            //*** Users from User_HETS.xml. This has effects on Table HET_USER and HET_USER_ROLE              
            ImportUser.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            //*** Owners: This has effects on Table HETS_OWNER and HETS_Contact            
            ImportOwner.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            //*** Import Dump_Truck  from Dump_Truck.xml               
            ImportProject.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            //*** Import Equipment type from EquipType.xml This has effects on Table HET_USER and HET_EQUIPMENT_TYPE              
            ImportDisEquipType.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            //*** Import Equiptment  from Equip.xml  This has effects on Table HET_USER and HET_EQUIP            
            ImportEquip.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            //*** Import Dump_Truck  from Dump_Truck.xml               
            ImportDumpTruck.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //*** Import Equipment_Attached  from Equip_Attach.xml               
            ImportEquipAttach.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            //*** Import the table of "HET_DISTRICT_EQUIPMENT_TYPE"  from Block.xml               
            ImportBlock.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            watch.Stop();
            long elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs.ToString());

            //*** Import the table of  "HET_RENTAL_AGREEMENT" and "HET_TIME_RECORD";  from Equip_Usage.xml               
            ImportEquipUsage.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);

            //*** Obfuscate rotations            
            ImportRotation.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
        }

    }
}
