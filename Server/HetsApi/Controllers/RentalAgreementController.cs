using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
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
using HetsReport;
using HetsData.Dtos;
using HetsData.Repositories;
using AutoMapper;
using HetsCommon;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Rental Agreement Controller
    /// </summary>
    [Route("/api/rentalAgreements")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class RentalAgreementController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly IRentalAgreementRepository _rentalAgreementRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<RentalAgreementController> _logger;

        public RentalAgreementController(DbAppContext context, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IRentalAgreementRepository rentalAgreementRepo,
            IMapper mapper,
            ILogger<RentalAgreementController> logger)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _rentalAgreementRepo = rentalAgreementRepo;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get rental agreement by id
        /// </summary>
        /// <param name="id">id of RentalAgreement to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<RentalAgreementDto> RentalAgreementsIdGet([FromRoute] int id)
        {
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRecord(id)));
        }

        /// <summary>
        /// Update rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementDto> RentalAgreementsIdPut([FromRoute] int id, [FromBody] RentalAgreementDto item)
        {
            if (item == null || id != item.RentalAgreementId)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreement agreement = _context.HetRentalAgreements
                .Include(a => a.HetRentalAgreementRates)
                .First(a => a.RentalAgreementId == id);

            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get overtime records
            List<HetProvincialRateType> overtime = _context.HetProvincialRateTypes.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            // get rate period type for the agreement
            int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);

            if (rateTypeId == null)
            {
                throw new DataException("Rate Period Id cannot be null");
            }

            string city = item.AgreementCity;

            if (!string.IsNullOrEmpty(city))
            {
                city = city.Trim();
            }

            // update the agreement record
            agreement.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            agreement.DatedOn = item.DatedOn;
            agreement.AgreementCity = city;
            agreement.EquipmentRate = item.EquipmentRate;
            agreement.EstimateHours = item.EstimateHours;
            agreement.EstimateStartWork = item.EstimateStartWork;
            agreement.Note = item.Note;
            agreement.Number = item.Number;
            agreement.RateComment = item.RateComment;
            agreement.RatePeriod = item.RatePeriod;
            agreement.RatePeriodTypeId = (int)rateTypeId;
            agreement.RentalAgreementStatusTypeId = (int)statusId;
            agreement.ProjectId = item.ProjectId;
            agreement.EquipmentId = item.EquipmentId;

            // update the rate period for all included rates and attachments
            foreach (HetRentalAgreementRate agreementRate in agreement.HetRentalAgreementRates.Where(x => !(x.Overtime ?? false) && x.IsIncludedInTotal))
            {
                agreementRate.RatePeriod = agreement.RatePeriod;
                agreementRate.RatePeriodTypeId = agreement.RatePeriodTypeId;
            }

            // update the agreement overtime records (default overtime flag)
            if (item.OvertimeRates != null)
            {
                foreach (var rate in item.OvertimeRates)
                {
                    bool found = false;

                    foreach (HetRentalAgreementRate agreementRate in agreement.HetRentalAgreementRates)
                    {
                        if (agreementRate.RentalAgreementRateId == rate.RentalAgreementRateId)
                        {
                            agreementRate.ConcurrencyControlNumber = rate.ConcurrencyControlNumber;
                            agreementRate.Comment = rate.Comment;
                            agreementRate.Overtime = true;
                            agreementRate.Active = rate.Active;
                            agreementRate.IsIncludedInTotal = rate.IsIncludedInTotal;
                            agreementRate.Rate = rate.Rate;

                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        // add the rate
                        HetRentalAgreementRate newAgreementRate = new HetRentalAgreementRate
                        {
                            ConcurrencyControlNumber = rate.ConcurrencyControlNumber,
                            Comment = rate.Comment,
                            ComponentName = rate.ComponentName,
                            Overtime = true,
                            Active = true,
                            IsIncludedInTotal = rate.IsIncludedInTotal,
                            Rate = rate.Rate
                        };

                        HetProvincialRateType overtimeRate = overtime.FirstOrDefault(x => x.Description == rate.Comment);

                        if (overtimeRate != null)
                        {
                            newAgreementRate.ComponentName = overtimeRate.RateType;
                        }

                        if (agreement.HetRentalAgreementRates == null)
                        {
                            agreement.HetRentalAgreementRates = new List<HetRentalAgreementRate>();
                        }

                        agreement.HetRentalAgreementRates.Add(newAgreementRate);
                    }
                }
            }

            // save the changes
            _context.SaveChanges();

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRecord(id)));
        }

        /// <summary>
        /// Create rental agreement
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementDto> RentalAgreementsPost([FromBody] RentalAgreementDto item)
        {
            // not found
            if (item == null) return new BadRequestObjectResult(new HetsResponse("HETS-04", ErrorViewModel.GetDescription("HETS-04", _configuration)));

            // set the rate period type id
            int? rateTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context);

            if (rateTypeId == null)
            {
                throw new DataException("Rate Period Id cannot be null");
            }

            // get overtime records
            List<HetProvincialRateType> overtime = _context.HetProvincialRateTypes.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            // get status for new agreement
            int? statusId = StatusHelper.GetStatusId(item.Status, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get user info - agreement city
            CurrentUserDto user = UserAccountHelper.GetUser(_context, _httpContext);
            string agreementCity = user.AgreementCity;

            // create agreement
            HetRentalAgreement agreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(item.Equipment?.LocalAreaId, _context),
                DatedOn = item.DatedOn,
                AgreementCity = agreementCity,
                EquipmentRate = item.EquipmentRate,
                EstimateHours = item.EstimateHours,
                EstimateStartWork = item.EstimateStartWork,
                Note = item.Note,
                RateComment = item.RateComment,
                RatePeriodTypeId = (int)rateTypeId,
                RentalAgreementStatusTypeId = (int)statusId,
                EquipmentId = item.EquipmentId,
                ProjectId = item.ProjectId
            };

            // agreement overtime records (default overtime flag)
            if (item.OvertimeRates != null)
            {
                foreach (var rate in item.OvertimeRates)
                {
                    // add the rate
                    HetRentalAgreementRate newAgreementRate = new HetRentalAgreementRate
                    {
                        ConcurrencyControlNumber = rate.ConcurrencyControlNumber,
                        Comment = rate.Comment,
                        ComponentName = rate.ComponentName,
                        Overtime = true,
                        Active = true,
                        IsIncludedInTotal = rate.IsIncludedInTotal,
                        Rate = rate.Rate
                    };

                    HetProvincialRateType overtimeRate = overtime.FirstOrDefault(x => x.Description == rate.Comment);

                    if (overtimeRate != null)
                    {
                        newAgreementRate.ComponentName = overtimeRate.RateType;
                    }

                    if (agreement.HetRentalAgreementRates == null)
                    {
                        agreement.HetRentalAgreementRates = new List<HetRentalAgreementRate>();
                    }

                    agreement.HetRentalAgreementRates.Add(newAgreementRate);
                }
            }

            // save the changes
            _context.SaveChanges();

            int id = agreement.RentalAgreementId;

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRecord(id)));
        }

        /// <summary>
        /// Release (terminate) a rental agreement
        /// </summary>
        /// <param name="id">id of RentalAgreement to release</param>
        [HttpPost]
        [Route("{id}/release")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementDto> RentalAgreementsIdReleasePost([FromRoute] int id)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetRentalAgreement agreement = _context.HetRentalAgreements.First(a => a.RentalAgreementId == id);

            // release (terminate) rental agreement
            int? statusIdComplete = StatusHelper.GetStatusId(HetRentalAgreement.StatusComplete, "rentalAgreementStatus", _context);
            if (statusIdComplete == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            agreement.RentalAgreementStatusTypeId = (int)statusIdComplete;
            agreement.Status = "Complete";

            // save the changes
            _context.SaveChanges();

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRecord(id)));
        }

        #region Rental Agreement Document

        /// <summary>
        /// Get an OpenXML version of a rental agreement
        /// </summary>
        /// <remarks>Returns an OpenXML version of the specified rental agreement</remarks>
        /// <param name="id">id of RentalAgreement to get</param>
        [HttpGet]
        [Route("{id}/doc")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalAgreementsIdDocGet([FromRoute] int id)
        {
            // get user info - agreement city
            CurrentUserDto user = UserAccountHelper.GetUser(_context, _httpContext);
            string agreementCity = user.AgreementCity;

            HetRentalAgreement rentalAgreement = _context.HetRentalAgreements.AsNoTracking()
                .Include(x => x.RatePeriodType)
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                        .ThenInclude(z => z.PrimaryContact)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.HetEquipmentAttachments)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(p => p.District.Region)
                .Include(x => x.HetRentalAgreementConditions)
                .Include(x => x.HetRentalAgreementRates)
                    .ThenInclude(x => x.RatePeriodType)
                .FirstOrDefault(a => a.RentalAgreementId == id);

            if (rentalAgreement == null) 
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // construct the view model
            RentalAgreementDocViewModel reportModel = _rentalAgreementRepo
                .GetRentalAgreementReportModel(_mapper.Map<RentalAgreementDto>(rentalAgreement), agreementCity);

            string ownerName = rentalAgreement.Equipment?.Owner?.OrganizationName?.Trim().ToLower();
            ownerName = CleanName(ownerName);
            ownerName = ownerName.Replace(" ", "");
            ownerName = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(ownerName);
            string fileName = rentalAgreement.Number + "_" + ownerName;

            // convert to open xml document
            string documentName = $"{fileName}.docx";
            byte[] document = RentalAgreement.GetRentalAgreement(reportModel, documentName, (errMessage, ex) => {
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

        private static string CleanName(string name)
        {
            if (name == null) return "";

            name = name.Replace("'", "");
            name = name.Replace("<", "");
            name = name.Replace(">", "");
            name = name.Replace("\"", "");
            name = name.Replace("|", "");
            name = name.Replace("?", "");
            name = name.Replace("*", "");
            name = name.Replace(":", "");
            name = name.Replace("/", "");
            name = name.Replace("\\", "");

            return name;
        }

        #endregion

        #region Rental Agreement Time Records

        /// <summary>
        /// Get time records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreements Time Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Time Records for</param>
        [HttpGet]
        [Route("{id}/timeRecords")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<TimeRecordLite> RentalAgreementsIdTimeRecordsGet(
            [FromRoute] int id)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // return time records
            return new ObjectResult(
                new HetsResponse(_rentalAgreementRepo.GetTimeRecords(id, districtId)));
        }

        /// <summary>
        /// Add or update a rental agreement time record
        /// </summary>
        /// <remarks>Adds Rental Agreement Time Records</remarks>
        /// <param name="id">id of Rental Agreement to add a time record for</param>
        /// <param name="item">Adds to Rental Agreement Time Records</param>
        [HttpPost]
        [Route("{id}/timeRecord")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<TimeRecordLite> RentalAgreementsIdTimeRecordsPost(
            [FromRoute] int id, 
            [FromBody] TimeRecordDto item)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || item == null)
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // set the time period type id
            int? timePeriodTypeId = StatusHelper.GetTimePeriodId(item.TimePeriod, _context) 
                ?? throw new DataException("Time Period Id cannot be null");

            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // add or update time record
            if (item.TimeRecordId > 0)
            {
                // get record
                HetTimeRecord time = _context.HetTimeRecords.First(a => a.TimeRecordId == item.TimeRecordId);

                // not found
                if (time == null) 
                    return new NotFoundObjectResult(
                        new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                time.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                time.EnteredDate = DateTime.UtcNow;
                time.Hours = item.Hours;
                time.TimePeriod = item.TimePeriod;
                time.TimePeriodTypeId = (int)timePeriodTypeId;
                time.WorkedDate = item.WorkedDate;
            }
            else // add time record
            {
                HetTimeRecord time = new()
                {
                    RentalAgreementId = id,
                    EnteredDate = DateTime.UtcNow,
                    Hours = item.Hours,
                    TimePeriod = item.TimePeriod,
                    TimePeriodTypeId = (int)timePeriodTypeId,
                    WorkedDate = item.WorkedDate
                };

                _context.HetTimeRecords.Add(time);
            }

            _context.SaveChanges();

            // retrieve updated time records to return to ui
            return new ObjectResult(
                new HetsResponse(_rentalAgreementRepo.GetTimeRecords(id, districtId)));
        }

        /// <summary>
        /// Update or create an array of time records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Time Records</remarks>
        /// <param name="id">id of Rental Agreement to add a time record for</param>
        /// <param name="items">Array of Rental Agreement Time Records</param>
        [HttpPost]
        [Route("{id}/timeRecords")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<TimeRecordLite> RentalAgreementsIdTimeRecordsBulkPostAsync(
            [FromRoute] int id, 
            [FromBody] TimeRecordDto[] items)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || items == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // process each time record
            foreach (var item in items)
            {
                // set the time period type id
                int? timePeriodTypeId = StatusHelper.GetTimePeriodId(item.TimePeriod, _context) 
                    ?? throw new DataException("Time Period Id cannot be null");

                // add or update time record
                if (item.TimeRecordId > 0)
                {
                    // get record
                    HetTimeRecord time = _context.HetTimeRecords.First(a => a.TimeRecordId == item.TimeRecordId);

                    // not found
                    if (time == null) 
                        return new NotFoundObjectResult(
                            new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                    time.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    time.EnteredDate = DateTime.UtcNow;
                    time.Hours = item.Hours;
                    time.TimePeriod = item.TimePeriod;
                    time.TimePeriodTypeId = (int)timePeriodTypeId;
                    time.WorkedDate = item.WorkedDate;
                }
                else // add time record
                {
                    HetTimeRecord time = new()
                    {
                        RentalAgreementId = id,
                        EnteredDate = DateTime.UtcNow,
                        Hours = item.Hours,
                        TimePeriod = item.TimePeriod,
                        TimePeriodTypeId = (int)timePeriodTypeId,
                        WorkedDate = item.WorkedDate
                    };

                    _context.HetTimeRecords.Add(time);
                }

                _context.SaveChanges();
            }

            // retrieve updated time records to return to ui
            return new ObjectResult(
                new HetsResponse(_rentalAgreementRepo.GetTimeRecords(id, districtId)));
        }

        #endregion

        #region Rental Agreement Rate Records

        /// <summary>
        /// Get rate records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreements Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Rate Records for</param>
        [HttpGet]
        [Route("{id}/rateRecords")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalAgreementRateDto>> RentalAgreementsIdRentalAgreementRatesGet([FromRoute] int id)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // return rental agreement records
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRentalRates(id)));
        }

        /// <summary>
        /// Add or update a rental agreement rate record
        /// </summary>
        /// <remarks>Adds Rental Agreement Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to add a rate record for</param>
        /// <param name="item">Adds to Rental Agreement Rate Records</param>
        [HttpPost]
        [Route("{id}/rateRecord")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<RentalAgreementRateDto>> RentalAgreementsIdRentalAgreementRatesPost([FromRoute] int id,
            [FromBody] RentalAgreementRateDto item)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || item == null)
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // set the rate period type id
            int ratePeriodTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context) ?? throw new DataException("Rate Period Id cannot be null");

            // add or update rate records
            if (item.RentalAgreementRateId > 0)
            {
                // get record
                HetRentalAgreementRate rate = _context.HetRentalAgreementRates.FirstOrDefault(a => a.RentalAgreementRateId == item.RentalAgreementRateId);

                // not found
                if (rate == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                rate.Comment = item.Comment;
                rate.ComponentName = item.ComponentName;
                rate.Overtime = false;
                rate.Active = true;
                rate.IsIncludedInTotal = item.IsIncludedInTotal;
                rate.Rate = item.Rate;
                rate.RatePeriodTypeId = ratePeriodTypeId;
                rate.Set = item.Set;
            }
            else // add rate records
            {
                int agreementId = item.RentalAgreement.RentalAgreementId;

                HetRentalAgreementRate rate = new HetRentalAgreementRate
                {
                    RentalAgreementId = agreementId,
                    Comment = item.Comment,
                    ComponentName = item.ComponentName,
                    Overtime = false,
                    Active = true,
                    IsIncludedInTotal = item.IsIncludedInTotal,
                    Rate = item.Rate,
                    RatePeriodTypeId = ratePeriodTypeId,
                    Set = item.Set
                };

                _context.HetRentalAgreementRates.Add(rate);
            }

            _context.SaveChanges();

            // retrieve updated rate records to return to ui
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRentalRates(id)));
        }

        /// <summary>
        /// Update or create an array of rate records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Rate Records</remarks>
        /// <param name="id">id of Rental Agreement to add rate records for</param>
        /// <param name="items">Array of Rental Agreement Rate Records</param>
        [HttpPost]
        [Route("{id}/rateRecords")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<RentalAgreementRateDto>> RentalAgreementsIdRentalAgreementRatesBulkPost([FromRoute] int id,
            [FromBody] RentalAgreementRateDto[] items)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || items == null)
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // process each rate records
            foreach (var item in items)
            {
                // set the rate period type id
                int ratePeriodTypeId = StatusHelper.GetRatePeriodId(item.RatePeriod, _context) ?? throw new DataException("Rate Period Id cannot be null");

                // add or update rate records
                if (item.RentalAgreementRateId > 0)
                {
                    // get record
                    HetRentalAgreementRate rate
                        = _context.HetRentalAgreementRates.FirstOrDefault(a => a.RentalAgreementRateId == item.RentalAgreementRateId);

                    // not found
                    if (rate == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                    rate.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    rate.Comment = item.Comment;
                    rate.ComponentName = item.ComponentName;
                    rate.Overtime = false;
                    rate.Active = true;
                    rate.IsIncludedInTotal = item.IsIncludedInTotal;
                    rate.Rate = item.Rate;
                    rate.RatePeriodTypeId = ratePeriodTypeId;
                    rate.Set = item.Set;
                }
                else // add rate records
                {
                    int agreementId = item.RentalAgreement.RentalAgreementId;

                    HetRentalAgreementRate rate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = agreementId,
                        Comment = item.Comment,
                        ComponentName = item.ComponentName,
                        Overtime = false,
                        Active = true,
                        IsIncludedInTotal = item.IsIncludedInTotal,
                        Rate = item.Rate,
                        RatePeriodTypeId = ratePeriodTypeId,
                        Set = item.Set
                    };

                    _context.HetRentalAgreementRates.Add(rate);
                }

                _context.SaveChanges();
            }

            // retrieve updated rate records to return to ui
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRentalRates(id)));
        }

        #endregion

        #region Rental Agreement Condition Records

        /// <summary>
        /// Get condition records associated with a rental agreement
        /// </summary>
        /// <remarks>Gets a Rental Agreement&#39;s Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to fetch Condition Records for</param>
        [HttpGet]
        [Route("{id}/conditionRecords")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalAgreementConditionDto>> RentalAgreementsIdRentalAgreementConditionsGet([FromRoute] int id)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // return rental agreement records
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetConditions(id)));
        }

        /// <summary>
        /// Add a rental agreement condition record
        /// </summary>
        /// <remarks>Adds Rental Agreement Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to add a condition record for</param>
        /// <param name="item">Adds to Rental Agreement Condition Records</param>
        [HttpPost]
        [Route("{id}/conditionRecord")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<RentalAgreementConditionDto>> RentalAgreementsIdRentalAgreementConditionsPost([FromRoute] int id, 
            [FromBody] RentalAgreementConditionDto item)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add or update condition records
            if (item.RentalAgreementConditionId > 0)
            {
                // get record
                HetRentalAgreementCondition condition = _context.HetRentalAgreementConditions
                    .FirstOrDefault(a => a.RentalAgreementConditionId == item.RentalAgreementConditionId);

                // not found
                if (condition == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                condition.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                condition.Comment = item.Comment;
                condition.ConditionName = item.ConditionName;
            }
            else // add condition records
            {
                int agreementId = item.RentalAgreement.RentalAgreementId;

                HetRentalAgreementCondition condition = new HetRentalAgreementCondition
                {
                    RentalAgreementId = agreementId,
                    Comment = item.Comment,
                    ConditionName = item.ConditionName
                };

                _context.HetRentalAgreementConditions.Add(condition);
            }

            _context.SaveChanges();

            // return rental agreement condition records
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetConditions(id)));
        }

        /// <summary>
        /// Update or create an array of condition records associated with a rental agreement
        /// </summary>
        /// <remarks>Adds Rental Agreement Condition Records</remarks>
        /// <param name="id">id of Rental Agreement to add condition records for</param>
        /// <param name="items">Array of Rental Agreement Condition Records</param>
        [HttpPost]
        [Route("{id}/conditionRecords")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<RentalAgreementConditionDto>> RentalAgreementsIdRentalAgreementConditionsBulkPost([FromRoute] int id, 
            [FromBody] RentalAgreementConditionDto[] items)
        {
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            // not found
            if (!exists || items == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // process each rate records
            foreach (var item in items)
            {
                // add or update condition records
                if (item.RentalAgreementConditionId > 0)
                {
                    // get record
                    HetRentalAgreementCondition condition = _context.HetRentalAgreementConditions
                        .FirstOrDefault(a => a.RentalAgreementConditionId == item.RentalAgreementConditionId);

                    // not found
                    if (condition == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                    condition.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    condition.Comment = item.Comment;
                    condition.ConditionName = item.ConditionName;
                }
                else // add condition records
                {
                    int agreementId = item.RentalAgreement.RentalAgreementId;

                    HetRentalAgreementCondition condition = new HetRentalAgreementCondition
                    {
                        RentalAgreementId = agreementId,
                        Comment = item.Comment,
                        ConditionName = item.ConditionName
                    };

                    _context.HetRentalAgreementConditions.Add(condition);
                }

                _context.SaveChanges();
            }

            // return rental agreement condition records
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetConditions(id)));
        }

        #endregion

        #region Blank Rental Agreement

        /// <summary>
        /// Create a new blank rental agreement (need a project id)
        /// </summary>
        [HttpPost]
        [Route("createBlankAgreement")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementDto> BlankRentalAgreementPost()
        {
            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            HetDistrict district = _context.HetDistricts.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get active status id
            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // HETS-825 - MAX number of Blank Rental Agreements and limit the functionality to ADMINS only
            // * Limit Blank rental agreements to a maximum of 3
            List<HetRentalAgreement> agreements = _context.HetRentalAgreements.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.District)
                .Include(x => x.Project)
                .Include(x => x.Equipment)
                .Where(x => x.District.DistrictId == districtId &&
                            x.RentalRequestId == null &&
                            x.RentalRequestRotationListId == null &&
                            x.RentalAgreementStatusTypeId == statusId)
                .ToList();

            string tempMax = _configuration.GetSection("Constants:Maximum-Blank-Agreements").Value;
            bool isNumeric = int.TryParse(tempMax, out int max);
            if (!isNumeric) max = 3; // default to 3

            if (agreements.Count >= max)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-29", ErrorViewModel.GetDescription("HETS-29", _configuration)));
            }

            // set the rate period type id
            int? rateTypeId = StatusHelper.GetRatePeriodId(HetRatePeriodType.PeriodHourly, _context);
            if (rateTypeId == null) return new BadRequestObjectResult(new HetsResponse("HETS-24", ErrorViewModel.GetDescription("HETS-24", _configuration)));

            // create new agreement
            HetRentalAgreement agreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(district, _context),
                DistrictId = districtId,
                RentalAgreementStatusTypeId = (int)statusId,
                RatePeriodTypeId = (int)rateTypeId
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

                if (agreement.HetRentalAgreementRates == null)
                {
                    agreement.HetRentalAgreementRates = new List<HetRentalAgreementRate>();
                }

                agreement.HetRentalAgreementRates.Add(newAgreementRate);
            }

            // save the changes
            _context.HetRentalAgreements.Add(agreement);
            _context.SaveChanges();

            int id = agreement.RentalAgreementId;

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRecord(id)));
        }

        /// <summary>
        /// Get blank rental agreements (for the current district)
        /// </summary>
        [HttpGet]
        [Route("blankAgreements")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalAgreementDto>> BlankRentalAgreementsGet()
        {
            // get the current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get active status id
            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get all active "blank" agreements
            List<HetRentalAgreement> agreements = _context.HetRentalAgreements.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.District)
                .Include(x => x.Project)
                .Include(x => x.Equipment)
                .Where(x => x.District.DistrictId == districtId &&
                            x.RentalRequestId == null &&
                            x.RentalRequestRotationListId == null &&
                            x.RentalAgreementStatusTypeId == statusId)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<RentalAgreementDto>>(agreements)));
        }

        /// <summary>
        /// Get blank rental agreements (for the current district)
        /// By Project Id and Equipment Id (ACTIVE AGREEMENTS ONLY)
        /// </summary>
        [HttpGet]
        [Route("blankAgreements/{projectId}/{equipmentId}")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalAgreementDto>> BlankRentalAgreementLookupGet([FromRoute] int projectId, [FromRoute] int equipmentId)
        {
            // get the current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get agreement status
            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get "blank" agreements
            List<HetRentalAgreement> agreements = _context.HetRentalAgreements.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.District)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Project)
                .Where(x => x.District.DistrictId == districtId &&
                            x.RentalRequestId == null &&
                            x.RentalRequestRotationListId == null &&
                            x.ProjectId == projectId &&
                            x.EquipmentId == equipmentId &&
                            x.RentalAgreementStatusTypeId == statusId)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<RentalAgreementDto>>(agreements)));
        }

        /// <summary>
        /// Delete a blank rental agreement
        /// </summary>
        /// <param name="id">id of Blank RentalAgreement to delete</param>
        [HttpPost]
        [Route("deleteBlankAgreement/{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementDto> DeleteBlankRentalAgreementPost([FromRoute] int id)
        {
            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            HetDistrict district = _context.HetDistricts.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // validate agreement id
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get agreement and validate
            HetRentalAgreement agreement = _context.HetRentalAgreements
                .Include(a => a.HetRentalAgreementRates)
                .Include(a => a.HetRentalAgreementConditions)
                .First(a => a.RentalAgreementId == id);

            if (agreement.RentalAgreementStatusTypeId != statusId)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-25", ErrorViewModel.GetDescription("HETS-25", _configuration)));
            }

            if (agreement.DistrictId != districtId)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-26", ErrorViewModel.GetDescription("HETS-26", _configuration)));
            }

            if (agreement.RentalRequestId != null)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-27", ErrorViewModel.GetDescription("HETS-27", _configuration)));
            }

            // delete rate
            foreach (HetRentalAgreementRate item in agreement.HetRentalAgreementRates)
            {
                _context.HetRentalAgreementRates.Remove(item);
            }

            // delete conditions
            foreach (HetRentalAgreementCondition item in agreement.HetRentalAgreementConditions)
            {
                _context.HetRentalAgreementConditions.Remove(item);
            }

            // delete the agreement
            _context.HetRentalAgreements.Remove(agreement);
            _context.SaveChanges();

            // return rental agreement
            return new ObjectResult(new HetsResponse(_mapper.Map<RentalAgreementDto>(agreement)));
        }

        /// <summary>
        /// Clone a blank rental agreement
        /// </summary>
        /// <param name="id">id of Blank RentalAgreement to clone</param>
        /// <param name="agreement"></param>
        [HttpPost]
        [Route("updateCloneBlankAgreement/{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementDto> CloneBlankRentalAgreementPost([FromRoute] int id, [FromBody] RentalAgreementDto agreement)
        {
            // check the ids
            if (id != agreement.RentalAgreementId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            HetDistrict district = _context.HetDistricts.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // validate agreement id
            bool exists = _context.HetRentalAgreements.Any(a => a.RentalAgreementId == id);

            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add overtime rates
            List<HetProvincialRateType> overtime = _context.HetProvincialRateTypes.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            // get agreement and clone
            HetRentalAgreement oldAgreement = _context.HetRentalAgreements.AsNoTracking()
                .Include(a => a.HetRentalAgreementRates)
                .Include(a => a.HetRentalAgreementConditions)
                .First(a => a.RentalAgreementId == id);

            // create new blank agreement as a duplicate
            HetRentalAgreement newAgreement = new HetRentalAgreement
            {
                Number = RentalAgreementHelper.GetRentalAgreementNumber(district, _context),
                DistrictId = districtId,
                RentalAgreementStatusTypeId = oldAgreement.RentalAgreementStatusTypeId,
                RatePeriodTypeId = oldAgreement.RatePeriodTypeId,
                EstimateHours = oldAgreement.EstimateHours,
                EstimateStartWork = oldAgreement.EstimateStartWork,
                RateComment = oldAgreement.RateComment?.Trim(),
                EquipmentRate = oldAgreement.EquipmentRate,
                Note = oldAgreement.Note?.Trim(),
                DatedOn = oldAgreement.DatedOn,
                AgreementCity = oldAgreement.AgreementCity
            };

            foreach (HetRentalAgreementCondition condition in oldAgreement.HetRentalAgreementConditions)
            {
                HetRentalAgreementCondition newCondition = new HetRentalAgreementCondition
                {
                    RentalAgreementId = id,
                    Comment = condition.Comment,
                    ConditionName = condition.ConditionName
                };

                newAgreement.HetRentalAgreementConditions.Add(newCondition);
            }

            if (oldAgreement.HetRentalAgreementRates != null)
            {
                foreach (HetRentalAgreementRate rate in oldAgreement.HetRentalAgreementRates)
                {
                    HetRentalAgreementRate newRate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = id,
                        Comment = rate.Comment,
                        Rate = rate.Rate,
                        ComponentName = rate.ComponentName,
                        Active = rate.Active,
                        IsIncludedInTotal = rate.IsIncludedInTotal,
                        Overtime = rate.Overtime
                    };

                    newAgreement.HetRentalAgreementRates.Add(newRate);
                }
            }

            // update overtime rates (and add if they don't exist)
            foreach (HetProvincialRateType overtimeRate in overtime)
            {
                bool found = newAgreement.HetRentalAgreementRates.Any(x => x.ComponentName == overtimeRate.RateType);

                if (found)
                {
                    HetRentalAgreementRate rate = newAgreement.HetRentalAgreementRates
                        .First(x => x.ComponentName == overtimeRate.RateType);

                    rate.Rate = overtimeRate.Rate;
                }
                else
                {
                    HetRentalAgreementRate newRate = new HetRentalAgreementRate
                    {
                        RentalAgreementId = id,
                        Comment = overtimeRate.Description,
                        Rate = overtimeRate.Rate,
                        ComponentName = overtimeRate.RateType,
                        Active = overtimeRate.Active,
                        IsIncludedInTotal = overtimeRate.IsIncludedInTotal,
                        Overtime = overtimeRate.Overtime
                    };

                    newAgreement.HetRentalAgreementRates.Add(newRate);
                }
            }

            // remove non-existent overtime rates
            List<string> remove =
                (from overtimeRate in newAgreement.HetRentalAgreementRates
                 where overtimeRate.Overtime ?? false
                 let found = overtime.Any(x => x.RateType == overtimeRate.ComponentName)
                 where !found
                 select overtimeRate.ComponentName).ToList();

            if (remove.Count > 0)
            {
                foreach (string component in remove)
                {
                    newAgreement.HetRentalAgreementRates.Remove(
                        newAgreement.HetRentalAgreementRates.First(x => x.ComponentName == component));
                }
            }

            // add new agreement and save changes
            _context.HetRentalAgreements.Add(newAgreement);
            _context.SaveChanges();

            int newAgreementId = newAgreement.RentalAgreementId;

            // retrieve updated rental agreement to return to ui
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRecord(newAgreementId)));
        }

        #endregion

        #region Search Rental Requests

        /// <summary>
        /// Find the latest agreement by project and equipment id
        /// </summary>
        /// <remarks>Used for the time entry page.</remarks>
        /// <param name="equipmentId">Equipment Id</param>
        /// <param name="projectId">Project Id</param>
        [HttpGet]
        [RequiresPermission(HetPermission.Login)]
        [Route("latest/{projectId}/{equipmentId}")]
        public virtual ActionResult<RentalAgreementDto> GetLatestRentalAgreement(
            [FromRoute] int projectId, 
            [FromRoute] int equipmentId)
        {
            // find the latest rental agreement
            HetRentalAgreement agreement = _context.HetRentalAgreements.AsNoTracking()
                .OrderByDescending(x => x.AppCreateTimestamp)
                .FirstOrDefault(x => x.EquipmentId == equipmentId &&
                                     x.ProjectId == projectId);

            // if nothing exists - return an error message
            if (agreement == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-35", ErrorViewModel.GetDescription("HETS-35", _configuration)));

            // get user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatuses.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYearStart = status.CurrentFiscalYear;
            if (fiscalYearStart == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            DateTime fiscalStart = DateUtils.ConvertPacificToUtcTime(
                new DateTime((int)fiscalYearStart, 4, 1, 0, 0, 0, DateTimeKind.Unspecified));

            // validate that agreement is in the current fiscal year
            DateTime agreementDate = DateUtils.AsUTC(agreement.DatedOn ?? agreement.DbCreateTimestamp);

            if (agreementDate < fiscalStart) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-36", ErrorViewModel.GetDescription("HETS-36", _configuration)));

            // return to the client
            return new ObjectResult(new HetsResponse(_mapper.Map<RentalAgreementDto>(agreement)));
        }

        #endregion

        #region Get all agreements by district for rental agreement summary filtering

        /// <summary>
        /// Get all agreements by district for rental agreement summary filtering
        /// </summary>
        [HttpGet]
        [Route("summaryLite")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalAgreementSummaryLite>> RentalAgreementsGetSummaryLite()
        {
            // get user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            IQueryable<HetRentalAgreement> agreements = _context.HetRentalAgreements.AsNoTracking()
                .Where(x => x.DistrictId.Equals(districtId) &&
                            !x.Number.StartsWith("BCBid"));

            // convert to "lite" model
            List<RentalAgreementSummaryLite> result = new List<RentalAgreementSummaryLite>();

            foreach (HetRentalAgreement item in agreements)
            {
                result.Add(RentalAgreementHelper.ToSummaryLiteModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region AIT Report

        /// <summary>
        /// Get rental agreements for AIT Report
        /// </summary>
        /// <param name="projects">Projects (comma separated list of id numbers)</param>
        /// <param name="districtEquipmentTypes">District Equipment Types (comma separated list of equipment types)</param>
        /// <param name="equipment">Equipment (comma separated list of id numbers)</param>
        /// <param name="rentalAgreementNumber">Rental Agreement Number</param>
        /// <param name="startDate">Start date for Dated On</param>
        /// <param name="endDate">End date for Dated On</param>
        /// <returns>AIT report</returns>
        [HttpGet]
        [Route("aitReport")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalRequestHires>> AitReportGet(
            [FromQuery] string projects,
            [FromQuery] string districtEquipmentTypes, 
            [FromQuery] string equipment, 
            [FromQuery] string rentalAgreementNumber,
            [FromQuery] DateTime? startDate, 
            [FromQuery] DateTime? endDate)
        {
            int?[] projectArray = ArrayHelper.ParseIntArray(projects);
            int?[] districtEquipmentTypeArray = ArrayHelper.ParseIntArray(districtEquipmentTypes);
            int?[] equipmentArray = ArrayHelper.ParseIntArray(equipment);

            IQueryable<HetRentalAgreement> agreements = _context.HetRentalAgreements.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Project);

            // limit to user's current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);
            agreements = agreements.Where(x => x.DistrictId == districtId);

            // HET-1137 do not show placeholder rental agreements created to imported from BCBid, (ones with agreement# BCBid-XX-XXXX)
            agreements = agreements.Where(x => !x.Number.StartsWith("BCBid"));

            if (!string.IsNullOrWhiteSpace(rentalAgreementNumber))
            {
                agreements = agreements.Where(x => 
                    x.Number.ToUpper().Contains(rentalAgreementNumber.Trim().ToUpper()));
            }

            if (projectArray != null && projectArray.Length > 0)
            {
                agreements = agreements.Where(x => projectArray.Contains(x.ProjectId));
            }

            if (districtEquipmentTypeArray != null && districtEquipmentTypeArray.Length > 0)
            {
                agreements = agreements.Where(x => 
                    districtEquipmentTypeArray.Contains(x.Equipment.DistrictEquipmentTypeId));
            }

            if (equipmentArray != null && equipmentArray.Length > 0)
            {
                agreements = agreements.Where(x => equipmentArray.Contains(x.EquipmentId));
            }

            if (startDate is DateTime startDt)
            {
                DateTime startDtUtc = DateUtils.AsUTC(startDt);
                agreements = agreements.Where(x => x.DatedOn >= startDtUtc);
            }

            if (endDate is DateTime endDt)
            {
                DateTime endDtUtc = DateUtils.AsUTC(endDt);
                agreements = agreements.Where(x => x.DatedOn <= endDtUtc);
            }

            var result = agreements
                .Select(x => AitReport.MapFromHetRentalAgreement(x))
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        #endregion
    }
}
