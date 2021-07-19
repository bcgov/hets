using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Contact Controller
    /// </summary>
    [Route("api/contacts")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ContactController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ContactController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete contact
        /// </summary>
        /// <param name="id">id of Contact to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<ContactDto> ContactsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetContacts.Any(a => a.ContactId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetContact item = _context.HetContacts.First(a => a.ContactId == id);

            // check if this is a project - and if this is a "primary contact"
            if (item.ProjectId != null && item.ProjectId > 0)
            {
                int projectId = (int)item.ProjectId;

                HetProject project = _context.HetProjects
                    .FirstOrDefault(x => x.ProjectId == projectId);

                if (project != null && project.PrimaryContactId == id)
                {
                    project.PrimaryContactId = null;
                }
            }

            _context.HetContacts.Remove(item);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<ContactDto>(item)));
        }
    }
}
