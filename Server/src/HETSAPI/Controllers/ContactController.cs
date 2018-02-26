using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using HETSAPI.Models;
using HETSAPI.Services;
using HETSAPI.Authorization;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Contact Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ContactController : Controller
    {
        private readonly IContactService _service;

        /// <summary>
        /// Contract Controller Constructor
        /// </summary>
        public ContactController(IContactService service)
        {
            _service = service;
        }

        /// <summary>
        /// Create bulk contact records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Contact created</response>
        [HttpPost]
        [Route("/api/contacts/bulk")]
        [SwaggerOperation("ContactsBulkPost")]
        [RequiresPermission(Permission.Admin)]
        public virtual IActionResult ContactsBulkPost([FromBody]Contact[] items)
        {
            return _service.ContactsBulkPostAsync(items);
        }        
    }
}
