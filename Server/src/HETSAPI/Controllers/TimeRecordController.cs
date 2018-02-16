using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Time Record Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class TimeRecordController : Controller
    {
        private readonly ITimeRecordService _service;

        /// <summary>
        /// Time Record Controller Constructor
        /// </summary>
        public TimeRecordController(ITimeRecordService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk time records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">TimeRecord created</response>
        [HttpPost]
        [Route("/api/timerecords/bulk")]
        [SwaggerOperation("TimerecordsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult TimerecordsBulkPost([FromBody]TimeRecord[] items)
        {
            return _service.TimerecordsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all time records
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/timerecords")]
        [SwaggerOperation("TimerecordsGet")]
        [SwaggerResponse(200, type: typeof(List<TimeRecord>))]
        public virtual IActionResult TimerecordsGet()
        {
            return _service.TimerecordsGetAsync();
        }

        /// <summary>
        /// Delete time record
        /// </summary>
        /// <param name="id">id of TimeRecord to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        [HttpPost]
        [Route("/api/timerecords/{id}/delete")]
        [SwaggerOperation("TimerecordsIdDeletePost")]
        public virtual IActionResult TimerecordsIdDeletePost([FromRoute]int id)
        {
            return _service.TimerecordsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get time record by id
        /// </summary>
        /// <param name="id">id of TimeRecord to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        [HttpGet]
        [Route("/api/timerecords/{id}")]
        [SwaggerOperation("TimerecordsIdGet")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult TimerecordsIdGet([FromRoute]int id)
        {
            return _service.TimerecordsIdGetAsync(id);
        }

        /// <summary>
        /// Update time record
        /// </summary>
        /// <param name="id">id of TimeRecord to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">TimeRecord not found</response>
        [HttpPut]
        [Route("/api/timerecords/{id}")]
        [SwaggerOperation("TimerecordsIdPut")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult TimerecordsIdPut([FromRoute]int id, [FromBody]TimeRecord item)
        {
            return _service.TimerecordsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create time record
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">TimeRecord created</response>
        [HttpPost]
        [Route("/api/timerecords")]
        [SwaggerOperation("TimerecordsPost")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        public virtual IActionResult TimerecordsPost([FromBody]TimeRecord item)
        {
            return _service.TimerecordsPostAsync(item);
        }
    }
}
