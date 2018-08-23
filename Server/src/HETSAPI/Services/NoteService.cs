using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
{
    /// <summary>
    /// Note Service
    /// </summary>
    public interface INoteService
    {
        /// <summary>
        /// Create bulk note records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Note created</response>
        IActionResult NotesBulkPostAsync(Note[] items);

        /// <summary>
        /// Delete note
        /// </summary>
        /// <param name="id">id of Note to delete</param>
        /// <response code="200">OK</response>
        IActionResult NotesIdDeletePostAsync(int id);

        /// <summary>
        /// Update note
        /// </summary>
        /// <param name="id">id of Note to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        IActionResult NotesIdPutAsync(int id, Note item);
    }
}

