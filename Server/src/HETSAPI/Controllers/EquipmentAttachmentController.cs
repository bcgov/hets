using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
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
    }
}
