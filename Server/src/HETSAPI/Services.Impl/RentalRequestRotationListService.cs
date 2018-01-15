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
    /// Rental Request Rotation List Service
    /// </summary>
    public class RentalRequestRotationListService : IRentalRequestRotationListService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Rental Request Rotation List Service Constructor
        /// </summary>
        public RentalRequestRotationListService(DbAppContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord(RentalRequestRotationList item)
        {
            if (item != null)
            {                
                if (item.Equipment != null)
                {
                    item.Equipment = _context.Equipments.First(a => a.Id == item.Equipment.Id);
                }

                if (item.RentalAgreement != null)
                {
                    item.RentalAgreement = _context.RentalAgreements.First(a => a.Id == item.RentalAgreement.Id);
                }
            }
        }

        /// <summary>
        /// Create bulk rental request rotation lists
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Rental Request Rotation List created</response>
        public virtual IActionResult RentalrequestrotationlistsBulkPostAsync(RentalRequestRotationList[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (RentalRequestRotationList item in items)
            {
                AdjustRecord(item);

                bool exists = _context.RentalRequestRotationLists.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.RentalRequestRotationLists.Update(item);
                }
                else
                {
                    _context.RentalRequestRotationLists.Add(item);
                }
            }
            // save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all rental request rottion lists
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestrotationlistsGetAsync()
        {
            List<RentalRequestRotationList> result = _context.RentalRequestRotationLists
                .Include(x => x.RentalAgreement)                            
                .Include(x => x.Equipment)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Delete rental request rotation list
        /// </summary>
        /// <param name="id">id of Rental Request Rotation List to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Rental Request Rotation List not found</response>
        public virtual IActionResult RentalrequestrotationlistsIdDeletePostAsync(int id)
        {
            bool exists = _context.RentalRequestRotationLists.Any(a => a.Id == id);

            if (exists)
            {
                RentalRequestRotationList item = _context.RentalRequestRotationLists.First(a => a.Id == id);

                if (item != null)
                {
                    _context.RentalRequestRotationLists.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get rental request rotation list by id
        /// </summary>
        /// <param name="id">id of Rental Request Rotation List to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Rental Request Rotation List not found</response>
        public virtual IActionResult RentalrequestrotationlistsIdGetAsync(int id)
        {
            bool exists = _context.RentalRequestRotationLists.Any(a => a.Id == id);

            if (exists)
            {
                RentalRequestRotationList result = _context.RentalRequestRotationLists
                    .Include(x => x.RentalAgreement)                   
                    .Include(x => x.Equipment)
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update rental request rotation list
        /// </summary>
        /// <param name="id">id of Rental Request Rotation List to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Rental Request Rotation List not found</response>
        public virtual IActionResult RentalrequestrotationlistsIdPutAsync(int id, RentalRequestRotationList item)
        {
            AdjustRecord(item);

            bool exists = _context.RentalRequestRotationLists.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.RentalRequestRotationLists.Update(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create rental request rotation list
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Rental Request Rotation List created</response>
        public virtual IActionResult RentalrequestrotationlistsPostAsync(RentalRequestRotationList item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.RentalRequestRotationLists.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.RentalRequestRotationLists.Update(item);
                }
                else
                {
                    // record not found
                    _context.RentalRequestRotationLists.Add(item);
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
