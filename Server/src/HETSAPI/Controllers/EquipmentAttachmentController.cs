using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Equipment Attachment Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentAttachmentController : Controller
    {
        private readonly IEquipmentAttachmentService _service;

        /// <summary>
        /// Equipment Attachment Controller Constructor
        /// </summary>
        public EquipmentAttachmentController(IEquipmentAttachmentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk equipment attachment records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">EquipmentAttachment created</response>
        [HttpPost]
        [Route("/api/equipmentAttachments/bulk")]
        [SwaggerOperation("EquipmentAttachmentsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult EquipmentAttachmentsBulkPost([FromBody]EquipmentAttachment[] items)
        {
            return _service.EquipmentAttachmentsBulkPostAsync(items);
        }        

        /// <summary>	
        /// Delete equipment attachment	
        /// </summary>	
        /// <param name="id">id of EquipmentAttachment to delete</param>	
        /// <response code="200">OK</response>	
        [HttpPost]	
        [Route("/api/equipmentAttachments/{id}/delete")]	
        [SwaggerOperation("EquipmentAttachmentsIdDeletePost")]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult EquipmentAttachmentsIdDeletePost([FromRoute]int id)
        {	
            return _service.EquipmentAttachmentsIdDeletePostAsync(id);	
        }	
	        
        /// <summary>	
        /// Update equipment attachment	
        /// </summary>	
        /// <param name="id">id of EquipmentAttachment to update</param>	
        /// <param name="item"></param>	
        /// <response code="200">OK</response>	
        [HttpPut]	
        [Route("/api/equipmentAttachments/{id}")]	
        [SwaggerOperation("EquipmentAttachmentsIdPut")]	
        [SwaggerResponse(200, type: typeof(EquipmentAttachment))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult EquipmentAttachmentsIdPut([FromRoute]int id, [FromBody]EquipmentAttachment item)
        {	
            return _service.EquipmentAttachmentsIdPutAsync(id, item);	
        }	
	
        /// <summary>	
        /// Create equipment attachment	
        /// </summary>	
        /// <param name="item"></param>	
        /// <response code="201">EquipmentAttachment created</response>	
        [HttpPost]	
        [Route("/api/equipmentAttachments")]	
        [SwaggerOperation("EquipmentAttachmentsPost")]	
        [SwaggerResponse(200, type: typeof(EquipmentAttachment))]
        [RequiresPermission(Permission.Login)]
        public virtual IActionResult EquipmentAttachmentsPost([FromBody]EquipmentAttachment item)
        {	
            return _service.EquipmentAttachmentsPostAsync(item);	
        }
    }
}
