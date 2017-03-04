using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using HETSAPI.Services;

namespace SchoolBusAPI.Services.Impl
{
    public class RentalAgreementConditionService : IRentalAgreementConditionService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public RentalAgreementConditionService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(RentalAgreementCondition item)
        {
            if (item != null)
            {

                if (item.RentalAgreement != null)
                {
                    int rentalAgreement_id = item.RentalAgreement.Id;
                    bool rentalAgreement_exists = _context.RentalAgreements.Any(a => a.Id == rentalAgreement_id);
                    if (rentalAgreement_exists)
                    {
                        RentalAgreement rentalAgreement = _context.RentalAgreements.First(a => a.Id == rentalAgreement_id);
                        item.RentalAgreement = rentalAgreement;
                    }
                    else
                    {
                        item.RentalAgreement = null;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
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
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalagreementconditionsGetAsync()
        {
            var result = _context.RentalAgreementConditions
                .Include(x => x.RentalAgreement)                
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementconditionsIdDeletePostAsync(int id)
        {
            var exists = _context.RentalAgreementConditions.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.RentalAgreementConditions.First(a => a.Id == id);
                if (item != null)
                {
                    _context.RentalAgreementConditions.Remove(item);
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
        public virtual IActionResult RentalagreementconditionsIdGetAsync(int id)
        {
            var exists = _context.RentalAgreementConditions.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.RentalAgreementConditions
                    .Include(x => x.RentalAgreement)
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
        public virtual IActionResult RentalagreementconditionsIdPutAsync(int id, RentalAgreementCondition item)
        {
            AdjustRecord(item);
            var exists = _context.RentalAgreementConditions.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.RentalAgreementConditions.Update(item);
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
        public virtual IActionResult RentalagreementconditionsPostAsync(RentalAgreementCondition item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.RentalAgreementConditions.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalAgreementConditions.Update(item);
                }
                else
                {
                    // record not found
                    _context.RentalAgreementConditions.Add(item);
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
