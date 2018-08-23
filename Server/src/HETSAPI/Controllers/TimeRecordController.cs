using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HetsApi.Controllers
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
        /// <response code="200">TimeRecord created</response>
        [HttpPost]
        [Route("/api/timerecords/bulk")]
        [SwaggerOperation("TimerecordsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult TimerecordsBulkPost([FromBody]TimeRecord[] items)
        {
            return _service.TimerecordsBulkPostAsync(items);
        }        

        /// <summary>	
        /// Delete a time record	
        /// </summary>	
        /// <param name="id">id of TimeRecord to delete</param>	
        /// <response code="200">OK</response>	
        [HttpPost]	
        [Route("/api/timerecords/{id}/delete")]	
        [SwaggerOperation("TimerecordsIdDeletePost")]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult TimerecordsIdDeletePost([FromRoute]int id)
        {	
            return _service.TimerecordsIdDeletePostAsync(id);	
        }
    }
}
