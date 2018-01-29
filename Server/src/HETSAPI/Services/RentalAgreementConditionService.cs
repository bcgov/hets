using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRentalAgreementConditionService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreementCondition created</response>
        IActionResult RentalagreementconditionsBulkPostAsync(RentalAgreementCondition[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RentalagreementconditionsGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementCondition not found</response>
        IActionResult RentalagreementconditionsIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementCondition not found</response>
        IActionResult RentalagreementconditionsIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementCondition to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementCondition not found</response>
        IActionResult RentalagreementconditionsIdPutAsync(int id, RentalAgreementCondition item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreementCondition created</response>
        IActionResult RentalagreementconditionsPostAsync(RentalAgreementCondition item);
    }
}
