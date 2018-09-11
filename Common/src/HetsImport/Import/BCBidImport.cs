using System;
using Hangfire.Console;
using Hangfire.Server;
using HetsData.Model;

namespace HetsImport.Import
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
        /// Hangfire job to do the data import tasks
        /// </summary>
        /// <param name="context"></param>
        /// <param name="seniorityScoringRules"></param>
        /// <param name="connectionString"></param>
        /// <param name="fileLocation"></param>
        public static void ImportJob(PerformContext context, string seniorityScoringRules, string connectionString, string fileLocation)
        {
            DbAppContext dbContext = new DbAppContext(connectionString);
            context.WriteLine("Starting Data Import Job");

            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(dbContext, SystemId);
            
            //*** Import Service Areas from ServiceArea.xml (HET_SERVICE_AREA)
            dbContext = new DbAppContext(connectionString);
            ImportServiceArea.Import(context, dbContext, fileLocation, SystemId);
            
            //*** Import Local Areas from Area.xml (HET_LOCAL_AREA)
            dbContext = new DbAppContext(connectionString);
            ImportLocalArea.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Users from User_HETS.xml (HET_USER and HET_USER_ROLE)
            dbContext = new DbAppContext(connectionString);
            ImportUser.Import(context, dbContext, fileLocation, SystemId);            

            //*** Import Owners from Owner.xml (HETS_OWNER and HETS_Contact)
            dbContext = new DbAppContext(connectionString);
            ImportOwner.Import(context, dbContext, fileLocation, SystemId);            

            //*** Import District Equipment Type from EquipType.xml (HET_DISTRICT_EQUIPMENT_TYPE)
            dbContext = new DbAppContext(connectionString);
            ImportDistrictEquipmentType.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equipment from Equip.xml (HET_EQUIPMENT)
            dbContext = new DbAppContext(connectionString);
            ImportEquip.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Dump Truck from Dump_Truck.xml (HET_EQUIPMENT_TYPE)  
            dbContext = new DbAppContext(connectionString);
            ImportDumpTruck.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equipment Attachments from Equip_Attach.xml (HET_EQUIPMENT_ATTACHMENT)  
            dbContext = new DbAppContext(connectionString);
            ImportEquipAttach.Import(context, dbContext, fileLocation, SystemId);

            //*** Process Equipment Block Assignments
            dbContext = new DbAppContext(connectionString);
            ImportEquip.ProcessBlocks(context, seniorityScoringRules, dbContext, SystemId);

            //*** Import Projects from Project.xml (HET_PROJECT)
            dbContext = new DbAppContext(connectionString);
            ImportProject.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Blocks / Local Area Rotation List from Block.xml (HET_DISTRICT_ROTATION_LIST)
            dbContext = new DbAppContext(connectionString);
            ImportBlock.Import(context, dbContext, fileLocation, SystemId);

            //*** Import Equipment Usage (Time) from Equip_Usage.xml (HET_RENTAL_AGREEMENT and HET_TIME_RECORD) 
            dbContext = new DbAppContext(connectionString);
            ImportEquipUsage.Import(context, dbContext, fileLocation, SystemId);

            // *** Fix Contact Foreign Key Relationships
            dbContext = new DbAppContext(connectionString);
            ImportOwner.FixPrimaryContacts(context, dbContext);

            // *** Final Step - fix the database sequences
            dbContext = new DbAppContext(connectionString);
            ImportServiceArea.ResetSequence(context, dbContext);
            ImportLocalArea.ResetSequence(context, dbContext);
            ImportUser.ResetSequence(context, dbContext);
            ImportOwner.ResetSequence(context, dbContext);            
            ImportDistrictEquipmentType.ResetSequence(context, dbContext);
            ImportEquip.ResetSequence(context, dbContext);
            ImportEquipAttach.ResetSequence(context, dbContext);
            ImportProject.ResetSequence(context, dbContext);
            ImportBlock.ResetSequence(context, dbContext);
            ImportEquipUsage.ResetSequence(context, dbContext);
        }

        /// <summary>
        /// Hangfire job to do the data import tasks
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionString"></param>
        /// <param name="sourceFileLocation"></param>
        /// <param name="destinationFileLocation"></param>
        public static void ObfuscationJob(PerformContext context, string connectionString, string sourceFileLocation, string destinationFileLocation)
        {
            // open a connection to the database.
            DbAppContext dbContext = new DbAppContext(connectionString);
            context.WriteLine("Starting Data Import Job");

            // adding system Account if not there in the database
            ImportUtility.InsertSystemUser(dbContext, SystemId);
            dbContext = new DbAppContext(connectionString);

            // Process service areas
            ImportServiceArea.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // Process local areas
            ImportLocalArea.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // Process users
            ImportUser.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);
            
            // process owners
            ImportOwner.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // process equipment types        
            ImportDistrictEquipmentType.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // process dump trucks             
            ImportDumpTruck.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // Process equipment attachments              
            ImportEquipAttach.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // process equipment
            ImportEquip.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // Process projects
            ImportProject.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // Process blocks
            ImportBlock.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);
            dbContext = new DbAppContext(connectionString);

            // Process equipment usage               
            ImportEquipUsage.Obfuscate(context, dbContext, sourceFileLocation, destinationFileLocation, SystemId);     
        }
    }
}
