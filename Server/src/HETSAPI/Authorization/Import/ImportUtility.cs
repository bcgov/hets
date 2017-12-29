using HETSAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HETSAPI.Import
{

    public class Pair
    {
        public float Hours { get; set; }
        public float Rate  { get; set; }

        public Pair (float hour, float rate)
        {
            this.Hours = hour;
            this.Rate = rate;
        }
    }

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
        static public void AddImportMap(DbAppContext dbContext, string oldTable, string oldKey, string newTable, int newKey)
        {
            ImportMap importMap = new Models.ImportMap();
            importMap.OldTable = oldTable;
            importMap.OldKey = oldKey;
            importMap.NewTable = newTable;
            importMap.NewKey = newKey;
            importMap.CreateTimestamp = DateTime.Now;
            importMap.LastUpdateTimestamp = DateTime.Now;
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
        static public void AddImportMap_For_Progress(DbAppContext dbContext, string oldTable, string oldKey, int newKey)
        {
            List < Models.ImportMap>  importMapList = dbContext.ImportMaps
                .Where(x => x.OldTable == oldTable && x.NewKey == newKey)
                .ToList();

            if (importMapList.Count == 0)
            {
                ImportUtility.AddImportMap(dbContext, oldTable, oldKey, BCBidImport.todayDate, newKey);
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
                        importMap.NewTable = BCBidImport.todayDate;
                        importMap.OldKey = Math.Max(int.Parse(oldKey), maxProgressCount).ToString();
                        importMap.LastUpdateTimestamp = DateTime.Now;
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
        /// 
        /// Return startPoint:  0       - if ImportMap entry with oldTable == "%%%%%_Progress" does not exist
        ///                     sigId   - if ImportMap entry with oldTable == "%%%%%_Progress" exist, and oldKey = signId.toString()
        ///                     Other int   - if ImportMap entry with oldTable == "%%%%%_Progress" exist, and oldKey != signId.toString()
        /// The Import_Table entry for this kind of record has NewTable as  BCBidImport.todayDate.           
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="oldTable_Progress"></param>
        /// <param name="sigId"></param>
        /// <returns></returns>
        static public int CheckInterMapForStartPoint(DbAppContext dbContext, string oldTable_Progress,  int sigId)
        {
            int startPoint;
          //  ImportMap importMap = dbContext.ImportMaps.FirstOrDefault(x => x.OldTable == oldTable_Progress && x.NewTable == BCBidImport.todayDate && x.NewKey == sigId);
            ImportMap importMap = (from u in dbContext.ImportMaps
                                   where u.OldTable == oldTable_Progress && u.NewKey == sigId
                                   orderby int.Parse(u.OldKey) descending select u ).FirstOrDefault();
            if (importMap != null)
            {    
                // OlkdKey is recorded where the import progress stopped last time.
                // When it store the value of sigId, it signal the completion of the import of the corresponding xml file.
                startPoint = int.Parse(importMap.OldKey);
            }
            else    //If the table of Import_MAP does not have any entry (row), it means the importing process has not started yet.
            {
                startPoint = 0;         
            }
            return startPoint;
        }
        
        /// <summary>
        /// Generates Memory stream from the input xml file
        /// </summary>
        /// <returns></returns>
        static public MemoryStream memoryStreamGenerator(string xmlFileName, string oldTable, string fileLocation, string rootAttr)
        {
            // fileLocation = @"C:\uploads\tmp";    //This is to test xml on network drive - network drive needs proper permission
            string fullPath = fileLocation + Path.DirectorySeparatorChar + xmlFileName;
            string contents = Regex.Replace(File.ReadAllText(fullPath), @"\r\n?|\n|s/\x00//g|[\x00-\x08\x0B\x0C\x0E-\x1F\x26]", "");  //Getting rid of all the new lines as well
            //added s/\x00//g  in replaceAll("s/\x00//g","")
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
        /// <param name="addUser"></param>  -- to add the corresponding user or not.
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
                return dbContext.Users.FirstOrDefault(x => string.Equals(x.SmUserId, smSystemId, StringComparison.OrdinalIgnoreCase) );
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
                    string smUserId = userString.Substring(startPos + 1, rightBreakPos - startPos - 1).Trim();
                    Models.User user = dbContext.Users.FirstOrDefault(x => string.Equals(x.SmUserId, smUserId, StringComparison.OrdinalIgnoreCase));
                    if (user == null)
                    {
                        user = new Models.User();
                        user.Surname = surName.Trim();
                        user.GivenName = givenName.Trim();
                        user.SmUserId = smUserId.Trim();
                        dbContext.Users.Add(user);
                        dbContext.SaveChangesForImport();
                    }
                    else if (user.Surname==null && surName.Trim().Length >=1)
                    {

                        user.Surname = surName.Trim();
                        user.GivenName = givenName.Trim();
                        dbContext.Users.Update(user);
                     //   dbContext.SaveChangesForImport();  to save database query time.
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
        ///  Add a special system user with smUserId as systemId if not in the database 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="systemId"></param>
        static public void InsertSystemUser(DbAppContext dbContext, string systemId)
        {
            try
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
                int iResult = dbContext.SaveChangesForImport();
            }          
            catch (Exception e)
            {
                string iStr = e.ToString();
            }
            return;
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
