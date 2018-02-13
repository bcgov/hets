using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Administration Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AdminController : Controller
    {
        private readonly IAdminService _service;

        /// <summary>
        /// Administration Controller Constructor
        /// </summary>
        public AdminController(IAdminService service)
        {
            _service = service;
        }

        /// <summary>
        /// Start the import process
        /// </summary>
        /// <param name="path">location of the extracted files to parse.  Relative to the folder where files are stored</param>
        /// <param name="districts">comma seperated list of district IDs to process.</param>
        /// <response code="200">OK</response>
        /// <response code="404">Attachment not found in system</response>
        ///         
        [HttpGet]
        [Route("/api/admin/import")]
        [SwaggerOperation("AdminImportGet")]
        public virtual IActionResult AdminImportGet([FromQuery]string path, [FromQuery]string districts)
        {
            return _service.AdminImportGetAsync(path, districts);
        }

        [HttpGet]
        [Route("/api/admin/obfuscate")]
        [SwaggerOperation("AdminObfuscateGet")]
        public virtual IActionResult AdminObfuscateGet([FromQuery]string sourcePath, [FromQuery]string destinationPath)
        {
            return _service.AdminObfuscateGetAsync(sourcePath, destinationPath);
        }

        /// <summary>
        /// Return the equipmap
        /// </summary>
        /// <param name="path">location of the extracted files to parse.  Relative to the folder where files are stored</param>
        /// <response code="200">OK</response>        
        [HttpGet]
        [Route("/api/admin/equipmap")]
        [SwaggerOperation("AdminEquipMap")]
        public async Task<IActionResult> AdminEquipMap(string path)
        {
            return await _service.GetSpreadsheet(path, "Equip.xlsx");
        }

        /// <summary>
        /// Return the ownermap
        /// </summary>
        /// <param name="path">location of the extracted files to parse.  Relative to the folder where files are stored</param>
        /// <response code="200">OK</response>        
        [HttpGet]
        [Route("/api/admin/ownermap")]
        [SwaggerOperation("AdminOwnerMap")]
        public async Task<IActionResult> AdminOwnerMap(string path)
        {
            return await _service.GetSpreadsheet(path, "Owner.xlsx");
        }

        /// <summary>
        /// Return the projectmap
        /// </summary>
        /// <param name="path">location of the extracted files to parse.  Relative to the folder where files are stored</param>
        /// <response code="200">OK</response>        
        [HttpGet]
        [Route("/api/admin/projectmap")]
        [SwaggerOperation("AdminProjectMap")]
        public async Task<IActionResult> AdminProjectMap(string path)
        {
            return await _service.GetSpreadsheet(path, "Project.xlsx");
        }


        /// <summary>
        /// Return the usermap
        /// </summary>
        /// <param name="path">location of the extracted files to parse.  Relative to the folder where files are stored</param>
        /// <response code="200">OK</response>        
        [HttpGet]
        [Route("/api/admin/usermap")]
        [SwaggerOperation("AdminUserMap")]
        public async Task<IActionResult> AdminUserMap(string path)
        {
            return await _service.GetSpreadsheet(path, "UserHETS.xlsx");
        }

    }
}
