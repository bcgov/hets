using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Lookup List Controller
    /// </summary>
    public class LookupListController : Controller
    {
        private readonly ILookupListService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public LookupListController(ILookupListService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LookupList created</response>
        [HttpPost]
        [Route("/api/lookuplists/bulk")]
        [SwaggerOperation("LookuplistsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult LookuplistsBulkPost([FromBody]LookupList[] items)
        {
            return this._service.LookuplistsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/lookuplists")]
        [SwaggerOperation("LookuplistsGet")]
        [SwaggerResponse(200, type: typeof(List<LookupList>))]
        public virtual IActionResult LookuplistsGet()
        {
            return this._service.LookuplistsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupList to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupList not found</response>
        [HttpPost]
        [Route("/api/lookuplists/{id}/delete")]
        [SwaggerOperation("LookuplistsIdDeletePost")]
        public virtual IActionResult LookuplistsIdDeletePost([FromRoute]int id)
        {
            return this._service.LookuplistsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupList to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupList not found</response>
        [HttpGet]
        [Route("/api/lookuplists/{id}")]
        [SwaggerOperation("LookuplistsIdGet")]
        [SwaggerResponse(200, type: typeof(LookupList))]
        public virtual IActionResult LookuplistsIdGet([FromRoute]int id)
        {
            return this._service.LookuplistsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of LookupList to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LookupList not found</response>
        [HttpPut]
        [Route("/api/lookuplists/{id}")]
        [SwaggerOperation("LookuplistsIdPut")]
        [SwaggerResponse(200, type: typeof(LookupList))]
        public virtual IActionResult LookuplistsIdPut([FromRoute]int id, [FromBody]LookupList item)
        {
            return this._service.LookuplistsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LookupList created</response>
        [HttpPost]
        [Route("/api/lookuplists")]
        [SwaggerOperation("LookuplistsPost")]
        [SwaggerResponse(200, type: typeof(LookupList))]
        public virtual IActionResult LookuplistsPost([FromBody]LookupList item)
        {
            return this._service.LookuplistsPostAsync(item);
        }
    }
}
