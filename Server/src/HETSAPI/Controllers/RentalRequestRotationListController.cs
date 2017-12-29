using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RentalRequestRotationListController : Controller
    {
        private readonly IRentalRequestRotationListService _service;

        /// <summary>
        /// Create a controller and set the service
        /// </summary>
        public RentalRequestRotationListController(IRentalRequestRotationListService service)
        {
            _service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalRequestRotationList created</response>
        [HttpPost]
        [Route("/api/rentalrequestrotationlists/bulk")]
        [SwaggerOperation("RentalrequestrotationlistsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RentalrequestrotationlistsBulkPost([FromBody]RentalRequestRotationList[] items)
        {
            return this._service.RentalrequestrotationlistsBulkPostAsync(items);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequestrotationlists")]
        [SwaggerOperation("RentalrequestrotationlistsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequestRotationList>))]
        public virtual IActionResult RentalrequestrotationlistsGet()
        {
            return this._service.RentalrequestrotationlistsGetAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestRotationList to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        [HttpPost]
        [Route("/api/rentalrequestrotationlists/{id}/delete")]
        [SwaggerOperation("RentalrequestrotationlistsIdDeletePost")]
        public virtual IActionResult RentalrequestrotationlistsIdDeletePost([FromRoute]int id)
        {
            return this._service.RentalrequestrotationlistsIdDeletePostAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestRotationList to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        [HttpGet]
        [Route("/api/rentalrequestrotationlists/{id}")]
        [SwaggerOperation("RentalrequestrotationlistsIdGet")]
        [SwaggerResponse(200, type: typeof(RentalRequestRotationList))]
        public virtual IActionResult RentalrequestrotationlistsIdGet([FromRoute]int id)
        {
            return this._service.RentalrequestrotationlistsIdGetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestRotationList to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        [HttpPut]
        [Route("/api/rentalrequestrotationlists/{id}")]
        [SwaggerOperation("RentalrequestrotationlistsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalRequestRotationList))]
        public virtual IActionResult RentalrequestrotationlistsIdPut([FromRoute]int id, [FromBody]RentalRequestRotationList item)
        {
            return this._service.RentalrequestrotationlistsIdPutAsync(id, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalRequestRotationList created</response>
        [HttpPost]
        [Route("/api/rentalrequestrotationlists")]
        [SwaggerOperation("RentalrequestrotationlistsPost")]
        [SwaggerResponse(200, type: typeof(RentalRequestRotationList))]
        public virtual IActionResult RentalrequestrotationlistsPost([FromBody]RentalRequestRotationList item)
        {
            return this._service.RentalrequestrotationlistsPostAsync(item);
        }
    }
}
