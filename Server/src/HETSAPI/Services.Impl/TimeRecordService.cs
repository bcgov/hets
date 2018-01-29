using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    public class TimeRecordService : ITimeRecordService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public TimeRecordService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord(TimeRecord item)
        {
            if (item != null)
            {
                if (item.RentalAgreement != null)
                {
                    item.RentalAgreement = _context.RentalAgreements.FirstOrDefault(a => a.Id == item.RentalAgreement.Id);
                }

                if (item.RentalAgreementRate != null)
                {
                    item.RentalAgreementRate = _context.RentalAgreementRates.FirstOrDefault(a => a.Id == item.RentalAgreement.Id);
                }
            }
        }

        /// <summary>
        /// Create bulk time records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Time Record created</response>
        public virtual IActionResult TimerecordsBulkPostAsync(TimeRecord[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (TimeRecord item in items)
            {
                AdjustRecord(item);

                bool exists = _context.TimeRecords.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.TimeRecords.Update(item);
                }
                else
                {
                    _context.TimeRecords.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Get all time records
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult TimerecordsGetAsync()
        {
            List<RentalAgreementCondition> result = _context.RentalAgreementConditions
                .Include(x => x.RentalAgreement)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete time records
        /// </summary>
        /// <param name="id">id of Time Record to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Time Record not found</response>
        public virtual IActionResult TimerecordsIdDeletePostAsync(int id)
        {
            bool exists = _context.TimeRecords.Any(a => a.Id == id);

            if (exists)
            {
                TimeRecord item = _context.TimeRecords.First(a => a.Id == id);

                if (item != null)
                {
                    _context.TimeRecords.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get time record by id
        /// </summary>
        /// <param name="id">id of Time Record to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Time Record not found</response>
        public virtual IActionResult TimerecordsIdGetAsync(int id)
        {
            bool exists = _context.TimeRecords.Any(a => a.Id == id);

            if (exists)
            {
                TimeRecord result = _context.TimeRecords                    
                    .Include(x => x.RentalAgreement)
                    .Include(x => x.RentalAgreementRate)                    
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update time record
        /// </summary>
        /// <param name="id">id of Time Record to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Time Record not found</response>
        public virtual IActionResult TimerecordsIdPutAsync(int id, TimeRecord item)
        {
            AdjustRecord(item);

            bool exists = _context.TimeRecords.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.TimeRecords.Update(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create time record
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Time Record created</response>
        public virtual IActionResult TimerecordsPostAsync(TimeRecord item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.TimeRecords.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.TimeRecords.Update(item);
                }
                else
                {
                    // record not found
                    _context.TimeRecords.Add(item);
                }

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            return new StatusCodeResult(400);
        }
    }
}
