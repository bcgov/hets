using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Hangfire.Console.Progress;
using HETSAPI.ImportModels;
using HETSAPI.Models;

namespace HETSAPI.Import
{
    /// <summary>
    /// Import Rotation Records
    /// </summary>
    public class ImportRotation
    {
        const string OldTable = "Rotation_Doc";
        const string NewTable = "HET_NOTE";
        const string XmlFileName = "Rotation_Doc.xml";

        /// <summary>
        /// Progress Property
        /// </summary>
        public static string OldTableProgress => OldTable + "_Progress";

        /// <summary>
        /// Import Rotations
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        public static void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            // check the start point. If startPoint ==  sigId then it is already completed
            int startPoint = ImportUtility.CheckInterMapForStartPoint(dbContext, OldTableProgress, BCBidImport.SigId);

            if (startPoint == BCBidImport.SigId)    // this means the import job it has done today is complete for all the records in the xml file.
            {
                performContext.WriteLine("*** Importing " + XmlFileName + " is complete from the former process ***");
                return;
            }

            try
            {
                string rootAttr = "ArrayOf" + OldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + OldTable);
                IProgressBar progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Rotation_Doc[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.MemoryStreamGenerator(XmlFileName, OldTable, fileLocation, rootAttr);
                Rotation_Doc[] legacyItems = (Rotation_Doc[])ser.Deserialize(memoryStream);

                //Use this list to save a trip to query database in each iteration
                List<Equipment> equips = dbContext.Equipments
                        .Include(x => x.DumpTruck)
                        .Include(x => x.DistrictEquipmentType)
                        .ToList();

                int ii = startPoint;

                // skip the portion already processed
                if (startPoint > 0)    
                {
                    legacyItems = legacyItems.Skip(ii).ToArray();
                }

                foreach (Rotation_Doc item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already
                    string oldKey = item.Equip_Id + item.Note_Dt + item.Created_Dt;
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == OldTable && x.OldKey == oldKey);

                    // new entry
                    if (importMap == null) 
                    {
                        Note instance = null;
                        CopyToInstance(dbContext, item, ref instance, equips);
                        ImportUtility.AddImportMap(dbContext, OldTable, oldKey, NewTable, instance.Id);
                    }
                    else // update
                    {
                        Note instance = dbContext.Notes.FirstOrDefault(x => x.Id == importMap.NewKey);

                        // record was deleted
                        if (instance == null) 
                        {
                            CopyToInstance(dbContext, item, ref instance, equips);

                            // update the import map
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(dbContext, item, ref instance, equips);

                            // touch the import map
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    // save change to database periodically to avoid frequent writing to the database
                    if (++ii % 1000 == 0)
                    {
                        try
                        {
                            ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, ii.ToString(), BCBidImport.SigId);
                            dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            performContext.WriteLine("Error saving data " + e.Message);
                        }
                    }
                }

                try
                {
                    performContext.WriteLine("*** Importing " + XmlFileName + " is Done ***");
                    ImportUtility.AddImportMapForProgress(dbContext, OldTableProgress, BCBidImport.SigId.ToString(), BCBidImport.SigId);
                    dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    performContext.WriteLine("Error saving data " + e.Message);
                }
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Map data
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="equips"></param>
        private static void CopyToInstance(DbAppContext dbContext, Rotation_Doc oldObject, ref Note instance, List<Equipment> equips)
        {
            if (instance == null)
            {
                instance = new Note();

                Equipment equip = equips.FirstOrDefault(x => x.Id == oldObject.Equip_Id);

                if (equip != null)
                {
                    if (equip.Notes == null)
                        equip.Notes = new List<Note>();

                    Note note = new Note
                    {
                        Text = new string(oldObject.Reason.Take(2048).ToArray()),
                        IsNoLongerRelevant = true
                    };
                    
                    equip.Notes.Add(note);
                    dbContext.Equipments.Update(equip);
                } 
            }
        }
    }
}

