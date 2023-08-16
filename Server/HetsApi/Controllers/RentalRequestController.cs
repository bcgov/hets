using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Entities;
using HetsData.Repositories;
using AutoMapper;
using HetsData.Dtos;
using HetsReport;
using HetsCommon;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Rental Request Controller
    /// </summary>
    [Route("api/rentalRequests")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalRequestController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IRentalRequestRepository _rentalRequestRepo;
        private readonly IMapper _mapper;
        private readonly HttpContext _httpContext;
        private readonly ILogger<RentalRequestController> _logger;

        public RentalRequestController(DbAppContext context, IConfiguration configuration, 
            IRentalRequestRepository rentalRequestRepo,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor, ILogger<RentalRequestController> logger)
        {
            _context = context;
            _configuration = configuration;
            _rentalRequestRepo = rentalRequestRepo;
            _mapper = mapper;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }

        /// <summary>
        /// Get rental request by id
        /// </summary>
        /// <param name="id">id of RentalRequest to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<RentalRequestDto> RentalRequestsIdGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            return new ObjectResult(new HetsResponse(_rentalRequestRepo.GetRecord(id)));
        }

        /// <summary>
        /// Get no project rental requests
        /// </summary>
        [HttpGet]
        [Route("noProject")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalRequestDto>> NoProjectsGet()
        {
            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            int? statusIdInProgress = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", _context);
            if (statusIdInProgress == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            List<HetRentalRequest> requests = _context.HetRentalRequests.AsNoTracking()
                .Include(x => x.LocalArea.ServiceArea.District)
                .Include(x => x.DistrictEquipmentType)
                .Where(x => x.LocalArea.ServiceArea.DistrictId == districtId &&
                            x.RentalRequestStatusTypeId == statusIdInProgress &&
                            x.ProjectId == null)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<RentalRequestDto>>(requests)));
        }

        /// <summary>
        /// Update rental request
        /// </summary>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalRequestDto> RentalRequestsIdPut([FromRoute]int id, [FromBody]RentalRequestDto item)
        {
            if (item == null || id != item.RentalRequestId)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalRequest rentalRequest = _context.HetRentalRequests
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(c => c.PrimaryContact)
                .Include(x => x.HetRentalRequestAttachments)
                .Include(x => x.DistrictEquipmentType)
                .Include(x => x.HetRentalRequestRotationLists)
                    .ThenInclude(y => y.Equipment)
                .First(a => a.RentalRequestId == id);

            // need to check if we are going over the "count" and close this request
            int hiredCount = 0;

            foreach (HetRentalRequestRotationList equipment in rentalRequest.HetRentalRequestRotationLists)
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
                return new BadRequestObjectResult(new HetsResponse("HETS-07", ErrorViewModel.GetDescription("HETS-07", _configuration)));
            }

            // if the number of hired records is now "over the count" - then close
            if (hiredCount >= item.EquipmentCount)
            {
                int? statusIdComplete = StatusHelper.GetStatusId(HetRentalRequest.StatusComplete, "rentalRequestStatus", _context);
                if (statusIdComplete == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                item.RentalRequestStatusTypeId = (int)statusIdComplete;
                item.Status = "Complete";
            }

            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalRequestStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // update rental request
            rentalRequest.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            rentalRequest.RentalRequestStatusTypeId = (int)statusId;
            rentalRequest.EquipmentCount = item.EquipmentCount;
            rentalRequest.ExpectedEndDate = item.ExpectedEndDate;
            rentalRequest.ExpectedStartDate = item.ExpectedStartDate;
            rentalRequest.ExpectedHours = item.ExpectedHours;

            // do we have any attachments (only a single string is ever stored)
            if (item.RentalRequestAttachments != null &&
                item.RentalRequestAttachments.Count > 0)
            {
                if (rentalRequest.HetRentalRequestAttachments == null)
                {
                    rentalRequest.HetRentalRequestAttachments = new List<HetRentalRequestAttachment>();
                }

                HetRentalRequestAttachment attachment = new HetRentalRequestAttachment
                {
                    Attachment = item.RentalRequestAttachments[0].Attachment
                };

                if (rentalRequest.HetRentalRequestAttachments.Count > 0)
                {
                    rentalRequest.HetRentalRequestAttachments.ElementAt(0).Attachment = attachment.Attachment;
                }
                else
                {
                    rentalRequest.HetRentalRequestAttachments.Add(attachment);
                }
            }

            // save the changes
            _context.SaveChanges();

            // retrieve updated rental request to return to ui
            return new ObjectResult(new HetsResponse(_rentalRequestRepo.GetRecord(id)));
        }

        /// <summary>
        /// Create rental request
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalRequestDto> RentalRequestsPost([FromBody] RentalRequestDto item)
        {
            return CreateRentalRequest(item);
        }

        /// <summary>
        /// Create rental request - view only (no project)
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("viewOnly")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalRequestDto> RentalRequestsViewOnlyPost([FromBody] RentalRequestDto item)
        {
            return CreateRentalRequest(item, true);
        }

        private ActionResult<RentalRequestDto> CreateRentalRequest(RentalRequestDto item, bool noProject = false)
        {
            // not found
            if (item == null) return new BadRequestObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));

            // check if we have an existing rental request for the same
            // local area and equipment type - if so - throw an error
            // Per discussion with the business (19 Jan 2018):
            //    * Don't create a record as New if another Request exists
            //    * Simply give the user an error and not allow the new request
            //
            // Note: leaving the "New" code in place in case this changes in the future
            int? statusIdInProgress = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", _context);
            if (statusIdInProgress == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            List<HetRentalRequest> requests = _context.HetRentalRequests
                .Where(x => x.DistrictEquipmentTypeId == item.DistrictEquipmentType.DistrictEquipmentTypeId &&
                            x.LocalAreaId == item.LocalArea.LocalAreaId &&
                            x.RentalRequestStatusTypeId == statusIdInProgress)
                .ToList();

            // in Progress Rental Request already exists
            if (requests.Count > 0)
            {
                int quantity = requests[0].EquipmentCount;
                int hiredCount = 0;

                foreach (HetRentalRequestRotationList equipment in requests[0].HetRentalRequestRotationLists)
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

                // ...Currently {0} of {1} requested equipment have been hired....
                string message = string.Format(ErrorViewModel.GetDescription("HETS-28", _configuration), hiredCount, quantity);
                return new BadRequestObjectResult(new HetsResponse("HETS-28", message));
            }

            // create new rental request
            HetRentalRequest rentalRequest = new HetRentalRequest
            {
                LocalAreaId = item.LocalArea.LocalAreaId,
                DistrictEquipmentTypeId = item.DistrictEquipmentType.DistrictEquipmentTypeId,
                RentalRequestStatusTypeId = (int)statusIdInProgress,
                EquipmentCount = item.EquipmentCount,
                ExpectedEndDate = item.ExpectedEndDate,
                ExpectedStartDate = item.ExpectedStartDate,
                ExpectedHours = item.ExpectedHours
            };

            // is this a "project-less" request? - can't be hired from
            if (!noProject)
            {
                rentalRequest.ProjectId = item.Project.ProjectId;
            }

            // build new list
            try
            {
                rentalRequest = RentalRequestHelper.CreateRotationList(rentalRequest, _context, _configuration, _mapper, (errMessage, ex) => {
                    _logger.LogError(errMessage);
                    _logger.LogError(ex.ToString());
                });
            }
            catch (Exception e)
            {
                // check if this a "no available equipment exception"
                if (e.Message == "HETS-42")
                {
                    return new NotFoundObjectResult(new HetsResponse("HETS-42", ErrorViewModel.GetDescription("HETS-42", _configuration)));
                }

                _logger.LogError($"CreateRentalRequest exception: {e.ToString()}");
                throw;
            }

            // check if we have an existing "In Progress" request
            // for the same Local Area and Equipment Type
            string tempStatus = RentalRequestHelper.RentalRequestStatus(rentalRequest, _context);

            statusIdInProgress = StatusHelper.GetStatusId(tempStatus, "rentalRequestStatus", _context);
            if (statusIdInProgress == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            rentalRequest.RentalRequestStatusTypeId = (int)statusIdInProgress;

            if (item.RentalRequestAttachments != null &&
                item.RentalRequestAttachments.Count > 0)
            {
                HetRentalRequestAttachment attachment = new HetRentalRequestAttachment
                {
                    Attachment = item.RentalRequestAttachments.ElementAt(0).Attachment
                };

                rentalRequest.HetRentalRequestAttachments.Add(attachment);
            }

            // save the changes
            _context.HetRentalRequests.Add(rentalRequest);
            _context.SaveChanges();

            // retrieve updated rental request to return to ui
            return new ObjectResult(new HetsResponse(_rentalRequestRepo.GetRecord(rentalRequest.RentalRequestId)));
        }

        /// <summary>
        /// Cancels a rental request (if no equipment has been hired)
        /// </summary>
        /// <param name="id">id of RentalRequest to cancel</param>
        [HttpGet]
        [Route("{id}/cancel")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<RentalRequestDto> RentalRequestsIdCancelGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalRequest rentalRequest = _context.HetRentalRequests.AsNoTracking()
                .Include(x => x.HetRentalRequestRotationLists)
                    .ThenInclude(y => y.RentalAgreement)
                .Include(x => x.HetRentalRequestRotationLists)
                    .ThenInclude(y => y.Equipment)
                .Include(x => x.HetRentalRequestSeniorityLists)
                .Include(x => x.HetRentalRequestAttachments)
                .Include(x => x.HetHistories)
                .Include(x => x.RentalRequestStatusType)
                .Include(x => x.HetNotes)
                .First(a => a.RentalRequestId == id);

            if (rentalRequest.HetRentalRequestRotationLists != null &&
                rentalRequest.HetRentalRequestRotationLists.Count > 0)
            {
                bool agreementExists = false;

                foreach (HetRentalRequestRotationList listItem in rentalRequest.HetRentalRequestRotationLists)
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
                    return new BadRequestObjectResult(new HetsResponse("HETS-09", ErrorViewModel.GetDescription("HETS-09", _configuration)));
                }
            }

            if (rentalRequest.RentalRequestStatusType.RentalRequestStatusTypeCode
                .Equals(HetRentalRequest.StatusComplete, StringComparison.InvariantCulture))
            {
                // cannot cancel - rental request is complete
                return new BadRequestObjectResult(new HetsResponse("HETS-10", ErrorViewModel.GetDescription("HETS-10", _configuration)));
            }

            // remove (delete) rental request rotation list
            if (rentalRequest.HetRentalRequestRotationLists != null)
            {
                foreach (HetRentalRequestRotationList rotationList in rentalRequest.HetRentalRequestRotationLists)
                {
                    _context.HetRentalRequestRotationLists.Remove(rotationList);
                }
            }

            // remove (delete) rental request attachments
            if (rentalRequest.HetRentalRequestAttachments != null)
            {
                foreach (HetRentalRequestAttachment attachment in rentalRequest.HetRentalRequestAttachments)
                {
                    _context.HetRentalRequestAttachments.Remove(attachment);
                }
            }

            // remove (delete) rental request attachments
            if (rentalRequest.HetDigitalFiles != null)
            {
                foreach (HetDigitalFile attachment in rentalRequest.HetDigitalFiles)
                {
                    _context.HetDigitalFiles.Remove(attachment);
                }
            }

            // remove (delete) rental request notes
            if (rentalRequest.HetNotes != null)
            {
                foreach (HetNote note in rentalRequest.HetNotes)
                {
                    _context.HetNotes.Remove(note);
                }
            }

            // remove (delete) rental request history
            if (rentalRequest.HetHistories != null)
            {
                foreach (HetHistory history in rentalRequest.HetHistories)
                {
                    _context.HetHistories.Remove(history);
                }
            }

            if (rentalRequest.HetRentalRequestSeniorityLists != null)
            {
                foreach(var list in rentalRequest.HetRentalRequestSeniorityLists)
                {
                    _context.HetRentalRequestSeniorityLists.Remove(list);
                }
            }

            // remove (delete) request
            _context.HetRentalRequests.Remove(rentalRequest);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<RentalRequestDto>(rentalRequest)));
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
        public virtual ActionResult<List<RentalRequestLite>> RentalRequestsSearchGet([FromQuery]string localAreas, [FromQuery]string project, [FromQuery]string status, [FromQuery]DateTime? startDate, [FromQuery]DateTime? endDate)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            IQueryable<HetRentalRequest> data = _context.HetRentalRequests.AsNoTracking()
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(y => y.EquipmentType)
                .Include(x => x.Project.PrimaryContact)
                .Include(x => x.RentalRequestStatusType)
                .Include(x => x.HetRentalRequestRotationLists)
                .OrderByDescending(x => x.AppCreateTimestamp)
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId));

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.LocalArea.LocalAreaId));
            }

            if (project != null)
            {
                data = data.Where(x => x.Project.Name.ToLower().Contains(project.ToLower()));
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
                result.Add(_rentalRequestRepo.ToLiteModel(item));
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
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<RentalRequestDto> RentalRequestsIdRotationListIdGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get the scoring rules
            SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration, (errMessage, ex) => {
                _logger.LogError(errMessage);
                _logger.LogError(ex.ToString());
            });

            return new ObjectResult(new HetsResponse(_rentalRequestRepo.GetRecordWithRotationList(id, scoringRules)));
        }

        /// <summary>
        /// Update a rental request rotation list record
        /// </summary>
        /// <remarks>Updates a rental request rotation list entry.  Side effect is the LocalAreaRotationList is also updated</remarks>
        /// <param name="id">id of RentalRequest to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}/rentalRequestRotationList")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalRequestDto> RentalRequestIdRotationListIdPut([FromRoute]int id, [FromBody]RentalRequestRotationListDto item)
        {
            // not found
            if (item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", _context);
            if (statusId == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // check if we have the rental request that is In Progress
            exists = _context.HetRentalRequests
                .Any(a => a.RentalRequestId == id &&
                          a.RentalRequestStatusTypeId == statusId);

            // rental request must be "in progress"
            if (!exists) return new BadRequestObjectResult(new HetsResponse("HETS-06", ErrorViewModel.GetDescription("HETS-06", _configuration)));

            // get rental request record
            HetRentalRequest request = _context.HetRentalRequests
                .Include(x => x.Project)
                    .ThenInclude(x => x.District)
                .Include(x => x.LocalArea)
                .Include(x => x.HetRentalRequestRotationLists)
                    .ThenInclude(x => x.Equipment)
                .First(a => a.RentalRequestId == id);

            // get rotation list record
            HetRentalRequestRotationList requestRotationList = _context.HetRentalRequestRotationLists
                .FirstOrDefault(a => a.RentalRequestRotationListId == item.RentalRequestRotationListId);

            // not found
            if (requestRotationList == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
                HetRentalAgreement rentalAgreement = _context.HetRentalAgreements
                    .FirstOrDefault(a => a.RentalAgreementId == item.RentalAgreementId);

                // create rental agreement if it doesn't exist
                if (rentalAgreement == null)
                {
                    // generate the rental agreement number
                    string agreementNumber = RentalAgreementHelper.GetRentalAgreementNumber(item.Equipment?.LocalAreaId, _context);

                    // get user info - agreement city
                    CurrentUserDto user = UserAccountHelper.GetUser(_context, _httpContext);
                    string agreementCity = user.AgreementCity;

                    int? rateTypeId = StatusHelper.GetRatePeriodId(HetRatePeriodType.PeriodHourly, _context);
                    if (rateTypeId == null) return new NotFoundObjectResult(new HetsResponse("HETS-24", ErrorViewModel.GetDescription("HETS-24", _configuration)));

                    rentalAgreement = new HetRentalAgreement
                    {
                        ProjectId = request.ProjectId,
                        DistrictId = request.Project.District.DistrictId,
                        EquipmentId = tempEquipmentId,
                        Number = agreementNumber,
                        RatePeriodTypeId = (int)rateTypeId,
                        AgreementCity = agreementCity
                    };

                    // add overtime rates
                    List<HetProvincialRateType> overtime = _context.HetProvincialRateTypes.AsNoTracking()
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

                        if (rentalAgreement.HetRentalAgreementRates == null)
                        {
                            rentalAgreement.HetRentalAgreementRates = new List<HetRentalAgreementRate>();
                        }

                        rentalAgreement.HetRentalAgreementRates.Add(newAgreementRate);
                    }

                    _context.HetRentalAgreements.Add(rentalAgreement);
                }

                int? statusIdAgreement = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
                if (statusIdAgreement == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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

            foreach (HetRentalRequestRotationList rotationList in request.HetRentalRequestRotationLists)
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
                if (statusIdComplete == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                request.RentalRequestStatusTypeId = (int)statusIdComplete;
                request.Status = "Complete";
                request.FirstOnRotationList = null;
            }

            RentalRequestHelper.UpdateRotationList(request);

            // save the changes
            _context.SaveChanges();

            // get the scoring rules
            SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration, (errMessage, ex) => {
                _logger.LogError(errMessage);
                _logger.LogError(ex.ToString());
            });

            return new ObjectResult(new HetsResponse(_rentalRequestRepo.GetRecordWithRotationList(id, scoringRules)));
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
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<DigitalFileDto>> RentalRequestsIdAttachmentsGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetRentalRequest equipment = _context.HetRentalRequests.AsNoTracking()
                .Include(x => x.HetDigitalFiles)
                .First(a => a.RentalRequestId == id);

            // extract the attachments and update properties for UI
            List<HetDigitalFile> attachments = new List<HetDigitalFile>();

            foreach (HetDigitalFile attachment in equipment.HetDigitalFiles)
            {
                if (attachment != null)
                {
                    attachment.FileSize = attachment.FileContents.Length;
                    attachment.LastUpdateTimestamp = attachment.AppLastUpdateTimestamp;
                    attachment.LastUpdateUserid = attachment.AppLastUpdateUserid;
                    attachment.UserName = UserHelper.GetUserName(attachment.LastUpdateUserid, _context);
                    attachments.Add(attachment);
                }
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<DigitalFileDto>>(attachments)));
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
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<History>> RentalRequestsIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<History>> RentalRequestsIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            if (exists)
            {
                HetHistory history = new HetHistory
                {
                    HistoryId = 0,
                    HistoryText = item.HistoryText,
                    CreatedDate = DateTime.UtcNow,
                    RentalRequestId = id
                };

                _context.HetHistories.Add(history);
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
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<NoteDto>> RentalRequestsIdNotesGet([FromRoute]int id)
        {
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetRentalRequest request = _context.HetRentalRequests.AsNoTracking()
                .Include(x => x.HetNotes)
                .First(x => x.RentalRequestId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in request.HetNotes)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<NoteDto>>(notes)));
        }

        /// <summary>
        /// Update or create a note associated with a rental request
        /// </summary>
        /// <remarks>Update a Rental Requests Notes</remarks>
        /// <param name="id">id of Rental Request to update Notes for</param>
        /// <param name="item">Rental Request Note</param>
        [HttpPost]
        [Route("{id}/note")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<NoteDto>> RentalRequestsIdNotePost([FromRoute]int id, [FromBody]NoteDto item)
        {
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            // not found
            if (!exists || item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add or update note
            if (item.NoteId > 0)
            {
                // get note
                HetNote note = _context.HetNotes.FirstOrDefault(a => a.NoteId == item.NoteId);

                // not found
                if (note == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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

                _context.HetNotes.Add(note);
            }

            _context.SaveChanges();

            // return updated note records
            HetRentalRequest request = _context.HetRentalRequests.AsNoTracking()
                .Include(x => x.HetNotes)
                .First(x => x.RentalRequestId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in request.HetNotes)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<NoteDto>>(notes)));
        }

        #endregion

        #region Rental Request Hiring Report

        /// <summary>
        /// Rental Request Hiring Report
        /// </summary>
        /// <remarks>Used for the rental request search page.</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="projects">Projects (comma separated list of id numbers)</param>
        /// <param name="owners">Owners (comma separated list of id numbers)</param>
        /// <param name="equipment">Equipment (comma separated list of id numbers)</param>
        [HttpGet]
        [Route("hireReport")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalRequestHires>> RentalRequestsHiresGet([FromQuery]string localAreas, [FromQuery]string projects,
            [FromQuery]string owners, [FromQuery]string equipment)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] projectArray = ArrayHelper.ParseIntArray(projects);
            int?[] ownerArray = ArrayHelper.ParseIntArray(owners);
            int?[] equipmentArray = ArrayHelper.ParseIntArray(equipment);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get fiscal year
            HetDistrictStatus district = _context.HetDistrictStatuses.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId == districtId);

            if (district?.CurrentFiscalYear == null) 
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-30", ErrorViewModel.GetDescription("HETS-30", _configuration)));

            int fiscalYear = (int)district.CurrentFiscalYear; // status table uses the start of the year
            DateTime fiscalStart = DateUtils.ConvertPacificToUtcTime(
                new DateTime(fiscalYear, 3, 31, 0, 0, 0)); // look for all records AFTER the 31st

            IQueryable<HetRentalRequestRotationList> data = _context.HetRentalRequestRotationLists.AsNoTracking()
                .Include(x => x.RentalRequest)
                    .ThenInclude(y => y.LocalArea)
                        .ThenInclude(z => z.ServiceArea)
                .Include(x => x.RentalRequest)
                    .ThenInclude(y => y.Project)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Where(x => 
                    x.RentalRequest.LocalArea.ServiceArea.DistrictId.Equals(districtId)
                    && x.AskedDateTime > fiscalStart 
                    && (x.IsForceHire == true || x.OfferResponse.ToLower() == "no"));

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.RentalRequest.LocalAreaId));
            }

            if (projectArray != null && projectArray.Length > 0)
            {
                data = data.Where(x => projectArray.Contains(x.RentalRequest.ProjectId));
            }

            if (ownerArray != null && ownerArray.Length > 0)
            {
                data = data.Where(x => ownerArray.Contains(x.Equipment.OwnerId));
            }

            if (equipmentArray != null && equipmentArray.Length > 0)
            {
                data = data.Where(x => equipmentArray.Contains(x.EquipmentId));
            }

            // convert Rental Request Model to the "RentalRequestHires" Model
            List<RentalRequestHires> result = new List<RentalRequestHires>();
            
            var items = data.ToList();
            foreach (HetRentalRequestRotationList item in items)
            {
                HetUser user = _context.HetUsers.AsNoTracking()
                    .FirstOrDefault(x => x.SmUserId.ToUpper() == item.AppCreateUserid.ToUpper());

                result.Add(RentalRequestHelper.ToHiresModel(item, user));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        [HttpGet]
        [Route("{id}/senioritylist")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult GetSeniorityList(int id, bool counterCopy = false)
        {
            var request = _context.HetRentalRequests
                .AsNoTracking()
                .Include(x => x.LocalArea)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(x => x.EquipmentType)
                .FirstOrDefault(a => a.RentalRequestId == id);

            if (request == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            var fiscalYear = request.FiscalYear;
            var fiscalStart = DateUtils.ConvertPacificToUtcTime(
                new DateTime(fiscalYear - 1, 4, 1, 0, 0, 0));

            var yearMinus1 = $"{fiscalYear - 2}/{fiscalYear - 1}";
            var yearMinus2 = $"{fiscalYear - 3}/{fiscalYear - 2}";
            var yearMinus3 = $"{fiscalYear - 4}/{fiscalYear - 3}";

            var seniorityList = new SeniorityListReportViewModel();
            seniorityList.Classification = $"23010-22/{(fiscalYear - 1).ToString().Substring(2, 2)}-{fiscalYear.ToString().Substring(2, 2)}";
            seniorityList.GeneratedOn = $"{DateUtils.ConvertUtcToPacificTime(request.AppCreateTimestamp):dd-MM-yyyy H:mm:ss}";

            var scoringRules = new SeniorityScoringRules(_configuration, (errMessage, ex) => {
                _logger.LogError(errMessage);
                _logger.LogError(ex.ToString());
            });
            var numberOfBlocks = request.DistrictEquipmentType.EquipmentType.IsDumpTruck
                ? scoringRules.GetTotalBlocks("DumpTruck") + 1
                : scoringRules.GetTotalBlocks() + 1;

            var listRecord = new SeniorityListRecord
            {
                LocalAreaName = request.LocalArea.Name,
                DistrictEquipmentTypeName = request.DistrictEquipmentType.DistrictEquipmentName,
                YearMinus1 = yearMinus1,
                YearMinus2 = yearMinus2,
                YearMinus3 = yearMinus3,
                SeniorityList = new List<SeniorityViewModel>()
            };

            seniorityList.SeniorityListRecords.Add(listRecord);

            var equipments = _context.HetRentalRequestSeniorityLists
                .AsNoTracking()
                .Include(x => x.Owner)
                .Where(x => x.RentalRequestId == id)
                .OrderBy(x => x.BlockNumber)
                .ThenBy(x => x.NumberInBlock);

            foreach (var equipment in equipments)
            {
                listRecord.SeniorityList.Add(SeniorityListHelper.ToSeniorityViewModel(equipment, numberOfBlocks));
            }

            string documentName = $"SeniorityList-{DateTime.Now:yyyy-MM-dd}{(counterCopy ? "-(CounterCopy)" : "")}.docx";
            byte[] document = SeniorityList.GetSeniorityList(seniorityList, documentName, counterCopy, (errMessage, ex) => {
                _logger.LogError(errMessage);
                _logger.LogError(ex.ToString());
            });

            // return document
            FileContentResult result = new FileContentResult(document, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = documentName
            };

            Response.Headers.Add("Content-Disposition", "inline; filename=" + documentName);

            return result;
        }
    }
}
