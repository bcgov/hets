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
    public class DistrictEquipmentTypeService : IDistrictEquipmentTypeService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public DistrictEquipmentTypeService(DbAppContext context)
        {
            _context = context;
        }


        private void AdjustRecord(DistrictEquipmentType item)
        {            
            int district_id = item.District.Id;
            var exists = _context.Districts.Any(a => a.Id == district_id);
            if (exists)
            {
                District district = _context.Districts.First(a => a.Id == district_id);
                item.District = district;
            }
            else
            {
                item.District = null;
            }
            if (item.EquipmentType != null)
            {
                int equipment_type_id = (int) item.EquipmentType.Id;
                var equipment_type_exists = _context.EquipmentTypes.Any(a => a.Id == equipment_type_id);
                if (equipment_type_exists)
                {
                    EquipmentType equipmentType = _context.EquipmentTypes.First(a => a.Id == district_id);
                    item.EquipmentType = equipmentType;
                }
                else
                {
                    item.EquipmentType = null;
                }

            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">DistrictEquipmentType created</response>
        public virtual IActionResult DistrictEquipmentTypesBulkPostAsync(DistrictEquipmentType[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (DistrictEquipmentType item in items)
            {
                AdjustRecord(item);
                
                // determine if this is an insert or an update            
                bool exists = _context.DistrictEquipmentTypes.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.Update(item);
                }
                else
                {
                    _context.Add(item);
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
        public virtual IActionResult DistrictEquipmentTypesGetAsync()
        {
            var result = _context.DistrictEquipmentTypes
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdDeletePostAsync(int id)
        {
            var exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.DistrictEquipmentTypes.First(a => a.Id == id);
                if (item != null)
                {
                    _context.DistrictEquipmentTypes.Remove(item);
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
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdGetAsync(int id)
        {
            var exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.DistrictEquipmentTypes.First(a => a.Id == id);
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
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">DistrictEquipmentType not found</response>
        public virtual IActionResult DistrictEquipmentTypesIdPutAsync(int id, DistrictEquipmentType item)
        {
            AdjustRecord(item);
            var exists = _context.DistrictEquipmentTypes.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.DistrictEquipmentTypes.Update(item);
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
        /// <response code="201">DistrictEquipmentType created</response>
        public virtual IActionResult DistrictEquipmentTypesPostAsync(DistrictEquipmentType item)
        {
            AdjustRecord(item);
            var exists = _context.DistrictEquipmentTypes.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.DistrictEquipmentTypes.Update(item);
            }
            else
            {
                // record not found
                _context.DistrictEquipmentTypes.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
        }
    }
}
