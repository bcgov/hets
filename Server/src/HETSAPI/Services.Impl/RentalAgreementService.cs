/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */


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
    public class RentalAgreementService : IRentalAgreementService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public RentalAgreementService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(RentalAgreement item)
        {
            if (item != null)
            {
                if (item.Equipment != null)
                {
                    item.Equipment = _context.Equipments.FirstOrDefault(a => a.Id == item.Equipment.Id);
                }

                if (item.Project != null)
                {
                    item.Project = _context.Projects.FirstOrDefault(a => a.Id == item.Project.Id);
                }
                

                if (item.RentalAgreementConditions != null)
                {
                    for (int i = 0; i < item.RentalAgreementConditions.Count; i++)
                    {                        
                        if (item.RentalAgreementConditions[i] != null)
                        {
                            item.RentalAgreementConditions[i] = _context.RentalAgreementConditions.FirstOrDefault(a => a.Id == item.RentalAgreementConditions[i].Id);
                        }
                    }
                }

                if (item.RentalAgreementRates != null)
                {
                    for (int i = 0; i < item.RentalAgreementRates.Count; i++)
                    {
                        if (item.RentalAgreementRates[i] != null)
                        {
                            item.RentalAgreementRates[i] = _context.RentalAgreementRates.FirstOrDefault(a => a.Id == item.RentalAgreementRates[i].Id);
                        }
                    }
                }

                if (item.TimeRecords != null)
                {
                    for (int i = 0; i < item.TimeRecords.Count; i++)
                    {
                        if (item.TimeRecords[i] != null)
                        {
                            item.TimeRecords[i] = _context.TimeRecords.First(a => a.Id == item.TimeRecords[i].Id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult RentalagreementsBulkPostAsync(RentalAgreement[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (RentalAgreement item in items)
            {
                AdjustRecord(item);
                bool exists = _context.RentalAgreements.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalAgreements.Update(item);
                }
                else
                {
                    _context.RentalAgreements.Add(item);
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
        public virtual IActionResult RentalagreementsGetAsync()
        {
            var result = _context.RentalAgreements                                
                .Include(x => x.Equipment).ThenInclude(y => y.Owner)
                .Include(x => x.Equipment).ThenInclude(y => y.EquipmentAttachments)
                .Include(x => x.Equipment).ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project).ThenInclude (p => p.District.Region)
                .Include(x => x.RentalAgreementConditions)
                .Include(x => x.RentalAgreementRates)
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
        public virtual IActionResult RentalagreementsIdDeletePostAsync(int id)
        {
            var exists = _context.RentalAgreements.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.RentalAgreements.First(a => a.Id == id);
                if (item != null)
                {
                    _context.RentalAgreements.Remove(item);
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
        public virtual IActionResult RentalagreementsIdGetAsync(int id)
        {
            var exists = _context.RentalAgreements.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.RentalAgreements
                    .Include(x => x.Equipment).ThenInclude(y => y.Owner)
                    .Include(x => x.Equipment).ThenInclude(y => y.EquipmentAttachments)
                    .Include(x => x.Equipment).ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.Project).ThenInclude(p => p.District.Region)
                    .Include(x => x.RentalAgreementConditions)
                    .Include(x => x.RentalAgreementRates)
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
        public virtual IActionResult RentalagreementsIdPutAsync(int id, RentalAgreement item)
        {
            AdjustRecord(item);
            var exists = _context.RentalAgreements.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.RentalAgreements.Update(item);
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
        public virtual IActionResult RentalagreementsPostAsync(RentalAgreement item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.RentalAgreements.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalAgreements.Update(item);
                }
                else
                {
                    // record not found
                    _context.RentalAgreements.Add(item);
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
