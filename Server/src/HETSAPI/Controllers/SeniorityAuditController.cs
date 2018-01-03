using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Seniority Audit Controller
    /// </summary>
    public class SeniorityAuditController : Controller
    {
        private readonly ISeniorityAuditService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public SeniorityAuditController(ISeniorityAuditService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">SeniorityAudit created</response>
        [HttpPost]
        [Route("/api/seniorityaudits/bulk")]
        [SwaggerOperation("SeniorityauditsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult SeniorityauditsBulkPost([FromBody]SeniorityAudit[] items)
        {
            return this._service.SeniorityauditsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/seniorityaudits")]
        [SwaggerOperation("SeniorityauditsGet")]
        [SwaggerResponse(200, type: typeof(List<SeniorityAudit>))]
        public virtual IActionResult SeniorityauditsGet()
        {
            return this._service.SeniorityauditsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        [HttpPost]
        [Route("/api/seniorityaudits/{id}/delete")]
        [SwaggerOperation("SeniorityauditsIdDeletePost")]
        public virtual IActionResult SeniorityauditsIdDeletePost([FromRoute]int id)
        {
            return this._service.SeniorityauditsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        [HttpGet]
        [Route("/api/seniorityaudits/{id}")]
        [SwaggerOperation("SeniorityauditsIdGet")]
        [SwaggerResponse(200, type: typeof(SeniorityAudit))]
        public virtual IActionResult SeniorityauditsIdGet([FromRoute]int id)
        {
            return this._service.SeniorityauditsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of SeniorityAudit to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">SeniorityAudit not found</response>
        [HttpPut]
        [Route("/api/seniorityaudits/{id}")]
        [SwaggerOperation("SeniorityauditsIdPut")]
        [SwaggerResponse(200, type: typeof(SeniorityAudit))]
        public virtual IActionResult SeniorityauditsIdPut([FromRoute]int id, [FromBody]SeniorityAudit item)
        {
            return this._service.SeniorityauditsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">SeniorityAudit created</response>
        [HttpPost]
        [Route("/api/seniorityaudits")]
        [SwaggerOperation("SeniorityauditsPost")]
        [SwaggerResponse(200, type: typeof(SeniorityAudit))]
        public virtual IActionResult SeniorityauditsPost([FromBody]SeniorityAudit item)
        {
            return this._service.SeniorityauditsPostAsync(item);
        }
    }
}
