using HETSAPI.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

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
        /// <param name="newKey"></param>
        /// <param name="newTable"></param>
        public static void AddImportMapForProgress(DbAppContext dbContext, string oldTable, string oldKey, int newKey, string newTable)
        {
            List<ImportMap> importMapList = dbContext.ImportMaps
                .Where(x => x.OldTable == oldTable && 
                            x.NewKey == newKey &&
                            x.NewTable == newTable)
                .ToList();

            if (importMapList.Count == 0)
            {
                AddImportMap(dbContext, oldTable, oldKey, newTable, newKey);
            }
            else
            {
                // Sometimes there are multiple progress entries exists for the same xml file import. 
                // In that case, the extra one should be deleted and the correct one should be updated. 
                int maxProgressCount = importMapList.Max(t => int.Parse(t.OldKey));

                foreach (ImportMap importMap in importMapList)
                {
                    if (importMap.OldKey == maxProgressCount.ToString())
                    {
                        importMap.NewTable = newTable;
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
        /// <param name="newTable"></param>
        /// <returns></returns>
        public static int CheckInterMapForStartPoint(DbAppContext dbContext, string oldTableProgress, int sigId, string newTable)
        {
            ImportMap importMap = (
                        from u in dbContext.ImportMaps
                        where u.OldTable == oldTableProgress && 
                              u.NewKey == sigId &&
                              u.NewTable == newTable
                        orderby int.Parse(u.OldKey) descending
                        select u)
                    .FirstOrDefault();

            // OlkdKey is recorded where the import progress stopped last time
            // when it stores the value of sigId, it signals the completion of the import of the corresponding xml file
            int startPoint = importMap != null ? int.Parse(importMap.OldKey) : 0;

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

            // determine if the file has not been processed.  
            string fixedXml;

            if (contents.IndexOf(rootAttr, StringComparison.Ordinal) == -1) // it has not been processed
            {
                // get the contents of the first tag
                int startPos = contents.IndexOf('<') + 1;
                int endPos = contents.IndexOf('>');
                string tag = contents.Substring(startPos, endPos - startPos);

                contents = contents.Replace(tag, oldTable);
                contents = contents.Replace("<" + oldTable, "\r\n<" + oldTable);

                fixedXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                                  "<" + rootAttr + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                                  contents + "\r\n</" + rootAttr + ">";

                string fixedPath = fullPath + ".fixed.xml";
                File.WriteAllText(fixedPath, fixedXml);
            }
            else
            {
                fixedXml = contents;
            }            

            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fixedXml));
            return memoryStream;
        }

        public static void CreateObfuscationDestination (string fileLocation)
        {
            Directory.CreateDirectory(fileLocation);
        }

        public static FileStream GetObfuscationDestination(string xmlFileName, string fileLocation)
        {
            string destinationPath = fileLocation + Path.DirectorySeparatorChar + xmlFileName;
            FileStream fs = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
            return fs;
        }

        /// <summary>
        /// Given a userString like: "Espey, Carol (IDIR\cespey)" - frmat the user and add the user if no in the database
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
                    string surName = userString.Substring(0, commaPos).Trim();
                    string givenName = userString.Substring(commaPos + 2, leftBreakPos - commaPos - 2).Trim();
                    string smUserId = userString.Substring(startPos + 1, rightBreakPos - startPos - 1).Trim().ToLower();

                    User user = dbContext.Users.FirstOrDefault(x => string.Equals(x.SmUserId, smUserId, StringComparison.OrdinalIgnoreCase));

                    if (user == null)
                    {
                        // always add as inactive!
                        user = new User
                        {
                            Surname = surName,
                            GivenName = givenName,
                            SmUserId = smUserId,
                            Active = false,
                            AppCreateTimestamp = DateTime.UtcNow,
                            AppCreateUserid = smSystemId,
                            AppLastUpdateTimestamp = DateTime.UtcNow,                            
                            AppLastUpdateUserid = smSystemId
                        };                        

                        dbContext.Users.Add(user);
                        dbContext.SaveChangesForImport();
                    }
                    else if (user.Surname == null && surName.Length >= 1)
                    {
                        user.Surname = surName;
                        user.GivenName = givenName;
                        user.AppLastUpdateTimestamp = DateTime.UtcNow;
                        user.AppLastUpdateUserid = smSystemId;

                        dbContext.Users.Update(user);
                    }

                    return user;
                }
                catch
                {
                    return dbContext.Users.FirstOrDefault(x => x.SmUserId == smSystemId);
                }
            }

            return dbContext.Users.FirstOrDefault(x => x.SmUserId == smSystemId);
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
                    sysUser = new User
                    {
                        SmUserId = systemId,
                        Surname = "System",
                        GivenName = "HETS",
                        Initials = "SH",
                        Active = true,
                        AppCreateTimestamp = DateTime.UtcNow,
                        AppLastUpdateTimestamp = DateTime.UtcNow,
                        AppCreateUserid = systemId,
                        AppLastUpdateUserid = systemId
                    };

                    dbContext.Users.Add(sysUser);
                    dbContext.SaveChangesForImport();
                }
            }          
            catch
            {
                // do nothing
            }
        }

        public static DateTime? CleanDateTime(string dateTimeField)
        {
            if (dateTimeField != null && dateTimeField != "1900-01-01T00:00:00")
            {
                dateTimeField = dateTimeField.Trim();

                if (dateTimeField.Length >= 10)
                {
                    return DateTime.ParseExact(dateTimeField.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }                
            }

            return null;
        }        

        public static float? GetFloatValue(string floatField)
        {
            if (!string.IsNullOrEmpty(floatField))
            {
                floatField = floatField.Trim();

                if (float.TryParse(floatField, out float temp))
                {
                    return temp;
                }
            }
            
            return null;
        }

        public static int? GetIntValue(string intField)
        {
            if (!string.IsNullOrEmpty(intField))
            {
                intField = intField.Trim();

                if (int.TryParse(intField, out int temp))
                {
                    return temp;
                }
            }

            return null;
        }

        public static string CleanString(string textField)
        {
            if (textField == null) { return ""; }

            string temp = textField.Trim().ToLower();
            temp = temp.Replace(@"#x0d;", "");
            temp = temp.Replace(@"#x20;", "");
            temp = temp.Replace(@"amp;", "&");
            temp = temp.Replace("unknown", "");
            temp = temp.Replace(@"?", "");

            return temp;
        }

        public static string GetCapitalCase(string textField)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textField.ToLower());
        }

        public static string GetUppercaseFirst(string textField)
        {
            if (string.IsNullOrEmpty(textField))
            {
                return string.Empty;
            }

            char[] a = textField.ToCharArray();
            a[0] = char.ToUpper(a[0]);

            // check if we have any periods (".")            
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == '.' && i < a.Length - 1)
                {
                    a[i + 1] = char.ToUpper(a[0]);
                }
            }

            // return result
            return new string(a);
        }

        // assumes name is broken up by a specific seperator type (e.g. " ")
        public static string GetNamePart(string fullName, int part, string seperator = " ")
        {
            string[] nameParts = fullName.Split(seperator);

            string tempNamePart = "";

            if (nameParts.Length >= part)
            {
                tempNamePart = nameParts[part - 1].Trim();
                return GetCapitalCase(tempNamePart);
            }

            return tempNamePart;
        }

        public static void UnknownElement(object sender, XmlElementEventArgs e)
        {
            Console.WriteLine("Unexpected element: {0} as line {1}, column {2}", e.Element.Name, e.LineNumber, e.LinePosition);
        }

        public static void UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            Console.WriteLine("Unexpected attribute: {0} as line {1}, column {2}", e.Attr.Name, e.LineNumber, e.LinePosition);
        }

        // string scramble function from https://www.codeproject.com/Articles/820667/Sample-code-to-scramble-a-Word-using-Csharp-String

        /// <summary>
        /// Scramble a String
        /// </summary>
        /// <param name="input">A string</param>
        /// <returns>The string scrambled</returns>
        public static string ScrambleString (string input)
        {
            if (input != null)
            {
                char[] chars = new char[input.Length];
                Random rand = new Random(10000);
                int index = 0;

                while (input.Length > 0)
                { 
                    // get a random number between 0 and the length of the word. 
                    int next = rand.Next(0, input.Length - 1); // Take the character from the random position 
                                                               // and add to our char array. 
                    chars[index] = input[next];                // Remove the character from the word. 
                    input = input.Substring(0, next) + input.Substring(next + 1);
                    ++index;
                }

                return new String(chars);
            }

            return null;
        }

        public static void WriteImportRecordsToExcel (string destinationLocation, List<ImportMapRecord> records, string tableName)
        {
           
            using (var fs = new FileStream(Path.Combine(destinationLocation, tableName + ".xlsx"), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(tableName + " Map");

                // Create the header row.
                IRow row = excelSheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("Table Name");
                row.CreateCell(1).SetCellValue("Mapped Column");
                row.CreateCell(2).SetCellValue("Original Value");
                row.CreateCell(3).SetCellValue("New Value");

                // use the import class to get data.
                int currentRow = 1;

                // convert the list to an excel spreadsheet.
                foreach (ImportMapRecord record in records)
                {
                    IRow newRow = excelSheet.CreateRow(currentRow);
                    newRow.CreateCell(0).SetCellValue(record.TableName);
                    newRow.CreateCell(1).SetCellValue(record.MappedColumn);
                    newRow.CreateCell(2).SetCellValue(record.OriginalValue);
                    newRow.CreateCell(3).SetCellValue(record.NewValue);
                    currentRow++;
                }

                workbook.Write(fs);
            }
        }
    }
}
