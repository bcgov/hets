using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
        /// <response code="200">Time Record created</response>
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
        /// Delete a time record
        /// </summary>
        /// <param name="id">id of Time Record to delete</param>
        /// <response code="200">OK</response>
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
    }
}
