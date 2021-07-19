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
    /// Note Controller
    /// </summary>
    [Route("api/notes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class NoteController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public NoteController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Delete note
        /// </summary>
        /// <param name="id">id of Note to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<NoteDto> NotesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetNotes.Any(a => a.NoteId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetNote note = _context.HetNotes.First(a => a.NoteId == id);

            if (note != null)
            {
                _context.HetNotes.Remove(note);

                // save the changes
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<NoteDto>(note)));
        }

        /// <summary>
        /// Update note
        /// </summary>
        /// <param name="id">id of Note to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<NoteDto> NotesIdPut([FromRoute]int id, [FromBody]NoteDto item)
        {
            bool exists = _context.HetNotes.Any(a => a.NoteId == id);

            // not found
            if (!exists || id != item.NoteId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get note
            HetNote note = _context.HetNotes.First(a => a.NoteId == id);

            // update note
            note.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            note.Text = item.Text;

            // save the changes
            _context.SaveChanges();

            // return the updated note record
            note = _context.HetNotes.First(a => a.NoteId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<NoteDto>(note)));
        }
    }
}
