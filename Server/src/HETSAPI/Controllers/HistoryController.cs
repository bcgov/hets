using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// History Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class HistoryController : Controller
    {
        private readonly IHistoryService _service;

        /// <summary>
        /// History Controller Constructor
        /// </summary>
        public HistoryController(IHistoryService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk history records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/histories/bulk")]
        [SwaggerOperation("HistoriesBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult HistoriesBulkPost([FromBody]History[] items)
        {
            return _service.HistoriesBulkPostAsync(items);
        }

        /// <summary>
        /// Get all history
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/histories")]
        [SwaggerOperation("HistoriesGet")]
        [SwaggerResponse(200, type: typeof(List<History>))]
        public virtual IActionResult HistoriesGet()
        {
            return _service.HistoriesGetAsync();
        }

        /// <summary>
        /// Delete history
        /// </summary>
        /// <param name="id">id of History to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        [HttpPost]
        [Route("/api/histories/{id}/delete")]
        [SwaggerOperation("HistoriesIdDeletePost")]
        public virtual IActionResult HistoriesIdDeletePost([FromRoute]int id)
        {
            return _service.HistoriesIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get history by id
        /// </summary>
        /// <param name="id">id of History to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        [HttpGet]
        [Route("/api/histories/{id}")]
        [SwaggerOperation("HistoriesIdGet")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult HistoriesIdGet([FromRoute]int id)
        {
            return _service.HistoriesIdGetAsync(id);
        }

        /// <summary>
        /// Update history
        /// </summary>
        /// <param name="id">id of History to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">History not found</response>
        [HttpPut]
        [Route("/api/histories/{id}")]
        [SwaggerOperation("HistoriesIdPut")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult HistoriesIdPut([FromRoute]int id, [FromBody]History item)
        {
            return _service.HistoriesIdPutAsync(id, item);
        }

        /// <summary>
        /// Create history
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        [HttpPost]
        [Route("/api/histories")]
        [SwaggerOperation("HistoriesPost")]
        [SwaggerResponse(200, type: typeof(History))]
        public virtual IActionResult HistoriesPost([FromBody]History item)
        {
            return _service.HistoriesPostAsync(item);
        }
    }
}
