/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Services;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NoteController : Controller
    {
        private readonly INoteService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public NoteController(INoteService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Note created</response>
        [HttpPost]
        [Route("/api/notes/bulk")]
        [SwaggerOperation("NotesBulkPost")]
        public virtual IActionResult NotesBulkPost([FromBody]Note[] items)
        {
            return this._service.NotesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/notes")]
        [SwaggerOperation("NotesGet")]
        [SwaggerResponse(200, type: typeof(List<Note>))]
        public virtual IActionResult NotesGet()
        {
            return this._service.NotesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Note to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        [HttpPost]
        [Route("/api/notes/{id}/delete")]
        [SwaggerOperation("NotesIdDeletePost")]
        public virtual IActionResult NotesIdDeletePost([FromRoute]int id)
        {
            return this._service.NotesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Note to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        [HttpGet]
        [Route("/api/notes/{id}")]
        [SwaggerOperation("NotesIdGet")]
        [SwaggerResponse(200, type: typeof(Note))]
        public virtual IActionResult NotesIdGet([FromRoute]int id)
        {
            return this._service.NotesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Note to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        [HttpPut]
        [Route("/api/notes/{id}")]
        [SwaggerOperation("NotesIdPut")]
        [SwaggerResponse(200, type: typeof(Note))]
        public virtual IActionResult NotesIdPut([FromRoute]int id, [FromBody]Note item)
        {
            return this._service.NotesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Note created</response>
        [HttpPost]
        [Route("/api/notes")]
        [SwaggerOperation("NotesPost")]
        [SwaggerResponse(200, type: typeof(Note))]
        public virtual IActionResult NotesPost([FromBody]Note item)
        {
            return this._service.NotesPostAsync(item);
        }
    }
}
