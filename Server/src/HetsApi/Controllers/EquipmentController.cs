using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;
using HetsReport;
using Hangfire;
using System.Text;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Equipment Controller
    /// </summary>
    [Route("api/equipment")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentController : Controller
    {
        private readonly Object _thisLock = new Object();
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly ILogger _logger;

        public EquipmentController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger<EquipmentController>();

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }

        /// <summary>
        /// Get equipment by id
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("EquipmentIdGet")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdGet([FromRoute]int id)
        {
            return new ObjectResult(new HetsResponse(EquipmentHelper.GetRecord(id, _context, _configuration)));
        }

        /// <summary>
        /// Get all approved equipment for this district (lite)
        /// </summary>
        [HttpGet]
        [Route("lite")]
        [SwaggerOperation("EquipmentGetLite")]
        [SwaggerResponse(200, type: typeof(List<EquipmentExtraLite>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentGetLite()
        {
            // get user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get approved status
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get all approved equipment for this district
            IEnumerable<EquipmentExtraLite> equipment = _context.HetEquipment.AsNoTracking()
                .Where(x => x.LocalArea.ServiceArea.DistrictId == districtId &&
                            x.EquipmentStatusTypeId == statusId)
                .OrderBy(x => x.EquipmentCode)
                .Select(x => new EquipmentExtraLite
                {
                    EquipmentCode = x.EquipmentCode,
                    Id = x.EquipmentId,
                });

            return new ObjectResult(new HetsResponse(equipment));
        }

        /// <summary>
        /// Get all equipment for this district that are associated with a project (lite)
        /// </summary>
        [HttpGet]
        [Route("liteTs")]
        [SwaggerOperation("EquipmentGetLiteTs")]
        [SwaggerResponse(200, type: typeof(List<EquipmentLiteList>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentGetLiteTs()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get active status
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatus.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;
            if (fiscalYear == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // fiscal year in the status table stores the "start" of the year
            DateTime fiscalYearStart = new DateTime((int)fiscalYear, 3, 31);

            // get all active owners for this district (and any projects they're associated with)
            IEnumerable<EquipmentLiteList> equipment = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Equipment)
                .Where(x => x.Equipment.LocalArea.ServiceArea.DistrictId == districtId &&
                            x.Equipment.EquipmentStatusTypeId == statusId &&
                            x.Project.DbCreateTimestamp > fiscalYearStart)
                .GroupBy(x => x.Equipment, (e, agreements) => new EquipmentLiteList
                {
                    EquipmentCode = e.EquipmentCode,
                    Id = e.EquipmentId,
                    OwnerId = e.OwnerId,
                    LocalAreaId = e.LocalAreaId,
                    ProjectIds = agreements.Select(y => y.ProjectId).Distinct().ToList(),
                    DistrictEquipmentTypeId = e.DistrictEquipmentTypeId ?? 0,
                });

            return new ObjectResult(new HetsResponse(equipment));
        }

        /// <summary>
        /// Get all equipment by district for rental agreement summary filtering
        /// </summary>
        [HttpGet]
        [Route("agreementSummary")]
        [SwaggerOperation("EquipmentGetAgreementSummary")]
        [SwaggerResponse(200, type: typeof(List<EquipmentAgreementSummary>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentGetAgreementSummary()
        {
            // get user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            IEnumerable<EquipmentAgreementSummary> equipment = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Equipment)
                .Where(x => x.DistrictId == districtId &&
                            !x.Number.StartsWith("BCBid"))
                .GroupBy(x => x.Equipment, (e, agreements) => new EquipmentAgreementSummary
                {
                    EquipmentCode = e.EquipmentCode,
                    Id = e.EquipmentId,
                    AgreementIds = agreements.Select(y => y.RentalAgreementId).Distinct().ToList(),
                    ProjectIds = agreements.Select(y => y.ProjectId).Distinct().ToList(),
                    DistrictEquipmentTypeId = e.DistrictEquipmentTypeId ?? 0,
                });

            return new ObjectResult(new HetsResponse(equipment));
        }

        /// <summary>
        /// Get all equipment for this district that are associated with a rotation list (lite)
        /// </summary>
        [HttpGet]
        [Route("liteHires")]
        [SwaggerOperation("EquipmentGetLiteHires")]
        [SwaggerResponse(200, type: typeof(List<EquipmentLiteList>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentGetLiteHires()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            IQueryable<EquipmentLiteList> equipment = _context.HetRentalRequestRotationList.AsNoTracking()
                .Include(x => x.RentalRequest)
                    .ThenInclude(y => y.LocalArea)
                        .ThenInclude(z => z.ServiceArea)
                .Include(x => x.RentalRequest)
                    .ThenInclude(y => y.Project)
                .Include(x => x.Equipment)
                .Where(x => x.RentalRequest.LocalArea.ServiceArea.DistrictId.Equals(districtId))
                .GroupBy(x => x.Equipment, (e, rotationLists) => new EquipmentLiteList
                {
                    EquipmentCode = e.EquipmentCode,
                    Id = e.EquipmentId,
                    OwnerId = e.OwnerId,
                    ProjectIds = rotationLists.Select(y => y.RentalRequest.ProjectId).Distinct().ToList(),
                    DistrictEquipmentTypeId = e.DistrictEquipmentTypeId ?? 0,
                });

            return new ObjectResult(new HetsResponse(equipment));
        }

        /// <summary>
        /// Update equipment
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("EquipmentIdPut")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult EquipmentIdPut([FromRoute]int id, [FromBody]HetEquipment item)
        {
            if (item == null || id != item.EquipmentId)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetEquipment equipment = _context.HetEquipment
                .Include(x => x.Owner)
                .First(x => x.EquipmentId == item.EquipmentId);

            DateTime? originalSeniorityEffectiveDate = equipment.SeniorityEffectiveDate;
            float? originalServiceHoursLastYear = equipment.ServiceHoursLastYear;
            float? originalServiceHoursTwoYearsAgo = equipment.ServiceHoursTwoYearsAgo;
            float? originalServiceHoursThreeYearsAgo = equipment.ServiceHoursThreeYearsAgo;
            int? originalLocalAreaId = equipment.LocalAreaId;
            int? originalDistrictEquipmentTypeId = equipment.DistrictEquipmentTypeId;
            float? originalYearsOfService = equipment.YearsOfService;

            // check if we need to rework the equipment's seniority
            bool rebuildSeniority = (originalSeniorityEffectiveDate == null && item.SeniorityEffectiveDate != null) ||
                                    (originalSeniorityEffectiveDate != null && item.SeniorityEffectiveDate != null &&
                                     originalSeniorityEffectiveDate != item.SeniorityEffectiveDate);

            bool rebuildOldSeniority = false;

            if (originalLocalAreaId != item.LocalArea.LocalAreaId)
            {
                rebuildSeniority = true;
                rebuildOldSeniority = true;
            }

            if (originalDistrictEquipmentTypeId != item.DistrictEquipmentTypeId)
            {
                rebuildSeniority = true;
                rebuildOldSeniority = true;
            }

            if ((originalServiceHoursLastYear == null && item.ServiceHoursLastYear != null) ||
                (originalServiceHoursLastYear != null && item.ServiceHoursLastYear != null &&
                 originalServiceHoursLastYear != item.ServiceHoursLastYear))
            {
                rebuildSeniority = true;
            }

            if ((originalServiceHoursTwoYearsAgo == null && item.ServiceHoursTwoYearsAgo != null) ||
                (originalServiceHoursTwoYearsAgo != null && item.ServiceHoursTwoYearsAgo != null &&
                 originalServiceHoursTwoYearsAgo != item.ServiceHoursTwoYearsAgo))
            {
                rebuildSeniority = true;
            }

            if ((originalServiceHoursThreeYearsAgo == null && item.ServiceHoursThreeYearsAgo != null) ||
                (originalServiceHoursThreeYearsAgo != null && item.ServiceHoursThreeYearsAgo != null &&
                 originalServiceHoursThreeYearsAgo != item.ServiceHoursThreeYearsAgo))
            {
                rebuildSeniority = true;
            }

            if ((originalYearsOfService == null && item.YearsOfService != null) ||
                (originalYearsOfService != null && item.YearsOfService != null &&
                 originalYearsOfService != item.YearsOfService))
            {
                rebuildSeniority = true;
            }

            // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
            if (EquipmentHelper.RentalRequestStatus(id, _context) && rebuildSeniority)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-41", ErrorViewModel.GetDescription("HETS-41", _configuration)));
            }

            // update equipment record
            equipment.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            equipment.ApprovedDate = item.ApprovedDate;
            equipment.EquipmentCode = item.EquipmentCode;
            equipment.Make = item.Make;
            equipment.Model = item.Model;
            equipment.Operator = item.Operator;
            equipment.ReceivedDate = item.ReceivedDate;
            equipment.LicencePlate = item.LicencePlate;
            equipment.SerialNumber = item.SerialNumber;
            equipment.Size = item.Size;
            equipment.YearsOfService = item.YearsOfService;
            equipment.Year = item.Year;
            equipment.LastVerifiedDate = item.LastVerifiedDate;
            equipment.IsSeniorityOverridden = item.IsSeniorityOverridden;
            equipment.SeniorityOverrideReason = item.SeniorityOverrideReason;
            equipment.Type = item.Type;
            equipment.ServiceHoursLastYear = item.ServiceHoursLastYear;
            equipment.ServiceHoursTwoYearsAgo = item.ServiceHoursTwoYearsAgo;
            equipment.ServiceHoursThreeYearsAgo = item.ServiceHoursThreeYearsAgo;
            equipment.SeniorityEffectiveDate = item.SeniorityEffectiveDate;
            equipment.LicencedGvw = item.LicencedGvw;
            equipment.LegalCapacity = item.LegalCapacity;
            equipment.PupLegalCapacity = item.PupLegalCapacity;
            equipment.LocalAreaId = item.LocalArea.LocalAreaId;
            equipment.DistrictEquipmentTypeId = item.DistrictEquipmentTypeId;

            // save the changes
            _context.SaveChanges();

            if (rebuildSeniority)
            {
                // update new area
                EquipmentHelper.RecalculateSeniority(item.LocalArea.LocalAreaId, item.DistrictEquipmentTypeId, _context, _configuration);

                // update old area
                if (rebuildOldSeniority)
                {
                    EquipmentHelper.RecalculateSeniority((int)originalLocalAreaId, (int)originalDistrictEquipmentTypeId, _context, _configuration);
                }
            }

            // retrieve updated equipment record to return to ui
            return new ObjectResult(new HetsResponse(EquipmentHelper.GetRecord(id, _context, _configuration)));
        }

        /// <summary>
        /// Update equipment status
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}/status")]
        [SwaggerOperation("EquipmentIdStatusPut")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult EquipmentIdStatusPut([FromRoute]int id, [FromBody]EquipmentStatus item)
        {
            // not found
            if (item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
            if (EquipmentHelper.RentalRequestStatus(id, _context))
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-41", ErrorViewModel.GetDescription("HETS-41", _configuration)));
            }

            bool recalculateSeniority = false;

            // get record
            HetEquipment equipment = _context.HetEquipment
                .Include(x => x.EquipmentStatusType)
                .Include(x => x.LocalArea)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Owner)
                .Include(x => x.HetEquipmentAttachment)
                .First(a => a.EquipmentId == id);

            // HETS-1069 - Do not allow an equipment whose Equipment type has been deleted to change status
            if (equipment.DistrictEquipmentType == null || equipment.DistrictEquipmentType.Deleted)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-39", ErrorViewModel.GetDescription("HETS-39", _configuration)));
            }

            // used for seniority recalculation
            int localAreaId = equipment.LocalArea.LocalAreaId;
            int districtEquipmentTypeId = equipment.DistrictEquipmentType.DistrictEquipmentTypeId;
            string oldStatus = equipment.EquipmentStatusType.EquipmentStatusTypeCode;

            // check the owner status
            int? ownStatusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (ownStatusId == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // update equipment status
            int? statusId = StatusHelper.GetStatusId(item.Status, "equipmentStatus", _context);
            if (statusId == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // can't make the status active if the owner is not active
            if (equipment.Owner.OwnerStatusTypeId != ownStatusId &&
                item.Status == HetEquipment.StatusApproved)
            {
                return new ConflictObjectResult(new HetsResponse("HETS-28", ErrorViewModel.GetDescription("HETS-28", _configuration)));
            }

            equipment.EquipmentStatusTypeId = (int)statusId;
            equipment.Status = item.Status;
            equipment.StatusComment = item.StatusComment;

            if (equipment.Status.Equals(HetEquipment.StatusArchived))
            {
                equipment.ArchiveCode = "Y";
                equipment.ArchiveDate = DateTime.UtcNow;
                equipment.ArchiveReason = "Equipment Archived";

                // recalculate seniority (move out of the block and adjust)
                recalculateSeniority = true;
            }
            else
            {
                equipment.ArchiveCode = "N";
                equipment.ArchiveDate = null;
                equipment.ArchiveReason = null;

                // make sure the seniority is set when shifting to "Active" state
                // (if this was a new record with no block/seniority yet)
                if (equipment.BlockNumber == null &&
                    equipment.Seniority == null &&
                    equipment.Status.Equals(HetEquipment.StatusApproved))
                {
                    // per HETS-536 -> ignore and let the user set the "Approved Date" date

                    // recalculation seniority (move into a block)
                    recalculateSeniority = true;
                }
                else if ((oldStatus.Equals(HetEquipment.StatusApproved) &&
                          !equipment.Status.Equals(HetEquipment.StatusApproved)) ||
                         (!oldStatus.Equals(HetEquipment.StatusApproved) &&
                          equipment.Status.Equals(HetEquipment.StatusApproved)))
                {
                    // recalculation seniority (move into or out of a block)
                    recalculateSeniority = true;
                }
            }

            // HETS-1119 - Add change of status comments to Notes
            string statusNote = $"(Status changed to: {equipment.Status}) {equipment.StatusComment}";
            HetNote note = new HetNote { EquipmentId = equipment.EquipmentId, Text = statusNote, IsNoLongerRelevant = false };
            _context.HetNote.Add(note);

            // save the changes
            _context.SaveChanges();

            // recalculation seniority (if required)
            if (recalculateSeniority)
            {
                EquipmentHelper.RecalculateSeniority(localAreaId, districtEquipmentTypeId, _context, _configuration);
            }

            // retrieve updated equipment record to return to ui
            return new ObjectResult(new HetsResponse(EquipmentHelper.GetRecord(id, _context, _configuration)));
        }

        /// <summary>
        /// Create equipment
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("EquipmentPost")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult EquipmentPost([FromBody]HetEquipment item)
        {
            // not found
            if (item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // set default values for new piece of Equipment
            // certain fields are set on new record - set defaults (including status = "Inactive")
            item = EquipmentHelper.SetNewRecordFields(item, _context);

            // ***********************************************************************************
            // Calculate Years of Service for new record
            // ***********************************************************************************
            // Business Rules:
            // 1. When the equipment is added the years registered is set to a fraction of the
            //    fiscal left from the registered date to the end of current fiscal
            //    (decimals: 3 places)
            // 2. On roll over the years registered increments by one for each year the equipment
            //    stays active ((might need use the TO_DATE field to track when last it was rolled over)
            //    TO_DATE = END OF CURRENT FISCAL

            // determine end of current fiscal year
            DateTime fiscalEnd;

            if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
            {
                fiscalEnd = new DateTime(DateTime.UtcNow.Year, 3, 31);
            }
            else
            {
                fiscalEnd = new DateTime(DateTime.UtcNow.AddYears(1).Year, 3, 31);
            }

            // is this a leap year?
            if (DateTime.IsLeapYear(fiscalEnd.Year))
            {
                item.YearsOfService = (float)Math.Round((fiscalEnd - DateTime.UtcNow).TotalDays / 366, 3);
            }
            else
            {
                item.YearsOfService = (float)Math.Round((fiscalEnd - DateTime.UtcNow).TotalDays / 365, 3);
            }

            item.ToDate = fiscalEnd;

            // save record
            _context.HetEquipment.Add(item);
            _context.SaveChanges();

            int id = item.EquipmentId;

            // HETS-834 - BVT - New Equipment Added default to APPROVED
            // * (already Set to approved)
            // * Update all equipment blocks, etc.
            // recalculation seniority (if required)
            int? localAreaId = item.LocalAreaId;
            int? districtEquipmentTypeId = item.DistrictEquipmentTypeId;
            EquipmentHelper.RecalculateSeniority(localAreaId, districtEquipmentTypeId, _context, _configuration);

            // retrieve updated equipment record to return to ui
            return new ObjectResult(new HetsResponse(EquipmentHelper.GetRecord(id, _context, _configuration)));
        }

        /// <summary>
        /// Recalculate Seniority List for the equipments with the same seniority and received date by equipment code
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("recalculatesenioritylist")]
        [SwaggerOperation("RecalculateSeniorityListPost")]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement, HetPermission.WriteAccess)]
        public virtual IActionResult RecalculateSeniorityListPost()
        {
            string connectionString = GetConnectionString();

            IConfigurationSection scoringRules = _configuration.GetSection("SeniorityScoringRules");
            string seniorityScoringRules = GetConfigJson(scoringRules);

            // queue the job
            BackgroundJob.Enqueue(() => EquipmentHelper.RecalculateSeniorityList(null,
                seniorityScoringRules, connectionString));

            // return ok
            return new ObjectResult(new HetsResponse("Recalculate job added to hangfire"));
        }

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

        #region Get Database Connection String

        /// <summary>
        /// Retrieve database connection string
        /// </summary>
        /// <returns></returns>
        private string GetConnectionString()
        {
            string connectionString;

            lock (_thisLock)
            {
                string host = _configuration["DATABASE_SERVICE_NAME"];
                string username = _configuration["POSTGRESQL_USER"];
                string password = _configuration["POSTGRESQL_PASSWORD"];
                string database = _configuration["POSTGRESQL_DATABASE"];

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                    string.IsNullOrEmpty(database))
                {
                    // When things get cleaned up properly, this is the only call we'll have to make.
                    connectionString = _configuration.GetConnectionString("HETS");
                }
                else
                {
                    // Environment variables override all other settings; same behaviour as the configuration provider when things get cleaned up.
                    connectionString = $"Host={host};Username={username};Password={password};Database={database};";
                }
            }

            return connectionString;
        }

        #endregion

        #region Equipment Search

        /// <summary>
        /// Search Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="types">Equipment Types (comma separated list of id numbers)</param>
        /// <param name="equipmentAttachment">Searches equipmentAttachment type</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notVerifiedSinceDate">Not Verified Since Date</param>
        /// <param name="equipmentId">Equipment Code</param>
        /// <param name="ownerName"></param>
        /// <param name="projectName"></param>
        /// <param name="twentyYears"></param>
        [HttpGet]
        [Route("search")]
        [SwaggerOperation("EquipmentSearchGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentLite>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentSearchGet([FromQuery]string localAreas, [FromQuery]string types,
            [FromQuery]string equipmentAttachment, [FromQuery]int? owner, [FromQuery]string status,
            [FromQuery]bool? hired, [FromQuery]DateTime? notVerifiedSinceDate,
            [FromQuery]string equipmentId = null, [FromQuery]string ownerName = null,
            [FromQuery]string projectName = null, [FromQuery]bool twentyYears = false)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] typesArray = ArrayHelper.ParseIntArray(types);

            // get agreement status
            int? agreementStatusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (agreementStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            IQueryable<HetEquipment> data = _context.HetEquipment.AsNoTracking()
                .Include(x => x.LocalArea)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(y => y.EquipmentType)
                .Include(x => x.Owner)
                .Include(x => x.HetEquipmentAttachment)
                .Include(x => x.HetRentalAgreement)
                    .ThenInclude(y => y.RentalAgreementStatusType)
                .Include(x => x.EquipmentStatusType)
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId));

            // filter results based on search criteria
            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.LocalArea.LocalAreaId));
            }

            if (equipmentAttachment != null)
            {
                data = data.Where(x => x.HetEquipmentAttachment
                    .Any(y => y.TypeName.ToLower().Contains(equipmentAttachment.ToLower())));
            }

            if (owner != null)
            {
                data = data.Where(x => x.Owner.OwnerId == owner);
            }

            if (ownerName != null)
            {
                data = data.Where(x => x.Owner.OrganizationName.ToLower().Contains(ownerName.ToLower()));
            }

            if (status != null)
            {
                int? statusId = StatusHelper.GetStatusId(status, "equipmentStatus", _context);

                if (statusId != null)
                {
                    data = data.Where(x => x.EquipmentStatusTypeId == statusId);
                }
            }

            if (projectName != null)
            {
                IQueryable<int?> hiredEquipmentQuery = _context.HetRentalAgreement.AsNoTracking()
                    .Where(x => x.Equipment.LocalArea.ServiceArea.DistrictId.Equals(districtId))
                    .Where(agreement => agreement.RentalAgreementStatusTypeId == agreementStatusId)
                    .Select(agreement => agreement.EquipmentId)
                    .Distinct();

                data = data.Where(e => hiredEquipmentQuery.Contains(e.EquipmentId));

                data = data.Where(x => x.HetRentalAgreement
                    .Any(y => y.Project.Name.ToLower().Contains(projectName.ToLower())));
            }
            else if (hired == true)
            {
                IQueryable<int?> hiredEquipmentQuery = _context.HetRentalAgreement.AsNoTracking()
                    .Where(x => x.Equipment.LocalArea.ServiceArea.DistrictId.Equals(districtId))
                    .Where(agreement => agreement.RentalAgreementStatusTypeId == agreementStatusId)
                    .Select(agreement => agreement.EquipmentId)
                    .Distinct();

                data = data.Where(e => hiredEquipmentQuery.Contains(e.EquipmentId));
            }

            if (typesArray != null && typesArray.Length > 0)
            {
                data = data.Where(x => typesArray.Contains(x.DistrictEquipmentType.DistrictEquipmentTypeId));
            }

            if (notVerifiedSinceDate != null)
            {
                data = data.Where(x => x.LastVerifiedDate < notVerifiedSinceDate);
            }

            // Ministry refer to the EquipmentCode as the "equipmentId" - its not the db id
            if (equipmentId != null)
            {
                data = data.Where(x => x.EquipmentCode.ToLower().Contains(equipmentId.ToLower()));
            }

            // HETS-942 - Search for Equipment > 20 yrs
            // ** only return equipment that are 20 years and older (using the equipment Year)
            if (twentyYears)
            {
                int twentyYearsInt = DateTime.Now.Year - 20;
                data = data.Where(x => string.IsNullOrWhiteSpace(x.Year) ||
                                       int.Parse(x.Year) <= twentyYearsInt);
            }

            // convert Equipment Model to the "EquipmentLite" Model
            SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);
            List<EquipmentLite> result = new List<EquipmentLite>();

            foreach (HetEquipment item in data)
            {
                result.Add(EquipmentHelper.ToLiteModel(item, scoringRules, (int)agreementStatusId, _context));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region Clone Project Agreements

        /// <summary>
        /// Get rental agreements associated with an equipment id
        /// </summary>
        /// <remarks>Gets as Equipment's Rental Agreements</remarks>
        /// <param name="id">id of Equipment to fetch agreements for</param>
        [HttpGet]
        [Route("{id}/rentalAgreements")]
        [SwaggerOperation("EquipmentIdRentalAgreementsGet")]
        [SwaggerResponse(200, type: typeof(List<HetRentalAgreement>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdRentalAgreementsGet([FromRoute]int id)
        {
            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            List<HetRentalAgreement> agreements = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Equipment)
                    .ThenInclude(d => d.DistrictEquipmentType)
                .Include(e => e.Equipment)
                    .ThenInclude(a => a.HetEquipmentAttachment)
                .Include(e => e.Project)
                .Where(x => x.EquipmentId == id)
                .ToList();

            // remove all of the additional agreements being returned
            foreach (HetRentalAgreement agreement in agreements)
            {
                agreement.Project.HetRentalAgreement = null;
            }

            return new ObjectResult(new HetsResponse(agreements));
        }

        /// <summary>
        /// Update a rental agreement by cloning a previous equipment rental agreement
        /// </summary>
        /// <param name="id">Project id</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/rentalAgreementClone")]
        [SwaggerOperation("EquipmentRentalAgreementClonePost")]
        [SwaggerResponse(200, type: typeof(HetRentalAgreement))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult EquipmentRentalAgreementClonePost([FromRoute]int id, [FromBody]EquipmentRentalAgreementClone item)
        {
            // not found
            if (item == null || id != item.EquipmentId) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get all agreements for this equipment
            List<HetRentalAgreement> agreements = _context.HetRentalAgreement
                .Include(x => x.Equipment)
                .ThenInclude(d => d.DistrictEquipmentType)
                .Include(e => e.Equipment)
                    .ThenInclude(a => a.HetEquipmentAttachment)
                .Include(x => x.HetRentalAgreementRate)
                .Include(x => x.HetRentalAgreementCondition)
                .Include(x => x.HetTimeRecord)
                .Where(x => x.EquipmentId == id)
                .ToList();

            // check that the rental agreements exists (that we want)
            exists = agreements.Any(a => a.RentalAgreementId == item.RentalAgreementId);

            // (RENTAL AGREEMENT) not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // check that the rental agreement to clone exist
            exists = agreements.Any(a => a.RentalAgreementId == item.AgreementToCloneId);

            // (RENTAL AGREEMENT) not found
            if (!exists) return new BadRequestObjectResult(new HetsResponse("HETS-11", ErrorViewModel.GetDescription("HETS-11", _configuration)));

            // get ids
            int agreementToCloneIndex = agreements.FindIndex(a => a.RentalAgreementId == item.AgreementToCloneId);
            int newRentalAgreementIndex = agreements.FindIndex(a => a.RentalAgreementId == item.RentalAgreementId);

            // ******************************************************************
            // Business Rules in the backend:
            // * Can't clone into an Agreement if it isn't Active
            // * Can't clone into an Agreement if it has existing time records
            // ******************************************************************
            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            if (agreements[newRentalAgreementIndex].RentalAgreementStatusTypeId != statusId)
            {
                // (RENTAL AGREEMENT) is not active
                return new BadRequestObjectResult(new HetsResponse("HETS-12", ErrorViewModel.GetDescription("HETS-12", _configuration)));
            }

            if (agreements[newRentalAgreementIndex].HetTimeRecord != null &&
                agreements[newRentalAgreementIndex].HetTimeRecord.Count > 0)
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
            agreements[newRentalAgreementIndex].HetRentalAgreementRate = null;

            foreach (HetRentalAgreementRate rate in agreements[agreementToCloneIndex].HetRentalAgreementRate)
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
            // return updated rental agreement to update the screen
            // ******************************************************************
            return new ObjectResult(new HetsResponse(RentalAgreementHelper.GetRecord(item.RentalAgreementId, _context)));
        }

        #endregion

        #region Duplicate Equipment Records

        /// <summary>
        /// Get all duplicate equipment records
        /// </summary>
        /// <param name="id">id of Equipment to fetch duplicates for</param>
        /// <param name="serialNumber"></param>
        /// <param name="typeId">District Equipment Type Id</param>
        [HttpGet]
        [Route("{id}/duplicates/{serialNumber}/{typeId?}")]
        [SwaggerOperation("EquipmentIdEquipmentDuplicatesGet")]
        [SwaggerResponse(200, type: typeof(List<DuplicateEquipmentModel>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdEquipmentDuplicatesGet([FromRoute]int id, [FromRoute]string serialNumber, [FromRoute]int? typeId)
        {
            bool exists = _context.HetEquipment.Any(x => x.EquipmentId == id);

            // not found [id > 0 -> need to allow for new records too]
            if (!exists && id > 0) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // HETS-845 - Verify Duplicate serial # functionality
            // Validate among the following:
            // * Same equipment types
            // * Among approved equipment

            // get status id
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get equipment duplicates
            List<HetEquipment> equipmentDuplicates;

            if (typeId != null && typeId > 0)
            {
                HetDistrictEquipmentType equipmentType = _context.HetDistrictEquipmentType.AsNoTracking()
                    .Include(x => x.EquipmentType)
                    .FirstOrDefault(x => x.DistrictEquipmentTypeId == typeId);

                int? equipmentTypeId = equipmentType?.EquipmentTypeId;

                // get equipment duplicates
                equipmentDuplicates = _context.HetEquipment.AsNoTracking()
                    .Include(x => x.LocalArea.ServiceArea.District)
                    .Include(x => x.Owner)
                    .Include(x => x.DistrictEquipmentType)
                    .Where(x => x.SerialNumber.ToLower() == serialNumber.ToLower() &&
                                x.EquipmentId != id &&
                                x.DistrictEquipmentType.EquipmentTypeId == equipmentTypeId &&
                                x.EquipmentStatusTypeId == statusId)
                    .ToList();
            }
            else
            {
                equipmentDuplicates = _context.HetEquipment.AsNoTracking()
                    .Include(x => x.LocalArea.ServiceArea.District)
                    .Include(x => x.Owner)
                    .Include(x => x.DistrictEquipmentType)
                    .Where(x => x.SerialNumber.ToLower() == serialNumber.ToLower() &&
                                x.EquipmentId != id &&
                                x.EquipmentStatusTypeId == statusId)
                    .ToList();
            }

            List<DuplicateEquipmentModel> duplicates = new List<DuplicateEquipmentModel>();
            int idCount = -1;

            foreach (HetEquipment equipment in equipmentDuplicates)
            {
                idCount++;

                DuplicateEquipmentModel duplicate = new DuplicateEquipmentModel
                {
                    Id = idCount,
                    SerialNumber = serialNumber,
                    DuplicateEquipment = equipment,
                    DistrictName = ""
                };

                if (equipment.LocalArea.ServiceArea.District != null &&
                    !string.IsNullOrEmpty(equipment.LocalArea.ServiceArea.District.Name))
                {
                    duplicate.DistrictName = equipment.LocalArea.ServiceArea.District.Name;
                }

                duplicates.Add(duplicate);
            }

            // return to the client
            return new ObjectResult(new HetsResponse(duplicates));
        }

        #endregion

        #region Equipment Attachment Records

        /// <summary>
        /// Get all equipment attachments for an equipment record
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentAttachments for</param>
        [HttpGet]
        [Route("{id}/equipmentAttachments")]
        [SwaggerOperation("EquipmentIdEquipmentAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<HetEquipmentAttachment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdEquipmentAttachmentsGet([FromRoute]int id)
        {
            bool exists = _context.HetEquipment.Any(x => x.EquipmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            List<HetEquipmentAttachment> attachments = _context.HetEquipmentAttachment.AsNoTracking()
                .Include(x => x.Equipment)
                .Where(x => x.Equipment.EquipmentId == id)
                .ToList();

            return new ObjectResult(new HetsResponse(attachments));
        }

        #endregion

        #region Attachments

        /// <summary>
        /// Get all attachments associated with an equipment record
        /// </summary>
        /// <remarks>Returns attachments for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch attachments for</param>
        [HttpGet]
        [Route("{id}/attachments")]
        [SwaggerOperation("EquipmentIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<HetDigitalFile>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdAttachmentsGet([FromRoute]int id)
        {
            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetEquipment equipment = _context.HetEquipment.AsNoTracking()
                .Include(x => x.HetDigitalFile)
                .First(a => a.EquipmentId == id);

            // extract the attachments and update properties for UI
            List<HetDigitalFile> attachments = new List<HetDigitalFile>();

            foreach (HetDigitalFile attachment in equipment.HetDigitalFile)
            {
                if (attachment != null)
                {
                    attachment.FileSize = attachment.FileContents.Length;
                    attachment.LastUpdateTimestamp = attachment.AppLastUpdateTimestamp;
                    attachment.LastUpdateUserid = attachment.AppLastUpdateUserid;

                    // don't send the file content
                    attachment.FileContents = null;

                    attachments.Add(attachment);
                }
            }

            return new ObjectResult(new HetsResponse(attachments));
        }

        #endregion

        #region Equipment History Records

        /// <summary>
        /// Get equipment history
        /// </summary>
        /// <remarks>Returns History for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        [HttpGet]
        [Route("{id}/history")]
        [SwaggerOperation("EquipmentIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<History>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            return new ObjectResult(new HetsResponse(EquipmentHelper.GetHistoryRecords(id, offset, limit, _context)));
        }

        /// <summary>
        /// Create equipment history
        /// </summary>
        /// <remarks>Add a History record to the Equipment</remarks>
        /// <param name="id">id of Equipment to add History for</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/history")]
        [SwaggerOperation("EquipmentIdHistoryPost")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult EquipmentIdHistoryPost([FromRoute]int id, [FromBody]HetHistory item)
        {
            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            if (exists)
            {
                HetHistory history = new HetHistory
                {
                    HistoryId = 0,
                    HistoryText = item.HistoryText,
                    CreatedDate = item.CreatedDate,
                    EquipmentId = id
                };

                _context.HetHistory.Add(history);
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(EquipmentHelper.GetHistoryRecords(id, null, null, _context)));
        }

        #endregion

        #region Equipment Note Records

        /// <summary>
        /// Get note records associated with equipment
        /// </summary>
        /// <param name="id">id of Equipment to fetch Notes for</param>
        [HttpGet]
        [Route("{id}/notes")]
        [SwaggerOperation("EquipmentsIdNotesGet")]
        [SwaggerResponse(200, type: typeof(List<HetNote>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdNotesGet([FromRoute]int id)
        {
            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetEquipment equipment = _context.HetEquipment.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.EquipmentId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in equipment.HetNote)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(notes));
        }

        /// <summary>
        /// Update or create a note associated with equipment
        /// </summary>
        /// <remarks>Update a Equipment&#39;s Notes</remarks>
        /// <param name="id">id of Equipment to update Notes for</param>
        /// <param name="item">Equipment Note</param>
        [HttpPost]
        [Route("{id}/note")]
        [SwaggerOperation("EquipmentIdNotePost")]
        [SwaggerResponse(200, type: typeof(HetNote))]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual IActionResult EquipmentIdNotePost([FromRoute]int id, [FromBody]HetNote item)
        {
            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists || item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // add or update note
            if (item.NoteId > 0)
            {
                // get note
                HetNote note = _context.HetNote.FirstOrDefault(a => a.NoteId == item.NoteId);

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
                    EquipmentId = id,
                    Text = item.Text,
                    IsNoLongerRelevant = item.IsNoLongerRelevant
                };

                _context.HetNote.Add(note);
            }

            _context.SaveChanges();

            // return updated note records
            HetEquipment equipment = _context.HetEquipment.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.EquipmentId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in equipment.HetNote)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(notes));
        }

        #endregion

        #region Seniority List Doc

        /// <summary>
        /// Get an openXml version of the seniority list
        /// </summary>
        /// <remarks>Returns an openXml version of the seniority list</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="types">Equipment Types (comma separated list of id numbers)</param>
        /// <param name="counterCopy">If true, use the Counter Copy template</param>
        [HttpGet]
        [Route("seniorityListDoc")]
        [SwaggerOperation("EquipmentSeniorityListDocGet")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentSeniorityListDocGet([FromQuery]string localAreas, [FromQuery]string types, [FromQuery]bool counterCopy = false)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] typesArray = ArrayHelper.ParseIntArray(types);

            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get fiscal year
            HetDistrictStatus district = _context.HetDistrictStatus.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId == districtId);

            if (district?.NextFiscalYear == null) return new BadRequestObjectResult(new HetsResponse("HETS-30", ErrorViewModel.GetDescription("HETS-30", _configuration)));

            // HETS-1195: Adjust seniority list and rotation list for lists hired between Apr1 and roll over
            // ** Need to use the "rollover date" to ensure we don't include records created
            //    after April 1 (but before rollover)
            DateTime fiscalEnd = district.RolloverEndDate;
            int fiscalYear = Convert.ToInt32(district.NextFiscalYear); // status table uses the start of the year

            fiscalEnd = fiscalEnd == new DateTime(0001, 01, 01, 00, 00, 00) ?
                new DateTime(fiscalYear, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 23, 59, 59) :
                new DateTime(fiscalYear, fiscalEnd.Month, fiscalEnd.Day, 23, 59, 59);

            // get status id
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get equipment record
            IQueryable<HetEquipment> data = _context.HetEquipment.AsNoTracking()
                .Include(x => x.LocalArea)
                    .ThenInclude(y => y.ServiceArea)
                        .ThenInclude(z => z.District)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(y => y.EquipmentType)
                .Include(x => x.Owner)
                .Include(x => x.HetRentalAgreement)
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.EquipmentStatusTypeId.Equals(statusId))
                .OrderBy(x => x.LocalArea.Name)
                    .ThenBy(x => x.DistrictEquipmentType.DistrictEquipmentName)
                    .ThenBy(x => x.BlockNumber)
                    .ThenByDescending(x => x.NumberInBlock);

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.LocalArea.LocalAreaId));
            }

            if (typesArray != null && typesArray.Length > 0)
            {
                data = data.Where(x => typesArray.Contains(x.DistrictEquipmentType.DistrictEquipmentTypeId));
            }

            // **********************************************************************
            // determine the year header values
            // * use the district status table
            // **********************************************************************
            string yearMinus1 = $"{fiscalYear - 2}/{fiscalYear - 1}";
            string yearMinus2 = $"{fiscalYear - 3}/{fiscalYear - 2}";
            string yearMinus3 = $"{fiscalYear - 4}/{fiscalYear - 3}";

            // **********************************************************************
            // convert Equipment Model to Pdf View Model
            // **********************************************************************
            SeniorityListReportViewModel seniorityList = new SeniorityListReportViewModel();
            SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);
            SeniorityListRecord listRecord = new SeniorityListRecord();

            // manage the rotation list data
            HetRentalRequestRotationList rotation = null;
            int currentBlock = -1;

            foreach (HetEquipment item in data)
            {
                if (listRecord.LocalAreaName != item.LocalArea.Name ||
                    listRecord.DistrictEquipmentTypeName != item.DistrictEquipmentType.DistrictEquipmentName)
                {
                    if (!string.IsNullOrEmpty(listRecord.LocalAreaName))
                    {
                        if (seniorityList.SeniorityListRecords == null)
                        {
                            seniorityList.SeniorityListRecords = new List<SeniorityListRecord>();
                        }

                        seniorityList.SeniorityListRecords.Add(listRecord);
                    }

                    listRecord = new SeniorityListRecord
                    {
                        LocalAreaName = item.LocalArea.Name,
                        DistrictEquipmentTypeName = item.DistrictEquipmentType.DistrictEquipmentName,
                        YearMinus1 = yearMinus1,
                        YearMinus2 = yearMinus2,
                        YearMinus3 = yearMinus3,
                        SeniorityList = new List<SeniorityViewModel>()
                    };

                    if (item.LocalArea.ServiceArea?.District != null)
                    {
                        listRecord.DistrictName = item.LocalArea.ServiceArea.District.Name;
                    }

                    // get the rotation info for the first block
                    if (item.BlockNumber != null) currentBlock = (int) item.BlockNumber;

                    rotation = GetRotationList(_context, item.LocalArea.LocalAreaId,
                        item.DistrictEquipmentType.DistrictEquipmentTypeId,
                        currentBlock, fiscalEnd);
                }
                else if (item.BlockNumber != null && currentBlock != item.BlockNumber)
                {
                    // get the rotation info for the next block
                    currentBlock = (int)item.BlockNumber;

                    rotation = GetRotationList(_context, item.LocalArea.LocalAreaId,
                        item.DistrictEquipmentType.DistrictEquipmentTypeId,
                        currentBlock, fiscalEnd);
                }

                listRecord.SeniorityList.Add(SeniorityListHelper.ToSeniorityViewModel(item, scoringRules, rotation, _context));
            }

            // add last record
            if (!string.IsNullOrEmpty(listRecord.LocalAreaName))
            {
                if (seniorityList.SeniorityListRecords == null)
                {
                    seniorityList.SeniorityListRecords = new List<SeniorityListRecord>();
                }

                seniorityList.SeniorityListRecords.Add(listRecord);
            }

            // sort seniority lists
            if (seniorityList.SeniorityListRecords != null)
            {
                foreach (SeniorityListRecord list in seniorityList.SeniorityListRecords)
                {
                    list.SeniorityList = list.SeniorityList.OrderBy(x => x.SenioritySortOrder).ToList();
                }
            }

            // classification and print date
            seniorityList.Classification = $"23010-22/{(fiscalYear - 1).ToString().Substring(2, 2)}-{fiscalYear.ToString().Substring(2, 2)}";
            seniorityList.PrintedOn = $"{DateTime.Now.AddHours(-8):dd-MM-yyyy H:mm:ss}";

            // convert to open xml document
            string documentName = $"SeniorityList-{DateTime.Now:yyyy-MM-dd}.docx";
            byte[] document = SeniorityList.GetSeniorityList(seniorityList, documentName);

            // return document
            FileContentResult result = new FileContentResult(document, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = documentName
            };

            Response.Headers.Add("Content-Disposition", "inline; filename=" + documentName);

            return result;
        }

        private static HetRentalRequestRotationList GetRotationList(DbAppContext context,
            int localAreaId, int districtEquipmentTypeId, int currentBlock, DateTime fiscalEnd)
        {
            try
            {
                // HETS-824 = BVT - Corrections to Seniority List PDF
                //   * This column should contain "Y" against the equipment that
                //     last responded (whether Yes/No) in a block
                //   * For "Forced Hire" there will be no changes to this
                //     column in the seniority list (as if nothing happened and nobody got called)
                //   * Must be this fiscal year
                HetRentalRequestRotationList blockRotation = context.HetRentalRequestRotationList.AsNoTracking()
                    .Include(x => x.Equipment)
                    .Include(x => x.RentalRequest)
                        .ThenInclude(x => x.LocalArea)
                    .Include(x => x.RentalRequest)
                        .ThenInclude(x => x.DistrictEquipmentType)
                    .OrderByDescending(x => x.RentalRequestId).ThenByDescending(x => x.RotationListSortOrder)
                    .FirstOrDefault(x => x.RentalRequest.LocalArea.LocalAreaId == localAreaId &&
                                         x.RentalRequest.DistrictEquipmentType.DistrictEquipmentTypeId == districtEquipmentTypeId &&
                                         x.IsForceHire == false &&
                                         x.WasAsked == true &&
                                         x.Equipment.BlockNumber == currentBlock &&
                                         (x.OfferResponse == "Yes" || x.OfferResponse == "No") &&
                                         x.AskedDateTime >= fiscalEnd.AddYears(-1).AddDays(1) &&
                                         x.AskedDateTime <= fiscalEnd);

                return blockRotation;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}
