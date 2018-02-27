using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        /// <param name="path">location of the extracted files to parse.  Relative to the folder where files are stored</param>
        /// <param name="districts">comma seperated list of district IDs to process.</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found in system</response>
        IActionResult AdminImportGetAsync(string path, string districts);

        // copies source files with obfuscation
        IActionResult AdminObfuscateGetAsync(string sourcePath, string destinationPath);

        /// <summary>
        /// Returns the obfuscation map stored as a spreadsheet
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task<IActionResult> GetSpreadsheet(string path, string filename);        
    }
}
