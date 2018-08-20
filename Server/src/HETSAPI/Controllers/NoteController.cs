using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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
        /// <response code="200">Note created</response>
        [HttpPost]
        [Route("/api/notes/bulk")]
        [SwaggerOperation("NotesBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult NotesBulkPost([FromBody]Note[] items)
        {
            return _service.NotesBulkPostAsync(items);
        }     
        
        /// <summary>	
        /// Delete note	
        /// </summary>	
        /// <param name="id">id of Note to delete</param>	
        /// <response code="200">OK</response>	
        [HttpPost]	
        [Route("/api/notes/{id}/delete")]	
        [SwaggerOperation("NotesIdDeletePost")]	
        public virtual IActionResult NotesIdDeletePost([FromRoute]int id)
        {	
            return _service.NotesIdDeletePostAsync(id);
        }

        /// <summary>	
        /// Update note	
        /// </summary>	
        /// <param name="id">id of Note to update</param>	
        /// <param name="item"></param>	
        /// <response code="200">OK</response>	
        [HttpPut]	
        [Route("/api/notes/{id}")]	
        [SwaggerOperation("NotesIdPut")]	
        [SwaggerResponse(200, type: typeof(Note))]	
        public virtual IActionResult NotesIdPut([FromRoute]int id, [FromBody]Note item)
        {	
            return _service.NotesIdPutAsync(id, item);	
        }
    }
}
