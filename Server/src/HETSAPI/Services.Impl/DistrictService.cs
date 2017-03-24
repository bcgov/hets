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

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class DistrictService : IDistrictService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public DistrictService(DbAppContext context)
        {
            _context = context;
        }

        public void AdjustRecord (District item)
        {
            if (item != null)
            {
                if (item.Region != null)
                {
                    item.Region = _context.Regions.FirstOrDefault(x => x.Id == item.Region.Id);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a number of districts.</remarks>
        /// <param name="items"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsBulkPostAsync(District[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (District item in items)
            {
                AdjustRecord(item);

                bool exists = _context.Districts.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.Districts.Update(item);
                }
                else
                {
                    _context.Districts.Add(item);
                }               
            }
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Returns a list of available districts</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsGetAsync()
        {
            // eager loading of regions
            var result = _context.Districts
                    .Include(x => x.Region)
                    .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Deletes a district</remarks>
        /// <param name="id">id of District to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">District not found</response>
        public virtual IActionResult DistrictsIdDeletePostAsync(int id)
        {
            var exists = _context.Districts.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.Districts.First(a => a.Id == id);
                if (item != null)
                {
                    _context.Districts.Remove(item);
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
        /// <remarks>Returns a specific district</remarks>
        /// <param name="id">id of Districts to fetch</param>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsIdGetAsync(int id)
        {
            var exists = _context.Districts.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.Districts
                    .Where(a => a.Id == id)
                    .Include(a => a.Region)
                    .FirstOrDefault();
                    
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
        /// <remarks>Updates a district</remarks>
        /// <param name="id">id of District to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">District not found</response>
        public virtual IActionResult DistrictsIdPutAsync(int id, District body)
        {
            var exists = _context.Districts.Any(a => a.Id == id);
            if (exists && id == body.Id)
            {
                AdjustRecord(body);
                _context.Districts.Update(body);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(body);
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
        /// <remarks>Returns the Service Areas for a specific region</remarks>
        /// <param name="id">id of District for which to fetch the ServiceAreas</param>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsIdServiceareasGetAsync(int id)
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Adds a district</remarks>
        /// <param name="body"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult DistrictsPostAsync(District body)
        {            
            AdjustRecord(body);

            var exists = _context.Districts.Any(a => a.Id == body.Id);
            if (exists )
            {
                _context.Districts.Update(body);
                // Save the changes             
            }
            else
            {
                // record not found
                _context.Districts.Add(body);                
            }

            _context.SaveChanges();
            return new ObjectResult(body);                        
        }
    }
}
