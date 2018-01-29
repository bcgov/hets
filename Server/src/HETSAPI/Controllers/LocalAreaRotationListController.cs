using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Local Area Rotation List Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class LocalAreaRotationListController : Controller
    {
        private readonly ILocalAreaRotationListService _service;

        /// <summary>
        /// Local Area Rotation List Controller Constructor
        /// </summary>
        public LocalAreaRotationListController(ILocalAreaRotationListService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk local area rotation lists
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">LocalAreaRotationList created</response>
        [HttpPost]
        [Route("/api/localarearotationlists/bulk")]
        [SwaggerOperation("LocalarearotationlistsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult LocalarearotationlistsBulkPost([FromBody]LocalAreaRotationList[] items)
        {
            return _service.LocalarearotationlistsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all local area rotation lists
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/localarearotationlists")]
        [SwaggerOperation("LocalarearotationlistsGet")]
        [SwaggerResponse(200, type: typeof(List<LocalAreaRotationList>))]
        public virtual IActionResult LocalarearotationlistsGet()
        {
            return _service.LocalarearotationlistsGetAsync();
        }

        /// <summary>
        /// Delete local area rotation list
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        [HttpPost]
        [Route("/api/localarearotationlists/{id}/delete")]
        [SwaggerOperation("LocalarearotationlistsIdDeletePost")]
        public virtual IActionResult LocalarearotationlistsIdDeletePost([FromRoute]int id)
        {
            return _service.LocalarearotationlistsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get local area rotation list by id
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        [HttpGet]
        [Route("/api/localarearotationlists/{id}")]
        [SwaggerOperation("LocalarearotationlistsIdGet")]
        [SwaggerResponse(200, type: typeof(LocalAreaRotationList))]
        public virtual IActionResult LocalarearotationlistsIdGet([FromRoute]int id)
        {
            return _service.LocalarearotationlistsIdGetAsync(id);
        }

        /// <summary>
        /// Update local area rotation list
        /// </summary>
        /// <param name="id">id of LocalAreaRotationList to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">LocalAreaRotationList not found</response>
        [HttpPut]
        [Route("/api/localarearotationlists/{id}")]
        [SwaggerOperation("LocalarearotationlistsIdPut")]
        [SwaggerResponse(200, type: typeof(LocalAreaRotationList))]
        public virtual IActionResult LocalarearotationlistsIdPut([FromRoute]int id, [FromBody]LocalAreaRotationList item)
        {
            return _service.LocalarearotationlistsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create local area rotation list
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">LocalAreaRotationList created</response>
        [HttpPost]
        [Route("/api/localarearotationlists")]
        [SwaggerOperation("LocalarearotationlistsPost")]
        [SwaggerResponse(200, type: typeof(LocalAreaRotationList))]
        public virtual IActionResult LocalarearotationlistsPost([FromBody]LocalAreaRotationList item)
        {
            return _service.LocalarearotationlistsPostAsync(item);
        }
    }
}
