using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Condition Type Service
    /// </summary>
    public interface IUserDistrictService
    {
        /// <summary>
        /// Create bulk user district records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">City created</response>
        IActionResult UserDistrictsBulkPostAsync(UserDistrict[] items);
        
        /// <summary>
        /// Get all user districts
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult UserDistrictsGetAsync();

        /// <summary>
        /// Delete user district
        /// </summary>
        /// <param name="id">id of User District to delete</param>
        /// <response code="200">OK</response>
        IActionResult UserDistrictsIdDeletePostAsync(int id);

        /// <summary>
        /// Get a specific user district record
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult UserDistrictsIdGetAsync(int id);

        /// <summary>
        /// Update or create a user district record
        /// </summary>
        /// <param name="id">id of user district for updating</param>
        /// <param name="item">Condition Type.</param>
        /// <response code="200">OK</response>
        IActionResult UserDistrictsIdPostAsync(int id, UserDistrict item);

        /// <summary>
        /// Switch user district record
        /// </summary>
        /// <param name="id">id of user district to switch to</param>
        /// <response code="200">OK</response>
        IActionResult UserDistrictsIdSwitchPostAsync(int id);

        /// <summary>
        /// Logoff user - switch to default district
        /// </summary>
        /// <param name="id">id of user district to logoff</param>
        /// <response code="200">OK</response>
        IActionResult UserDistrictsIdLogoffPostAsync(int id);
    }
}
