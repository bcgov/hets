using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// Rental Agreement Condition Service
    /// </summary>
    public interface IRentalAgreementConditionService
    {
        /// <summary>
        /// Create bulk rental agreement condition records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">RentalAgreementCondition created</response>
        IActionResult RentalagreementconditionsBulkPostAsync(RentalAgreementCondition[] items);       
    }
}
