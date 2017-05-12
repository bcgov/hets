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
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserFavouriteController : Controller
    {
        private readonly IUserFavouriteService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public UserFavouriteController(IUserFavouriteService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">UserFavourite created</response>
        [HttpPost]
        [Route("/api/userfavourites/bulk")]
        [SwaggerOperation("UserfavouritesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult UserfavouritesBulkPost([FromBody]UserFavourite[] items)
        {
            return this._service.UserfavouritesBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/userfavourites")]
        [SwaggerOperation("UserfavouritesGet")]
        [SwaggerResponse(200, type: typeof(List<UserFavourite>))]
        public virtual IActionResult UserfavouritesGet()
        {
            return this._service.UserfavouritesGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserFavourite to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        [HttpPost]
        [Route("/api/userfavourites/{id}/delete")]
        [SwaggerOperation("UserfavouritesIdDeletePost")]
        public virtual IActionResult UserfavouritesIdDeletePost([FromRoute]int id)
        {
            return this._service.UserfavouritesIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserFavourite to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        [HttpGet]
        [Route("/api/userfavourites/{id}")]
        [SwaggerOperation("UserfavouritesIdGet")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        public virtual IActionResult UserfavouritesIdGet([FromRoute]int id)
        {
            return this._service.UserfavouritesIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of UserFavourite to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">UserFavourite not found</response>
        [HttpPut]
        [Route("/api/userfavourites/{id}")]
        [SwaggerOperation("UserfavouritesIdPut")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        public virtual IActionResult UserfavouritesIdPut([FromRoute]int id, [FromBody]UserFavourite item)
        {
            return this._service.UserfavouritesIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">UserFavourite created</response>
        [HttpPost]
        [Route("/api/userfavourites")]
        [SwaggerOperation("UserfavouritesPost")]
        [SwaggerResponse(200, type: typeof(UserFavourite))]
        public virtual IActionResult UserfavouritesPost([FromBody]UserFavourite item)
        {
            return this._service.UserfavouritesPostAsync(item);
        }
    }
}
