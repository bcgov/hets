using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.AspNetCore.Http;
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
            List<RentalRequest> result = _context.RentalRequests.AsNoTracking()
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
                // remove the rotation list if it exists
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
                RentalRequest result = _context.RentalRequests.AsNoTracking()
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
        /// Create rental request
        /// </summary>
        /// <param name="item"></param>
        /// <response>Rental Request</response>
        /// <response code="200">Rental Request created</response>
        /// <response code="405">In Progress Rental Request already exists</response>
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
                    // check if we have an existing rental request for the same 
                    // local area and equipment type - if so - throw an error
                    // Per discussion with the business (19 Jan 2018):
                    //    * Don't create a record as New if another Request exists
                    //    * Simply give the user an error and not allow the new request
                    // 
                    // Note: leaving the "New" code in place in case this changes in the future
                    List<RentalRequest> requests = _context.RentalRequests
                        .Where(x => x.DistrictEquipmentType == item.DistrictEquipmentType &&
                                    x.LocalArea.Id == item.LocalArea.Id &&
                                    x.Status.Equals("In Progress", StringComparison.CurrentCultureIgnoreCase))
                        .ToList();

                    if (requests.Count > 0)
                    {
                        // In Progress Rental Request already exists
                        return new StatusCodeResult(405);
                    }

                    // record not found
                    BuildRentalRequestRotationList(item);

                    // check if we have an existing "In Progress" request
                    // for the same Loac Area and Equipment Type
                    SetRentalRequestStatusOnCreate(item);

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
        /// Move rental request to "In Progress"
        /// </summary>
        /// <param name="id"></param>
        /// <response>Rental Request</response>
        public virtual IActionResult RentalrequestsInProgressPostAsync(int id)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists)
            {
                // get rental request
                RentalRequest rentalRequest = _context.RentalRequests
                    .Include(x => x.LocalArea)
                    .Include(x => x.DistrictEquipmentType)
                    .First(a => a.Id == id);

                // check if this record is in the correct state
                if (rentalRequest.Status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
                {
                    // try to set the status
                    SetRentalRequestStatusOnCreate(rentalRequest);

                    // check the status and update the rotation list
                    BuildRentalRequestRotationList(rentalRequest);
                }

                // save the changes
                _context.SaveChanges();

                // get the complete and updated rental request
                rentalRequest = _context.RentalRequests.AsNoTracking()
                    .Include(x => x.FirstOnRotationList)
                    .Include(x => x.RentalRequestRotationList)
                        .ThenInclude(y => y.Equipment)
                        .ThenInclude(r => r.EquipmentAttachments)
                    .Include(x => x.RentalRequestRotationList)
                        .ThenInclude(y => y.Equipment)
                        .ThenInclude(e => e.Owner)
                        .ThenInclude(c => c.PrimaryContact)
                    .First(a => a.Id == id);                

                return new ObjectResult(new HetsResponse(rentalRequest.ToRentalRequestViewModel()));
            }

            // no record to insert
            return new ObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));
        }

        /// <summary>
        /// Move rental request to "Complete"
        /// </summary>
        /// <param name="id"></param>
        /// <response>Rental Request</response>
        public virtual IActionResult RentalrequestsCompletePostAsync(int id)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists)
            {
                // get rental request
                RentalRequest rentalRequest = _context.RentalRequests
                    .Include(x => x.LocalArea)
                    .Include(x => x.DistrictEquipmentType)
                    .First(a => a.Id == id);

                // check if this record is in the correct state
                if (rentalRequest.Status.Equals("In Progress", StringComparison.InvariantCultureIgnoreCase))
                {
                    // try to set the status
                    rentalRequest.Status = "Complete";
                }

                // save the changes
                _context.SaveChanges();

                // get the complete and updated rental request
                rentalRequest = _context.RentalRequests.AsNoTracking()
                    .Include(x => x.FirstOnRotationList)
                    .Include(x => x.RentalRequestRotationList)
                        .ThenInclude(y => y.Equipment)
                        .ThenInclude(r => r.EquipmentAttachments)
                    .Include(x => x.RentalRequestRotationList)
                        .ThenInclude(y => y.Equipment)
                        .ThenInclude(e => e.Owner)
                        .ThenInclude(c => c.PrimaryContact)
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(rentalRequest.ToRentalRequestViewModel()));
            }

            // no record to insert
            return new ObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));
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

            IQueryable<RentalRequest> data = _context.RentalRequests.AsNoTracking()
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

        #region Rental Request Attachment

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

        #endregion

        #region Rental Request Rotation List

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
                    .Include(x => x.DistrictEquipmentType)
                        .ThenInclude(y => y.EquipmentType)
                    .Include(x => x.FirstOnRotationList)
                    .Include(x => x.RentalRequestAttachments)
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

                // return the number of blocks in this list
                RentalRequestViewModel rentalRequest = result.ToRentalRequestViewModel();
                rentalRequest.NumberOfBlocks = GetNumberOfBlocks(result) + 1;

                // return view model
                return new ObjectResult(new HetsResponse(rentalRequest));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update rental request rotation list
        /// </summary>
        /// <remarks>Updates a rental request rotation list entry</remarks>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">RentalRequestRotationList not found</response>
        public virtual IActionResult RentalrequestRotationListIdPutAsync(int id, RentalRequestRotationList item)
        {
            // update the rental request rotation list item
            AdjustRentalRequestRotationListRecord(item);

            // check if we have the rental request and the rotation list is attached
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (!exists)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // check if we have the rental request that is In Progress
            exists = _context.RentalRequests
                    .Any(a => a.Id == id &&
                              a.Status.Equals("In Progress", StringComparison.InvariantCultureIgnoreCase));

            if (!exists)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-06", ErrorViewModel.GetDescription("HETS-06", _configuration)));
            }

            exists = _context.RentalRequests
                .Any(a => a.Id == id &&
                          a.Status.Equals("In Progress", StringComparison.InvariantCultureIgnoreCase) &&
                          a.RentalRequestRotationList.Any(b => b.Id == item.Id));            

            if (!exists)
            {
                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // ******************************************************************
            // update the rental request rotation list record
            // and count the "Yes"es
            // ******************************************************************  
            RentalRequest rentalRequest = _context.RentalRequests                
                .AsNoTracking()
                .Include(x => x.RentalRequestRotationList)
                    .ThenInclude(e => e.Equipment)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(d => d.EquipmentType)    
                .Include(x => x.LocalArea)
                .Include(x => x.Project)
                    .ThenInclude(y => y.RentalAgreements)
                .First(a => a.Id == id);

            _context.Entry(rentalRequest).State = EntityState.Detached;            

            // ******************************************************************
            // find the rotation list record to update
            // ******************************************************************
            int rotationListIndex = -1;

            for (int i = 0; i < rentalRequest.RentalRequestRotationList.Count; i++)
            {
                if (rentalRequest.RentalRequestRotationList[i].Id == item.Id)
                {
                    rotationListIndex = i;
                    break;
                }                
            }

            // ******************************************************************
            // update the rental request rotation list record
            // ******************************************************************
            item.AppCreateTimestamp = rentalRequest.RentalRequestRotationList[rotationListIndex].AppCreateTimestamp;
            item.AppCreateUserid = rentalRequest.RentalRequestRotationList[rotationListIndex].AppCreateUserid;
            rentalRequest.RentalRequestRotationList[rotationListIndex] = item;

            // ******************************************************************
            // do we need to create a Rental Agreement?
            // ******************************************************************
            if (item.IsForceHire == true ||
                item.OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                RentalAgreement rentalAgreement = new RentalAgreement
                {
                    Equipment = item.Equipment,
                    Project = rentalRequest.Project,
                    Status = "Active",
                    DatedOn = DateTime.Now                    
                };

                // generate the rental agreeement number
                rentalAgreement.Number = GetRentalAgreementNumber(rentalAgreement);

                // add new rental agreement to the project
                rentalRequest.Project.RentalAgreements.Add(rentalAgreement);

                // relate the new rental agreement to the original rotation list record
                rentalRequest.RentalRequestRotationList[rotationListIndex].RentalAgreement = rentalAgreement;
                
                // relate it to return to our client (not for the db)
                item.RentalAgreement = rentalAgreement;
            }

            // ******************************************************************
            // can we "Complete" this rental request
            // (if the Yes or Forced Hires = Request.EquipmentCount)
            // ******************************************************************   
            int countOfYeses = 0;
            int equipmentRequestCount = rentalRequest.EquipmentCount;

            foreach (RentalRequestRotationList rotationList in rentalRequest.RentalRequestRotationList)
            {
                if (rotationList.OfferResponse != null &&
                    rotationList.OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    countOfYeses = countOfYeses + 1;
                }
                else if (rotationList.IsForceHire != null &&
                         rotationList.IsForceHire == true)
                {
                    countOfYeses = countOfYeses + 1;
                }
            }

            if (countOfYeses >= equipmentRequestCount)
            {
                rentalRequest.Status = "Complete";
                rentalRequest.FirstOnRotationList = null;
            }                        

            // ******************************************************************
            // If the request is still In Progress:
            // 1. get the number of blocks for this equipment type
            // 2. set which rotation list record is currently "active"
            // ******************************************************************                
            if (!rentalRequest.Status.Equals("Complete", StringComparison.InvariantCultureIgnoreCase))
            {
                int numberOfBlocks = GetNumberOfBlocks(rentalRequest);
                UpdateLocalAreaRotationList(rentalRequest, numberOfBlocks);
            }

            // ******************************************************************
            // save the changes - Rental Request
            // ******************************************************************
            _context.RentalRequests.Update(rentalRequest);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));            
        }

        /// <summary>
        /// Create the rental agreement number
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetRentalAgreementNumber(RentalAgreement item)
        {
            string result = "";

            // validate item.
            if (item.Equipment != null && item.Equipment.LocalArea != null)
            {
                DateTime currentTime = DateTime.UtcNow;

                int fiscalYear = currentTime.Year;

                // fiscal year always ends in March.
                if (currentTime.Month > 3)
                {
                    fiscalYear++;
                }

                int localAreaNumber = item.Equipment.LocalArea.LocalAreaNumber;
                int localAreaId = item.Equipment.LocalArea.Id;

                DateTime fiscalYearStart = new DateTime(fiscalYear - 1, 1, 1);

                // count the number of rental agreements in the system.
                int currentCount = _context.RentalAgreements
                    .Include(x => x.Equipment.LocalArea)
                    .Count(x => x.Equipment.LocalArea.Id == localAreaId && x.AppCreateTimestamp >= fiscalYearStart);

                currentCount++;

                // format of the Rental Agreement number is YYYY-#-####
                result = fiscalYear + "-" + localAreaNumber + "-" + currentCount.ToString("D4");
            }

            return result;
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

                if (result.Status.Equals("In Progress", StringComparison.InvariantCultureIgnoreCase))
                {
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

                    // repopulate the results with latest data
                    result = _context.RentalRequests
                        .Include(x => x.DistrictEquipmentType)
                        .Include(x => x.DistrictEquipmentType.EquipmentType)
                        .Include(x => x.LocalArea)
                        .Include(x => x.LocalArea.ServiceArea.District.Region)
                        .Include(x => x.RentalRequestRotationList)
                        .First(a => a.Id == rentalRequestId);
                }

                return new ObjectResult(new HetsResponse(result.ToRentalRequestViewModel()));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Rental Request History

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

                List<History> data = rentalRequest.History.OrderByDescending(y => y.AppLastUpdateTimestamp).ToList();

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
            result.LastUpdateTimestamp = item.AppLastUpdateTimestamp;
            result.LastUpdateUserid = item.AppLastUpdateUserid;
            result.AffectedEntityId = id;

            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region Manage Rental Request Rotation List

        /// <summary>
        /// Set the Status of the Rental Request Rotation List
        /// </summary>
        /// <param name="item"></param>
        private void SetRentalRequestStatusOnCreate(RentalRequest item)
        {
            // validate input parameters
            if (item != null &&
                item.LocalArea != null &&
                item.DistrictEquipmentType != null &&
                item.DistrictEquipmentType.EquipmentType != null)
            {
                // check if there is an existing "In Progress" Rental Request
                List<RentalRequest> requests = _context.RentalRequests
                    .Where(x => x.DistrictEquipmentType == item.DistrictEquipmentType &&
                                x.LocalArea.Id == item.LocalArea.Id &&
                                x.Status.Equals("In Progress", StringComparison.CurrentCultureIgnoreCase))
                    .ToList();

                item.Status = requests.Count == 0 ? "In Progress" : "New";
            }
        }

        /// <summary>
        /// Get the number of blocks for this type of equipment 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int GetNumberOfBlocks(RentalRequest item)
        {
            int numberOfBlocks = -1;

            try
            {            
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);

                numberOfBlocks = item.DistrictEquipmentType.EquipmentType.IsDumpTruck ?
                    scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();
            }
            catch
            {
                // do nothing
            }

            return numberOfBlocks;
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
                // (default blocks plus 1 (for the open block))
                // *******************************************************************************
                int numberOfBlocks = GetNumberOfBlocks(item);
                numberOfBlocks = numberOfBlocks + 1;

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
                                AppCreateTimestamp = DateTime.UtcNow,
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

                // update the local area rotation list
                UpdateLocalAreaRotationList(item, numberOfBlocks);
            }
        }

        /// <summary>
        /// Update the Local Area Rotation List
        /// </summary>
        /// <param name="item"></param>
        /// <param name="numberOfBlocks"></param>
        private void UpdateLocalAreaRotationList(RentalRequest item, int numberOfBlocks)
        {
            // *******************************************************************************
            // first get the localAreaRotationList.askNextBlock"N" for the given local area
            // *******************************************************************************
            bool exists = _context.LocalAreaRotationLists.Any(a => a.LocalArea.Id == item.LocalArea.Id);

            LocalAreaRotationList localAreaRotationList = new LocalAreaRotationList();

            if (exists)
            {
                localAreaRotationList = _context.LocalAreaRotationLists
                    .AsNoTracking()
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
            // 1. the "next on the list" table for the Local Area
            //   (HET_LOCAL_AREA_ROTATION_LIST)
            // 2. the first on the list id for the Rental Request 
            //   (HET_RENTAL_REQUEST.FIRST_ON_ROTATION_LIST_ID)
            // *******************************************************************************
            if (item.RentalRequestRotationList.Count > 0)
            {
                item.RentalRequestRotationList = item.RentalRequestRotationList.OrderBy(x => x.RotationListSortOrder).ToList();

                item.FirstOnRotationListId = item.RentalRequestRotationList[0].Equipment.Id;

                LocalAreaRotationList newAreaRotationList = new LocalAreaRotationList
                {
                    LocalAreaId = item.LocalAreaId,
                    DistrictEquipmentTypeId = item.DistrictEquipmentTypeId                    
                };

                if (nextId != null)
                {
                    newAreaRotationList.Id = localAreaRotationList.Id;
                    newAreaRotationList.AskNextBlock1 = localAreaRotationList.AskNextBlock1;
                    newAreaRotationList.AskNextBlock1Id = localAreaRotationList.AskNextBlock1Id;
                    newAreaRotationList.AskNextBlock1Seniority = localAreaRotationList.AskNextBlock1Seniority;
                    newAreaRotationList.AskNextBlock2 = localAreaRotationList.AskNextBlock2;
                    newAreaRotationList.AskNextBlock2Id = localAreaRotationList.AskNextBlock2Id;
                    newAreaRotationList.AskNextBlock2Seniority = localAreaRotationList.AskNextBlock2Seniority;
                    newAreaRotationList.AskNextBlockOpen = localAreaRotationList.AskNextBlockOpen;
                    newAreaRotationList.AskNextBlockOpenId = localAreaRotationList.AskNextBlockOpenId;
                }

                // find our next record
                int nextRecordToAskIndex = 0;
                bool foundCurrentRecord = false;

                if (nextId != null)
                {
                    for (int i = 0; i < item.RentalRequestRotationList.Count; i++)
                    {
                        bool forcedHire;
                        bool hired;

                        if (foundCurrentRecord &&
                            item.RentalRequestRotationList[i].IsForceHire != null &&
                            item.RentalRequestRotationList[i].IsForceHire == false)
                        {
                            forcedHire = false;
                        }
                        else if (foundCurrentRecord && item.RentalRequestRotationList[i].IsForceHire == null)
                        {
                            forcedHire = false;
                        }
                        else
                        {
                            forcedHire = true;
                        }

                        if (foundCurrentRecord &&
                            item.RentalRequestRotationList[i].OfferResponse != null &&
                            !item.RentalRequestRotationList[i].OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                        {
                            hired = false;
                        }
                        else if (foundCurrentRecord && item.RentalRequestRotationList[i].OfferResponse == null)
                        {
                            hired = false;
                        }
                        else
                        {
                            hired = true;
                        }

                        if (foundCurrentRecord && !forcedHire && !hired)
                        {
                            // we've found our next record - exit and update the lists
                            nextRecordToAskIndex = i;
                            break;
                        }

                        if (!foundCurrentRecord &&
                            item.RentalRequestRotationList[i].Equipment.Id == nextId)
                        {
                            foundCurrentRecord = true;
                        }
                    }
                }

                if (item.RentalRequestRotationList[nextRecordToAskIndex].Equipment.BlockNumber == 1 &&
                    item.RentalRequestRotationList[nextRecordToAskIndex].Equipment.BlockNumber <= numberOfBlocks)
                {

                    newAreaRotationList.AskNextBlock1 = item.RentalRequestRotationList[nextRecordToAskIndex].Equipment;
                    newAreaRotationList.AskNextBlock1Id = newAreaRotationList.AskNextBlock1.Id;
                    newAreaRotationList.AskNextBlock1Seniority = item.RentalRequestRotationList[nextRecordToAskIndex].Equipment.Seniority;
                    newAreaRotationList.AskNextBlock2Id = null;
                    newAreaRotationList.AskNextBlock2Seniority = null;
                    newAreaRotationList.AskNextBlockOpen = null;
                    newAreaRotationList.AskNextBlockOpenId = null;
                }
                else if (item.RentalRequestRotationList[nextRecordToAskIndex].Equipment.BlockNumber == 2 &&
                         item.RentalRequestRotationList[nextRecordToAskIndex].Equipment.BlockNumber <= numberOfBlocks)
                {
                    newAreaRotationList.AskNextBlock2 = item.RentalRequestRotationList[nextRecordToAskIndex].Equipment;
                    newAreaRotationList.AskNextBlock2Id = newAreaRotationList.AskNextBlock2.Id;
                    newAreaRotationList.AskNextBlock2Seniority = item.RentalRequestRotationList[nextRecordToAskIndex].Equipment.Seniority;
                    newAreaRotationList.AskNextBlock1 = null;
                    newAreaRotationList.AskNextBlock1Id = null;
                    newAreaRotationList.AskNextBlock1Seniority = null;
                    newAreaRotationList.AskNextBlockOpen = null;
                    newAreaRotationList.AskNextBlockOpenId = null;
                }
                else
                {
                    newAreaRotationList.AskNextBlockOpen = item.RentalRequestRotationList[nextRecordToAskIndex].Equipment;
                    newAreaRotationList.AskNextBlockOpenId = newAreaRotationList.AskNextBlockOpen.Id;
                    newAreaRotationList.AskNextBlock1 = null;
                    newAreaRotationList.AskNextBlock1Id = null;
                    newAreaRotationList.AskNextBlock1Seniority = null;
                    newAreaRotationList.AskNextBlock2 = null;
                    newAreaRotationList.AskNextBlock2Id = null;
                    newAreaRotationList.AskNextBlock2Seniority = null;
                }
                
                if (nextId == null)
                {
                    _context.LocalAreaRotationLists.Add(newAreaRotationList);
                }
                else
                {
                    _context.LocalAreaRotationLists.Update(newAreaRotationList);
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

        #endregion

        #region Rental Request Note Records

        /// <summary>
        /// Get note records associated with rental request
        /// </summary>
        /// <param name="id">id of Rental Request to fetch Notes for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestsIdNotesGetAsync(int id)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists)
            {
                RentalRequest rentalRequest = _context.RentalRequests.AsNoTracking()
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);

                List<Note> notes = new List<Note>();

                foreach (Note note in rentalRequest.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a note associated with a rental request
        /// </summary>
        /// <remarks>Update a Rental Request&#39;s Notes</remarks>
        /// <param name="id">id of Rental Request to update Notes for</param>
        /// <param name="item">Rental Request Note</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestsIdNotesPostAsync(int id, Note item)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists && item != null)
            {
                RentalRequest rentalRequest = _context.RentalRequests
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);

                // ******************************************************************
                // add or update note
                // ******************************************************************
                if (item.Id > 0)
                {
                    int noteIndex = rentalRequest.Notes.FindIndex(a => a.Id == item.Id);

                    if (noteIndex < 0)
                    {
                        // record not found
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    rentalRequest.Notes[noteIndex].Text = item.Text;
                    rentalRequest.Notes[noteIndex].IsNoLongerRelevant = item.IsNoLongerRelevant;
                }
                else  // add note
                {
                    rentalRequest.Notes.Add(item);
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated time records
                // *************************************************************              
                List<Note> notes = new List<Note>();

                foreach (Note note in rentalRequest.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create an array of notes associated with a rental request
        /// </summary>
        /// <remarks>Update a Rental Request&#39;s Notes</remarks>
        /// <param name="id">id of Rental Request to update Notes for</param>
        /// <param name="items">Array of Rental Request Notes</param>
        /// <response code="200">OK</response>
        public virtual IActionResult RentalrequestsIdNotesBulkPostAsync(int id, Note[] items)
        {
            bool exists = _context.RentalRequests.Any(a => a.Id == id);

            if (exists && items != null)
            {
                RentalRequest rentalRequest = _context.RentalRequests
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);

                // process each note
                foreach (Note item in items)
                {
                    // ******************************************************************
                    // add or update note
                    // ******************************************************************
                    if (item.Id > 0)
                    {
                        int noteIndex = rentalRequest.Notes.FindIndex(a => a.Id == item.Id);

                        if (noteIndex < 0)
                        {
                            // record not found
                            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        }

                        rentalRequest.Notes[noteIndex].Text = item.Text;
                        rentalRequest.Notes[noteIndex].IsNoLongerRelevant = item.IsNoLongerRelevant;
                    }
                    else  // add note
                    {
                        rentalRequest.Notes.Add(item);
                    }

                    _context.SaveChanges();
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated notes                
                // *************************************************************
                List<Note> notes = new List<Note>();

                foreach (Note note in rentalRequest.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion
    }
}
