using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Equipment Controller
    /// </summary>
    [Route("api/equipment")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentController : Controller
    {
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
        /// Update equipment
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("EquipmentIdPut")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdPut([FromRoute]int id, [FromBody]HetEquipment item)
        {
            if (item == null || id != item.EquipmentId)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetEquipment equipment = _context.HetEquipment
                .Include(x => x.Owner)
                .First(x => x.EquipmentId == item.EquipmentId);
            
            DateTime? originalSeniorityEffectiveDate = equipment.SeniorityEffectiveDate;
            float? originalServiceHoursLastYear = equipment.ServiceHoursLastYear;
            float? originalServiceHoursTwoYearsAgo = equipment.ServiceHoursTwoYearsAgo;
            float? originalServiceHoursThreeYearsAgo = equipment.ServiceHoursThreeYearsAgo;

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

            // save the changes
            _context.SaveChanges();

            // check if we need to rework the equipment's seniority
            bool rebuildSeniority = (originalSeniorityEffectiveDate == null && item.SeniorityEffectiveDate != null) ||
                                    (originalSeniorityEffectiveDate != null && item.SeniorityEffectiveDate != null &&
                                     originalSeniorityEffectiveDate != item.SeniorityEffectiveDate);

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

            if (rebuildSeniority)
            {
                EquipmentHelper.RecalculateSeniority(item.LocalAreaId, item.DistrictEquipmentTypeId, _context, _configuration);
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
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdStatusPut([FromRoute]int id, [FromBody]EquipmentStatus item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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

            // used for seniority recalculation
            int localAreaId = equipment.LocalArea.LocalAreaId;
            int districtEquipmentTypeId = equipment.DistrictEquipmentType.DistrictEquipmentTypeId;
            string oldStatus = equipment.EquipmentStatusType.EquipmentStatusTypeCode;

            // check the owner status
            int? ownStatusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (ownStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));
            
            // update equipment status
            int? statusId = StatusHelper.GetStatusId(item.Status, "equipmentStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // can't make the status active if the owner is not active
            if (equipment.Owner.OwnerStatusTypeId != ownStatusId &&
                item.Status == HetEquipment.StatusApproved)
            {
                return new ObjectResult(new HetsResponse("HETS-28", ErrorViewModel.GetDescription("HETS-28", _configuration)));
            }

            equipment.EquipmentStatusTypeId = (int)statusId;
            equipment.Status = item.Status;
            equipment.StatusComment = item.StatusComment;

            if (equipment.Status.Equals(HetEquipment.StatusArchived, StringComparison.CurrentCultureIgnoreCase))
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
                    equipment.Status.Equals(HetEquipment.StatusApproved, StringComparison.CurrentCultureIgnoreCase))
                {
                    // per HETS-536 -> ignore and let the user set the "Approved Date" date

                    // recalculation seniority (move into a block)
                    recalculateSeniority = true;
                }
                else if ((oldStatus.Equals(HetEquipment.StatusApproved, StringComparison.CurrentCultureIgnoreCase) &&
                          !equipment.Status.Equals(HetEquipment.StatusApproved, StringComparison.CurrentCultureIgnoreCase)) ||
                         (!oldStatus.Equals(HetEquipment.StatusApproved, StringComparison.CurrentCultureIgnoreCase) &&
                          equipment.Status.Equals(HetEquipment.StatusApproved, StringComparison.CurrentCultureIgnoreCase)))
                {
                    // recalculation seniority (move into or out of a block)
                    recalculateSeniority = true;
                }
            }

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
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentPost([FromBody]HetEquipment item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
        [HttpGet]
        [Route("search")]
        [SwaggerOperation("EquipmentSearchGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentLite>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentSearchGet([FromQuery]string localAreas, [FromQuery]string types, 
            [FromQuery]string equipmentAttachment, [FromQuery]int? owner, [FromQuery]string status, 
            [FromQuery]bool? hired, [FromQuery]DateTime? notVerifiedSinceDate, 
            [FromQuery]string equipmentId = null, [FromQuery]string ownerName = null,
            [FromQuery]string projectName = null)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] typesArray = ArrayHelper.ParseIntArray(types);

            // get agreement status
            int? agreementStatusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (agreementStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
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
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentRentalAgreementClonePost([FromRoute]int id, [FromBody]EquipmentRentalAgreementClone item)
        {
            // not found
            if (item == null || id != item.EquipmentId) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
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
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // check that the rental agreement to clone exist
            exists = agreements.Any(a => a.RentalAgreementId == item.AgreementToCloneId);

            // (RENTAL AGREEMENT) not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-11", ErrorViewModel.GetDescription("HETS-11", _configuration)));
            
            // get ids
            int agreementToCloneIndex = agreements.FindIndex(a => a.RentalAgreementId == item.AgreementToCloneId);
            int newRentalAgreementIndex = agreements.FindIndex(a => a.RentalAgreementId == item.RentalAgreementId);

            // ******************************************************************
            // Business Rules in the backend:
            // * Can't clone into an Agreement if it isn't Active
            // * Can't clone into an Agreement if it has existing time records
            // ******************************************************************
            int? statusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            if (agreements[newRentalAgreementIndex].RentalAgreementStatusTypeId != statusId)
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
            // return updated rental agreement to update the screen
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

        #region Duplicate Equipment Records

        /// <summary>
        /// Get all duplicate equipment records
        /// </summary>
        /// <param name="id">id of Equipment to fetch duplicates for</param>
        /// <param name="serialNumber"></param>
        /// <param name="typeId">District Equipment Type Id</param>
        [HttpGet]
        [Route("{id}/duplicates/{serialNumber}/{typeId}")]
        [SwaggerOperation("EquipmentIdEquipmentDuplicatesGet")]
        [SwaggerResponse(200, type: typeof(List<DuplicateEquipmentModel>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdEquipmentDuplicatesGet([FromRoute]int id, [FromRoute]string serialNumber, [FromRoute]int? typeId)
        {
            bool exists = _context.HetEquipment.Any(x => x.EquipmentId == id);

            // not found [id > 0 -> need to allow for new records too]
            if (!exists && id > 0) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // HETS-845 - Verify Duplicate serial # functionality
            // Validate among the following:
            // * Same equipment types
            // * Among approved equipment

            // get status id
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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
                    .Where(x => x.SerialNumber == serialNumber &&
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
                    .Where(x => x.SerialNumber == serialNumber &&
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
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
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
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
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
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
        [RequiresPermission(HetPermission.Login)]
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
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdNotePost([FromRoute]int id, [FromBody]HetNote item)
        {
            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

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

        #region Seniority List Pdf

        /// <summary>
        /// Get a pdf version of the seniority list
        /// </summary>
        /// <remarks>Returns a PDF version of the seniority list</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="types">Equipment Types (comma separated list of id numbers)</param>
        [HttpGet]
        [Route("seniorityListPdf")]
        [SwaggerOperation("EquipmentSeniorityListPdfGet")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentSeniorityListPdfGet([FromQuery]string localAreas, [FromQuery]string types)
        {
            _logger.LogInformation("Equipment Seniority List Pdf");

            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] typesArray = ArrayHelper.ParseIntArray(types);

            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get fiscal year
            HetDistrictStatus district = _context.HetDistrictStatus.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId == districtId);

            if (district?.NextFiscalYear == null) return new ObjectResult(new HetsResponse("HETS-30", ErrorViewModel.GetDescription("HETS-30", _configuration)));

            int fiscalYear = (int)district.NextFiscalYear; // status table uses the start of the tear
            DateTime fiscalEnd = new DateTime(fiscalYear, 3, 31);

            // get status id
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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
                .OrderBy(x => x.LocalArea)
                    .ThenBy(x => x.DistrictEquipmentType)
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
            string yearMinus1 = string.Format("{0}/{1}", fiscalYear - 2, fiscalYear - 1);
            string yearMinus2 = string.Format("{0}/{1}", fiscalYear - 3, fiscalYear - 2);
            string yearMinus3 = string.Format("{0}/{1}", fiscalYear - 4, fiscalYear - 3);

            // **********************************************************************
            // convert Equipment Model to Pdf View Model
            // **********************************************************************
            SeniorityListPdfViewModel seniorityList = new SeniorityListPdfViewModel();
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
                    currentBlock = (int)item.BlockNumber;

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

            seniorityList.PrintedOn = string.Format("{0:dd-MM-yyyy H:mm:ss}", DateTime.Now.AddHours(-8));
            
            // **********************************************************************
            // create the payload and call the pdf service
            // **********************************************************************
            string payload = JsonConvert.SerializeObject(seniorityList, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            _logger.LogInformation("Equipment Seniority List Pdf - Payload Length: {0}", payload.Length);

            // pass the request on to the Pdf Micro Service
            string pdfHost = _configuration["PDF_SERVICE_NAME"];
            string pdfUrl = _configuration.GetSection("Constants:SeniorityListPdfUrl").Value;
            string targetUrl = pdfHost + pdfUrl;

            // generate pdf document name [unique portion only]
            string fileName = "HETS_SeniorityList";

            targetUrl = targetUrl + "/" + fileName;

            _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Service Url: {0}", targetUrl);

            // call the MicroService
            try
            {
                HttpClient client = new HttpClient();
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                _logger.LogInformation("Equipment Seniority List Pdf - Calling HETS Pdf Service");
                HttpResponseMessage response = client.PostAsync(targetUrl, stringContent).Result;

                // success
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Service Response: OK");

                    var pdfResponseBytes = GetPdf(response);

                    // convert to string and log
                    string pdfResponse = Encoding.Default.GetString(pdfResponseBytes);

                    fileName = fileName + string.Format("-{0:yyyy-MM-dd-H-mm}", DateTime.Now) + ".pdf";

                    _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Filename: {0}", fileName);
                    _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Size: {0}", pdfResponse.Length);

                    // return content
                    FileContentResult pdfResult = new FileContentResult(pdfResponseBytes, "application/pdf")
                    {
                        FileDownloadName = fileName
                    };

                    Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);

                    return pdfResult;
                }

                _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Service Response: {0}", response.StatusCode);

                // problem occured
                return new ObjectResult(new HetsResponse("HETS-05", ErrorViewModel.GetDescription("HETS-05", _configuration)));
            }
            catch (Exception ex)
            {
                Debug.Write("Error generating pdf: " + ex.Message);
                return new ObjectResult(new HetsResponse("HETS-05", ErrorViewModel.GetDescription("HETS-05", _configuration)));
            }
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

        private static byte[] GetPdf(HttpResponseMessage response)
        {
            try
            {
                var pdfResponseBytes = response.Content.ReadAsByteArrayAsync();
                pdfResponseBytes.Wait();

                return pdfResponseBytes.Result;
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
