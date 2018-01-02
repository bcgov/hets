using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRentalRequestRotationListService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalRequestRotationList created</response>
        IActionResult RentalrequestrotationlistsBulkPostAsync(RentalRequestRotationList[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RentalrequestrotationlistsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestRotationList to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        IActionResult RentalrequestrotationlistsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestRotationList to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        IActionResult RentalrequestrotationlistsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalRequestRotationList to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        IActionResult RentalrequestrotationlistsIdPutAsync(int id, RentalRequestRotationList item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalRequestRotationList created</response>
        IActionResult RentalrequestrotationlistsPostAsync(RentalRequestRotationList item);
    }
}
