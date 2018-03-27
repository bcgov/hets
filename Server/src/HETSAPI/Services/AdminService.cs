using Microsoft.AspNetCore.Mvc;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Starts the import process
        /// </summary>
        /// <param name="path">Location of the extracted files to parse (relative to the folder where files are stored)</param>
        /// <param name="realTime">Execute in real time</param>
        /// <response code="200">OK</response>
        IActionResult AdminImportGetAsync(string path, bool realTime);

        // copies source files with obfuscation
        IActionResult AdminObfuscateGetAsync(string sourcePath, string destinationPath);

        /// <summary>
        /// Returns the obfuscation map stored as a spreadsheet
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        IActionResult GetSpreadsheet(string path, string filename);

        /// <summary>
        /// Return the data file (testing only)
        /// </summary>
        /// <param name="filename"></param>
        /// <response code="200">OK</response>        
        IActionResult GetData(string filename);
    }
}
