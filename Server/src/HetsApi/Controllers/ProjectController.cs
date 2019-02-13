using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Project Controller
    /// </summary>
    [Route("api/projects")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ProjectController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;

        public ProjectController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        /// Get project by id
        /// </summary>
        /// <param name="id">id of Project to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("ProjectsIdGet")]
        [SwaggerResponse(200, type: typeof(HetProject))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdGet([FromRoute]int id)
        {
            // get current district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            return new ObjectResult(new HetsResponse(ProjectHelper.GetRecord(id, _context, districtId)));
        }

        /// <summary>
        /// Update project
        /// </summary>
        /// <param name="id">id of Project to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("ProjectsIdPut")]
        [SwaggerResponse(200, type: typeof(HetProject))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdPut([FromRoute]int id, [FromBody]HetProject item)
        {
            if (item == null || id != item.ProjectId)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetProject project = _context.HetProject.First(a => a.ProjectId == id);

            int? statusId = StatusHelper.GetStatusId(item.Status, "projectStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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
            return new ObjectResult(new HetsResponse(ProjectHelper.GetRecord(id, _context)));
        }

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("ProjectsPost")]
        [SwaggerResponse(200, type: typeof(HetProject))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsPost([FromBody]HetProject item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(item.Status, "projectStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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

            _context.HetProject.Add(project);

            // save record
            _context.SaveChanges();

            int id = project.ProjectId;

            // retrieve updated project record to return to ui
            return new ObjectResult(new HetsResponse(ProjectHelper.GetRecord(id, _context)));
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
        [SwaggerOperation("ProjectsSearchGet")]
        [SwaggerResponse(200, type: typeof(List<ProjectLite>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsSearchGet([FromQuery]string districts, 
            [FromQuery]string project, [FromQuery]bool? hasRequests, [FromQuery]bool? hasHires, 
            [FromQuery]string status, [FromQuery]string projectNumber,
            [FromQuery]string fiscalYear)
        {
            int?[] districtTokens = ArrayHelper.ParseIntArray(districts);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            IQueryable<HetProject> data = _context.HetProject.AsNoTracking()
                .Include(x => x.ProjectStatusType)
                .Include(x => x.District.Region)
                .Include(x => x.PrimaryContact)
                .Include(x => x.HetRentalAgreement)
                .Include(x => x.HetRentalRequest)
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
                result.Add(ProjectHelper.ToLiteModel(item));
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
        [SwaggerOperation("ProjectsGet")]
        [SwaggerResponse(200, type: typeof(List<HetProject>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsGet()
        {
            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatus.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;
            if (fiscalYear == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // fiscal year in the status table stores the "start" of the year
            DateTime fiscalYearStart = new DateTime((int)fiscalYear, 3, 31);

            // HETS-1005 - Time entry tab search issues
            // * we need retrieve all projects that have been created this year
            //   since users can add time records to closed projects
            IQueryable<HetProject> projects = _context.HetProject.AsNoTracking()
                .Where(x => x.DistrictId.Equals(districtId) &&
                            x.AppCreateTimestamp > fiscalYearStart);

            // convert Project Model to the "ProjectLite" Model
            List<ProjectLite> result = new List<ProjectLite>();

            foreach (HetProject item in projects)
            {
                result.Add(ProjectHelper.ToLiteModel(item));
            }

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
        [SwaggerOperation("ProjectsIdRentalAgreementsGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreement>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdRentalAgreementsGet([FromRoute]int id)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            List<HetRentalAgreement> agreements = _context.HetProject.AsNoTracking()
                .Include(x => x.ProjectStatusType)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(e => e.Equipment)
                        .ThenInclude(d => d.DistrictEquipmentType)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(e => e.Equipment)
                        .ThenInclude(a => a.HetEquipmentAttachment)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(e => e.RentalAgreementStatusType)
                .First(x => x.ProjectId == id)
                .HetRentalAgreement
                .ToList();

            return new ObjectResult(new HetsResponse(agreements));
        }

        /// <summary>
        /// Update a rental agreement by cloning a previous project rental agreement
        /// </summary>
        /// <param name="id">Project id</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/rentalAgreementClone")]
        [SwaggerOperation("ProjectsRentalAgreementClonePost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsRentalAgreementClonePost([FromRoute]int id, [FromBody]ProjectRentalAgreementClone item)
        {
            // not found
            if (item == null || id != item.ProjectId) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get all agreements for this project
            HetProject project = _context.HetProject
                .Include(x => x.ProjectStatusType)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(y => y.HetRentalAgreementRate)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(y => y.HetRentalAgreementCondition)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(e => e.RentalAgreementStatusType)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(y => y.HetTimeRecord)
                .First(a => a.ProjectId == id);

            List<HetRentalAgreement> agreements = project.HetRentalAgreement.ToList();

            // check that the rental agreements exist
            exists = agreements.Any(a => a.RentalAgreementId == item.RentalAgreementId);

            // (RENTAL AGREEMENT) not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // check that the rental agreement to clone exist
            exists = agreements.Any(a => a.RentalAgreementId == item.AgreementToCloneId);

            // (RENTAL AGREEMENT) not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-11", ErrorViewModel.GetDescription("HETS-11", _configuration)));

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
                return new ObjectResult(new HetsResponse("HETS-12", ErrorViewModel.GetDescription("HETS-12", _configuration)));
            }

            if (agreements[newRentalAgreementIndex].HetTimeRecord != null &&
                agreements[newRentalAgreementIndex].HetTimeRecord.Count > 0)
            {
                // (RENTAL AGREEMENT) has time records
                return new ObjectResult(new HetsResponse("HETS-13", ErrorViewModel.GetDescription("HETS-13", _configuration)));
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
            agreements[newRentalAgreementIndex].HetRentalAgreementRate = null;

            foreach (HetRentalAgreementRate rate in agreements[agreementToCloneIndex].HetRentalAgreementRate)
            {
                HetRentalAgreementRate temp = new HetRentalAgreementRate
                {
                    RentalAgreementId = id,
                    Comment = rate.Comment,
                    ComponentName = rate.ComponentName,
                    Rate = rate.Rate,
                    Overtime = rate.Overtime,
                    Active = rate.Active,
                    IsIncludedInTotal = rate.IsIncludedInTotal
                };

                if (agreements[newRentalAgreementIndex].HetRentalAgreementRate == null)
                {
                    agreements[newRentalAgreementIndex].HetRentalAgreementRate = new List<HetRentalAgreementRate>();
                }

                agreements[newRentalAgreementIndex].HetRentalAgreementRate.Add(temp);
            }

            // update overtime rates (and add if they don't exist)
            List<HetProvincialRateType> overtime = _context.HetProvincialRateType.AsNoTracking()
                .Where(x => x.Overtime)
                .ToList();

            foreach (HetProvincialRateType overtimeRate in overtime)
            {
                bool found = false;

                if (agreements[newRentalAgreementIndex] != null &&
                    agreements[newRentalAgreementIndex].HetRentalAgreementRate != null)
                {
                    found = agreements[newRentalAgreementIndex].HetRentalAgreementRate.Any(x => x.ComponentName == overtimeRate.RateType);
                }

                if (found)
                {
                    HetRentalAgreementRate rate = agreements[newRentalAgreementIndex].HetRentalAgreementRate
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

                    if (agreements[newRentalAgreementIndex].HetRentalAgreementRate == null)
                    {
                        agreements[newRentalAgreementIndex].HetRentalAgreementRate = new List<HetRentalAgreementRate>();
                    }

                    agreements[newRentalAgreementIndex].HetRentalAgreementRate.Add(newRate);
                }
            }

            // remove non-existent overtime rates
            List<string> remove =
                (from overtimeRate in agreements[newRentalAgreementIndex].HetRentalAgreementRate
                    where overtimeRate.Overtime
                    let found = overtime.Any(x => x.RateType == overtimeRate.ComponentName)
                    where !found
                    select overtimeRate.ComponentName).ToList();

            if (remove.Count > 0 &&
                agreements[newRentalAgreementIndex] != null &&
                agreements[newRentalAgreementIndex].HetRentalAgreementRate != null)
            {
                foreach (string component in remove)
                {
                    agreements[newRentalAgreementIndex].HetRentalAgreementRate.Remove(
                        agreements[newRentalAgreementIndex].HetRentalAgreementRate.First(x => x.ComponentName == component));
                }
            }

            // update conditions
            agreements[newRentalAgreementIndex].HetRentalAgreementCondition = null;

            foreach (HetRentalAgreementCondition condition in agreements[agreementToCloneIndex].HetRentalAgreementCondition)
            {
                HetRentalAgreementCondition temp = new HetRentalAgreementCondition
                {
                    Comment = condition.Comment,
                    ConditionName = condition.ConditionName
                };

                if (agreements[newRentalAgreementIndex].HetRentalAgreementCondition == null)
                {
                    agreements[newRentalAgreementIndex].HetRentalAgreementCondition = new List<HetRentalAgreementCondition>();
                }

                agreements[newRentalAgreementIndex].HetRentalAgreementCondition.Add(temp);
            }

            // save the changes
            _context.SaveChanges();

            // ******************************************************************
            // return update rental agreement to update the screen
            // ******************************************************************
            HetRentalAgreement result = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.HetEquipmentAttachment)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(p => p.District.Region)
                .Include(x => x.HetRentalAgreementCondition)
                .Include(x => x.HetRentalAgreementRate)
                .Include(x => x.HetTimeRecord)
                .First(a => a.RentalAgreementId == item.RentalAgreementId);

            return new ObjectResult(new HetsResponse(result));
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
        [SwaggerOperation("ProjectIdTimeRecordsGet")]
        [SwaggerResponse(200, type: typeof(List<HetTimeRecord>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult HetProjectIdTimeRecordsGet([FromRoute]int id)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get current district and fiscal year
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatus.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;
            if (fiscalYear == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // fiscal year in the status table stores the "start" of the year
            DateTime fiscalYearStart = new DateTime((int)fiscalYear, 4, 1);

            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(t => t.HetTimeRecord)
                .First(x => x.ProjectId == id);

            // create a single array of all time records
            List<HetTimeRecord> timeRecords = new List<HetTimeRecord>();

            foreach (HetRentalAgreement rentalAgreement in project.HetRentalAgreement)
            {
                // only add records from this fiscal year
                timeRecords.AddRange(rentalAgreement.HetTimeRecord.Where(x => x.WorkedDate >= fiscalYearStart));
            }

            return new ObjectResult(new HetsResponse(timeRecords));
        }

        /// <summary>
        /// Add a project time record
        /// </summary>
        /// <remarks>Adds Project Time Records</remarks>
        /// <param name="id">id of Project to add a time record for</param>
        /// <param name="item">Adds to Project Time Records</param>
        [HttpPost]
        [Route("{id}/timeRecord")]
        [SwaggerOperation("ProjectsIdTimeRecordsPost")]
        [SwaggerResponse(200, type: typeof(HetTimeRecord))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdTimeRecordsPost([FromRoute]int id, [FromBody]HetTimeRecord item)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get project record
            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(t => t.HetTimeRecord)
                .First(x => x.ProjectId == id);

            List<HetRentalAgreement> agreements = project.HetRentalAgreement.ToList();

            // ******************************************************************
            // must have a valid rental agreement id
            // ******************************************************************
            if (item.RentalAgreement.RentalAgreementId == 0)
            {
                // (RENTAL AGREEMENT) record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            exists = agreements.Any(a => a.RentalAgreementId == item.RentalAgreement.RentalAgreementId);

            if (!exists)
            {
                // (RENTAL AGREEMENT) record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // ******************************************************************
            // add or update time record
            // ******************************************************************
            int rentalAgreementId = item.RentalAgreement.RentalAgreementId;

            // set the time period type id
            int? timePeriodTypeId = StatusHelper.GetTimePeriodId(item.TimePeriod, _context);
            if (timePeriodTypeId == null) throw new DataException("Time Period Id cannot be null");

            if (item.TimeRecordId > 0)
            {
                // get time record
                HetTimeRecord time = _context.HetTimeRecord.FirstOrDefault(x => x.TimeRecordId == item.TimeRecordId);

                // not found
                if (time == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                time.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                time.RentalAgreementId = rentalAgreementId;
                time.EnteredDate = DateTime.UtcNow;
                time.Hours = item.Hours;
                time.TimePeriod = item.TimePeriod;
                time.TimePeriodTypeId = (int)timePeriodTypeId;
                time.WorkedDate = item.WorkedDate;
            }
            else // add time record
            {
                HetTimeRecord time = new HetTimeRecord
                {
                    RentalAgreementId = rentalAgreementId,
                    EnteredDate = DateTime.UtcNow,
                    Hours = item.Hours,
                    TimePeriod = item.TimePeriod,
                    TimePeriodTypeId = (int)timePeriodTypeId,
                    WorkedDate = item.WorkedDate
                };

                _context.HetTimeRecord.Add(time);
            }

            // save record
            _context.SaveChanges();

            // *************************************************************
            // return updated time records
            // *************************************************************
            project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(t => t.HetTimeRecord)
                .First(x => x.ProjectId == id);

            // create a single array of all time records
            List<HetTimeRecord> timeRecords = new List<HetTimeRecord>();

            foreach (HetRentalAgreement rentalAgreement in project.HetRentalAgreement)
            {
                timeRecords.AddRange(rentalAgreement.HetTimeRecord);
            }

            return new ObjectResult(new HetsResponse(timeRecords));
        }

        /// <summary>
        /// Update or create an array of time records associated with a project
        /// </summary>
        /// <remarks>Adds Project Time Records</remarks>
        /// <param name="id">id of Project to add a time record for</param>
        /// <param name="items">Array of Project Time Records</param>
        [HttpPost]
        [Route("{id}/timeRecords")]
        [SwaggerOperation("ProjectsIdTimeRecordsBulkPostAsync")]
        [SwaggerResponse(200, type: typeof(HetTimeRecord))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdTimeRecordsBulkPostAsync([FromRoute]int id, [FromBody]HetTimeRecord[] items)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get project record
            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetRentalAgreement)
                .ThenInclude(t => t.HetTimeRecord)
                .First(x => x.ProjectId == id);

            List<HetRentalAgreement> agreements = project.HetRentalAgreement.ToList();

            // ******************************************************************
            // process each time record
            // ******************************************************************
            foreach (HetTimeRecord item in items)
            {
                // must have a valid rental agreement id
                if (item.RentalAgreement.RentalAgreementId == 0)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                exists = agreements.Any(a => a.RentalAgreementId == item.RentalAgreement.RentalAgreementId);

                if (!exists)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // add or update time record
                int rentalAgreementId = item.RentalAgreement.RentalAgreementId;

                // set the time period type id
                int? timePeriodTypeId = StatusHelper.GetTimePeriodId(item.TimePeriod, _context);
                if (timePeriodTypeId == null) throw new DataException("Time Period Id cannot be null");

                if (item.TimeRecordId > 0)
                {
                    // get time record
                    HetTimeRecord time = _context.HetTimeRecord.FirstOrDefault(x => x.TimeRecordId == item.TimeRecordId);

                    // not found
                    if (time == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                    time.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    time.RentalAgreementId = rentalAgreementId;
                    time.EnteredDate = DateTime.UtcNow;
                    time.Hours = item.Hours;
                    time.TimePeriod = item.TimePeriod;
                    time.TimePeriodTypeId = (int)timePeriodTypeId;
                    time.WorkedDate = item.WorkedDate;
                }
                else // add time record
                {
                    HetTimeRecord time = new HetTimeRecord
                    {
                        RentalAgreementId = rentalAgreementId,
                        EnteredDate = DateTime.UtcNow,
                        Hours = item.Hours,
                        TimePeriod = item.TimePeriod,
                        TimePeriodTypeId = (int)timePeriodTypeId,
                        WorkedDate = item.WorkedDate
                    };

                    _context.HetTimeRecord.Add(time);
                }

                // save record
                _context.SaveChanges();
            }

            // *************************************************************
            // return updated time records
            // *************************************************************
            project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetRentalAgreement)
                .ThenInclude(t => t.HetTimeRecord)
                .First(x => x.ProjectId == id);

            // create a single array of all time records
            List<HetTimeRecord> timeRecords = new List<HetTimeRecord>();

            foreach (HetRentalAgreement rentalAgreement in project.HetRentalAgreement)
            {
                timeRecords.AddRange(rentalAgreement.HetTimeRecord);
            }

            return new ObjectResult(new HetsResponse(timeRecords));
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
        [SwaggerOperation("ProjectsIdEquipmentGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreement>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdEquipmentGet([FromRoute]int id)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(e => e.Equipment)
                        .ThenInclude(o => o.Owner)
                            .ThenInclude(c => c.PrimaryContact)
                .First(x => x.ProjectId == id);

            return new ObjectResult(new HetsResponse(project.HetRentalAgreement));
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
        [SwaggerOperation("ProjectsIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<HetDigitalFile>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdAttachmentsGet([FromRoute]int id)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetDigitalFile)
                .First(a => a.ProjectId == id);

            // extract the attachments and update properties for UI
            List<HetDigitalFile> attachments = new List<HetDigitalFile>();

            foreach (HetDigitalFile attachment in project.HetDigitalFile)
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

        #region Project Contacts

        /// <summary>
        /// Get contacts associated with a project
        /// </summary>
        /// <remarks>Gets an Projects Contacts</remarks>
        /// <param name="id">id of Project to fetch Contacts for</param>
        [HttpGet]
        [Route("{id}/contacts")]
        [SwaggerOperation("ProjectsIdContactsGet")]
        [SwaggerResponse(200, type: typeof(List<HetContact>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdContactsGet([FromRoute]int id)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetContact)
                .First(a => a.ProjectId == id);

            return new ObjectResult(new HetsResponse(project.HetContact.ToList()));
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
        [SwaggerOperation("ProjectsIdContactsPost")]
        [SwaggerResponse(200, type: typeof(HetContact))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdContactsPost([FromRoute]int id, [FromBody]HetContact item, bool primary)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists || item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int contactId;

            // get project record
            HetProject project = _context.HetProject
                .Include(x => x.HetContact)
                .First(a => a.ProjectId == id);

            // add or update contact
            if (item.ContactId > 0)
            {
                HetContact contact = project.HetContact.FirstOrDefault(a => a.ContactId == item.ContactId);

                // not found
                if (contact == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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

                _context.HetContact.Add(contact);

                _context.SaveChanges();

                contactId = contact.ContactId;

                if (primary)
                {
                    project.PrimaryContactId = contactId;
                }
            }

            _context.SaveChanges();

            // get updated contact record
            HetProject updatedProject = _context.HetProject.AsNoTracking()
                .Include(x => x.HetContact)
                .First(a => a.ProjectId == id);

            HetContact updatedContact = updatedProject.HetContact
                .FirstOrDefault(a => a.ContactId == contactId);

            return new ObjectResult(new HetsResponse(updatedContact));
        }

        /// <summary>
        /// Update all project contacts
        /// </summary>
        /// <remarks>Replaces an Project&#39;s Contacts</remarks>
        /// <param name="id">id of Project to replace Contacts for</param>
        /// <param name="items">Replacement Project contacts.</param>
        [HttpPut]
        [Route("{id}/contacts")]
        [SwaggerOperation("ProjectsIdContactsPut")]
        [SwaggerResponse(200, type: typeof(List<HetContact>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdContactsPut([FromRoute]int id, [FromBody]HetContact[] items)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists || items == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get project record
            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetContact)
                .First(a => a.ProjectId == id);

            // adjust the incoming list
            for (int i = 0; i < items.Length; i++)
            {
                HetContact item = items[i];

                if (item != null)
                {
                    bool contactExists = _context.HetContact.Any(x => x.ContactId == item.ContactId);

                    if (contactExists)
                    {
                        HetContact temp = _context.HetContact.First(x => x.ContactId == item.ContactId);

                        temp.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                        temp.ProjectId = id;
                        temp.Notes = item.Notes;
                        temp.Address1 = item.Address1;
                        temp.Address2 = item.Address2;
                        temp.City = item.City;
                        temp.EmailAddress = item.EmailAddress;
                        temp.FaxPhoneNumber = item.FaxPhoneNumber;
                        temp.GivenName = item.GivenName;
                        temp.MobilePhoneNumber = item.MobilePhoneNumber;
                        temp.PostalCode = item.PostalCode;
                        temp.Province = item.Province;
                        temp.Surname = item.Surname;
                        temp.Role = item.Role;

                        items[i] = temp;
                    }
                    else
                    {
                        HetContact temp = new HetContact
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

                        project.HetContact.Add(temp);
                        items[i] = temp;
                    }
                }
            }

            // remove contacts that are no longer attached.
            foreach (HetContact contact in project.HetContact)
            {
                if (contact != null && items.All(x => x.ContactId != contact.ContactId))
                {
                    _context.HetContact.Remove(contact);
                }
            }

            // save changes
            _context.SaveChanges();

            // get updated contact records
            HetProject updatedProject = _context.HetProject.AsNoTracking()
                .Include(x => x.HetContact)
                .First(a => a.ProjectId == id);

            return new ObjectResult(new HetsResponse(updatedProject.HetContact.ToList()));
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
        [SwaggerOperation("ProjectsIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HetHistory>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
        [SwaggerOperation("ProjectsIdHistoryPost")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdHistoryPost([FromRoute]int id, [FromBody]HetHistory item)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            if (exists)
            {
                HetHistory history = new HetHistory
                {
                    HistoryId = 0,
                    HistoryText = item.HistoryText,
                    CreatedDate = item.CreatedDate,
                    ProjectId = id
                };

                _context.HetHistory.Add(history);
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(ProjectHelper.GetHistoryRecords(id, null, null, _context)));
        }

        #endregion

        #region Project Note Records

        /// <summary>
        /// Get note records associated with project
        /// </summary>
        /// <param name="id">id of Project to fetch Notes for</param>
        [HttpGet]
        [Route("{id}/notes")]
        [SwaggerOperation("ProjectsIdNotesGet")]
        [SwaggerResponse(200, type: typeof(List<HetNote>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdNotesGet([FromRoute]int id)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.ProjectId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in project.HetNote)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(notes));
        }

        /// <summary>
        /// Update or create a note associated with a project
        /// </summary>
        /// <remarks>Update a Projects Notes</remarks>
        /// <param name="id">id of Project to update Notes for</param>
        /// <param name="item">Project Note</param>
        [HttpPost]
        [Route("{id}/note")]
        [SwaggerOperation("ProjectsIdNotePost")]
        [SwaggerResponse(200, type: typeof(HetNote))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsIdNotePost([FromRoute]int id, [FromBody]HetNote item)
        {
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add or update note
            if (item.NoteId > 0)
            {
                // get note
                HetNote note = _context.HetNote.FirstOrDefault(a => a.NoteId == item.NoteId);

                // not found
                if (note == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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

                _context.HetNote.Add(note);
            }

            _context.SaveChanges();

            // return updated note records
            HetProject project = _context.HetProject.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.ProjectId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in project.HetNote)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(notes));
        }

        #endregion

        #region Get Project by Name and District

        /// <summary>
        /// Get a project by name and district (active only)
        /// </summary>
        /// <param name="name">name of the project to find</param>
        [HttpPost]
        [Route("projectsByName")]
        [SwaggerOperation("ProjectsGetByName")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectsGetByName([FromBody]string name)
        {
            // get current users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            HetDistrict district = _context.HetDistrict.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId.Equals(districtId));

            if (district == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // find agreements
            IQueryable<HetProject> data = _context.HetProject.AsNoTracking()
                .Include(x => x.ProjectStatusType)
                .Include(x => x.District.Region)
                .Include(x => x.PrimaryContact)
                .Include(x => x.HetRentalAgreement)
                .Include(x => x.HetRentalRequest)
                .Where(x => x.DistrictId.Equals(districtId));

            if (name != null)
            {
                // allow for case insensitive search of project name
                data = data.Where(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase));
            }

            // convert Project Model to the "ProjectLite" Model
            List<ProjectLite> result = new List<ProjectLite>();

            foreach (HetProject item in data)
            {
                result.Add(ProjectHelper.ToLiteModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion
    }
}
