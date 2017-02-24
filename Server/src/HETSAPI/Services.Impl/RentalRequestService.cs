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

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class RentalRequestService : IRentalRequestService
    {
        private readonly DbAppContext _context;        

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public RentalRequestService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(RentalRequest item)
        {
            if (item != null)
            {
                // Adjust the record to allow it to be updated / inserted
                if (item.LocalArea != null)
                {
                    int localarea_id = item.LocalArea.Id;
                    bool localarea_exists = _context.LocalAreas.Any(a => a.Id == localarea_id);
                    if (localarea_exists)
                    {
                        LocalArea localarea = _context.LocalAreas.First(a => a.Id == localarea_id);
                        item.LocalArea = localarea;
                    }
                    else
                    {
                        item.LocalArea = null;
                    }
                }

                if (item.Project != null)
                {
                    int project_id = item.Project.Id;
                    bool project_exists = _context.Projects.Any(a => a.Id == project_id);
                    if (project_exists)
                    {
                        Project project = _context.Projects.First(a => a.Id == project_id);
                        item.Project = project;
                    }
                    else
                    {
                        item.LocalArea = null;
                    }
                }

                if (item.EquipmentType != null)
                {
                    int equipment_type_id = item.EquipmentType.Id;
                    bool equipment_type_exists = _context.EquipmentTypes.Any(a => a.Id == equipment_type_id);
                    if (equipment_type_exists)
                    {
                        EquipmentType equipmentType = _context.EquipmentTypes.First(a => a.Id == equipment_type_id);
                        item.EquipmentType = equipmentType;
                    }
                    else
                    {
                        item.LocalArea = null;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult RentalrequestsBulkPostAsync(RentalRequest[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (RentalRequest item in items)
            {
                AdjustRecord(item);
                bool exists = _context.RentalRequests.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalRequests.Update(item);
                }
                else
                {
                    _context.RentalRequests.Add(item);
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
        public virtual IActionResult RentalrequestsGetAsync()
        {
            var result = _context.RentalRequests
                .Include(x => x.Attachments)
                .Include(x => x.EquipmentType)
                .Include(x => x.FirstOnRotationList)
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Notes)
                .Include(x => x.Project)
                .Include(x => x.RentalRequestRotationList)
                .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalrequestsIdDeletePostAsync(int id)
        {
            var exists = _context.RentalRequests.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.RentalRequests.First(a => a.Id == id);
                if (item != null)
                {
                    _context.RentalRequests.Remove(item);
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
        public virtual IActionResult RentalrequestsIdGetAsync(int id)
        {
            var exists = _context.RentalRequests.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.RentalRequests
                    .Include(x => x.Attachments)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.FirstOnRotationList)
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.Project)
                    .Include(x => x.RentalRequestRotationList)
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
        public virtual IActionResult RentalrequestsIdPutAsync(int id, RentalRequest item)
        {
            AdjustRecord(item);
            var exists = _context.RentalRequests.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.RentalRequests.Update(item);
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
        public virtual IActionResult RentalrequestsPostAsync(RentalRequest item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.RentalRequests.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.RentalRequests.Update(item);
                }
                else
                {
                    // record not found
                    _context.RentalRequests.Add(item);
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

        /// <summary>
        /// Searches Projects
        /// </summary>
        /// <remarks>Used for the project search page.</remarks>
        /// <param name="localareas">Local areas (array of id numbers)</param>
        /// <param name="project">name or partial name for a Project</param>
        /// <param name="hasRequests">if true then only include Projects with active Requests</param>
        /// <param name="hasHires">if true then only include Projects with active Rental Agreements</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestsSearchGetAsync(int?[] localareas, string project, string status, bool? hasHires)
        {
            var data = _context.RentalRequests
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.Project.PrimaryContact)
                    .Select(x => x);

            if (localareas != null)
            {
                foreach (int? localarea in localareas)
                {
                    if (localarea != null)
                    {
                        data = data.Where(x => x.LocalArea.Id == localarea);
                    }
                }
            }
            
            if (hasHires != null)
            {
                // hired is not currently implemented.                 
            }

            if (project != null)
            {
                data = data.Where(x => x.Project.Name.Contains (project));
            }

            var result = new List<RentalRequestSearchResultViewModel>();
            foreach (var item in data)
            {
                RentalRequestSearchResultViewModel newItem = item.ToViewModel();
                result.Add(item.ToViewModel());
            }

           // no calculated fields in a RentalRequest search yet.
                           
            return new ObjectResult(result);
        }
    }
}
