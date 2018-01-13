using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Note Service
    /// </summary>
    public class NoteService : INoteService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Note Service Constructor
        /// </summary>
        public NoteService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Create bulk note records
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

            // save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Get all notes
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult NotesGetAsync()
        {
            List<Note> result = _context.Notes.ToList();
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete note
        /// </summary>
        /// <param name="id">id of Note to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        public virtual IActionResult NotesIdDeletePostAsync(int id)
        {
            bool exists = _context.Notes.Any(a => a.Id == id);

            if (exists)
            {
                Note item = _context.Notes.First(a => a.Id == id);

                if (item != null)
                {
                    _context.Notes.Remove(item);
                    
                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get note by id
        /// </summary>
        /// <param name="id">id of Note to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        public virtual IActionResult NotesIdGetAsync(int id)
        {
            bool exists = _context.Notes.Any(a => a.Id == id);

            if (exists)
            {
                Note result = _context.Notes.First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update note
        /// </summary>
        /// <param name="id">id of Note to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Note not found</response>
        public virtual IActionResult NotesIdPutAsync(int id, Note item)
        {
            bool exists = _context.Notes.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.Notes.Update(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create note
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Note created</response>
        public virtual IActionResult NotesPostAsync(Note item)
        {
            bool exists = _context.Notes.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.Notes.Update(item);
            }
            else
            {
                // record not found
                _context.Notes.Add(item);
            }

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }
    }
}
