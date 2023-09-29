using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Entities;
using HetsData.Repositories;
using HetsData.Dtos;
using AutoMapper;
using HetsCommon;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Project Controller
    /// </summary>
    [Route("api/projects")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProjectController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepo;
        private readonly IRentalRequestRepository _rentalRequestRepo;
        private readonly IRentalAgreementRepository _rentalAgreementRepo;

        public ProjectController(DbAppContext context, IConfiguration configuration, IMapper mapper,
            IProjectRepository projectRepo,
            IRentalRequestRepository rentalRequestRepo,
            IRentalAgreementRepository rentalAgreementRepo)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _projectRepo = projectRepo;
            _rentalRequestRepo = rentalRequestRepo;
            _rentalAgreementRepo = rentalAgreementRepo;
        }

        /// <summary>
        /// Get project by id
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<ProjectDto> ProjectsIdGet([FromRoute] int id)
        {
            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            return new ObjectResult(new HetsResponse(_projectRepo.GetRecord(id, districtId)));
        }

        /// <summary>
        /// Update project
        /// </summary>
        /// <param name="id">id of Project to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<ProjectDto> ProjectsIdPut([FromRoute] int id, [FromBody] ProjectDto item)
        {
            if (item == null || id != item.ProjectId)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetProject project = _context.HetProjects.First(a => a.ProjectId == id);

            int? statusId = StatusHelper.GetStatusId(item.Status, "projectStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            project.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            project.Name = item.Name;
            project.ProvincialProjectNumber = item.ProvincialProjectNumber;
            project.ProjectStatusTypeId = (int)statusId;
            project.Information = item.Information;

            // HETS-1006 - Go - Live: Add additional fields for projects
            project.FiscalYear = item.FiscalYear;
            project.ResponsibilityCentre = item.ResponsibilityCentre;
            project.ServiceLine = item.ServiceLine;
            project.Stob = item.Stob;
            project.Product = item.Product;
            project.BusinessFunction = item.BusinessFunction;
            project.WorkActivity = item.WorkActivity;
            project.CostType = item.CostType;

            if (item.District != null)
            {
                project.DistrictId = item.District.DistrictId;
            }

            if (item.PrimaryContact != null)
            {
                project.PrimaryContactId = item.PrimaryContact.ContactId;
            }

            // save the changes
            _context.SaveChanges();

            // retrieve updated project record to return to ui
            return new ObjectResult(new HetsResponse(_projectRepo.GetRecord(id)));
        }

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<ProjectDto> ProjectsPost([FromBody] ProjectDto item)
        {
            // not found
            if (item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(item.Status, "projectStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            HetProject project = new HetProject
            {
                Name = item.Name,
                ProvincialProjectNumber = item.ProvincialProjectNumber,
                ProjectStatusTypeId = (int)statusId,
                Information = item.Information,
                DistrictId = item.District.DistrictId,

                // HETS-1006 - Go - Live: Add additional fields for projects
                FiscalYear = item.FiscalYear,
                ResponsibilityCentre = item.ResponsibilityCentre,
                ServiceLine = item.ServiceLine,
                Stob = item.Stob,
                Product = item.Product,
                BusinessFunction = item.BusinessFunction,
                WorkActivity = item.WorkActivity,
                CostType = item.CostType
            };

            _context.HetProjects.Add(project);

            // save record
            _context.SaveChanges();

            int id = project.ProjectId;

            // retrieve updated project record to return to ui
            return new ObjectResult(new HetsResponse(_projectRepo.GetRecord(id)));
        }

        #region Project Search

        /// <summary>
        /// Searches Projects
        /// </summary>
        /// <remarks>Used for the project search page.</remarks>
        /// <param name="districts">Districts (comma separated list of id numbers)</param>
        /// <param name="project">name or partial name for a Project</param>
        /// <param name="hasRequests">if true then only include Projects with active Requests</param>
        /// <param name="hasHires">if true then only include Projects with active Rental Agreements</param>
        /// <param name="status">if included, filter the results to those with a status matching this string</param>
        /// <param name="projectNumber"></param>
        /// <param name="fiscalYear"></param>
        [HttpGet]
        [Route("search")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ProjectLite>> ProjectsSearchGet([FromQuery] string districts,
            [FromQuery] string project, [FromQuery] bool? hasRequests, [FromQuery] bool? hasHires,
            [FromQuery] string status, [FromQuery] string projectNumber,
            [FromQuery] string fiscalYear)
        {
            int?[] districtTokens = ArrayHelper.ParseIntArray(districts);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            IQueryable<HetProject> data = _context.HetProjects.AsNoTracking()
                .Include(x => x.ProjectStatusType)
                .Include(x => x.District.Region)
                .Include(x => x.PrimaryContact)
                .Include(x => x.HetRentalAgreements)
                .Include(x => x.HetRentalRequests)
                .Where(x => x.DistrictId.Equals(districtId));

            if (districtTokens != null && districts.Length > 0)
            {
                data = data.Where(x => districtTokens.Contains(x.DistrictId));
            }

            if (project != null)
            {
                data = data.Where(x => x.Name.ToLower().Contains(project.ToLower()));
            }

            if (status != null)
            {
                int? statusId = StatusHelper.GetStatusId(status, "projectStatus", _context);

                if (statusId != null)
                {
                    data = data.Where(x => x.ProjectStatusTypeId == statusId);
                }
            }

            if (projectNumber != null)
            {
                // allow for case insensitive search of project name
                data = data.Where(x => x.ProvincialProjectNumber.ToLower().Contains(projectNumber.ToLower()));
            }

            if (!string.IsNullOrEmpty(fiscalYear))
            {
                data = data.Where(x => x.FiscalYear.Equals(fiscalYear));
            }

            // convert Project Model to the "ProjectLite" Model
            List<ProjectLite> result = new List<ProjectLite>();

            foreach (HetProject item in data)
            {
                result.Add(_projectRepo.ToLiteModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region Get all projects by region

        /// <summary>
        /// Get all projects by district
        /// </summary>
        [HttpGet]
        [Route("")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ProjectLite>> ProjectsGet([FromQuery] bool currentFiscal = true)
        {
            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatuses.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;
            if (fiscalYear == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            string fiscalYearStr = $"{fiscalYear}/{fiscalYear + 1}";

            // HETS-1005 - Time entry tab search issues
            // * we need retrieve all projects that have been created this year
            //   since users can add time records to closed projects
            IQueryable<HetProject> projects = _context.HetProjects.AsNoTracking()
                .Where(x => x.DistrictId.Equals(districtId) &&
                            (!currentFiscal || x.FiscalYear.Equals(fiscalYearStr)));

            // convert Project Model to the "ProjectLite" Model
            List<ProjectLite> result = new List<ProjectLite>();

            foreach (HetProject item in projects)
            {
                result.Add(_projectRepo.ToLiteModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Get all projects by district for rental agreement summary filtering
        /// </summary>
        [HttpGet]
        [Route("agreementSummary")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ProjectAgreementSummary>> ProjectsGetAgreementSummary()
        {
            // get user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            var result = _context.HetRentalAgreements.AsNoTracking()
                .Include(x => x.Project)
                .Where(x => x.DistrictId.Equals(districtId) &&
                            !x.Number.StartsWith("BCBid"))
                .ToList()
                .GroupBy(x => x.Project, (p, agreements) => new ProjectAgreementSummary
                {
                    Id = p.ProjectId,
                    Name = p.Name,
                    AgreementIds = agreements.Select(y => y.RentalAgreementId).Distinct().ToList(),
                })
                .ToList();

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region Clone Project Agreements

        /// <summary>
        /// Get rental agreements associated with a project by id
        /// </summary>
        /// <remarks>Gets a Projects Rental Agreements</remarks>
        /// <param name="id">id of Project to fetch agreements for</param>
        [HttpGet]
        [Route("{id}/rentalAgreements")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalAgreementDto>> ProjectsIdRentalAgreementsGet([FromRoute] int id)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            List<HetRentalAgreement> agreements = _context.HetProjects.AsNoTracking()
                .Include(x => x.ProjectStatusType)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(e => e.Equipment)
                        .ThenInclude(d => d.DistrictEquipmentType)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(e => e.Equipment)
                        .ThenInclude(a => a.HetEquipmentAttachments)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(e => e.RentalAgreementStatusType)
                .First(x => x.ProjectId == id)
                .HetRentalAgreements
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<RentalAgreementDto>>(agreements)));
        }

        /// <summary>
        /// Update a rental agreement by cloning a previous project rental agreement
        /// </summary>
        /// <param name="id">Project id</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/rentalAgreementClone")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<RentalAgreementDto> ProjectsRentalAgreementClonePost([FromRoute] int id, [FromBody] ProjectRentalAgreementClone item)
        {
            // not found
            if (item == null || id != item.ProjectId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get all agreements for this project
            HetProject project = _context.HetProjects
                .Include(x => x.ProjectStatusType)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(y => y.HetRentalAgreementRates)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(y => y.HetRentalAgreementConditions)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(e => e.RentalAgreementStatusType)
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(y => y.HetTimeRecords)
                .First(a => a.ProjectId == id);

            List<HetRentalAgreement> agreements = project.HetRentalAgreements.ToList();

            // check that the rental agreements exist
            exists = agreements.Any(a => a.RentalAgreementId == item.RentalAgreementId);

            // (RENTAL AGREEMENT) not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // check that the rental agreement to clone exist
            exists = agreements.Any(a => a.RentalAgreementId == item.AgreementToCloneId);

            // (RENTAL AGREEMENT) not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-11", ErrorViewModel.GetDescription("HETS-11", _configuration)));

            int agreementToCloneIndex = agreements.FindIndex(a => a.RentalAgreementId == item.AgreementToCloneId);
            int newRentalAgreementIndex = agreements.FindIndex(a => a.RentalAgreementId == item.RentalAgreementId);

            // ******************************************************************
            // Business Rules in the backend:
            // *Can't clone into an Agreement if it isn't Active
            // *Can't clone into an Agreement if it has existing time records
            // ******************************************************************
            if (!agreements[newRentalAgreementIndex].RentalAgreementStatusType.Description
                .Equals("Active", StringComparison.InvariantCultureIgnoreCase))
            {
                // (RENTAL AGREEMENT) is not active
                return new BadRequestObjectResult(new HetsResponse("HETS-12", ErrorViewModel.GetDescription("HETS-12", _configuration)));
            }

            if (agreements[newRentalAgreementIndex].HetTimeRecords != null &&
                agreements[newRentalAgreementIndex].HetTimeRecords.Count > 0)
            {
                // (RENTAL AGREEMENT) has time records
                return new BadRequestObjectResult(new HetsResponse("HETS-13", ErrorViewModel.GetDescription("HETS-13", _configuration)));
            }

            // ******************************************************************
            // clone agreement
            // ******************************************************************
            agreements[newRentalAgreementIndex].AgreementCity = agreements[agreementToCloneIndex].AgreementCity;
            agreements[newRentalAgreementIndex].EquipmentRate = agreements[agreementToCloneIndex].EquipmentRate;
            agreements[newRentalAgreementIndex].Note = agreements[agreementToCloneIndex].Note;
            agreements[newRentalAgreementIndex].RateComment = agreements[agreementToCloneIndex].RateComment;
            agreements[newRentalAgreementIndex].RatePeriodTypeId = agreements[agreementToCloneIndex].RatePeriodTypeId;

            // update rates
            agreements[newRentalAgreementIndex].HetRentalAgreementRates = null;

            foreach (HetRentalAgreementRate rate in agreements[agreementToCloneIndex].HetRentalAgreementRates)
            {
                HetRentalAgreementRate temp = new HetRentalAgreementRate
                {
                    RentalAgreementId = id,
                    Comment = rate.Comment,
                    ComponentName = rate.ComponentName,
                    Rate = rate.Rate,
                    Set = rate.Set,
                    Overtime = rate.Overtime,
                    Active = rate.Active,
                    IsIncludedInTotal = rate.IsIncludedInTotal,
                    RatePeriodTypeId = rate.RatePeriodTypeId
                };

                if (agreements[newRentalAgreementIndex].HetRentalAgreementRates == null)
                {
                    agreements[newRentalAgreementIndex].HetRentalAgreementRates = new List<HetRentalAgreementRate>();
                }

                agreements[newRentalAgreementIndex].HetRentalAgreementRates.Add(temp);
            }

            // update overtime rates (and add if they don't exist)
            List<HetProvincialRateType> overtime = _context.HetProvincialRateTypes.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            foreach (HetProvincialRateType overtimeRate in overtime)
            {
                bool found = false;

                if (agreements[newRentalAgreementIndex] != null &&
                    agreements[newRentalAgreementIndex].HetRentalAgreementRates != null)
                {
                    found = agreements[newRentalAgreementIndex].HetRentalAgreementRates.Any(x => x.ComponentName == overtimeRate.RateType);
                }

                if (found)
                {
                    HetRentalAgreementRate rate = agreements[newRentalAgreementIndex].HetRentalAgreementRates
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

                    if (agreements[newRentalAgreementIndex].HetRentalAgreementRates == null)
                    {
                        agreements[newRentalAgreementIndex].HetRentalAgreementRates = new List<HetRentalAgreementRate>();
                    }

                    agreements[newRentalAgreementIndex].HetRentalAgreementRates.Add(newRate);
                }
            }

            // remove non-existent overtime rates
            List<string> remove =
                (from overtimeRate in agreements[newRentalAgreementIndex].HetRentalAgreementRates
                 where overtimeRate.Overtime ?? false
                 let found = overtime.Any(x => x.RateType == overtimeRate.ComponentName)
                 where !found
                 select overtimeRate.ComponentName).ToList();

            if (remove.Count > 0 &&
                agreements[newRentalAgreementIndex] != null &&
                agreements[newRentalAgreementIndex].HetRentalAgreementRates != null)
            {
                foreach (string component in remove)
                {
                    agreements[newRentalAgreementIndex].HetRentalAgreementRates.Remove(
                        agreements[newRentalAgreementIndex].HetRentalAgreementRates.First(x => x.ComponentName == component));
                }
            }

            // update conditions
            agreements[newRentalAgreementIndex].HetRentalAgreementConditions = null;

            foreach (HetRentalAgreementCondition condition in agreements[agreementToCloneIndex].HetRentalAgreementConditions)
            {
                HetRentalAgreementCondition temp = new HetRentalAgreementCondition
                {
                    Comment = condition.Comment,
                    ConditionName = condition.ConditionName
                };

                if (agreements[newRentalAgreementIndex].HetRentalAgreementConditions == null)
                {
                    agreements[newRentalAgreementIndex].HetRentalAgreementConditions = new List<HetRentalAgreementCondition>();
                }

                agreements[newRentalAgreementIndex].HetRentalAgreementConditions.Add(temp);
            }

            // save the changes
            _context.SaveChanges();

            // ******************************************************************
            // return update rental agreement to update the screen
            // ******************************************************************
            return new ObjectResult(new HetsResponse(_rentalAgreementRepo.GetRecord(item.RentalAgreementId)));
        }

        #endregion

        #region Project Time Records

        /// <summary>
        /// Get time records associated with a project
        /// </summary>
        /// <remarks>Gets a Projects Time Records</remarks>
        /// <param name="id">id of Project to fetch Time Records for</param>
        [HttpGet]
        [Route("{id}/timeRecords")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<TimeRecordDto>> HetProjectIdTimeRecordsGet([FromRoute] int id)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current district and fiscal year
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatuses.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;
            if (fiscalYear == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // fiscal year in the status table stores the "start" of the year
            DateTime fiscalYearStart = DateUtils.ConvertPacificToUtcTime(
                new DateTime((int)fiscalYear, 4, 1, 0, 0, 0, DateTimeKind.Unspecified));

            HetProject project = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(t => t.HetTimeRecords)
                .First(x => x.ProjectId == id);

            // create a single array of all time records
            List<HetTimeRecord> timeRecords = new();

            foreach (HetRentalAgreement rentalAgreement in project.HetRentalAgreements)
            {
                // only add records from this fiscal year
                timeRecords.AddRange(rentalAgreement.HetTimeRecords.Where(x => x.WorkedDate >= fiscalYearStart));
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<TimeRecordDto>>(timeRecords)));
        }

        /// <summary>
        /// Add a project time record
        /// </summary>
        /// <remarks>Adds Project Time Records</remarks>
        /// <param name="id">id of Project to add a time record for</param>
        /// <param name="item">Adds to Project Time Records</param>
        [HttpPost]
        [Route("{id}/timeRecord")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<TimeRecordDto>> ProjectsIdTimeRecordsPost([FromRoute] int id, [FromBody] TimeRecordDto item)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get project record
            HetProject project = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(t => t.HetTimeRecords)
                .First(x => x.ProjectId == id);

            List<HetRentalAgreement> agreements = project.HetRentalAgreements.ToList();

            // ******************************************************************
            // must have a valid rental agreement id
            // ******************************************************************
            if (item.RentalAgreementId == null || item.RentalAgreementId == 0)
            {
                // (RENTAL AGREEMENT) record not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            exists = agreements.Any(a => a.RentalAgreementId == item.RentalAgreementId);

            if (!exists)
            {
                // (RENTAL AGREEMENT) record not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // ******************************************************************
            // add or update time record
            // ******************************************************************
            int rentalAgreementId = (int)item.RentalAgreementId;

            // set the time period type id
            int? timePeriodTypeId = StatusHelper.GetTimePeriodId(item.TimePeriod, _context) 
                ?? throw new DataException("Time Period Id cannot be null");

            if (item.TimeRecordId > 0)
            {
                // get time record
                HetTimeRecord time = _context.HetTimeRecords.FirstOrDefault(x => x.TimeRecordId == item.TimeRecordId);

                // not found
                if (time == null) 
                    return new NotFoundObjectResult(
                        new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                time.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                time.RentalAgreementId = rentalAgreementId;
                time.EnteredDate = DateTime.UtcNow;
                time.Hours = item.Hours;
                time.TimePeriod = item.TimePeriod;
                time.TimePeriodTypeId = (int)timePeriodTypeId;
                time.WorkedDate = DateUtils.AsUTC(item.WorkedDate);
            }
            else // add time record
            {
                HetTimeRecord time = new()
                {
                    RentalAgreementId = rentalAgreementId,
                    EnteredDate = DateTime.UtcNow,
                    Hours = item.Hours,
                    TimePeriod = item.TimePeriod,
                    TimePeriodTypeId = (int)timePeriodTypeId,
                    WorkedDate = DateUtils.AsUTC(item.WorkedDate)
                };

                _context.HetTimeRecords.Add(time);
            }

            // save record
            _context.SaveChanges();

            // *************************************************************
            // return updated time records
            // *************************************************************
            project = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(t => t.HetTimeRecords)
                .First(x => x.ProjectId == id);

            // create a single array of all time records
            List<HetTimeRecord> timeRecords = new();

            foreach (HetRentalAgreement rentalAgreement in project.HetRentalAgreements)
            {
                timeRecords.AddRange(rentalAgreement.HetTimeRecords);
            }

            return new ObjectResult(
                new HetsResponse(_mapper.Map<List<TimeRecordDto>>(timeRecords)));
        }

        #endregion

        #region Project Equipment

        /// <summary>
        /// Get equipment associated with a project
        /// </summary>
        /// <remarks>Gets a Projects Equipment</remarks>
        /// <param name="id">id of Project to fetch Equipment for</param>
        [HttpGet]
        [Route("{id}/equipment")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<RentalAgreementDto>> ProjectsIdEquipmentGet([FromRoute] int id)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetProject project = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetRentalAgreements)
                    .ThenInclude(e => e.Equipment)
                        .ThenInclude(o => o.Owner)
                            .ThenInclude(c => c.PrimaryContact)
                .First(x => x.ProjectId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<List<RentalAgreementDto>>(project.HetRentalAgreements)));
        }

        #endregion

        #region Project Attachments

        /// <summary>
        /// Get attachments associated with a project
        /// </summary>
        /// <remarks>Returns attachments for a particular Project</remarks>
        /// <param name="id">id of Project to fetch attachments for</param>
        [HttpGet]
        [Route("{id}/attachments")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<DigitalFileDto>> ProjectsIdAttachmentsGet([FromRoute] int id)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetProject project = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetDigitalFiles)
                .First(a => a.ProjectId == id);

            // extract the attachments and update properties for UI
            List<HetDigitalFile> attachments = new();

            foreach (HetDigitalFile attachment in project.HetDigitalFiles)
            {
                if (attachment != null)
                {
                    attachment.FileSize = attachment.FileContents.Length;
                    attachment.LastUpdateTimestamp = DateUtils.AsUTC(attachment.AppLastUpdateTimestamp);
                    attachment.LastUpdateUserid = attachment.AppLastUpdateUserid;
                    attachment.UserName = UserHelper.GetUserName(attachment.LastUpdateUserid, _context);
                    attachments.Add(attachment);
                }
            }

            return new ObjectResult(
                new HetsResponse(_mapper.Map<List<DigitalFileDto>>(attachments)));
        }

        #endregion

        #region Project Contacts

        /// <summary>
        /// Get contacts associated with a project
        /// </summary>
        /// <remarks>Gets an Projects Contacts</remarks>
        /// <param name="id">id of Project to fetch Contacts for</param>
        [HttpGet]
        [Route("{id}/contacts")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ContactDto>> ProjectsIdContactsGet([FromRoute] int id)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetProject project = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetContacts)
                .First(a => a.ProjectId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<List<ContactDto>>(project.HetContacts.ToList())));
        }

        /// <summary>
        /// Add a project contact
        /// </summary>
        /// <remarks>Adds Project Contact</remarks>
        /// <param name="id">id of Project to add a contact for</param>
        /// <param name="primary">is this the primary contact</param>
        /// <param name="item">Adds to Project Contact</param>
        [HttpPost]
        [Route("{id}/contacts/{primary}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<ContactDto> ProjectsIdContactsPost([FromRoute] int id, [FromBody] ContactDto item, bool primary)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists || item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int contactId;

            // get project record
            HetProject project = _context.HetProjects
                .Include(x => x.HetContacts)
                .First(a => a.ProjectId == id);

            // add or update contact
            if (item.ContactId > 0)
            {
                HetContact contact = project.HetContacts.FirstOrDefault(a => a.ContactId == item.ContactId);

                // not found
                if (contact == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                contactId = item.ContactId;

                contact.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                contact.ProjectId = project.ProjectId;
                contact.Notes = item.Notes;
                contact.Address1 = item.Address1;
                contact.Address2 = item.Address2;
                contact.City = item.City;
                contact.EmailAddress = item.EmailAddress;
                contact.WorkPhoneNumber = item.WorkPhoneNumber;
                contact.FaxPhoneNumber = item.FaxPhoneNumber;
                contact.GivenName = item.GivenName;
                contact.MobilePhoneNumber = item.MobilePhoneNumber;
                contact.PostalCode = item.PostalCode;
                contact.Province = item.Province;
                contact.Surname = item.Surname;
                contact.Role = item.Role;

                if (primary)
                {
                    project.PrimaryContactId = contactId;
                }
            }
            else  // add contact
            {
                HetContact contact = new HetContact
                {
                    ProjectId = project.ProjectId,
                    Notes = item.Notes,
                    Address1 = item.Address1,
                    Address2 = item.Address2,
                    City = item.City,
                    EmailAddress = item.EmailAddress,
                    WorkPhoneNumber = item.WorkPhoneNumber,
                    FaxPhoneNumber = item.FaxPhoneNumber,
                    GivenName = item.GivenName,
                    MobilePhoneNumber = item.MobilePhoneNumber,
                    PostalCode = item.PostalCode,
                    Province = item.Province,
                    Surname = item.Surname,
                    Role = item.Role
                };

                _context.HetContacts.Add(contact);

                _context.SaveChanges();

                contactId = contact.ContactId;

                if (primary)
                {
                    project.PrimaryContactId = contactId;
                }
            }

            _context.SaveChanges();

            // get updated contact record
            HetProject updatedProject = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetContacts)
                .First(a => a.ProjectId == id);

            HetContact updatedContact = updatedProject.HetContacts
                .FirstOrDefault(a => a.ContactId == contactId);

            return new ObjectResult(new HetsResponse(_mapper.Map<ContactDto>(updatedContact)));
        }

        /// <summary>
        /// Update all project contacts
        /// </summary>
        /// <remarks>Replaces an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to replace Contacts for</param>
        /// <param name="items">Replacement Project contacts.</param>
        [HttpPut]
        [Route("{id}/contacts")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<ContactDto>> ProjectsIdContactsPut([FromRoute] int id, [FromBody] ContactDto[] items)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists || items == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get project record
            HetProject project = _context.HetProjects
                .Include(x => x.HetContacts)
                .First(a => a.ProjectId == id);

            // adjust the incoming list
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                if (item != null)
                {
                    bool contactExists = project.HetContacts.Any(x => x.ContactId == item.ContactId);

                    if (contactExists)
                    {
                        HetContact contact = _context.HetContacts.First(x => x.ContactId == item.ContactId);

                        contact.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                        contact.ProjectId = id;
                        contact.Notes = item.Notes;
                        contact.Address1 = item.Address1;
                        contact.Address2 = item.Address2;
                        contact.City = item.City;
                        contact.EmailAddress = item.EmailAddress;
                        contact.FaxPhoneNumber = item.FaxPhoneNumber;
                        contact.GivenName = item.GivenName;
                        contact.MobilePhoneNumber = item.MobilePhoneNumber;
                        contact.PostalCode = item.PostalCode;
                        contact.Province = item.Province;
                        contact.Surname = item.Surname;
                        contact.Role = item.Role;
                    }
                    else
                    {
                        HetContact contact = new HetContact
                        {
                            ProjectId = id,
                            Notes = item.Notes,
                            Address1 = item.Address1,
                            Address2 = item.Address2,
                            City = item.City,
                            EmailAddress = item.EmailAddress,
                            FaxPhoneNumber = item.FaxPhoneNumber,
                            GivenName = item.GivenName,
                            MobilePhoneNumber = item.MobilePhoneNumber,
                            PostalCode = item.PostalCode,
                            Province = item.Province,
                            Surname = item.Surname,
                            Role = item.Role
                        };

                        project.HetContacts.Add(contact);
                    }
                }
            }

            // remove contacts that are no longer attached.
            foreach (HetContact contact in project.HetContacts)
            {
                if (contact != null && items.All(x => x.ContactId != contact.ContactId))
                {
                    _context.HetContacts.Remove(contact);
                }
            }

            // save changes
            _context.SaveChanges();

            // get updated contact records
            HetProject updatedProject = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetContacts)
                .First(a => a.ProjectId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<List<ContactDto>>(updatedProject.HetContacts.ToList())));
        }

        #endregion

        #region Project History

        /// <summary>
        /// Get history associated with a project
        /// </summary>
        /// <remarks>Returns History for a particular Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        [HttpGet]
        [Route("{id}/history")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<History>> ProjectsIdHistoryGet([FromRoute] int id, [FromQuery] int? offset, [FromQuery] int? limit)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            return new ObjectResult(new HetsResponse(ProjectHelper.GetHistoryRecords(id, offset, limit, _context)));
        }

        /// <summary>
        /// Create project history
        /// </summary>
        /// <remarks>Add a History record to the Project</remarks>
        /// <param name="id">id of Project to fetch History for</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/history")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<History> ProjectsIdHistoryPost(
            [FromRoute] int id, 
            [FromBody] History item)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            if (exists)
            {
                HetHistory history = new()
                {
                    HistoryText = item.HistoryText,
                    CreatedDate = DateTime.UtcNow,
                    ProjectId = id
                };

                _context.HetHistories.Add(history);
                _context.SaveChanges();
            }

            return new ObjectResult(
                new HetsResponse(ProjectHelper.GetHistoryRecords(id, null, null, _context)));
        }

        #endregion

        #region Project Note Records

        /// <summary>
        /// Get note records associated with project
        /// </summary>
        /// <param name="id">id of Project to fetch Notes for</param>
        [HttpGet]
        [Route("{id}/notes")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdNotesGet([FromRoute] int id)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetProject project = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetNotes)
                .First(x => x.ProjectId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in project.HetNotes)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<NoteDto>>(notes)));
        }

        /// <summary>
        /// Update or create a note associated with a project
        /// </summary>
        /// <remarks>Update a Projects Notes</remarks>
        /// <param name="id">id of Project to update Notes for</param>
        /// <param name="item">Project Note</param>
        [HttpPost]
        [Route("{id}/note")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<NoteDto>> ProjectsIdNotePost([FromRoute] int id, [FromBody] NoteDto item)
        {
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add or update note
            if (item.NoteId > 0)
            {
                // get note
                HetNote note = _context.HetNotes.FirstOrDefault(a => a.NoteId == item.NoteId);

                // not found
                if (note == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                note.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                note.ProjectId = id;
                note.Text = item.Text;
                note.IsNoLongerRelevant = item.IsNoLongerRelevant;
            }
            else  // add note
            {
                HetNote note = new HetNote
                {
                    ProjectId = id,
                    Text = item.Text,
                    IsNoLongerRelevant = item.IsNoLongerRelevant
                };

                _context.HetNotes.Add(note);
            }

            _context.SaveChanges();

            // return updated note records
            HetProject project = _context.HetProjects.AsNoTracking()
                .Include(x => x.HetNotes)
                .First(x => x.ProjectId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in project.HetNotes)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<NoteDto>>(notes)));
        }

        #endregion

        #region Get Project by Name and District

        /// <summary>
        /// Get a project by name and district (active only)
        /// </summary>
        /// <param name="name">name of the project to find</param>
        [HttpPost]
        [Route("projectsByName")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ProjectLite>> ProjectsGetByName([FromBody] string name)
        {
            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            HetDistrict district = _context.HetDistricts.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // find agreements
            IQueryable<HetProject> data = _context.HetProjects.AsNoTracking()
                .Include(x => x.ProjectStatusType)
                .Include(x => x.District.Region)
                .Include(x => x.PrimaryContact)
                .Include(x => x.HetRentalAgreements)
                .Include(x => x.HetRentalRequests)
                .Where(x => x.DistrictId.Equals(districtId));

            if (name != null)
            {
                // allow for case insensitive search of project name
                data = data.Where(x => x.Name.ToUpper() == name.ToUpper());
            }

            // convert Project Model to the "ProjectLite" Model
            List<ProjectLite> result = new List<ProjectLite>();

            foreach (HetProject item in data)
            {
                result.Add(_projectRepo.ToLiteModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion
    }
}
