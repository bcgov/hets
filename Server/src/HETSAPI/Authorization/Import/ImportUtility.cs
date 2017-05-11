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
    public class ImportUtility
    {
        /// <summary>
        /// Utility function to add a new ImportMap to the database
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldTable"></param>
        /// <param name="oldKey"></param>
        /// <param name="newTable"></param>
        /// <param name="newKey"></param>
        static public void AddImportMap(DbAppContext dbContext, string oldTable, int oldKey, string newTable, int newKey)
        {
            ImportMap importMap = new Models.ImportMap();
            importMap.OldTable = oldTable;
            importMap.OldKey = oldKey;
            importMap.NewTable = newTable;
            importMap.NewKey = newKey;
            dbContext.ImportMaps.Add(importMap);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Generates Memory stream from the input xml file
        /// </summary>
        /// <returns></returns>
        static public MemoryStream memoryStreamGenerator(string xmlFileName, string oldTable, string fileLocation, string rootAttr)
        {
            // fileLocation = @"H:\uploads\tmp";    //This is to test xml on network drive - network drive needs proper permission
            string fullPath = fileLocation + Path.DirectorySeparatorChar + xmlFileName;
            string contents = Regex.Replace(File.ReadAllText(fullPath), @"\r\n?|\n|[\x00-\x08\x0B\x0C\x0E-\x1F\x26]", "");  //Getting rid of all the new lines as well

            //string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            //return Regex.Replace(txt, r, "", RegexOptions.Compiled);


            // get the contents of the first tag.
            int startPos = contents.IndexOf('<') + 1;
            int endPos = contents.IndexOf('>');
            string tag = contents.Substring(startPos, endPos - startPos);

            contents = contents.Replace(tag, oldTable);


            string fixedXML = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
                              + "<" + rootAttr + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\n"
                              + contents
                              + "</" + rootAttr + ">";

            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fixedXML));
            return memoryStream;
        }


        /// <summary>
        /// /// <summary>
        /// Given a userString like: "Espey, Carol  (IDIR\cespey)"  Format the user and Add the user if no in the database
        /// Return the user or a default system user called "SYSTEM_HETS" as smSystemId
        /// </summary>
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userString"></param>
        /// <param name="smSystemId"></param>
        /// <returns></returns>
        static public Models.User AddUserFromString(DbAppContext dbContext, string userString, string smSystemId)
        {
            // Find out the smUserId for the <Modified_By>
            int index = -1;
            try
            {
                index = userString.IndexOf(@"(IDIR\");
            }
            catch
            {
                return dbContext.Users.FirstOrDefault(x => x.SmUserId == smSystemId);
            }
            if (index > 0)
            {
                try
                {
                    int commaPos = userString.IndexOf(@",");
                    int leftBreakPos = userString.IndexOf(@"(");
                    int startPos = userString.IndexOf(@"\");
                    int rightBreakPos = userString.IndexOf(@")");
                    string surName = userString.Substring(0, commaPos);
                    string givenName = userString.Substring(commaPos + 2, leftBreakPos - commaPos - 2);
                    string smUserId = userString.Substring(startPos + 1, rightBreakPos - startPos - 1);
                    Models.User user = dbContext.Users.FirstOrDefault(x => x.SmUserId == smUserId);
                    if (user == null)
                    {
                        user = new Models.User();
                        user.Surname = surName.Trim();
                        user.GivenName = givenName.Trim();
                        user.SmUserId = smUserId.Trim();
                        dbContext.Users.Add(user);
                        dbContext.SaveChanges();
                    }
                    return user;
                }
                catch (Exception e)
                {
                    return dbContext.Users.FirstOrDefault(x => x.SmUserId == smSystemId);
                }
            }
            else
            {
                return dbContext.Users.FirstOrDefault(x => x.SmUserId == smSystemId);
            }
        }


        /// <summary>
        /// Convert Authority to userRole id
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        static public int GetRoleIdFromAuthority(string authority)
        {
            int roleId;
            switch (authority)
            {
                case "A":
                    roleId = 2; // Adminsitrator
                    break;
                case "N":
                    roleId = 1; // Regular User
                    break;
                case "R":
                    roleId = 3; // Special Admin
                    break;
                case "U":
                    roleId = 4; // User Management
                    break;
                default:
                    roleId = 1; // Unknown as regular user
                    break;
            }
            return roleId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="performContext"></param>
        /// <param name="dbContext"></param>
        static private void SaveChangesToDatabase(PerformContext performContext, DbAppContext dbContext)
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                performContext.WriteLine("*** ERROR ***");
                performContext.WriteLine(e.ToString());
            }
        }
    }
}
