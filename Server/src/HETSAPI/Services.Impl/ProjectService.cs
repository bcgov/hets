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
    public class ProjectService : IProjectService
    {
        private readonly DbAppContext _context;
        

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public ProjectService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord(Project item)
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
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult ProjectsBulkPostAsync(Project[] items)
        {
            var result = "";
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult ProjectsGetAsync()
        {
            var result = _context.Projects.ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult ProjectsIdDeletePostAsync(int id)
        {
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.Projects.First(a => a.Id == id);
                if (item != null)
                {
                    _context.Projects.Remove(item);
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
        public virtual IActionResult ProjectsIdGetAsync(int id)
        {
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.Projects.First(a => a.Id == id);
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
        public virtual IActionResult ProjectsIdPutAsync(int id, Project item)
        {
            var exists = _context.Projects.Any(a => a.Id == id);
            if (exists && id == item.Id)
            {
                _context.Projects.Update(item);
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
        public virtual IActionResult ProjectsPostAsync(Project item)
        {
            var exists = _context.Projects.Any(a => a.Id == item.Id);
            if (exists)
            {
                _context.Projects.Update(item);                
            }
            else
            {
                // record not found
                _context.Projects.Add(item);
            }
            // Save the changes
            _context.SaveChanges();
            return new ObjectResult(item);
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
        public virtual IActionResult ProjectsSearchGetAsync(int?[] localareas, string project, bool? hasRequests, bool? hasHires)
        {
            var data = _context.Projects
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.PrimaryContact)
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

            if (hasRequests != null)
            {
                
            }

            if (hasHires != null)
            {
                // hired is not currently implemented.                 
            }

            if (project != null)
            {
                data = data.Where(x => x.Name.Contains (project));
            }

            var result = new List<ProjectSearchResultViewModel>();
            foreach (var item in data)
            {
                ProjectSearchResultViewModel newItem = item.ToViewModel();
                // calculated fields.
                newItem.Requests = _context.RentalRequests
                    .Include(x => x.Project)
                    .Where(x => x.Project.Id == item.Id)
                    .Count();

                // TODO filter on status once RentalAgreements has a status field.
                newItem.Hires = _context.RentalAgreements
                    .Include(x => x.Project)
                    .Where(x => x.Project.Id == item.Id)
                    .Count();
                
                result.Add(newItem);               
            }
            return new ObjectResult(result);
        }
    }
}
