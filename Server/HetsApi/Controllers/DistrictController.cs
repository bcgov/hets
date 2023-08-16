using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using HetsData.Hangfire;
using Hangfire.Storage;
using Hangfire.Common;
using HetsData.Dtos;
using AutoMapper;
using HetsCommon;

namespace HetsApi.Controllers
{
    /// <summary>
    /// District Controller
    /// </summary>
    [Route("api/districts")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DistrictController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAnnualRollover _annualRollover;
        private IMonitoringApi _monitoringApi;

        public DistrictController(DbAppContext context, IConfiguration configuration, IMapper mapper, IAnnualRollover annualRollover)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _annualRollover = annualRollover;
            _monitoringApi = JobStorage.Current.GetMonitoringApi();
        }

        /// <summary>
        /// Get all districts
        /// </summary>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public virtual ActionResult<List<DistrictDto>> DistrictsGet()
        {
            List<HetDistrict> districts = _context.HetDistricts.AsNoTracking()
                .Include(x => x.Region)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<DistrictDto>>(districts)));
        }

        #region Owners by District

        /// <summary>
        /// Get all owners by district
        /// </summary>
        [HttpGet]
        [Route("{id}/owners")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<OwnerDto>> DistrictOwnersGet([FromRoute]int id)
        {
            bool exists = _context.HetDistricts.Any(a => a.DistrictId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            List<HetOwner> owners = _context.HetOwners.AsNoTracking()
                .Where(x => x.LocalArea.ServiceArea.District.DistrictId == id)
                .OrderBy(x => x.OrganizationName)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<OwnerDto>>(owners)));
        }

        #endregion

        #region Local Areas by District

        /// <summary>
        /// Get all local areas by district
        /// </summary>
        [HttpGet]
        [Route("{id}/localAreas")]
        [AllowAnonymous]
        public virtual ActionResult<List<LocalAreaDto>> DistrictLocalAreasGet([FromRoute]int id)
        {
            var now = DateTime.UtcNow;
            var nowDate = DateUtils.ConvertPacificToUtcTime(
                new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));
            List<HetLocalArea> localAreas = _context.HetLocalAreas.AsNoTracking()
                .Where(x => 
                    x.ServiceArea.District.DistrictId == id 
                    && x.StartDate <= nowDate 
                    && (x.EndDate > nowDate || x.EndDate == null))
                .OrderBy(x => x.Name)
                .ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<LocalAreaDto>>(localAreas)));
        }

        #endregion

        #region District Rollover

        /// <summary>
        /// Get district rollover status
        /// </summary>
        [HttpGet]
        [Route("{id}/rolloverStatus")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<DistrictStatusDto> RolloverStatusGet([FromRoute]int id)
        {
            var typeFullName = "HetsData.Hangfire.AnnualRollover";
            var methodName = "AnnualRolloverJob";
            var rolloverJob = $"{typeFullName}-{methodName}-{id}";

            var jobProcessing = _monitoringApi.ProcessingJobs(0, 10000)
                .ToList();

            var jobExists = jobProcessing.Any(x => GetJobFingerprint(x.Value.Job) == rolloverJob);

            var status = _annualRollover.GetRecord(id);

            var progress = _context.HetRolloverProgresses.FirstOrDefault(a => a.DistrictId == id);

            // not found
            if (progress == null) 
                return new ObjectResult(
                    new HetsResponse(
                        new RolloverProgressDto { DistrictId = id, ProgressPercentage = null }));

            if (!jobExists)
            {
                return new ObjectResult(new HetsResponse(status));
            }

            // get status of current district
            return new ObjectResult(
                new HetsResponse(
                    new RolloverProgressDto { DistrictId = id, ProgressPercentage = progress.ProgressPercentage }));
        }

        private string GetJobFingerprint(Hangfire.Common.Job job)
        {
            var args = "";

            if (job.Args.Count > 0)
            {
                args = job.Args[0].ToString();
            }

            return $"{job.Type.FullName}-{job.Method.Name}-{args}";
        }


        /// <summary>
        /// Dismiss district rollover status message
        /// </summary>
        [HttpPost]
        [Route("{id}/dismissRolloverMessage")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<DistrictStatusDto> DismissRolloverMessagePost([FromRoute]int id)
        {
            bool exists = _context.HetDistrictStatuses.Any(a => a.DistrictId == id);

            // not found - return new status record
            if (!exists) return NotFound();

            // get record and update
            HetDistrictStatus status = _context.HetDistrictStatuses
                .First(a => a.DistrictId == id);

            // ensure the process is complete
            if (status.DisplayRolloverMessage == true &&
                status.ProgressPercentage != null &&
                status.ProgressPercentage == 100)
            {
                status.ProgressPercentage = null;
                status.DisplayRolloverMessage = false;
            }

            var progress = _context.HetRolloverProgresses.FirstOrDefault(a => a.DistrictId == id);

            if (progress == null)
            {
                progress = new HetRolloverProgress { DistrictId = id, ProgressPercentage = null };
                _context.HetRolloverProgresses.Add(progress);
            }

            progress.ProgressPercentage = null;

            _context.SaveChanges();

            // get status of current district
            return new ObjectResult(new HetsResponse(_annualRollover.GetRecord(id)));
        }

        /// <summary>
        /// Start the annual rollover process
        /// </summary>
        [HttpGet]
        [Route("{id}/annualRollover")]
        [RequiresPermission(HetPermission.DistrictRollover)]
        public virtual ActionResult<DistrictStatusDto> AnnualRolloverGet([FromRoute]int id)
        {
            bool exists = _context.HetDistricts.Any(a => a.DistrictId == id);

            // not found
            if (!exists) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // determine the current fiscal year
            DateTime fiscalStart;
            DateTime now = DateTime.Now;
            if (now.Month == 1 || now.Month == 2 || now.Month == 3)
            {
                fiscalStart = DateUtils.ConvertPacificToUtcTime(
                    new DateTime(now.AddYears(-1).Year, 4, 1, 0, 0, 0));
            }
            else
            {
                fiscalStart = DateUtils.ConvertPacificToUtcTime(
                    new DateTime(now.Year, 4, 1, 0, 0, 0));
            }

            // get record and ensure it isn't already processing
            var status = _annualRollover.GetRecord(id);

            if (status == null)
            {
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            if (status.CurrentFiscalYear == fiscalStart.Year)
            {
                // return - cannot rollover again
                return new ObjectResult(status);
            }

            if (status.DisplayRolloverMessage == true ||
                (status.ProgressPercentage != null && status.ProgressPercentage > 0))
            {
                // return already active
                return new ObjectResult(status);
            }

            // serialize scoring rules from config into json string
            IConfigurationSection scoringRules = _configuration.GetSection("SeniorityScoringRules");
            string seniorityScoringRules = GetConfigJson(scoringRules);

            // queue the job
            BackgroundJob.Enqueue<AnnualRollover>(x => x.AnnualRolloverJob(id, seniorityScoringRules));
            var progressDto = _annualRollover.KickoffProgress(id);

            return new ObjectResult(progressDto);
        }

        #endregion

        #region Get Scoring Rules

        private string GetConfigJson(IConfigurationSection scoringRules)
        {
            string jsonString = RecurseConfigJson(scoringRules);

            if (jsonString.EndsWith("},"))
            {
                jsonString = jsonString.Substring(0, jsonString.Length - 1);
            }

            return jsonString;
        }

        private string RecurseConfigJson(IConfigurationSection scoringRules)
        {
            StringBuilder temp = new StringBuilder();

            temp.Append("{");

            // check for children
            foreach (IConfigurationSection section in scoringRules.GetChildren())
            {
                temp.Append(@"""" + section.Key + @"""" + ":");

                if (section.Value == null)
                {
                    temp.Append(RecurseConfigJson(section));
                }
                else
                {
                    temp.Append(@"""" + section.Value + @"""" + ",");
                }
            }

            string jsonString = temp.ToString();

            if (jsonString.EndsWith(","))
            {
                jsonString = jsonString.Substring(0, jsonString.Length - 1);
            }

            jsonString = jsonString + "},";
            return jsonString;
        }

        #endregion

        #region Fiscal Years by District

        /// <summary>
        /// Get all fiscal years by district
        /// </summary>
        [HttpGet]
        [Route("{id}/fiscalYears")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<string>> DistrictFiscalYearsGet([FromRoute]int id)
        {
            bool exists = _context.HetDistricts.Any(a => a.DistrictId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

             HetDistrictStatus status = _context.HetDistrictStatuses
                .AsNoTracking()
                .FirstOrDefault(x => x.DistrictId == id);

            if (status == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            List<string> fiscalYears = new List<string>();

            string current = $"{status.CurrentFiscalYear.ToString()}/{(status.CurrentFiscalYear + 1).ToString()}";
            string next = $"{status.NextFiscalYear.ToString()}/{(status.NextFiscalYear + 1).ToString()}";

            fiscalYears.Add(current);
            fiscalYears.Add(next);

            return new ObjectResult(new HetsResponse(fiscalYears));
        }

        #endregion
    }
}
