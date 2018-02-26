using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Note Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class NoteController : Controller
    {
        private readonly INoteService _service;

        /// <summary>
        /// Note Controller Constructor
        /// </summary>
        public NoteController(INoteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk note records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Note created</response>
        [HttpPost]
        [Route("/api/notes/bulk")]
        [SwaggerOperation("NotesBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult NotesBulkPost([FromBody]Note[] items)
        {
            return _service.NotesBulkPostAsync(items);
        }        
    }
}
