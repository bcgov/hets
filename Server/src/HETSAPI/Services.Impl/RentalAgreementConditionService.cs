using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Rental Agreement Condition Service
    /// </summary>
    public class RentalAgreementConditionService : IRentalAgreementConditionService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Rental Agreement Condition Service Constructor
        /// </summary>
        public RentalAgreementConditionService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord(RentalAgreementCondition item)
        {
            if (item != null && item.RentalAgreement != null)
                item.RentalAgreement = _context.RentalAgreements.FirstOrDefault(a => a.Id == item.RentalAgreement.Id);
        }

        /// <summary>
        /// Create bulk rental agreement conditions
        /// </summary>
        /// <param name="items"></param>
        /// <response code="200">Rental agreement conditions created</response>
        public virtual IActionResult RentalagreementconditionsBulkPostAsync(RentalAgreementCondition[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (RentalAgreementCondition item in items)
            {
                AdjustRecord(item);

                bool exists = _context.RentalAgreementConditions.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalAgreementConditions.Update(item);
                }
                else
                {
                    _context.RentalAgreementConditions.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }        

        /// <summary>	
        /// Delete rental agreement conditions	
        /// </summary>	
        /// <param name="id">id of Project to delete</param>	
        /// <response code="200">OK</response>	
        /// <response code="404">Project not found</response>	
        public virtual IActionResult RentalagreementconditionsIdDeletePostAsync(int id)
        {	
            bool exists = _context.RentalAgreementConditions.Any(a => a.Id == id);	
            
            if (exists)	
            {	
                RentalAgreementCondition item = _context.RentalAgreementConditions.First(a => a.Id == id);	
                
                    if (item != null)	
                    {	
                        _context.RentalAgreementConditions.Remove(item);	
                    
                            // save the changes	
                                _context.SaveChanges();	
                    }	
                
                    return new ObjectResult(new HetsResponse(item));	
            }	
            
            // record not found	
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>	
        /// Update rental agreement condition	
        /// </summary>	
        /// <param name="id">id of Project to update</param>	
        /// <param name="item"></param>	
        /// <response code="200">OK</response>	
        /// <response code="404">Project not found</response>	
        public virtual IActionResult RentalagreementconditionsIdPutAsync(int id, RentalAgreementCondition item)
        {	
            AdjustRecord(item);	
            
            bool exists = _context.RentalAgreementConditions.Any(a => a.Id == id);	
            
            if (exists && id == item.Id)	
            {	
                _context.RentalAgreementConditions.Update(item);	
                
                    // save the changes	
                    _context.SaveChanges();	
                
                    return new ObjectResult(new HetsResponse(item));	
            }	
            
            // record not found	
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));	
        }
    }
}
