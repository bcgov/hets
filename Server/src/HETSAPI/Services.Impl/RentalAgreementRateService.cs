using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Rental Agreeent Rate Service
    /// </summary>
    public class RentalAgreementRateService : IRentalAgreementRateService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Rental Agreeent Rate Service Constructor
        /// </summary>
        public RentalAgreementRateService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

        /// <summary>
        /// Get all rental agreement rates
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalagreementratesGetAsync()
        {
            List<RentalAgreementRate> result = _context.RentalAgreementRates
                .Include(x => x.RentalAgreement)
                .Include(x => x.TimeRecords)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete rental agreement rate
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementratesIdDeletePostAsync(int id)
        {
            bool exists = _context.RentalAgreementRates.Any(a => a.Id == id);

            if (exists)
            {
                RentalAgreementRate item = _context.RentalAgreementRates.First(a => a.Id == id);

                if (item != null)
                {
                    _context.RentalAgreementRates.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get renatl agreenment rate by id
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementratesIdGetAsync(int id)
        {
            bool exists = _context.RentalAgreementRates.Any(a => a.Id == id);

            if (exists)
            {
                RentalAgreementRate result = _context.RentalAgreementRates                    
                    .Include(x => x.RentalAgreement)
                    .Include(x => x.TimeRecords)
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update rental agreement rate
        /// </summary>
        /// <param name="id">id of Project to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementratesIdPutAsync(int id, RentalAgreementRate item)
        {
            AdjustRecord(item);

            bool exists = _context.RentalAgreementRates.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.RentalAgreementRates.Update(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create rental agreement rate
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult RentalagreementratesPostAsync(RentalAgreementRate item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.RentalAgreementRates.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.RentalAgreementRates.Update(item);
                }
                else
                {
                    // record not found
                    _context.RentalAgreementRates.Add(item);
                }

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // no record to insert
            return new ObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));
        }
    }
}
