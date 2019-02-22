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
    /// Note Controller
    /// </summary>
    [Route("api/notes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class NoteController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public NoteController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }

        /// <summary>
        /// Delete note
        /// </summary>
        /// <param name="id">id of Note to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("NotesIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetNote))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult NotesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetNote.Any(a => a.NoteId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetNote note = _context.HetNote.First(a => a.NoteId == id);

            if (note != null)
            {
                _context.HetNote.Remove(note);

                // save the changes
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(note));
        }

        /// <summary>
        /// Update note
        /// </summary>
        /// <param name="id">id of Note to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("NotesIdPut")]
        [SwaggerResponse(200, type: typeof(HetNote))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult NotesIdPut([FromRoute]int id, [FromBody]HetNote item)
        {
            bool exists = _context.HetNote.Any(a => a.NoteId == id);

            // not found
            if (!exists || id != item.NoteId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get note
            HetNote note = _context.HetNote.First(a => a.NoteId == id);

            // update note
            note.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            note.Text = item.Text;

            // save the changes
            _context.SaveChanges();

            // return the updated note record
            note = _context.HetNote.First(a => a.NoteId == id);

            return new ObjectResult(new HetsResponse(note));
        }
    }
}
