using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Contact Controller
    /// </summary>
    [Route("api/contacts")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ContactController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public ContactController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">id of Contact to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("ContactsIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetContact))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult ContactsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetContact.Any(a => a.ContactId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetContact item = _context.HetContact.First(a => a.ContactId == id);

            // check if this is a project - and if this is a "primary contact"
            if (item.ProjectId != null && item.ProjectId > 0)
            {
                int projectId = (int)item.ProjectId;

                HetProject project = _context.HetProject
                    .FirstOrDefault(x => x.ProjectId == projectId);

                if (project != null && project.PrimaryContactId == id)
                {
                    project.PrimaryContactId = null;
                    _context.HetProject.Update(project);
                }
            }

            _context.HetContact.Remove(item);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
