using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Time Record Service
    /// </summary>
    public interface ITimeRecordService
    {
        /// <summary>
        /// Create bulk time records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">TimeRecord created</response>
        IActionResult TimerecordsBulkPostAsync(TimeRecord[] items);

        /// <summary>
        /// Delete a time record
        /// </summary>
        /// <param name="id">id of TimeRecord to delete</param>
        /// <response code="200">OK</response>
        IActionResult TimerecordsIdDeletePostAsync(int id);        
    }
}
