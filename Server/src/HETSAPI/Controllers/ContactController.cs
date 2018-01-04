using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
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
        [RequiresPermission(Permission.ADMIN)]
        public virtual IActionResult ContactsBulkPost([FromBody]Contact[] items)
        {
            return _service.ContactsBulkPostAsync(items);
        }

        /// <summary>
        /// Get all contacts
        /// </summary>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/api/contacts")]
        [SwaggerOperation("ContactsGet")]
        [SwaggerResponse(200, type: typeof(List<Contact>))]
        public virtual IActionResult ContactsGet()
        {
            return _service.ContactsGetAsync();
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">id of Contact to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        [HttpPost]
        [Route("/api/contacts/{id}/delete")]
        [SwaggerOperation("ContactsIdDeletePost")]
        public virtual IActionResult ContactsIdDeletePost([FromRoute]int id)
        {
            return _service.ContactsIdDeletePostAsync(id);
        }

        /// <summary>
        /// Get contact
        /// </summary>
        /// <param name="id">id of Contact to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        [HttpGet]
        [Route("/api/contacts/{id}")]
        [SwaggerOperation("ContactsIdGet")]
        [SwaggerResponse(200, type: typeof(Contact))]
        public virtual IActionResult ContactsIdGet([FromRoute]int id)
        {
            return _service.ContactsIdGetAsync(id);
        }

        /// <summary>
        /// Update contact
        /// </summary>
        /// <param name="id">id of Contact to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Contact not found</response>
        [HttpPut]
        [Route("/api/contacts/{id}")]
        [SwaggerOperation("ContactsIdPut")]
        [SwaggerResponse(200, type: typeof(Contact))]
        public virtual IActionResult ContactsIdPut([FromRoute]int id, [FromBody]Contact item)
        {
            return _service.ContactsIdPutAsync(id, item);
        }

        /// <summary>
        /// Create contact
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Contact created</response>
        [HttpPost]
        [Route("/api/contacts")]
        [SwaggerOperation("ContactsPost")]
        [SwaggerResponse(200, type: typeof(Contact))]
        public virtual IActionResult ContactsPost([FromBody]Contact item)
        {
            return _service.ContactsPostAsync(item);
        }
    }
}
