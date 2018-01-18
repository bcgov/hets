using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Rental Request Service
    /// </summary>
    public class RentalRequestService : ServiceBase, IRentalRequestService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Rental Request Service Constructor
        /// </summary>
        public RentalRequestService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord(RentalRequest item)
        {
            if (item != null)
            {
                // Adjust the record to allow it to be updated / inserted
                if (item.LocalArea != null)
                {
                    item.LocalArea = _context.LocalAreas.FirstOrDefault(a => a.Id == item.LocalArea.Id);
                }

                if (item.Project != null)
                {
                    item.Project = _context.Projects.FirstOrDefault(a => a.Id == item.Project.Id);
                }

                if (item.DistrictEquipmentType != null)
                {
                    item.DistrictEquipmentType = _context.DistrictEquipmentTypes
                        .Include(x => x.EquipmentType)
                        .First(a => a.Id == item.DistrictEquipmentType.Id);
                }
            }
        }

        /// <summary>
        /// Create bulk rental request records
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

            // save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all rental requests
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestsGetAsync()
        {
            List<RentalRequest> result = _context.RentalRequests
                .Include(x => x.RentalRequestAttachments)
                .Include(x => x.DistrictEquipmentType)
                .Include(x => x.FirstOnRotationList)
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Notes)
                .Include(x => x.Project)
                .Include(x => x.RentalRequestRotationList)
                .ToList();

            List<RentalRequestViewModel> resultModel = new List<RentalRequestViewModel>();

            foreach (RentalRequest rentalRequest in result)
            {
                resultModel.Add(rentalRequest.ToRentalRequestViewModel());
            }

            return new ObjectResult(new HetsResponse(resultModel));
        }

        /// <summary>
        /// Get attachments associated with a rental request
        /// </summary>
        /// <remarks>Returns attachments for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequest not found</response>

        public virtual IActionResult RentalrequestsIdAttachmentsGetAsync(int id)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists)
            {
                RentalRequest rentalRequest = _context.RentalRequests
                    .Include(x => x.Attachments)
                    .First(a => a.Id == id);

                List<AttachmentViewModel> result = MappingExtensions.GetAttachmentListAsViewModel(rentalRequest.Attachments);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Delete rental request
        /// </summary>
        /// <param name="id">id of Project to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalrequestsIdDeletePostAsync(int id)
        {
            RentalRequest item = _context.RentalRequests
                .Include(x => x.RentalRequestRotationList)
                .FirstOrDefault(a => a.Id == id);

            if (item != null)
            {
                // Remove the rotation list if it exists.
                if (item.RentalRequestRotationList != null)
                {
                    foreach (RentalRequestRotationList rentalRequestRotationList in item.RentalRequestRotationList)
                    {
                        _context.RentalRequestRotationLists.Remove(rentalRequestRotationList);
                    }
                }

                _context.RentalRequests.Remove(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get history associated with a rental request
        /// </summary>
        /// <remarks>Returns History for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch History for</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
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
                    limit = data.Count - offset;
                }

                List<HistoryViewModel> result = new List<HistoryViewModel>();

                for (int i = (int)offset; i < data.Count && i < offset + limit; i++)
                {
                    result.Add(data[i].ToViewModel(id));
                }

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create history associated with a rental request
        /// </summary>
        /// <remarks>Add a History record to the RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
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

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Get rental request by id
        /// </summary>
        /// <param name="id">id of Rental Request to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalrequestsIdGetAsync(int id)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists)
            {
                RentalRequest result = _context.RentalRequests
                    .Include(x => x.RentalRequestAttachments)
                    .Include(x => x.DistrictEquipmentType)
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.Notes)
                    .Include(x => x.Project.PrimaryContact)
                    .First(a => a.Id == id);
                
                return new ObjectResult(new HetsResponse(result.ToRentalRequestViewModel()));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get rental request rotation list by rental request id
        /// </summary>
        /// <param name="id">id of Rental Request to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalrequestsIdRotationListGetAsync(int id)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists)
            {
                // check that we have a rotation list
                RentalRequest result = _context.RentalRequests     
                    .Include(x => x.FirstOnRotationList)
                    .Include(x => x.RentalRequestRotationList)
                        .ThenInclude(y => y.Equipment)
                        .ThenInclude(r => r.EquipmentAttachments)
                    .Include(x => x.RentalRequestRotationList)
                        .ThenInclude(y => y.Equipment)
                        .ThenInclude(e => e.Owner)
                        .ThenInclude(c => c.PrimaryContact)
                    .First(a => a.Id == id);
                
                // resort list using: LocalArea / District Equipment Type and SenioritySortOrder (desc)
                result.RentalRequestRotationList =
                    result.RentalRequestRotationList.OrderBy(e => e.RotationListSortOrder).ToList();

                // return view model
                return new ObjectResult(new HetsResponse(result.ToRentalRequestViewModel()));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update rental request
        /// </summary>
        /// <param name="id">id of Rental Request to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Project not found</response>
        public virtual IActionResult RentalrequestsIdPutAsync(int id, RentalRequest item)
        {
            AdjustRecord(item);

            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists && id == item.Id)
            {
                _context.RentalRequests.Update(item);

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item.ToRentalRequestViewModel()));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create Rental Request Rotation List
        /// </summary>
        /// <param name="item"></param>
        private void BuildRentalRequestRotationList(RentalRequest item)
        {
            // validate input parameters
            if (item != null && item.LocalArea != null && item.DistrictEquipmentType != null && item.DistrictEquipmentType.EquipmentType != null)
            {
                int currentSortOrder = 1;

                item.RentalRequestRotationList = new List<RentalRequestRotationList>();                

                // *******************************************************************************
                // get the number of blocks for this piece of equipment
                // *******************************************************************************
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);
                int numberOfBlocks = item.DistrictEquipmentType.EquipmentType.IsDumpTruck ? 
                    scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();

                // *******************************************************************************
                // get the equipment based on the current seniority list for the area
                // (and sort the results based on seniority)
                // *******************************************************************************
                for (int currentBlock = 1; currentBlock <= numberOfBlocks; currentBlock++)
                {
                    // start by getting the current set of equipment for the given equipment type
                    List<Equipment> blockEquipment = _context.Equipments
                        .Where(x => x.DistrictEquipmentType == item.DistrictEquipmentType && 
                                    x.BlockNumber == currentBlock && 
                                    x.LocalArea.Id == item.LocalArea.Id)
                        .OrderByDescending(x => x.Seniority)
                        .ToList();

                    int listSize = blockEquipment.Count;

                    // check if any items have "Active" contracts - and drop them from the list
                    for (int i = listSize - 1; i >= 0; i--)
                    {
                        bool agreementExists = _context.RentalAgreements
                            .Any(x => x.EquipmentId == blockEquipment[i].Id && 
                                      x.Status == "Active");

                        if (agreementExists)
                        {
                            blockEquipment.RemoveAt(i);
                        }
                    }

                    // update list size for sorting
                    listSize = blockEquipment.Count;

                    // sets the rotation list sort order
                    int currentPosition = 0;

                    for (int i = 0; i < listSize; i++)
                    {
                        RentalRequestRotationList rentalRequestRotationList =
                            new RentalRequestRotationList
                            {
                                Equipment = blockEquipment[i],
                                CreateTimestamp = DateTime.UtcNow,
                                RotationListSortOrder = currentSortOrder
                            };

                        item.RentalRequestRotationList.Add(rentalRequestRotationList);

                        currentPosition++;
                        currentSortOrder++;

                        if (currentPosition >= listSize)
                        {
                            currentPosition = 0;
                        }
                    }
                }                

                // *******************************************************************************
                // first get the localAreaRotationList.askNextBlock"N" for the given local area
                // Identifies if an existing request is in progress
                // *******************************************************************************
                bool exists = _context.LocalAreaRotationLists.Any(a => a.LocalArea.Id == item.LocalArea.Id);

                LocalAreaRotationList localAreaRotationList = new LocalAreaRotationList();

                if (exists)
                {
                    localAreaRotationList = _context.LocalAreaRotationLists
                        .Include(x => x.LocalArea)
                        .Include(x => x.AskNextBlock1)
                        .Include(x => x.AskNextBlock2)
                        .Include(x => x.AskNextBlockOpen)
                        .FirstOrDefault(x => x.LocalArea.Id == item.LocalArea.Id &&
                                             x.DistrictEquipmentTypeId == item.DistrictEquipmentTypeId);
                }

                // determine what the next id is
                int? nextId = null;

                if (localAreaRotationList != null && localAreaRotationList.Id > 0)
                {
                    if (localAreaRotationList.AskNextBlock1Id != null)
                    {
                        nextId = localAreaRotationList.AskNextBlock1Id;
                    }
                    else if (localAreaRotationList.AskNextBlock2Id != null)
                    {
                        nextId = localAreaRotationList.AskNextBlock2Id;
                    }
                    else
                    {
                        nextId = localAreaRotationList.AskNextBlockOpenId;                                        
                    }
                }                

                // *******************************************************************************
                // populate:
                // 1. the "first on the list" table for the Local Area
                //   (HET_LOCAL_AREA_ROTATION_LIST)
                // 2. the first on the list id for the Rental Request 
                //   (HET_RENTAL_REQUEST.FIRST_ON_ROTATION_LIST_ID)
                // *******************************************************************************
                if (item.RentalRequestRotationList.Count > 0)
                {
                    // no local area record exists - create!
                    item.FirstOnRotationListId = item.RentalRequestRotationList[0].Equipment.Id;

                    LocalAreaRotationList areaRotationList = new LocalAreaRotationList
                    {
                        LocalAreaId = item.LocalAreaId,
                        DistrictEquipmentTypeId = item.DistrictEquipmentTypeId
                    };

                    // put into the correct field
                    if (item.RentalRequestRotationList[0].Equipment.BlockNumber == 1 &&
                        item.RentalRequestRotationList[0].Equipment.BlockNumber <= numberOfBlocks)
                    {
                        areaRotationList.AskNextBlock1Id = item.RentalRequestRotationList[0].Equipment.Id;
                        areaRotationList.AskNextBlock1Seniority = item.RentalRequestRotationList[0].Equipment.Seniority;
                    }
                    else if (item.RentalRequestRotationList[0].Equipment.BlockNumber == 2 &&
                             item.RentalRequestRotationList[0].Equipment.BlockNumber <= numberOfBlocks)
                    {
                        areaRotationList.AskNextBlock2Id = item.RentalRequestRotationList[0].Equipment.Id;
                        areaRotationList.AskNextBlock2Seniority = item.RentalRequestRotationList[0].Equipment.Seniority;
                    }
                    else
                    {
                        areaRotationList.AskNextBlockOpenId = item.RentalRequestRotationList[0].Equipment.Id;
                    }

                    if (nextId == null)
                    {
                        _context.LocalAreaRotationLists.Add(areaRotationList);
                    }
                }                   
            }
        }

        /// <summary>
        /// Adjust the Rental Request Rotation List record
        /// </summary>
        /// <param name="item"></param>
        private void AdjustRentalRequestRotationListRecord(RentalRequestRotationList item)
        {
            if (item != null)
            {
                if (item.Equipment != null)
                {
                    item.Equipment = _context.Equipments.FirstOrDefault(a => a.Id == item.Equipment.Id);
                }

                if (item.RentalAgreement != null)
                {
                    item.RentalAgreement = _context.RentalAgreements.FirstOrDefault(a => a.Id == item.RentalAgreement.Id);
                }
            }
        }

        /// <summary>
        /// Update rental request
        /// </summary>
        /// <remarks>Updates a rental request rotation list entry.  Side effect is the LocalAreaRotationList is also updated</remarks>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="rentalRequestRotationListId">id of RentalRequestRotationList to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        public virtual IActionResult RentalrequestRotationListIdPutAsync(int id, int rentalRequestRotationListId, RentalRequestRotationList item)
        {
            // update the rental request rotation list item.
            AdjustRentalRequestRotationListRecord(item);

            bool exists = _context.RentalRequestRotationLists.Any(a => a.Id == rentalRequestRotationListId);

            if (exists && rentalRequestRotationListId == item.Id)
            {
                _context.RentalRequestRotationLists.Update(item);

                // save the changes
                _context.SaveChanges();

                _context.Entry(item).State = EntityState.Detached;

                // now update the corresponding entry in the LocalAreaRotationList.
                _context.UpdateLocalAreaRotationList(item.Id);

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create rental request
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Project created</response>
        public virtual IActionResult RentalrequestsPostAsync(RentalRequest item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.RentalRequests.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.RentalRequests.Update(item);
                }
                else
                {
                    // record not found
                    BuildRentalRequestRotationList(item);
                    _context.RentalRequests.Add(item);
                }

                // save the changes
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // no record to insert
            return new ObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));
        }

        /// <summary>
        /// Create rental request rotation list
        /// </summary>
        /// <param name="rentalRequestId"></param>
        public virtual IActionResult RentalRequestsRotationListRecalcGetAsync(int rentalRequestId)
        {           
            bool exists = _context.RentalRequests.Any(a => a.Id == rentalRequestId);

            if (exists)
            {
                // get rental request
                RentalRequest result = _context.RentalRequests
                        .Include(x => x.DistrictEquipmentType)
                        .Include(x => x.DistrictEquipmentType.EquipmentType)
                        .Include(x => x.LocalArea)
                        .Include(x => x.LocalArea.ServiceArea.District.Region)
                        .Include(x => x.RentalRequestRotationList)
                        .First(a => a.Id == rentalRequestId);

                // delete any existing rotation list records
                foreach (RentalRequestRotationList rentalRequestRotationList in result.RentalRequestRotationList)
                {
                    _context.RentalRequestRotationLists.Remove(rentalRequestRotationList);
                }

                // update
                _context.SaveChanges();
                
                // generate rotation list
                BuildRentalRequestRotationList(result);                

                // save the changes
                _context.SaveChanges();

                result = _context.RentalRequests
                    .Include(x => x.DistrictEquipmentType)
                    .Include(x => x.DistrictEquipmentType.EquipmentType)
                    .Include(x => x.LocalArea)
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.RentalRequestRotationList)
                    .First(a => a.Id == rentalRequestId);

                return new ObjectResult(new HetsResponse(result.ToRentalRequestViewModel()));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Searches rental requests
        /// </summary>
        /// <remarks>Used for the rental request search page.</remarks>
        /// <param name="localareas">Local areas (comma seperated list of id numbers)</param>
        /// <param name="project">name or partial name for a Project</param>
        /// <param name="status">Status</param>
        /// <param name="startDate">Inspection start date</param>
        /// <param name="endDate">Inspection end date</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestsSearchGetAsync(string localareas, string project, string status, DateTime? startDate, DateTime? endDate)
        {
            int?[] localareasArray = ParseIntArray(localareas);

            IQueryable<RentalRequest> data = _context.RentalRequests
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType.EquipmentType)
                    .Include(x => x.Project.PrimaryContact)
                    .Select(x => x);

            // Default search results must be limited to user
            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();
            data = data.Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId));

            if (localareasArray != null && localareasArray.Length > 0)
            {
                data = data.Where(x => localareasArray.Contains(x.LocalArea.Id));
            }

            if (project != null)
            {
                data = data.Where(x => x.Project.Name.ToLowerInvariant().Contains(project.ToLowerInvariant()));
            }

            if (startDate != null)
            {
                data = data.Where(x => x.ExpectedStartDate >= startDate);
            }

            if (endDate != null)
            {
                data = data.Where(x => x.ExpectedStartDate <= endDate);
            }

            if (status != null)
            {
                data = data.Where(x => String.Equals(x.Status, status, StringComparison.CurrentCultureIgnoreCase));
            }

            List<RentalRequestSearchResultViewModel> result = new List<RentalRequestSearchResultViewModel>();

            foreach (RentalRequest item in data)
            {
                if (item != null)
                {
                    RentalRequestSearchResultViewModel newItem = item.ToViewModel();
                    result.Add(newItem);
                }
            }

            // no calculated fields in a RentalRequest search yet
            return new ObjectResult(new HetsResponse(result));
        }
    }
}
