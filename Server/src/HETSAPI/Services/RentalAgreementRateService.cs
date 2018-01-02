using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRentalAgreementRateService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreementRate created</response>
        IActionResult RentalagreementratesBulkPostAsync(RentalAgreementRate[] items);

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        IActionResult RentalagreementratesGetAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementRate not found</response>
        IActionResult RentalagreementratesIdDeletePostAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementRate not found</response>
        IActionResult RentalagreementratesIdGetAsync(int id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of RentalAgreementRate to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalAgreementRate not found</response>
        IActionResult RentalagreementratesIdPutAsync(int id, RentalAgreementRate item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">RentalAgreementRate created</response>
        IActionResult RentalagreementratesPostAsync(RentalAgreementRate item);
    }
}
