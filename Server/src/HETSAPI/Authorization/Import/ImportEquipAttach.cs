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
    public class ImportEquipAttach
    {
        const string oldTable = "EquipAttach";
        const string newTable = "HET_EQUIPMENT_ATTACHMENT";
        const string xmlFileName = "Equip_Attach.xml";

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
                XmlSerializer ser = new XmlSerializer(typeof(EquipAttach[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.EquipAttach[] legacyItems = (HETSAPI.Import.EquipAttach[])ser.Deserialize(memoryStream);
                int ii = 0;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already. We used old combine because item.Equip_Id is not unique.
                    int oldKeyCombined = item.Equip_Id??0 * 100 + item.Attach_Seq_Num??0;
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == oldKeyCombined);

                    if (importMap == null) // new entry
                    {
                        if (item.Equip_Id > 0)
                        {
                            Models.EquipmentAttachment instance = null;
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            ImportUtility.AddImportMap(dbContext, oldTable, oldKeyCombined, newTable, instance.Id);
                        }
                    }
                    else // update
                    {
                        Models.EquipmentAttachment instance = dbContext.EquipmentAttachments.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (instance == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            // update the import map.
                            importMap.NewKey = instance.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref instance, systemId);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }

                    if (++ii % 1000 == 0)
                    {
                        try
                        {
                            dbContext.SaveChanges();
                        }
                        catch
                        {

                        }
                    }
                }
                performContext.WriteLine("*** Done ***");
            }

            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch
            {

            }
        }


        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.EquipAttach oldObject, ref Models.EquipmentAttachment instance, string systemId)
        {
            if (oldObject.Equip_Id <= 0)
                return;

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
          //  Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, "", systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            if (instance == null)
            {
                instance = new Models.EquipmentAttachment();
                int equipId = oldObject.Equip_Id ?? -1;

                Models.Equipment equipment = dbContext.Equipments.FirstOrDefault(x => x.Id == equipId);
                if (equipment != null)
                {
                    instance.Equipment = equipment;
                    instance.EquipmentId = equipment.Id;
                }

                instance.Description = oldObject.Attach_Desc == null ? "" : oldObject.Attach_Desc;
                instance.TypeName = (oldObject.Attach_Seq_Num ?? -1).ToString();
                instance.CreateTimestamp = DateTime.Parse((oldObject.Created_Dt == null ? "1900-01-01" : oldObject.Created_Dt).Trim().Substring(0, 10));
                instance.CreateUserid = createdBy.SmUserId;

                dbContext.EquipmentAttachments.Add(instance);
            }
            else
            {
                instance = dbContext.EquipmentAttachments
                    .First(x => x.EquipmentId == oldObject.Equip_Id && x.TypeName == (oldObject.Attach_Seq_Num?? -2).ToString());
                instance.LastUpdateUserid = systemId; // modifiedBy.SmUserId;
                instance.LastUpdateTimestamp = DateTime.UtcNow;
                dbContext.EquipmentAttachments.Update(instance);
            }
        }

    }
}

