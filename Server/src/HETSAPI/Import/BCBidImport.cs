using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using HETSAPI.Models;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Import
{
    /// <summary>
    /// BC Bid Data Import Functions
    /// </summary>
    public static class BcBidImport
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
        /// <param name="configuration"></param>
        /// <param name="connectionstring"></param>
        /// <param name="fileLocation"></param>
        public static void ImportJob(PerformContext context, IConfiguration configuration, string connectionstring, string fileLocation)
        {
            // open a connection to the database.
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionstring);

            DbAppContext dbContext = new DbAppContext(null, options.Options);
            context.WriteLine("Starting Data Import Job");

            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(dbContext, SystemId);
            
            //*** Import Service Areas from ServiceArea.xml (HET_SERVICE_AREA)
            dbContext = new DbAppContext(null, options.Options);
            ImportServiceArea.Import(context, dbContext, fileLocation, SystemId);
            
            //*** Import Local Areas from Area.xml (HET_LOCAL_AREA)
            dbContext = new DbAppContext(null, options.Options);
            ImportLocalArea.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Cities from HETS_City.xml (HET_CITY)   
            dbContext = new DbAppContext(null, options.Options);
            ImportCity.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Users from User_HETS.xml (HET_USER and HET_USER_ROLE)
            dbContext = new DbAppContext(null, options.Options);
            ImportUser.Import(context, dbContext, fileLocation, SystemId);            

            //*** Import Owners from Owner.xml (HETS_OWNER and HETS_Contact)
            dbContext = new DbAppContext(null, options.Options);
            ImportOwner.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equipment Type from EquipType.xml (HET_EQUIPMENT_TYPE)
            dbContext = new DbAppContext(null, options.Options);
            ImportEquipmentType.Import(context, dbContext, fileLocation, SystemId);

            //*** Import District Equipment Type from EquipType.xml (HET_DISTRICT_EQUIPMENT_TYPE)
            dbContext = new DbAppContext(null, options.Options);
            ImportDistrictEquipmentType.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equipment from Equip.xml (HET_EQUIPMENT)
            dbContext = new DbAppContext(null, options.Options);
            ImportEquip.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Dump Truck from Dump_Truck.xml (HET_EQUIPMENT_TYPE)  
            dbContext = new DbAppContext(null, options.Options);
            ImportDumpTruck.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equipment Attachments from Equip_Attach.xml (HET_EQUIPMENT_ATTACHMENT)  
            dbContext = new DbAppContext(null, options.Options);
            ImportEquipAttach.Import(context, dbContext, fileLocation, SystemId);

            //*** Process Equipment Block Assigments
            dbContext = new DbAppContext(null, options.Options);
            ImportEquip.ProcessBlocks(context, configuration, dbContext, SystemId);

            //*** Import Projects from Project.xml (HET_PROJECT)
            dbContext = new DbAppContext(null, options.Options);
            ImportProject.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Blocks / Local Area Rotation List from Block.xml (HET_DISTRICT_ROTATION_LIST)
            dbContext = new DbAppContext(null, options.Options);
            ImportBlock.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equipment Usage (Time) from Equip_Usage.xml (HET_RENTAL_AGREEMENT and HET_TIME_RECORD) 
            dbContext = new DbAppContext(null, options.Options);
            ImportEquipUsage.Import(context, dbContext, fileLocation, SystemId);

            // *** Final Step - fix the database sequences
            dbContext = new DbAppContext(null, options.Options);
            ImportServiceArea.ResetSequence(context, dbContext);
            ImportLocalArea.ResetSequence(context, dbContext);
            ImportCity.ResetSequence(context, dbContext);
            ImportUser.ResetSequence(context, dbContext);
            ImportOwner.ResetSequence(context, dbContext);            
            ImportEquipmentType.ResetSequence(context, dbContext);
            ImportDistrictEquipmentType.ResetSequence(context, dbContext);
            ImportEquip.ResetSequence(context, dbContext);
            ImportEquipAttach.ResetSequence(context, dbContext);
            ImportProject.ResetSequence(context, dbContext);
            ImportBlock.ResetSequence(context, dbContext);
            ImportEquipUsage.ResetSequence(context, dbContext);
        }
        

        /// <summary>
        /// Hangfire job to do the data import tasks.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        /// <param name="sourceFileLocation"></param>
        /// <param name="destinationFileLocation"></param>
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

            // Process service areas
            ImportServiceArea.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // Process local areas
            ImportLocalArea.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // Process local areas
            ImportCity.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // Process users
            ImportUser.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);
            
            // process owners
            ImportOwner.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // process equipment
            ImportEquip.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // process dump trucks             
            ImportDumpTruck.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // Process equipment attachments              
            ImportEquipAttach.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // Process projects
            ImportProject.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // Process blocks
            ImportBlock.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(null, options.Options);

            // Process equipment usage               
            ImportEquipUsage.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);     
        }
    }
}
