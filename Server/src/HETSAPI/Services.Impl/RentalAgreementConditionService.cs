using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Rental Agreement Condition Service
    /// </summary>
    public class RentalAgreementConditionService : IRentalAgreementConditionService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Rental Agreement Condition Service Constructor
        /// </summary>
        public RentalAgreementConditionService(DbAppContext context)
        {
            _context = context;
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
    }
}
