using HETSAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HETSAPI.Import
{
    /// <summary>
    /// Pair
    /// </summary>
    public class Pair
    {
        /// <summary>
        /// Hours
        /// </summary>
        public float Hours { get; set; }

        /// <summary>
        /// Rate
        /// </summary>
        public float Rate  { get; set; }

        /// <summary>
        /// Pair Constructir
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="rate"></param>
        public Pair (float hour, float rate)
        {
            Hours = hour;
            Rate = rate;
        }
    }

    /// <summary>
    /// Import Utility
    /// </summary>
    public static class ImportUtility
    {
        /// <summary>
        /// Utility function to add a new ImportMap to the database
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldTable"></param>
        /// <param name="oldKey"></param>
        /// <param name="newTable"></param>
        /// <param name="newKey"></param>
        public static void AddImportMap(DbAppContext dbContext, string oldTable, string oldKey, string newTable, int newKey)
        {
            ImportMap importMap = new ImportMap
            {
                OldTable = oldTable,
                OldKey = oldKey,
                NewTable = newTable,
                NewKey = newKey,
                AppCreateTimestamp = DateTime.Now,
                AppLastUpdateTimestamp = DateTime.Now
            };

            dbContext.ImportMaps.Add(importMap);
        }

        /// <summary>
        /// This is recording where the last import was stopped for specific table
        /// Use BCBidImport.todayDate as newTable entry
        /// Please note that NewTable enrty of the Import_Map table is aleats today's dat: BCBidImport.todayDate for identifying purpose. This means the restarting point inly carew
        /// what has done for today, not in the past.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldTable"></param>   This is lile "Owner_Progress" for th eimport progress entry (row) of Import_Map table
        /// <param name="oldKey"></param>  This is where stopped last time in string If this is "388888", then complete
        /// <param name="newKey"></param>  This is always a constant; for identification of progress entry of the table only. This is extraneous.
        public static void AddImportMapForProgress(DbAppContext dbContext, string oldTable, string oldKey, int newKey)
        {
            List<ImportMap> importMapList = dbContext.ImportMaps
                .Where(x => x.OldTable == oldTable && x.NewKey == newKey)
                .ToList();

            if (importMapList.Count == 0)
            {
                AddImportMap(dbContext, oldTable, oldKey, BCBidImport.TodayDate, newKey);
            }
            else
            {
                //Sometimes there are multiple progress entries exists for the same xml file import. 
                // In that case, the extra one should be deleted and the correct one should be updated. 
                int maxProgressCount = importMapList.Max(t => int.Parse(t.OldKey));

                foreach (ImportMap importMap in importMapList)
                {
                    if (importMap.OldKey == maxProgressCount.ToString())
                    {
                        importMap.NewTable = BCBidImport.TodayDate;
                        importMap.OldKey = Math.Max(int.Parse(oldKey), maxProgressCount).ToString();
                        importMap.AppLastUpdateTimestamp = DateTime.Now;
                        dbContext.ImportMaps.Update(importMap);
                    }
                    else
                    {
                        dbContext.ImportMaps.Remove(importMap);
                    }
                }
            }
        }

        /// <summary>
        /// Return startPoint:  0 - if ImportMap entry with oldTable == "%%%%%_Progress" does not exist
        ///                     sigId - if ImportMap entry with oldTable == "%%%%%_Progress" exist, and oldKey = signId.toString()
        ///                     Other int - if ImportMap entry with oldTable == "%%%%%_Progress" exist, and oldKey != signId.toString()
        /// The Import_Table entry for this kind of record has NewTable as  BCBidImport.todayDate.           
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldTableProgress"></param>
        /// <param name="sigId"></param>
        /// <returns></returns>
        public static int CheckInterMapForStartPoint(DbAppContext dbContext, string oldTableProgress, int sigId)
        {
            int startPoint;

            ImportMap importMap = (from u in dbContext.ImportMaps
                                   where u.OldTable == oldTableProgress && u.NewKey == sigId
                                   orderby int.Parse(u.OldKey) descending select u ).FirstOrDefault();

            // OlkdKey is recorded where the import progress stopped last time
            // when it stores the value of sigId, it signals the completion of the import of the corresponding xml file
            if (importMap != null)
            {                    
                startPoint = int.Parse(importMap.OldKey);
            }
            else    
            {
                // if the table of Import_MAP does not have any entry (row), it means the importing process has not started yet
                startPoint = 0;         
            }
            return startPoint;
        }
        
        /// <summary>
        /// Returns Memory stream from the input xml file
        /// </summary>
        /// <returns></returns>
        public static MemoryStream MemoryStreamGenerator(string xmlFileName, string oldTable, string fileLocation, string rootAttr)
        {
            string fullPath = fileLocation + Path.DirectorySeparatorChar + xmlFileName;

            // remove all new lines
            string contents = Regex.Replace(File.ReadAllText(fullPath), @"\r\n?|\n|s/\x00//g|[\x00-\x08\x0B\x0C\x0E-\x1F\x26]", "");  

            // get the contents of the first tag
            int startPos = contents.IndexOf('<') + 1;
            int endPos = contents.IndexOf('>');
            string tag = contents.Substring(startPos, endPos - startPos);

            contents = contents.Replace(tag, oldTable);
            contents = contents.Replace("<" + oldTable, "\r\n<" + oldTable);

            string fixedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" + 
                              "<" + rootAttr + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + 
                              contents + "\r\n</" + rootAttr + ">";

            string fixedPath = fullPath + ".fixed.xml";
            System.IO.File.WriteAllText(fixedPath, fixedXml);


            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fixedXml));
            return memoryStream;
        }

        /// <summary>
        /// Given a userString like: "Espey, Carol  (IDIR\cespey)"  Format the user and Add the user if no in the database
        /// Return the user or a default system user called "SYSTEM_HETS" as smSystemId
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userString"></param>
        /// <param name="smSystemId"></param>
        /// <returns></returns>
        public static User AddUserFromString(DbAppContext dbContext, string userString, string smSystemId)
        {
            // find the smUserId for the <Modified_By>
            int index;

            try
            {
                index = userString.IndexOf(@"(IDIR\", StringComparison.Ordinal);
            }
            catch
            {
                return dbContext.Users.FirstOrDefault(x => string.Equals(x.SmUserId, smSystemId, StringComparison.OrdinalIgnoreCase));
            }

            if (index >= 0)
            {
                try
                {
                    int commaPos = userString.IndexOf(@",", StringComparison.Ordinal);
                    int leftBreakPos = userString.IndexOf(@"(", StringComparison.Ordinal);
                    int startPos = userString.IndexOf(@"\", StringComparison.Ordinal);
                    int rightBreakPos = userString.IndexOf(@")", StringComparison.Ordinal);
                    string surName = userString.Substring(0, commaPos);
                    string givenName = userString.Substring(commaPos + 2, leftBreakPos - commaPos - 2);
                    string smUserId = userString.Substring(startPos + 1, rightBreakPos - startPos - 1).Trim();

                    User user = dbContext.Users.FirstOrDefault(x => string.Equals(x.SmUserId, smUserId, StringComparison.OrdinalIgnoreCase));

                    if (user == null)
                    {
                        user = new User
                        {
                            Surname = surName.Trim(),
                            GivenName = givenName.Trim(),
                            SmUserId = smUserId.Trim()
                        };

                        dbContext.Users.Add(user);
                        dbContext.SaveChangesForImport();
                    }
                    else if (user.Surname==null && surName.Trim().Length >=1)
                    {

                        user.Surname = surName.Trim();
                        user.GivenName = givenName.Trim();
                        dbContext.Users.Update(user);
                    }

                    return user;
                }
                catch
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
        /// Add a user with smUserId as systemId if not in the database 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="systemId"></param>
        public static void InsertSystemUser(DbAppContext dbContext, string systemId)
        {
            try
            {
                User sysUser = dbContext.Users.FirstOrDefault(x => x.SmUserId == systemId);
                if (sysUser == null)
                {
                    sysUser = new User();
                    sysUser.SmUserId = systemId;
                    sysUser.Surname = "System";
                    sysUser.GivenName = "HETS";
                    sysUser.Active = true;
                    sysUser.AppCreateTimestamp = DateTime.UtcNow;
                    dbContext.Users.Add(sysUser);
                    dbContext.SaveChangesForImport();
                }
            }          
            catch
            {
                // do nothing
            }
        }

        /// <summary>
        /// Convert Authority to userRole id
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        public static int GetRoleIdFromAuthority(string authority)
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
                    roleId = 1; // Special Admin?
                    break;
                case "U":
                    roleId = 1; // User Management?
                    break;
                default:
                    roleId = 1; // Unknown as regular user?
                    break;
            }

            return roleId;
        }
    }
}
