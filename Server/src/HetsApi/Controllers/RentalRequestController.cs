using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Rental Request Controller
    /// </summary>
    [Route("api/rentalRequests")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalRequestController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;

        public RentalRequestController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }
        
        /// <summary>
        /// Get rental request by id
        /// </summary>
        /// <param name="id">id of RentalRequest to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("RentalRequestsIdGet")]
        [SwaggerResponse(200, type: typeof(HetRentalRequest))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        
            return new ObjectResult(new HetsResponse(RentalRequestHelper.GetRecord(id, _context)));            
        }

        /// <summary>
        /// Update rental request
        /// </summary>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("RentalRequestsIdPut")]
        [SwaggerResponse(200, type: typeof(HetRentalRequest))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdPut([FromRoute]int id, [FromBody]HetRentalRequest item)
        {
            if (item == null || id != item.RentalRequestId)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalRequest rentalRequest = _context.HetRentalRequest
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(c => c.PrimaryContact)
                .Include(x => x.HetRentalRequestAttachment)
                .Include(x => x.DistrictEquipmentType)
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(y => y.Equipment)
                .First(a => a.RentalRequestId == id);

            // need to check if we are going over the "count" and close this request
            int hiredCount = 0;

            foreach (HetRentalRequestRotationList equipment in rentalRequest.HetRentalRequestRotationList)
            {
                if (equipment.OfferResponse != null &&
                    equipment.OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    hiredCount++;
                }

                if (equipment.IsForceHire != null &&
                    equipment.IsForceHire == true)
                {
                    hiredCount++;
                }
            }

            // has the count changed - and is now less than the already "hired" equipment
            if (item.EquipmentCount != rentalRequest.EquipmentCount &&
                hiredCount > item.EquipmentCount)
            {
                //"HETS-07": "Rental Request count cannot be less than equipment already hired"
                return new ObjectResult(new HetsResponse("HETS-07", ErrorViewModel.GetDescription("HETS-07", _configuration)));
            }

            // if the number of hired records is now "over the count" - then close 
            if (hiredCount >= item.EquipmentCount)
            {
                int? statusIdComplete = StatusHelper.GetStatusId(HetRentalRequest.StatusComplete, "rentalRequestStatus", _context);
                if (statusIdComplete == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                item.RentalRequestStatusTypeId = (int)statusIdComplete;
                item.Status = "Complete";
                item.FirstOnRotationList = null;
            }

            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalRequestStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // update rental request
            rentalRequest.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            rentalRequest.RentalRequestStatusTypeId = (int)statusId;
            rentalRequest.EquipmentCount = item.EquipmentCount;
            rentalRequest.ExpectedEndDate = item.ExpectedEndDate;
            rentalRequest.ExpectedStartDate = item.ExpectedStartDate;
            rentalRequest.ExpectedHours = item.ExpectedHours;
            rentalRequest.HetDigitalFile = item.HetDigitalFile;
            rentalRequest.FirstOnRotationList = item.FirstOnRotationList;

            // do we have any attachments (only a single string is ever stored)
            if (item.HetRentalRequestAttachment != null &&
                item.HetRentalRequestAttachment.Count > 0)
            {
                if (rentalRequest.HetRentalRequestAttachment == null)
                {
                    rentalRequest.HetRentalRequestAttachment = new List<HetRentalRequestAttachment>();
                }

                HetRentalRequestAttachment attachment = new HetRentalRequestAttachment
                {
                    Attachment = item.HetRentalRequestAttachment.ElementAt(0).Attachment
                };

                if (rentalRequest.HetRentalRequestAttachment.Count > 0)
                {
                    rentalRequest.HetRentalRequestAttachment.ElementAt(0).Attachment = attachment.Attachment;
                }
                else
                {
                    rentalRequest.HetRentalRequestAttachment.Add(attachment);
                }                
            }

            // save the changes
            _context.SaveChanges();

            // retrieve updated rental request to return to ui
            return new ObjectResult(new HetsResponse(RentalRequestHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Create rental request
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("RentalRequestsPost")]
        [SwaggerResponse(200, type: typeof(HetRentalRequest))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsPost([FromBody]HetRentalRequest item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));

            // check if we have an existing rental request for the same 
            // local area and equipment type - if so - throw an error
            // Per discussion with the business (19 Jan 2018):
            //    * Don't create a record as New if another Request exists
            //    * Simply give the user an error and not allow the new request
            // 
            // Note: leaving the "New" code in place in case this changes in the future
            int? statusIdInProgress = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", _context);
            if (statusIdInProgress == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            List<HetRentalRequest> requests = _context.HetRentalRequest
                .Where(x => x.DistrictEquipmentTypeId == item.DistrictEquipmentType.DistrictEquipmentTypeId &&
                            x.LocalAreaId == item.LocalArea.LocalAreaId &&
                            x.RentalRequestStatusTypeId == statusIdInProgress)
                .ToList();

            // in Progress Rental Request already exists
            if (requests.Count > 0) return new ObjectResult(new HetsResponse("HETS-28", ErrorViewModel.GetDescription("HETS-28", _configuration)));

            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalRequestStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));
            
            // create new rental request
            HetRentalRequest rentalRequest = new HetRentalRequest
            {
                LocalAreaId = item.LocalArea.LocalAreaId,
                ProjectId = item.Project.ProjectId,
                DistrictEquipmentTypeId = item.DistrictEquipmentType.DistrictEquipmentTypeId,
                RentalRequestStatusTypeId = (int)statusId,
                EquipmentCount = item.EquipmentCount,
                ExpectedEndDate = item.ExpectedEndDate,
                ExpectedStartDate = item.ExpectedStartDate,
                ExpectedHours = item.ExpectedHours
            };

            // record not found - build new list
            rentalRequest.HetRentalRequestRotationList = RentalRequestHelper.CreateRotationList(rentalRequest, _context, _configuration);

            // check if we have an existing "In Progress" request
            // for the same Local Area and Equipment Type
            string tempStatus = RentalRequestHelper.RentalRequestStatus(rentalRequest, _context);

            statusId = StatusHelper.GetStatusId(tempStatus, "rentalRequestStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            rentalRequest.RentalRequestStatusTypeId = (int)statusId;

            if (item.HetRentalRequestAttachment != null &&
                item.HetRentalRequestAttachment.Count > 0)
            {
                HetRentalRequestAttachment attachment = new HetRentalRequestAttachment
                {
                    Attachment = item.HetRentalRequestAttachment.ElementAt(0).Attachment
                };

                rentalRequest.HetRentalRequestAttachment.Add(attachment);
            }

            // save the changes
            _context.HetRentalRequest.Add(rentalRequest);
            _context.SaveChanges();

            int id = rentalRequest.RentalRequestId;

            // retrieve updated rental request to return to ui
            return new ObjectResult(new HetsResponse(RentalRequestHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Cancels a rental request (if no equipment has been hired)
        /// </summary>
        /// <param name="id">id of RentalRequest to cancel</param>
        [HttpGet]
        [Route("{id}/cancel")]
        [SwaggerOperation("RentalRequestsIdCancelGet")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdCancelGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalRequest rentalRequest = _context.HetRentalRequest.AsNoTracking()
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(y => y.RentalAgreement)
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(y => y.Equipment)
                .Include(x => x.HetRentalRequestAttachment)
                .Include(x => x.HetHistory)
                .Include(x => x.RentalRequestStatusType)
                .Include(x => x.HetNote)
                .First(a => a.RentalRequestId == id);

            if (rentalRequest.HetRentalRequestRotationList != null &&
                rentalRequest.HetRentalRequestRotationList.Count > 0)
            {
                bool agreementExists = false;

                foreach (HetRentalRequestRotationList listItem in rentalRequest.HetRentalRequestRotationList)
                {
                    if (listItem.RentalAgreement != null && listItem.RentalAgreement.RentalAgreementId != 0)
                    {
                        agreementExists = true;
                        break; // agreement found
                    }
                }

                // cannot cancel - rental agreements exist
                if (agreementExists)
                {
                    return new ObjectResult(new HetsResponse("HETS-09", ErrorViewModel.GetDescription("HETS-09", _configuration)));
                }
            }

            if (rentalRequest.RentalRequestStatusType.RentalRequestStatusTypeCode
                .Equals(HetRentalRequest.StatusComplete, StringComparison.InvariantCulture))
            {
                // cannot cancel - rental request is complete
                return new ObjectResult(new HetsResponse("HETS-10", ErrorViewModel.GetDescription("HETS-10", _configuration)));
            }

            // remove (delete) rental request rotation list
            if (rentalRequest.HetRentalRequestRotationList != null)
            {
                foreach (HetRentalRequestRotationList rotationList in rentalRequest.HetRentalRequestRotationList)
                {
                    _context.HetRentalRequestRotationList.Remove(rotationList);
                }
            }

            // remove (delete) rental request attachments
            if (rentalRequest.HetRentalRequestAttachment != null)
            {
                foreach (HetRentalRequestAttachment attachment in rentalRequest.HetRentalRequestAttachment)
                {
                    _context.HetRentalRequestAttachment.Remove(attachment);
                }
            }

            // remove (delete) rental request attachments
            if (rentalRequest.HetDigitalFile != null)
            {
                foreach (HetDigitalFile attachment in rentalRequest.HetDigitalFile)
                {
                    _context.HetDigitalFile.Remove(attachment);
                }
            }

            // remove (delete) rental request notes
            if (rentalRequest.HetNote != null)
            {
                foreach (HetNote note in rentalRequest.HetNote)
                {
                    _context.HetNote.Remove(note);
                }
            }

            // remove (delete) rental request history
            if (rentalRequest.HetHistory != null)
            {
                foreach (HetHistory history in rentalRequest.HetHistory)
                {
                    _context.HetHistory.Remove(history);
                }
            }

            // remove (delete) request
            _context.HetRentalRequest.Remove(rentalRequest);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(rentalRequest));            
        }

        #region Search Rental Requests

        /// <summary>
        /// Search Rental Requests
        /// </summary>
        /// <remarks>Used for the rental request search page.</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="project">Searches equipmentAttachment type</param>
        /// <param name="status">Status</param>
        /// <param name="startDate">Inspection start date</param>
        /// <param name="endDate">Inspection end date</param>
        [HttpGet]
        [Route("search")]
        [SwaggerOperation("RentalRequestsSearchGet")]
        [SwaggerResponse(200, type: typeof(List<RentalRequestLite>))]
        public virtual IActionResult RentalRequestsSearchGet([FromQuery]string localAreas, [FromQuery]string project, [FromQuery]string status, [FromQuery]DateTime? startDate, [FromQuery]DateTime? endDate)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            IQueryable<HetRentalRequest> data = _context.HetRentalRequest.AsNoTracking()
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(y => y.EquipmentType)
                .Include(x => x.Project.PrimaryContact)
                .Include(x => x.RentalRequestStatusType)
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId));            

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.LocalArea.LocalAreaId));
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
                int? statusId = StatusHelper.GetStatusId(status, "rentalRequestStatus", _context);

                if (statusId != null)
                {
                    data = data.Where(x => x.RentalRequestStatusTypeId == statusId);
                }
            }

            // convert Rental Request Model to the "RentalRequestLite" Model
            List<RentalRequestLite> result = new List<RentalRequestLite>();

            foreach (HetRentalRequest item in data)
            {
                result.Add(RentalRequestHelper.ToLiteModel(item));
            }

            // return to the client            
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region Rental Request Rotation List

        /// <summary>
        /// Get rental request rotation list for the rental request
        /// </summary>
        /// <param name="id">id of RentalRequest to fetch</param>
        [HttpGet]
        [Route("{id}/rotationList")]
        [SwaggerOperation("RentalRequestsIdRotationListGet")]
        [SwaggerResponse(200, type: typeof(HetRentalRequest))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdRotationListIdGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get the scoring rules
            SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);

            return new ObjectResult(new HetsResponse(RentalRequestHelper.GetRecordWithRotationList(id, scoringRules, _context)));
        }
        
        /// <summary>
        /// Update a rental request rotation list record
        /// </summary>
        /// <remarks>Updates a rental request rotation list entry.  Side effect is the LocalAreaRotationList is also updated</remarks>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}/rentalRequestRotationList")]
        [SwaggerOperation("RentalRequestRotationListIdPut")]
        [SwaggerResponse(200, type: typeof(HetRentalRequestRotationList))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestIdRotationListIdPut([FromRoute]int id, [FromBody]HetRentalRequestRotationList item)
        {
            // not found 
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // check if we have the rental request that is In Progress
            exists = _context.HetRentalRequest
                .Any(a => a.RentalRequestId == id &&
                          a.RentalRequestStatusTypeId == statusId);

            // rental request must be "in progress"
            if (!exists) return new ObjectResult(new HetsResponse("HETS-06", ErrorViewModel.GetDescription("HETS-06", _configuration)));
                       
            // get rental request record
            HetRentalRequest request = _context.HetRentalRequest
                .Include(x => x.Project)
                    .ThenInclude(x => x.District)
                .Include(x => x.LocalArea)
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(x => x.Equipment)
                .First(a => a.RentalRequestId == id);

            // get rotation list record
            HetRentalRequestRotationList requestRotationList = _context.HetRentalRequestRotationList                
                .FirstOrDefault(a => a.RentalRequestRotationListId == item.RentalRequestRotationListId);

            // not found
            if (requestRotationList == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // update rotation list record
            int tempEquipmentId = item.Equipment.EquipmentId;

            requestRotationList.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            requestRotationList.EquipmentId = tempEquipmentId;
            requestRotationList.IsForceHire = item.IsForceHire;
            requestRotationList.AskedDateTime = DateTime.UtcNow;
            requestRotationList.Note = item.Note;
            requestRotationList.OfferRefusalReason = item.OfferRefusalReason;
            requestRotationList.OfferResponse = item.OfferResponse;
            requestRotationList.OfferResponseDatetime = item.OfferResponseDatetime;
            requestRotationList.WasAsked = item.WasAsked;
            requestRotationList.OfferResponseNote = item.OfferResponseNote;

            // do we need to create or modify a Rental Agreement?
            if (item.IsForceHire == true ||
                item.OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
            {
                // get rental agreement record
                HetRentalAgreement rentalAgreement = _context.HetRentalAgreement
                    .FirstOrDefault(a => a.RentalAgreementId == item.RentalAgreementId);

                // create rental agreement if it doesn't exist
                if (rentalAgreement == null)
                {
                    // generate the rental agreement number
                    string agreementNumber = RentalAgreementHelper.GetRentalAgreementNumber(item.Equipment, _context);

                    int? rateTypeId = StatusHelper.GetRatePeriodId(HetRatePeriodType.PeriodHourly, _context);
                    if (rateTypeId == null) return new ObjectResult(new HetsResponse("HETS-24", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                    rentalAgreement = new HetRentalAgreement
                    {
                        ProjectId = request.ProjectId,
                        DistrictId = request.Project.District.DistrictId,
                        EquipmentId = tempEquipmentId,
                        Number = agreementNumber,
                        RatePeriodTypeId = (int)rateTypeId,
                    };

                    // add overtime rates
                    List<HetProvincialRateType> overtime = _context.HetProvincialRateType.AsNoTracking()
                        .Where(x => x.Overtime)
                        .ToList();

                    // agreement overtime records (default overtime flag)            
                    foreach (HetProvincialRateType rate in overtime)
                    {
                        // add the rate
                        HetRentalAgreementRate newAgreementRate = new HetRentalAgreementRate
                        {
                            Comment = rate.Description,
                            ComponentName = rate.RateType,
                            Overtime = true,
                            Active = rate.Active,
                            IsIncludedInTotal = rate.IsIncludedInTotal,
                            Rate = rate.Rate
                        };

                        if (rentalAgreement.HetRentalAgreementRate == null)
                        {
                            rentalAgreement.HetRentalAgreementRate = new List<HetRentalAgreementRate>();
                        }

                        rentalAgreement.HetRentalAgreementRate.Add(newAgreementRate);
                    }

                    _context.HetRentalAgreement.Add(rentalAgreement);
                }

                int? statusIdAgreement = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
                if (statusIdAgreement == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                // update rental agreement
                rentalAgreement.RentalAgreementStatusTypeId = (int)statusIdAgreement;
                rentalAgreement.DatedOn = DateTime.UtcNow;
                rentalAgreement.EstimateHours = request.ExpectedHours;
                rentalAgreement.EstimateStartWork = request.ExpectedStartDate;
                rentalAgreement.RentalRequestId = request.RentalRequestId;
                rentalAgreement.RentalRequestRotationListId = requestRotationList.RentalRequestRotationListId;

                // have to save the agreement
                _context.SaveChanges();

                // relate the new rental agreement to the original rotation list record
                int tempRentalAgreementId = rentalAgreement.RentalAgreementId;
                requestRotationList.RentalAgreementId = tempRentalAgreementId;
                requestRotationList.RentalAgreement = rentalAgreement;
            }
          
            // can we "Complete" this rental request (if the Yes or Forced Hires = Request.EquipmentCount)
            int countOfYeses = 0;
            int equipmentRequestCount = request.EquipmentCount;

            foreach (HetRentalRequestRotationList rotationList in request.HetRentalRequestRotationList)
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
                int? statusIdComplete = StatusHelper.GetStatusId(HetRentalRequest.StatusComplete, "rentalRequestStatus", _context);
                if (statusIdComplete == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                request.RentalRequestStatusTypeId = (int)statusIdComplete;
                request.Status = "Complete";
                request.FirstOnRotationList = null;
            }

            // 1. get the number of blocks for this equipment type
            // 2. set which rotation list record is currently "active"
            int numberOfBlocks = EquipmentHelper.GetNumberOfBlocks(item.Equipment, _configuration);
            RentalRequestHelper.UpdateRotationList(request, numberOfBlocks, _context);

            // save the changes
            _context.SaveChanges();

            // get the scoring rules
            SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);

            return new ObjectResult(new HetsResponse(RentalRequestHelper.GetRecordWithRotationList(id, scoringRules, _context)));
        }
        
        #endregion

        #region Rental Request Attachments

        /// <summary>
        /// Get attachments associated with a rental request
        /// </summary>
        /// <remarks>Returns attachments for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch attachments for</param>
        [HttpGet]
        [Route("{id}/attachments")]
        [SwaggerOperation("RentalRequestsIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<HetDigitalFile>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdAttachmentsGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetRentalRequest equipment = _context.HetRentalRequest.AsNoTracking()
                .Include(x => x.HetDigitalFile)
                .First(a => a.RentalRequestId == id);

            // extract the attachments and update properties for UI
            List<HetDigitalFile> attachments = new List<HetDigitalFile>();

            foreach (HetDigitalFile attachment in equipment.HetDigitalFile)
            {
                if (attachment != null)
                {
                    attachment.FileSize = attachment.FileContents.Length;
                    attachment.LastUpdateTimestamp = attachment.AppLastUpdateTimestamp;
                    attachment.LastUpdateUserid = attachment.AppLastUpdateUserid;

                    attachments.Add(attachment);
                }
            }

            return new ObjectResult(new HetsResponse(attachments));
        }

        #endregion

        #region Rental Request History

        /// <summary>
        /// Get history associated with a rental request
        /// </summary>
        /// <remarks>Returns History for a particular RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        [HttpGet]
        [Route("{id}/history")]
        [SwaggerOperation("RentalRequestsIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HetHistory>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            return new ObjectResult(new HetsResponse(RentalRequestHelper.GetHistoryRecords(id, offset, limit, _context)));
        }

        /// <summary>
        /// Create history for a rental request
        /// </summary>
        /// <remarks>Add a History record to the RentalRequest</remarks>
        /// <param name="id">id of RentalRequest to add History for</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/history")]
        [SwaggerOperation("RentalRequestsIdHistoryPost")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdHistoryPost([FromRoute]int id, [FromBody]HetHistory item)
        {
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            if (exists)
            {
                HetHistory history = new HetHistory
                {
                    HistoryId = 0,
                    HistoryText = item.HistoryText,
                    CreatedDate = item.CreatedDate,
                    RentalRequestId = id
                };

                _context.HetHistory.Add(history);
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(RentalRequestHelper.GetHistoryRecords(id, null, null, _context)));
        }

        #endregion

        #region Rental Request Note Records

        /// <summary>
        /// Get note records associated with rental request
        /// </summary>
        /// <param name="id">id of Rental Request to fetch Notes for</param>
        [HttpGet]
        [Route("{id}/notes")]
        [SwaggerOperation("RentalRequestsIdNotesGet")]
        [SwaggerResponse(200, type: typeof(List<HetNote>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdNotesGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetRentalRequest request = _context.HetRentalRequest.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.RentalRequestId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in request.HetNote)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(notes));
        }

        /// <summary>
        /// Update or create a note associated with a rental request
        /// </summary>
        /// <remarks>Update a Rental Requests Notes</remarks>
        /// <param name="id">id of Rental Request to update Notes for</param>
        /// <param name="item">Rental Request Note</param>
        [HttpPost]
        [Route("{id}/note")]
        [SwaggerOperation("RentalRequestsIdNotePost")]
        [SwaggerResponse(200, type: typeof(HetNote))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestsIdNotePost([FromRoute]int id, [FromBody]HetNote item)
        {
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists || item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // add or update note
            if (item.NoteId > 0)
            {
                // get note
                HetNote note = _context.HetNote.FirstOrDefault(a => a.NoteId == item.NoteId);

                // not found
                if (note == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                note.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                note.Text = item.Text;
                note.IsNoLongerRelevant = item.IsNoLongerRelevant;
            }
            else  // add note
            {
                HetNote note = new HetNote
                {
                    RentalRequestId = id,
                    Text = item.Text,
                    IsNoLongerRelevant = item.IsNoLongerRelevant
                };

                _context.HetNote.Add(note);
            }

            _context.SaveChanges();

            // return updated note records
            HetRentalRequest request = _context.HetRentalRequest.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.RentalRequestId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in request.HetNote)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(notes));
        }
        
        #endregion          
    }
}
