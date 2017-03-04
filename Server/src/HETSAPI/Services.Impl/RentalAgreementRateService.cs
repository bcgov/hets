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
    public class RentalAgreementRateService : IRentalAgreementRateService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
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
                    int rentalAgreement_id = item.RentalAgreement.Id;
                    bool rentalAgreement_exists = _context.Equipments.Any(a => a.Id == rentalAgreement_id);
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

                if (item.TimeRecords != null)
                {
                    for (int i = 0; i < item.TimeRecords.Count; i++)
                    {
                        TimeRecord timeRecord = item.TimeRecords[i];
                        if (timeRecord != null)
                        {
                            int timeRecord_id = item.Id;
                            bool timeRecord_exists = _context.TimeRecords.Any(a => a.Id == timeRecord_id);
                            if (timeRecord_exists)
                            {
                                TimeRecord newItem = _context.TimeRecords.First(a => a.Id == timeRecord_id);
                                item.TimeRecords[i] = newItem;
                            }
                            else
                            {
                                item.TimeRecords[i] = null;
                            }
                        }
                    }
                }                
            }
        }

        /// <summary>
        /// 
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
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalagreementratesGetAsync()
        {
            var result = _context.RentalAgreementRates
                .Include(x => x.RentalAgreement)
                .Include(x => x.TimeRecords)
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalagreementratesIdDeletePostAsync(int id)
        {
            var exists = _context.RentalAgreementRates.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.RentalAgreementRates.First(a => a.Id == id);
                if (item != null)
                {
                    _context.RentalAgreementRates.Remove(item);
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
        public virtual IActionResult RentalagreementratesIdGetAsync(int id)
        {
            var exists = _context.RentalAgreementRates.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.RentalAgreementRates                    
                    .Include(x => x.RentalAgreement)
                    .Include(x => x.TimeRecords)
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
        public virtual IActionResult RentalagreementratesIdPutAsync(int id, RentalAgreementRate item)
        {
            AdjustRecord(item);
            var exists = _context.RentalAgreementRates.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.RentalAgreementRates.Update(item);
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
        public virtual IActionResult RentalagreementratesPostAsync(RentalAgreementRate item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.RentalAgreementRates.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalAgreementRates.Update(item);
                }
                else
                {
                    // record not found
                    _context.RentalAgreementRates.Add(item);
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
