using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HetsApi.Services
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
        
        /// <summary>	
        /// Delete rental agreement condition
        /// </summary>	
        /// <param name="id">id of RentalAgreementCondition to delete</param>	
        /// <response code="200">OK</response>	
        IActionResult RentalagreementconditionsIdDeletePostAsync(int id);

        /// <summary>	
        /// Update rental agreement condition
        /// </summary>	
        /// <param name="id">id of RentalAgreementCondition to fetch</param>	
        /// <param name="item"></param>	
        /// <response code="200">OK</response>	
        IActionResult RentalagreementconditionsIdPutAsync(int id, RentalAgreementCondition item);
    }
}
