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
    public class ImportRotation
    {
        const string oldTable = "Rotation_Doc";
        const string newTable = "HET_NOTE";
        const string xmlFileName = "Rotation_Doc.xml";

        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Rotation_Doc[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Rotation_Doc[] legacyItems = (HETSAPI.Import.Rotation_Doc[])ser.Deserialize(memoryStream);

                //Use this list to save a trip to query database in each iteration
                List<Models.Equipment> equips = dbContext.Equipments
                        .Include(x => x.DumpTruck)
                        .Include(x => x.DistrictEquipmentType)
                        .ToList();
                List<Models.Project> projs = dbContext.Projects
                        .ToList();

                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    string oldKey = item.Equip_Id + item.Note_Dt + item.Created_Dt;
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == oldKey);

                    if (importMap == null) // new entry
                    {
                        Models.Note instance = null;
                        CopyToInstance(performContext, dbContext, item, ref instance, equips, projs, systemId);
                        ImportUtility.AddImportMap(dbContext, oldTable, oldKey, newTable, instance.Id);
                    }
                    else // update
                    {
                        Models.Note instance = dbContext.Notes.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, equips, projs, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, equips, projs, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    if (++ii % 1000 == 0)
                    {
                        try
                        {
                            int iResult = dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            string iStr = e.ToString();
                        }
                    }
                }
                performContext.WriteLine("*** Done ***");
                try
                {
                    int iResult = dbContext.SaveChangesForImport();
                }
                catch (Exception e)
                {
                    string iStr = e.ToString();
                }
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Copy xml item to instance (table entries)
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="instance"></param>
        /// <param name="systemId"></param>
        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Rotation_Doc oldObject, ref Models.Note instance,
            List<Models.Equipment> equips, List<Models.Project> projs, string systemId)
        {
            bool isNew = false;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                isNew = true;
                instance = new Models.Note();

                Models.Project proj = projs.FirstOrDefault(x => x.Id == oldObject.Project_Id);    
                Models.Equipment equip = equips.FirstOrDefault(x => x.Id == oldObject.Equip_Id);
                if (equip != null)
                {
                    if (equip.Notes == null)
                        equip.Notes = new List<Note>();
                    Models.Note note = new Models.Note(oldObject.Reason, true);
                    if (proj != null)
                    { // The current model does not allow Project Id to be added to thge Note. while Note model should have Project ID
                       // note. = oldObject.Project_Id;
                    }
                    equip.Notes.Add(note);
                    dbContext.Equipments.Update(equip);
                } 
            }
        }
    }
}

