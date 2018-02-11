using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Rental Request Attachment Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalRequestAttachmentController : Controller
    {
        private readonly IRentalRequestAttachmentService _service;

        /// <summary>
        /// Rental Request Attachment Controller Constructor
        /// </summary>
        public RentalRequestAttachmentController(IRentalRequestAttachmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk rental request attachment records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalRequestAttachment created</response>
        [HttpPost]
        [Route("/api/rentalrequestattachments/bulk")]
        [SwaggerOperation("RentalrequestattachmentsBulkPost")]
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult RentalrequestattachmentsBulkPost([FromBody]RentalRequestAttachment[] items)
        {
            return _service.RentalrequestattachmentsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all rental request attachments
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/rentalrequestattachments")]
        [SwaggerOperation("RentalrequestattachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequestAttachment>))]
        public virtual IActionResult RentalrequestattachmentsGet()
        {
            return _service.RentalrequestattachmentsGetAsync();
        }

        /// <summary>
        /// Delete rental request attachment
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        [HttpPost]
        [Route("/api/rentalrequestattachments/{id}/delete")]
        [SwaggerOperation("RentalrequestattachmentsIdDeletePost")]
        public virtual IActionResult RentalrequestattachmentsIdDeletePost([FromRoute]int id)
        {
            return _service.RentalrequestattachmentsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get rental request attachment by id
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        [HttpGet]
        [Route("/api/rentalrequestattachments/{id}")]
        [SwaggerOperation("RentalrequestattachmentsIdGet")]
        [SwaggerResponse(200, type: typeof(RentalRequestAttachment))]
        public virtual IActionResult RentalrequestattachmentsIdGet([FromRoute]int id)
        {
            return _service.RentalrequestattachmentsIdGetAsync(id);
        }

        /// <summary>
        /// Update rental request attachment
        /// </summary>
        /// <param name="id">id of RentalRequestAttachment to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestAttachment not found</response>
        [HttpPut]
        [Route("/api/rentalrequestattachments/{id}")]
        [SwaggerOperation("RentalrequestattachmentsIdPut")]
        [SwaggerResponse(200, type: typeof(RentalRequestAttachment))]
        public virtual IActionResult RentalrequestattachmentsIdPut([FromRoute]int id, [FromBody]RentalRequestAttachment item)
        {
            return _service.RentalrequestattachmentsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create rental request attachment
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalRequestAttachment created</response>
        [HttpPost]
        [Route("/api/rentalrequestattachments")]
        [SwaggerOperation("RentalrequestattachmentsPost")]
        [SwaggerResponse(200, type: typeof(RentalRequestAttachment))]
        public virtual IActionResult RentalrequestattachmentsPost([FromBody]RentalRequestAttachment item)
        {
            return _service.RentalrequestattachmentsPostAsync(item);
        }
    }
}
