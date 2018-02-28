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
    }
}
