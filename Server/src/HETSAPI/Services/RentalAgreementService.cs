using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Rental Agreement Service
    /// </summary>
    public interface IRentalAgreementService
    {
        /// <summary>
        /// Create bulk rental agreement records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreement created</response>
        IActionResult RentalagreementsBulkPostAsync(RentalAgreement[] items);

        /// <summary>
        /// Get all rental agreements
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RentalagreementsGetAsync();

        /// <summary>
        /// Delete rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        IActionResult RentalagreementsIdDeletePostAsync(int id);

        /// <summary>
        /// Get rental agreement by id
        /// </summary>
        /// <param name="id">id of RentalAgreement to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        IActionResult RentalagreementsIdGetAsync(int id);

        /// <summary>
        /// Get pdf of rental agreement
        /// </summary>
        /// <remarks>Returns a PDF version of the specified rental agreement</remarks>
        /// <param name="id">id of RentalAgreement to obtain the PDF for</param>
        /// <response code="200">OK</response>
        IActionResult RentalagreementsIdPdfGetAsync(int id);

        /// <summary>
        /// Update a rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreement not found</response>
        IActionResult RentalagreementsIdPutAsync(int id, RentalAgreement item);

        /// <summary>
        /// Create a rental agreement
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreement created</response>
        IActionResult RentalagreementsPostAsync(RentalAgreement item);

        /// <summary>
        /// Release (terminate) a rental agreement
        /// </summary>
        /// /// <param name="id">Id of Rental Agreement to release</param>
        /// <response code="201">Rental Agreement released</response>
        IActionResult RentalagreementsIdReleasePostAsync(int id);

        /// <summary>
        /// Get time records for a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreement&#39;s Time Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Time Records for</param>
        /// <response code="200">OK</response>
        IActionResult RentalAgreementsIdTimeRecordsGetAsync(int id);

        /// <summary>
        /// Add a time record to a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Time Record</remarks>
        /// <param name="id">id of Rental Agreement to add a time record for</param>
        /// <param name="item">Adds to Rental Agreement Time Record</param>
        /// <response code="200">OK</response>
        IActionResult RentalAgreementsIdTimeRecordsPostAsync(int id, TimeRecord item);

        /// <summary>
        /// Update or create an array of time records associated with a rental agreement
        /// </summary>
        /// <remarks>Update a Rental Agreement&#39;s Time Records</remarks>
        /// <param name="id">id of Rental Agreement to update Time Records for</param>
        /// <param name="items">Array of Rental Agreement Time Records</param>
        /// <response code="200">OK</response>
        IActionResult RentalAgreementsIdTimeRecordsBulkPostAsync(int id, TimeRecord[] items);
    }
}
