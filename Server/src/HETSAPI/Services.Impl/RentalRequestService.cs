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

                if (item.DistrictEquipmentType != null)
                {
                    int equipment_type_id = item.DistrictEquipmentType.Id;
                    bool equipment_type_exists = _context.EquipmentTypes.Any(a => a.Id == equipment_type_id);
                    if (equipment_type_exists)
                    {
                        DistrictEquipmentType districtEquipmentType = _context.DistrictEquipmentTypes.First(a => a.Id == equipment_type_id);
                        item.DistrictEquipmentType = districtEquipmentType;
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
                .Include(x => x.DistrictEquipmentType)
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
        /// <remarks>Returns History for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch History for</param>
        /// <response code="200">OK</response>

        public virtual IActionResult RentalrequestsIdHistoryGetAsync(int id, int? offset, int? limit)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);
            if (exists)
            {
                RentalRequest rentalRequest = _context.RentalRequests
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                List<History> data = rentalRequest.History.OrderByDescending(y => y.LastUpdateTimestamp).ToList();

                if (offset == null)
                {
                    offset = 0;
                }
                if (limit == null)
                {
                    limit = data.Count() - offset;
                }
                List<HistoryViewModel> result = new List<HistoryViewModel>();

                for (int i = (int)offset; i < data.Count() && i < offset + limit; i++)
                {
                    result.Add(data[i].ToViewModel(id));
                }

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
        /// <remarks>Add a History record to the Rental Request</remarks>
        /// <param name="id">id of RentalRequest to add History for</param>
        /// <param name="item"></param>
        /// <response code="201">History created</response>
        public virtual IActionResult RentalrequestsIdHistoryPostAsync(int id, History item)
        {
            HistoryViewModel result = new HistoryViewModel();

            bool exists = _context.RentalRequests.Any(a => a.Id == id);
            if (exists)
            {
                RentalRequest rentalRequest = _context.RentalRequests
                    .Include(x => x.History)
                    .First(a => a.Id == id);
                if (rentalRequest.History == null)
                {
                    rentalRequest.History = new List<History>();
                }
                // force add
                item.Id = 0;
                rentalRequest.History.Add(item);
                _context.RentalRequests.Update(rentalRequest);
                _context.SaveChanges();
            }

            result.HistoryText = item.HistoryText;
            result.Id = item.Id;
            result.LastUpdateTimestamp = item.LastUpdateTimestamp;
            result.LastUpdateUserid = item.LastUpdateUserid;
            result.AffectedEntityId = id;

            return new ObjectResult(result);
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
                    .Include(x => x.DistrictEquipmentType)
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


        private void BuildRentalRequestRotationList (RentalRequest item)
        {
            int currentSortOrder = 0;

            item.RentalRequestRotationList = new List<RentalRequestRotationList>();

            // first get the localAreaRotationList.askNextBlock1 for the given local area.
            LocalAreaRotationList localAreaRotationList = _context.LocalAreaRotationLists
                    .Include(x => x.LocalArea)
                    .Include(x => x.AskNextBlock1)
                    .Include(x => x.AskNextBlock2)
                    .Include(x => x.AskNextBlockOpen)
                    .FirstOrDefault(x => x.LocalArea.Id == item.LocalArea.Id);
            
            int numberOfBlocks = item.DistrictEquipmentType.EquipmentType.NumberOfBlocks; 
            
            for (int currentBlock = 1; currentBlock < numberOfBlocks; currentBlock++ )
            {
                // start by getting the current set of equipment for the given equipment type.

                List<Equipment> block1equipment = _context.Equipments
                    .Where(x => x.DistrictEquipmentType == item.DistrictEquipmentType && x.BlockNumber == 1)
                    .OrderByDescending(x => x.Seniority)
                    .ToList();

                int listSize = block1equipment.Count;

                // find the starting position.
                int currentPosition = 0;
                if (localAreaRotationList != null && localAreaRotationList.AskNextBlock1 != null)
                {
                    for (int i = 0; i < listSize; i++)
                    {
                        if (block1equipment[i] != null && block1equipment[i].Id == localAreaRotationList.AskNextBlock1.Id)
                        {
                            currentPosition = i;
                        }
                    }
                }

                // next pass sets the rotation list sort order.
                for (int i = 0; i < listSize; i++)
                {
                    RentalRequestRotationList rentalRequestRotationList = new RentalRequestRotationList();
                    rentalRequestRotationList.Equipment = block1equipment[currentPosition];
                    rentalRequestRotationList.CreateTimestamp = DateTime.UtcNow;
                    rentalRequestRotationList.RentalRequest = item;
                    rentalRequestRotationList.RotationListSortOrder = currentSortOrder;

                    currentPosition++;
                    currentSortOrder++;
                    if (currentPosition >= listSize)
                    {
                        currentPosition = 0;
                    }
                }
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
        public virtual IActionResult RentalrequestsSearchGetAsync(int?[] localareas, string project, string status, DateTime? startDate, DateTime? endDate)
        {
            var data = _context.RentalRequests
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType)
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
                        
            if (project != null)
            {
                data = data.Where(x => x.Project.Name.ToLower().Contains (project.ToLower()));
            }

            if (startDate != null)
            {
                data = data.Where(x => x.ExpectedStartDate >= startDate);
            }

            if (endDate != null)
            {
                data = data.Where(x => x.ExpectedStartDate <= endDate);
            }

            var result = new List<RentalRequestSearchResultViewModel>();
            foreach (var item in data)
            {
                if (item != null)
                {
                    RentalRequestSearchResultViewModel newItem = item.ToViewModel();
                    result.Add(newItem);
                }
            }

            // no calculated fields in a RentalRequest search yet.                           
            return new ObjectResult(result);
        }
    }
}
