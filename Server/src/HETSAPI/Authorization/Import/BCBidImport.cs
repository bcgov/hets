using Hangfire.Console;
using Hangfire.Server;
using HETSAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace HETSAPI.Import
{

    public class BCBidImport
    {
        const string systemId = "SYSTEM_HETS";
        public static string todayDate = DateTime.Now.ToString("d");
        public static int sigId =  388888;

        static private void InsertSystemUser(DbAppContext dbContext)
        {
            Models.User sysUser = dbContext.Users.FirstOrDefault(x => x.SmUserId == systemId);
            if (sysUser == null)
                sysUser = new User();
            sysUser.SmUserId = systemId;
            sysUser.Surname = @"simon.di@gov.bc.ca";
            sysUser.Surname = "System";
            sysUser.GivenName = "HETS";
            sysUser.Active = true;
            sysUser.CreateTimestamp = DateTime.UtcNow;
            dbContext.Users.Add(sysUser);
            try
            {
                int iResult = dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                string iStr = e.ToString();
            }
            return;
        }
 
        /// <summary>
        /// Hangfire job to do the Annual Rollover tasks.
        /// </summary>
        /// <param name="connectionstring"></param>
        static public void ImportJob(PerformContext context, string connectionstring, string fileLocation)
        {

            // open a connection to the database.
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionstring);

            DbAppContext dbContext = new DbAppContext(null, options.Options);
            context.WriteLine("Starting Data Import Job");

            //Adding system Account if not there in the database.
            InsertSystemUser(dbContext);

            //*** start by importing Region from Region.xml. THis goes to table HETS_REGION
            // ImportRegion.Import(context, dbContext, fileLocation, systemId);

            //*** start by importing districts from District.xml. THis goes to table HETS_DISTRICT
           // dbContext = new DbAppContext(null, options.Options);
           // ImportDistrict.Import(context, dbContext, fileLocation, systemId);

            //*** start by importing Cities from HETS_City.xml to HET_CITY
            //dbContext = new DbAppContext(null, options.Options);
            // ImportCity.Import(context, dbContext, fileLocation, systemId);

            //*** Service Areas: from the file of Service_Area.xml to the table of HET_SERVICE_AREA
            //dbContext = new DbAppContext(null, options.Options);
            // ImportServiceArea.Import(context, dbContext, fileLocation, systemId);

            //############################ We start here #####################################  
            //*** Importing the Local Areas from the file of Area.xml to the table of HET_LOCAL_AREA
            dbContext = new DbAppContext(null, options.Options);
            ImportLocalArea.Import(context, dbContext, fileLocation, systemId);

            //*** Users from User_HETS.xml. This has effects on Table HET_USER and HET_USER_ROLE  
            dbContext = new DbAppContext(null, options.Options);
            ImportUser.Import(context, dbContext, fileLocation, systemId);

            //*** Owners: This has effects on Table HETS_OWNER and HETS_Contact
            dbContext = new DbAppContext(null, options.Options);
            ImportOwner.Import(context, dbContext, fileLocation, systemId);

            //*** Import Dump_Truck  from Dump_Truck.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportProject.Import(context, dbContext, fileLocation, systemId);

            //*** Import Equiptment type from EquipType.xml This has effects on Table HET_USER and HET_EQUIPMENT_TYPE  
            dbContext = new DbAppContext(null, options.Options);
            ImportDisEquipType.Import(context, dbContext, fileLocation, systemId);

            //*** Import Equiptment  from Equip.xml  This has effects on Table HET_USER and HET_EQUIP
            dbContext = new DbAppContext(null, options.Options);
            ImportEquip.Import(context, dbContext, fileLocation, systemId);

            //*** Import Dump_Truck  from Dump_Truck.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportDumpTruck.Import(context, dbContext, fileLocation,  systemId);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            //*** Import Equipment_Attached  from Equip_Attach.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportEquipAttach.Import(context, dbContext, fileLocation,  systemId);


            //*** Import the table of "HET_DISTRICT_EQUIPMENT_TYPE"  from Block.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportBlock.Import(context, dbContext, fileLocation,  systemId);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs.ToString());

            //*** Import the table of  "HET_RENTAL_AGREEMENT" and "HET_TIME_RECORD";  from Equip_Usage.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportEquipUsage.Import(context, options, dbContext, fileLocation, systemId);

            //*** Import the table of  "HET_RENTAL_AGREEMENT" and "HET_TIME_RECORD";  from Equip_Usage.xml   
            dbContext = new DbAppContext(null, options.Options);
            ImportRotation.Import(context, dbContext, fileLocation, systemId);
        }
    }
}
