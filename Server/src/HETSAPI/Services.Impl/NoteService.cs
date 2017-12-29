using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class NoteService : INoteService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public NoteService(DbAppContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Note created</response>
        public virtual IActionResult NotesBulkPostAsync(Note[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (Note item in items)
            {

                // determine if this is an insert or an update            
                bool exists = _context.Notes.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.Update(item);
                }
                else
                {
                    _context.Add(item);
                }
            }
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult NotesGetAsync()
        {
            var result = _context.Notes.ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Note to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        public virtual IActionResult NotesIdDeletePostAsync(int id)
        {
            var exists = _context.Notes.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.Notes.First(a => a.Id == id);
                if (item != null)
                {
                    _context.Notes.Remove(item);
                    // Save the changes
                    _context.SaveChanges();
                }
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Note to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        public virtual IActionResult NotesIdGetAsync(int id)
        {
            var exists = _context.Notes.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.Notes.First(a => a.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Note to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        public virtual IActionResult NotesIdPutAsync(int id, Note item)
        {
            var exists = _context.Notes.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.Notes.Update(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Note created</response>
        public virtual IActionResult NotesPostAsync(Note item)
        {
            var exists = _context.Notes.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.Notes.Update(item);
            }
            else
            {
                // record not found
                _context.Notes.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
