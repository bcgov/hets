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
    public class ImportOwner
    {
        public static string oldTable = "Owner";
        public static string newTable = "HET_OWNER";
        public static string xmlFileName = "Owner.xml";
       // public static string jsonOwnerFileName = @"C:\temp\Import\Owners\Owner.json";
       // public static string jsonContactFileName = @"C:\temp\Import\Owners\Contact.json";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="fileLocation"></param>
        /// <param name="systemId"></param>
        static public void Import(PerformContext performContext, DbAppContext dbContext, string fileLocation, string systemId)
        {
            List<Models.Owner> _data = new List<Models.Owner>();
            int maxOwnerIndex = dbContext.Owners.Max(x => x.Id);
            int maxContactIndex = dbContext.Contacts.Max(x => x.Id);

            try
            {
                string rootAttr = "ArrayOf" + oldTable;

                //Create Processer progress indicator
                performContext.WriteLine("Processing " + oldTable);
                var progress = performContext.WriteProgressBar();
                progress.SetValue(0);

                // create serializer and serialize xml file
                XmlSerializer ser = new XmlSerializer(typeof(Owner[]), new XmlRootAttribute(rootAttr));
                MemoryStream memoryStream = ImportUtility.memoryStreamGenerator(xmlFileName, oldTable, fileLocation, rootAttr);
                HETSAPI.Import.Owner[] legacyItems = (HETSAPI.Import.Owner[])ser.Deserialize(memoryStream);
                int ii = 1;
                foreach (var item in legacyItems.WithProgress(progress))
                {
                    // see if we have this one already.
                    ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable && x.OldKey == item.Popt_Id.ToString());

                    if (importMap == null) // new entry
                    {
                        Models.Owner owner = null;
                        CopyToInstance(performContext, dbContext, item, ref owner, systemId, ref maxOwnerIndex, ref maxContactIndex);
                        _data.Add(owner);
                        ImportUtility.AddImportMap(dbContext, oldTable, item.Popt_Id.ToString(), newTable, owner.Id);
                    }
                    else // update
                    {
                        Models.Owner owner = dbContext.Owners.FirstOrDefault(x => x.Id == importMap.NewKey);
                        if (owner == null) // record was deleted
                        {
                            CopyToInstance(performContext, dbContext, item, ref owner, systemId, ref maxOwnerIndex, ref maxContactIndex);
                            // update the import map.
                            importMap.NewKey = owner.Id;
                            dbContext.ImportMaps.Update(importMap);
                        }
                        else // ordinary update.
                        {
                            CopyToInstance(performContext, dbContext, item, ref owner, systemId, ref maxOwnerIndex, ref maxContactIndex);
                            // touch the import map.
                            importMap.LastUpdateTimestamp = DateTime.UtcNow;
                            dbContext.ImportMaps.Update(importMap);
                        }
                    }
                    if (ii++ % 500 == 0)
                    {
                        try
                        {
                            int iResult =  dbContext.SaveChangesForImport();
                        }
                        catch (Exception e)
                        {
                            string iStr = e.ToString();
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

            // Write to Json file
            //WriteToOwnerJsonFile(jsonOwnerFileName, _data);

            try
            {
                int iResult = dbContext.SaveChangesForImport();
            }
            catch (Exception e)
            {
                string iStr = e.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        /// <param name="oldObject"></param>
        /// <param name="owner"></param>
        /// <param name="systemId"></param>
        /// <param name="maxOwnerIndex"></param>
        /// <param name="maxContactIndex"></param>
        static private void CopyToInstance(PerformContext performContext, DbAppContext dbContext, HETSAPI.Import.Owner oldObject, ref Models.Owner owner,
          string systemId, ref int maxOwnerIndex, ref int maxContactIndex)
        {
            bool isNew = false;
            if (owner == null)
            {
                isNew = true;
                owner = new Models.Owner();
                owner.Id =  ++maxOwnerIndex;
            }

            //Add the user specified in oldObject.Modified_By and oldObject.Created_By if not there in the database
            Models.User modifiedBy = ImportUtility.AddUserFromString(dbContext, oldObject.Modified_By, systemId);
            Models.User createdBy = ImportUtility.AddUserFromString(dbContext, oldObject.Created_By, systemId);

            // The followings are the data mapping
            //  owner.LocalAreaId = oldObject.Area_Id;
            try
            {
                owner.IsMaintenanceContractor = (oldObject.Maintenance_Contractor.Trim() == "Y") ? true : false;
            }
            catch
            {

            }
            try
            {
                // owner.LocalAreaId = oldObject.Area_Id;
                owner.LocalArea = dbContext.LocalAreas.FirstOrDefault(x => x.Id == oldObject.Area_Id);
            }
            catch
            {

            }
            try
            {
                owner.CGLEndDate = DateTime.Parse(oldObject.CGL_End_Dt.Trim().Substring(0, 10));
            }
            catch
            {

            }
            try
            {
                owner.WorkSafeBCExpiryDate = DateTime.Parse(oldObject.WCB_Expiry_Dt.Trim().Substring(0, 10));
            }
            catch
            {
            }
            try
            {
                owner.WorkSafeBCPolicyNumber = oldObject.WCB_Num.Trim();
            }
            catch
            {
            }
            try
            {
                owner.OrganizationName = oldObject.CGL_Company.Trim();
            }
            catch
            {
            }
            try
            {
                owner.ArchiveCode = oldObject.Archive_Cd;
            }
            catch
            {
            }

            try
            {
                owner.Status = oldObject.Status_Cd.Trim();
            }
            catch
            {
            }


            Contact con = dbContext.Contacts.FirstOrDefault(x => x.GivenName.ToUpper() == oldObject.Owner_First_Name.Trim().ToUpper() && x.Surname.ToUpper() == oldObject.Owner_Last_Name.Trim().ToUpper());
            if (con == null)
            {
                con = new Contact(++maxContactIndex);
                try
                {
                    con.Surname = oldObject.Owner_Last_Name.Trim();
                    con.GivenName = oldObject.Owner_First_Name.Trim();
                    owner.OwnerEquipmentCodePrefix = con.GivenName.Substring(0, 1) + con.Surname.Substring(0, 1);
                }
                catch
                {
                }

                con.FaxPhoneNumber = "";
                con.Province = "BC";
                //    owner.PrimaryContact.PostalCode = oldObject.
                try
                {
                    con.Notes = new string(oldObject.Comment.Take(511).ToArray());
                }
                catch
                {
                }
                dbContext.Contacts.Add(con);
            }


            // TODO finish mapping here 
            if (isNew)
            {
                owner.CreateUserid = createdBy.SmUserId;
                try
                {
                    owner.CreateTimestamp = DateTime.Parse(oldObject.Created_Dt.Trim().Substring(0, 10)); // DateTime.UtcNow;
                }
                catch
                {
                }
                con.CreateTimestamp = DateTime.UtcNow;
                con.CreateUserid = createdBy.SmUserId;
                owner.PrimaryContact = con;
                dbContext.Owners.Add(owner);
            }
            else  // The owner existed in the database
            {
                try
                {
                    owner.LastUpdateUserid = systemId;
                    owner.LastUpdateTimestamp = DateTime.UtcNow;
                    con.LastUpdateTimestamp = DateTime.UtcNow;
                    con.LastUpdateUserid = modifiedBy.SmUserId;
                    owner.PrimaryContact = con;
                }
                catch
                {

                }
                dbContext.Owners.Update(owner);
            }
        }

        static private void WriteToOwnerJsonFile(string fileName, List<Models.Owner> _data)
        {
            string newJson = JsonConvert.SerializeObject(_data, Formatting.Indented);
            File.WriteAllText(fileName, newJson);
             
        }
    }
}


