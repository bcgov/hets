using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Rental Agreement Rate Service
    /// </summary>
    public interface IRentalAgreementRateService
    {
        /// <summary>
        /// Create bulk rental agreement rate records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreementRate created</response>
        IActionResult RentalagreementratesBulkPostAsync(RentalAgreementRate[] items);     
        
        /// <summary>	
        /// Delete rental agreement rate
        /// </summary>	
        /// <param name="id">id of RentalAgreementRate to delete</param>	
        /// <response code="200">OK</response>	
        IActionResult RentalagreementratesIdDeletePostAsync(int id);

        /// <summary>	
        /// Update rental agreement rate	
        /// </summary>	
        /// <param name="id">id of RentalAgreementRate to update</param>	
        /// <param name="item"></param>	
        /// <response code="200">OK</response>	
        IActionResult RentalagreementratesIdPutAsync(int id, RentalAgreementRate item);
    }
}
