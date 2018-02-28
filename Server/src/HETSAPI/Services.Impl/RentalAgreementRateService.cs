using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Rental Agreeent Rate Service
    /// </summary>
    public class RentalAgreementRateService : IRentalAgreementRateService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Rental Agreeent Rate Service Constructor
        /// </summary>
        public RentalAgreementRateService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(RentalAgreementRate item)
        {
            if (item != null)
            {
                if (item.RentalAgreement != null)
                {
                    item.RentalAgreement = _context.RentalAgreements.FirstOrDefault(a => a.Id == item.RentalAgreement.Id);
                }

                if (item.TimeRecords != null)
                {
                    for (int i = 0; i < item.TimeRecords.Count; i++)
                    {                        
                        if (item.TimeRecords[i] != null)
                        {
                            item.TimeRecords[i] = _context.TimeRecords.FirstOrDefault(a => a.Id == item.TimeRecords[i].Id);
                        }
                    }
                }                
            }
        }

        /// <summary>
        /// Create bulk rental afreement rate records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">RentalAgreementRate created</response>
        public virtual IActionResult RentalagreementratesBulkPostAsync(RentalAgreementRate[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (RentalAgreementRate item in items)
            {
                AdjustRecord(item);

                bool exists = _context.RentalAgreementRates.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.RentalAgreementRates.Update(item);
                }
                else
                {
                    _context.RentalAgreementRates.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }        
    }
}
