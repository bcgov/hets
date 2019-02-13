using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Hangfire;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsData.Model;
using HetsImport.Import;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Authentication Controller
    /// </summary>
    [Route("api/admin")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Object _thisLock = new Object();

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Start the import process
        /// </summary>
        /// <param name="path">Location of the extracted files to parse (relative to the folder where files are stored)</param>
        /// <param name="realTime">Execute in real time</param>
        [HttpGet]
        [Route("import")]
        [SwaggerOperation("AdminImportGet")]
        [RequiresPermission(HetPermission.Admin)]
        public virtual IActionResult AdminImportGet([FromQuery]string path, [FromQuery]string realTime)
        {
            // get realtime execution value
            bool boolRealTime = !string.IsNullOrEmpty(realTime) && realTime.ToLower() == "true";

            string result;

            lock (_thisLock)
            {
                try
                {
                    string uploadPath = _configuration["UploadPath"];

                    // serialize scoring rules from config into json string
                    IConfigurationSection scoringRules = _configuration.GetSection("SeniorityScoringRules");
                    string seniorityScoringRules = GetConfigJson(scoringRules);

                    // get connection string
                    string connectionString = GetConnectionString();

                    if (boolRealTime)
                    {
                        // not using Hangfire
                        BcBidImport.ImportJob(null, seniorityScoringRules, connectionString, uploadPath + path);
                        result = "Import complete";
                    }
                    else
                    {
                        // use Hangfire
                        result = "Created Job: ";
                        string jobId = BackgroundJob.Enqueue(() => BcBidImport.ImportJob(null, seniorityScoringRules, connectionString, uploadPath + path));
                        result += jobId;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    result = @"*** Import Error ***: " + e.Message;
                }
            }

            return new ObjectResult(result);
        }

        [HttpGet]
        [Route("obfuscate")]
        [SwaggerOperation("AdminObfuscateGet")]
        [RequiresPermission(HetPermission.Admin)]
        public virtual IActionResult AdminObfuscateGet([FromQuery]string sourcePath, [FromQuery]string destinationPath)
        {
            string result = "Created Obfuscation Job: ";

            lock (_thisLock)
            {
                // get upload path
                string uploadPath = _configuration["UploadPath"];

                // get connection string
                string connectionString = GetConnectionString();

                ImportUtility.CreateObfuscationDestination(uploadPath + destinationPath);

                // use Hangfire
                string jobId = BackgroundJob.Enqueue(() => BcBidImport.ObfuscationJob(null, connectionString, uploadPath + sourcePath, uploadPath + destinationPath));
                result += jobId;
            }

            return new ObjectResult(result);
        }

        /// <summary>
        /// Return the equipment map
        /// </summary>
        /// <param name="path">location of the extracted files to parse (relative to the folder where files are stored)</param>
        [HttpGet]
        [Route("equipMap")]
        [SwaggerOperation("AdminEquipMap")]
        [RequiresPermission(HetPermission.Admin)]
        public IActionResult AdminEquipMap(string path)
        {
            return GetSpreadsheet(path, "Equip.xlsx");
        }

        /// <summary>
        /// Return the owner map
        /// </summary>
        /// <param name="path">location of the extracted files to parse (relative to the folder where files are stored)</param>
        [HttpGet]
        [Route("ownerMap")]
        [SwaggerOperation("AdminOwnerMap")]
        [RequiresPermission(HetPermission.Admin)]
        public IActionResult AdminOwnerMap(string path)
        {
            return GetSpreadsheet(path, "Owner.xlsx");
        }

        /// <summary>
        /// Return the project map
        /// </summary>
        /// <param name="path">location of the extracted files to parse (relative to the folder where files are stored)</param>
        [HttpGet]
        [Route("projectMap")]
        [SwaggerOperation("AdminProjectMap")]
        [RequiresPermission(HetPermission.Admin)]
        public IActionResult AdminProjectMap(string path)
        {
            return GetSpreadsheet(path, "Project.xlsx");
        }

        /// <summary>
        /// Return the user map
        /// </summary>
        /// <param name="path">location of the extracted files to parse (relative to the folder where files are stored)</param>
        [HttpGet]
        [Route("userMap")]
        [SwaggerOperation("AdminUserMap")]
        [RequiresPermission(HetPermission.Admin)]
        public IActionResult AdminUserMap(string path)
        {
            return GetSpreadsheet(path, "UserHETS.xlsx");
        }

        #region Read Spreadsheet

        private IActionResult GetSpreadsheet(string path, string filename)
        {
            // create an excel spreadsheet that will show the data
            lock (_thisLock)
            {
                if (_configuration != null)
                {
                    string uploadPath = _configuration["UploadPath"];
                    string fullPath = Path.Combine(uploadPath + path, filename);

                    MemoryStream memory = new MemoryStream();

                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        stream.CopyToAsync(memory).Wait();
                    }

                    memory.Position = 0;

                    FileStreamResult fileStreamResult = new FileStreamResult(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = filename
                    };

                    return fileStreamResult;
                }
            }

            return null;
        }

        #endregion

        #region Get Database Connection String

        /// <summary>
        /// Retrieve database connection string
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string connectionString;

            lock (_thisLock)
            {
                string host = _configuration["DATABASE_SERVICE_NAME"];
                string username = _configuration["POSTGRESQL_USER"];
                string password = _configuration["POSTGRESQL_PASSWORD"];
                string database = _configuration["POSTGRESQL_DATABASE"];

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                    string.IsNullOrEmpty(database))
                {
                    // When things get cleaned up properly, this is the only call we'll have to make.
                    connectionString = _configuration.GetConnectionString("HETS");
                }
                else
                {
                    // Environment variables override all other settings; same behaviour as the configuration provider when things get cleaned up.
                    connectionString = $"Host={host};Username={username};Password={password};Database={database};";
                }
            }

            return connectionString;
        }

        #endregion

        #region Get Scoring Rules

        private string GetConfigJson(IConfigurationSection scoringRules)
        {
            string jsonString = RecurseConfigJson(scoringRules);

            if (jsonString.EndsWith("},"))
            {
                jsonString = jsonString.Substring(0, jsonString.Length - 1);
            }

            return jsonString;
        }

        private string RecurseConfigJson(IConfigurationSection scoringRules)
        {
            StringBuilder temp = new StringBuilder();

            temp.Append("{");

            // check for children
            foreach (IConfigurationSection section in scoringRules.GetChildren())
            {
                temp.Append(@"""" + section.Key + @"""" + ":");

                if (section.Value == null)
                {
                    temp.Append(RecurseConfigJson(section));
                }
                else
                {
                    temp.Append(@"""" + section.Value + @"""" + ",");
                }
            }

            string jsonString = temp.ToString();

            if (jsonString.EndsWith(","))
            {
                jsonString = jsonString.Substring(0, jsonString.Length - 1);
            }

            jsonString = jsonString + "},";
            return jsonString;
        }

        #endregion
    }
}
