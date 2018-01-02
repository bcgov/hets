using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class RentalRequestRotationListService : IRentalRequestRotationListService
    {
        private readonly DbAppContext _context;        

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public RentalRequestRotationListService(DbAppContext context)
        {
            _context = context;
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
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
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
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestrotationlistsGetAsync()
        {
            var result = _context.RentalRequestRotationLists
                .Include(x => x.RentalAgreement)                            
                .Include(x => x.Equipment)
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalrequestrotationlistsIdDeletePostAsync(int id)
        {
            var exists = _context.RentalRequestRotationLists.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.RentalRequestRotationLists.First(a => a.Id == id);
                if (item != null)
                {
                    _context.RentalRequestRotationLists.Remove(item);
                    // Save the changes
                    _context.SaveChanges();
                }
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalrequestrotationlistsIdGetAsync(int id)
        {
            var exists = _context.RentalRequestRotationLists.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.RentalRequestRotationLists
                    .Include(x => x.RentalAgreement)                   
                    .Include(x => x.Equipment)

                    .First(a => a.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalrequestrotationlistsIdPutAsync(int id, RentalRequestRotationList item)
        {
            AdjustRecord(item);
            var exists = _context.RentalRequestRotationLists.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.RentalRequestRotationLists.Update(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult RentalrequestrotationlistsPostAsync(RentalRequestRotationList item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.RentalRequestRotationLists.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalRequestRotationLists.Update(item);
                }
                else
                {
                    // record not found
                    _context.RentalRequestRotationLists.Add(item);
                }
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                return new StatusCodeResult(400);
            }
        }
        
    }
}
