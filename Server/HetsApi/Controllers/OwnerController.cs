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
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Entities;
using HetsReport;
using HetsData.Repositories;
using HetsData.Dtos;
using AutoMapper;
using HetsCommon;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace HetsApi.Controllers
{
    #region Parameter Models

    /// <summary>
    /// Owner Status Class - required to update the status record only
    /// </summary>
    public class OwnerStatus
    {
        public string Status { get; set; }
        public string StatusComment { get; set; }
    }

    public class ReportParameters
    {
        public int?[] LocalAreas { get; set; }
        public int?[] Owners { get; set; }
    }

    #endregion

    /// <summary>
    /// Owner Controller
    /// </summary>
    [Route("api/owners")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class OwnerController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly IOwnerRepository _ownerRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<OwnerController> _logger;

        public OwnerController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, 
            IOwnerRepository ownerRepo,
            IMapper mapper,
            ILogger<OwnerController> logger)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _ownerRepo = ownerRepo;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get owner by id
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<OwnerDto> OwnersIdGet([FromRoute]int id)
        {
            return new ObjectResult(new HetsResponse(_ownerRepo.GetRecord(id)));
        }

        /// <summary>
        /// Get all owners for this district
        /// </summary>
        [HttpGet]
        [Route("lite")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<OwnerLiteProjects>> OwnersGetLite()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get active status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get all active owners for this district
            IEnumerable<OwnerLiteProjects> owners = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetRentalAgreements)
                .Where(x => x.LocalArea.ServiceArea.DistrictId == districtId &&
                            x.OwnerStatusTypeId == statusId)
                .OrderBy(x => x.OwnerCode)
                .Select(x => new OwnerLiteProjects
                {
                    OwnerCode = x.OwnerCode,
                    OrganizationName = x.OrganizationName,
                    Id = x.OwnerId,
                    LocalAreaId = x.LocalAreaId,
                });

            return new ObjectResult(new HetsResponse(owners));
        }

        /// <summary>
        /// Get all owners for this district for hiring report (lite)
        /// </summary>
        [HttpGet]
        [Route("liteHires")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<OwnerLiteProjects>> OwnersGetLiteHires()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get active status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get all active owners for this district (and any projects they're associated with)
            var owners = _context.HetRentalRequestRotationLists.AsNoTracking()
                .Include(x => x.RentalRequest)
                    .ThenInclude(y => y.LocalArea)
                        .ThenInclude(z => z.ServiceArea)
                .Include(x => x.RentalRequest)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Where(x => x.RentalRequest.LocalArea.ServiceArea.DistrictId.Equals(districtId))
                .ToList()
                .GroupBy(x => x.Equipment.Owner, (o, rotationLists) => new OwnerLiteProjects
                {
                    OwnerCode = o.OwnerCode,
                    OrganizationName = o.OrganizationName,
                    Id = o.OwnerId,
                    LocalAreaId = o.LocalAreaId,
                    ProjectIds = rotationLists.Select(y => y.RentalRequest.ProjectId).Where(y => y != null).Distinct().ToList()
                });

            return new ObjectResult(new HetsResponse(owners));
        }

        /// <summary>
        /// Get all owners for this district for time entries (lite)
        /// </summary>
        [HttpGet]
        [Route("liteTs")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<OwnerLiteProjects>> OwnersGetLiteTs()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get active status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) 
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatuses.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;
            if (fiscalYear == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // fiscal year in the status table stores the "start" of the year
            DateTime fiscalYearStart = DateUtils.ConvertPacificToUtcTime(
                new DateTime((int)fiscalYear, 3, 31, 0, 0, 0, DateTimeKind.Unspecified));

            // get all active owners for this district (and any projects they're associated with)
            var owners = _context.HetRentalAgreements.AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Where(x => x.Equipment.LocalArea.ServiceArea.DistrictId == districtId &&
                            x.Equipment.Owner.OwnerStatusTypeId == statusId &&
                            x.Project.AppCreateTimestamp > fiscalYearStart)
                .ToList()
                .GroupBy(x => x.Equipment.Owner, (o, agreements) => new OwnerLiteProjects
                {
                    OwnerCode = o.OwnerCode,
                    OrganizationName = o.OrganizationName,
                    Id = o.OwnerId,
                    LocalAreaId = o.LocalAreaId,
                    ProjectIds = agreements.Select(y => y.ProjectId)
                });

            return new ObjectResult(new HetsResponse(owners));
        }

        /// <summary>
        /// Update owner
        /// </summary>
        /// <param name="id">id of Owner to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<OwnerDto> OwnersIdPut([FromRoute]int id, [FromBody]OwnerDto item)
        {
            if (item == null || id != item.OwnerId)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetOwner owner = _context.HetOwners.First(x => x.OwnerId == item.OwnerId);

            int? oldLocalArea = owner.LocalAreaId;
            bool? oldIsMaintenanceContractor = owner.IsMaintenanceContractor;

            // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
            if (_ownerRepo.RentalRequestStatus(id) && oldLocalArea != item.LocalArea.LocalAreaId)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-40", ErrorViewModel.GetDescription("HETS-40", _configuration)));
            }

            if (item.RegisteredCompanyNumber == "") item.RegisteredCompanyNumber = null;
            if (item.WorkSafeBcpolicyNumber == "") item.WorkSafeBcpolicyNumber = null;

            owner.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            owner.CglCompany = item.CglCompany;
            if (item.CglendDate is DateTime cglEndDateUtc)
            {
                owner.CglendDate = DateUtils.AsUTC(cglEndDateUtc);
            }
            owner.CglPolicyNumber = item.CglPolicyNumber;
            owner.LocalAreaId = item.LocalArea.LocalAreaId;
            if (item.WorkSafeBcexpiryDate is DateTime workSafeBcExpiryDateUtc)
            {
                owner.WorkSafeBcexpiryDate = DateUtils.AsUTC(workSafeBcExpiryDateUtc);
            }
            owner.WorkSafeBcpolicyNumber = item.WorkSafeBcpolicyNumber;
            owner.IsMaintenanceContractor = item.IsMaintenanceContractor;
            owner.OrganizationName = item.OrganizationName;
            owner.OwnerCode = item.OwnerCode;
            owner.DoingBusinessAs = item.DoingBusinessAs;
            owner.RegisteredCompanyNumber = item.RegisteredCompanyNumber;
            owner.Address1 = item.Address1;
            owner.Address2 = item.Address2;
            owner.City = item.City;
            owner.PostalCode = item.PostalCode;
            owner.Province = item.Province;
            owner.GivenName = item.GivenName;
            owner.Surname = item.Surname;
            owner.MeetsResidency = item.MeetsResidency;

            // we need to update the equipment records to match any change in local area
            if (oldLocalArea != item.LocalArea.LocalAreaId)
            {
                IQueryable<HetEquipment> equipmentList = _context.HetEquipments
                    .Include(x => x.Owner)
                    .Include(x => x.LocalArea)
                    .Where(x => x.OwnerId == id);

                foreach (HetEquipment equipment in equipmentList)
                {
                    equipment.LocalAreaId = item.LocalArea.LocalAreaId;
                }
            }

            // do we need to update the block assignment?
            if (oldIsMaintenanceContractor != item.IsMaintenanceContractor)
            {
                // get equipment active status
                int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
                if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                // get processing rules
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration, (errMessage, ex) => {
                    _logger.LogError(errMessage);
                    _logger.LogError(ex.ToString());
                });

                // get active equipment
                IQueryable<HetEquipment> equipmentListB = _context.HetEquipments
                    .Include(x => x.Owner)
                    .Include(x => x.LocalArea)
                    .Include(x => x.DistrictEquipmentType)
                        .ThenInclude(y => y.EquipmentType)
                    .Where(x => x.OwnerId == id &&
                                x.EquipmentStatusTypeId == statusId);

                foreach (HetEquipment equipment in equipmentListB)
                {
                    int localAreaId = equipment.LocalArea.LocalAreaId;
                    int districtEquipmentTypeId = equipment.DistrictEquipmentType.DistrictEquipmentTypeId;

                    // get rules
                    int blockSize = equipment.DistrictEquipmentType.EquipmentType.IsDumpTruck
                        ? scoringRules.GetBlockSize("DumpTruck")
                        : scoringRules.GetBlockSize();
                    int totalBlocks = equipment.DistrictEquipmentType.EquipmentType.IsDumpTruck
                        ? scoringRules.GetTotalBlocks("DumpTruck")
                        : scoringRules.GetTotalBlocks();

                    // update block assignments
                    SeniorityListHelper.AssignBlocks(
                        localAreaId, districtEquipmentTypeId, blockSize, totalBlocks, _context, 
                        (errMessage, ex) => {
                            _logger.LogError(errMessage);
                            _logger.LogError(ex.ToString());
                        });

                    // update old area block assignments
                    if (oldLocalArea != null)
                    {
                        SeniorityListHelper.AssignBlocks(
                            (int)oldLocalArea, districtEquipmentTypeId, blockSize, totalBlocks, _context, 
                            (errMessage, ex) => {
                                _logger.LogError(errMessage);
                                _logger.LogError(ex.ToString());
                            });
                    }
                }
            }

            // save the changes
            _context.SaveChanges();

            // retrieve updated owner record to return to ui
            return new ObjectResult(new HetsResponse(_ownerRepo.GetRecord(id)));
        }

        /// <summary>
        /// Update owner status
        /// </summary>
        /// <param name="id">id of Owner to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}/status")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<OwnerDto> OwnersIdStatusPut([FromRoute]int id, [FromBody]OwnerStatus item)
        {
            // not found
            if (item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
            if (_ownerRepo.RentalRequestStatus(id))
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-40", ErrorViewModel.GetDescription("HETS-40", _configuration)));
            }

            // get record
            HetOwner owner = _context.HetOwners
                .Include(x => x.HetEquipments)
                    .ThenInclude(y => y.LocalArea)
                .Include(x => x.HetEquipments)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .First(x => x.OwnerId == id);

            // get status id
            int? statusId = StatusHelper.GetStatusId(item.Status, "ownerStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            owner.OwnerStatusTypeId = (int)statusId;
            owner.Status = item.Status;
            owner.StatusComment = item.StatusComment;

            // set owner archive attributes (if necessary)
            if (owner.Status.Equals(HetOwner.StatusArchived, StringComparison.CurrentCultureIgnoreCase))
            {
                owner.ArchiveCode = "Y";
                owner.ArchiveDate = DateTime.UtcNow;
                owner.ArchiveReason = "Owner Archived";
            }
            else
            {
                owner.ArchiveCode = "N";
                owner.ArchiveDate = null;
                owner.ArchiveReason = null;
            }

            // if the status = Archived or Pending - need to update all associated equipment too
            // (if the Owner is "Activated" - it DOES NOT automatically activate the equipment)
            if (!item.Status.Equals(HetOwner.StatusApproved, StringComparison.InvariantCultureIgnoreCase))
            {
                foreach (HetEquipment equipment in owner.HetEquipments)
                {
                    // used for seniority recalculation
                    int localAreaId = equipment.LocalArea.LocalAreaId;
                    int districtEquipmentTypeId = equipment.DistrictEquipmentType.DistrictEquipmentTypeId;

                    // get status id
                    int? eqStatusId = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _context);
                    if (eqStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                    // if the equipment is already archived - leave it archived
                    // if the equipment is already in the same state as the owner's new state - then ignore
                    if (!equipment.EquipmentStatusTypeId.Equals(eqStatusId) && !equipment.EquipmentStatusTypeId.Equals(statusId))
                    {
                        eqStatusId = StatusHelper.GetStatusId(item.Status, "equipmentStatus", _context);
                        if (eqStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                        equipment.EquipmentStatusTypeId = (int)eqStatusId;
                        equipment.Status = item.Status;
                        equipment.StatusComment = item.StatusComment;

                        if (equipment.Status.Equals(HetEquipment.StatusArchived, StringComparison.CurrentCultureIgnoreCase))
                        {
                            equipment.ArchiveCode = "Y";
                            equipment.ArchiveDate = DateTime.UtcNow;
                            equipment.ArchiveReason = "Owner Archived";
                        }
                        else
                        {
                            equipment.ArchiveCode = "N";
                            equipment.ArchiveDate = null;
                            equipment.ArchiveReason = null;
                        }

                        // recalculate the seniority
                        EquipmentHelper.RecalculateSeniority(localAreaId, districtEquipmentTypeId, _context, _configuration, (errMessage, ex) => {
                            _logger.LogError(errMessage);
                            _logger.LogError(ex.ToString());
                        });

                        // HETS-1119 - Add change of status comments to Notes
                        string equipmentStatusNote = $"(Status changed to: {equipment.Status}) {equipment.StatusComment}";
                        HetNote equipmentNote = new HetNote { EquipmentId = equipment.EquipmentId, Text = equipmentStatusNote, IsNoLongerRelevant = false };
                        _context.HetNotes.Add(equipmentNote);
                    }
                }
            }

            // HETS-1119 - Add change of status comments to Notes
            string statusNote = $"(Status changed to: {owner.Status}) {owner.StatusComment}";
            HetNote note = new HetNote { OwnerId = owner.OwnerId, Text = statusNote, IsNoLongerRelevant = false };
            _context.HetNotes.Add(note);

            // save the changes
            _context.SaveChanges();

            // retrieve updated owner record to return to ui
            return new ObjectResult(new HetsResponse(_ownerRepo.GetRecord(id)));
        }

        /// <summary>
        /// Create owner
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<OwnerDto> OwnersPost([FromBody]OwnerDto item)
        {
            // not found
            if (item == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get status id
            int? statusId = StatusHelper.GetStatusId(item.Status, "ownerStatus", _context);
            if (statusId == null) 
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // create record
            HetOwner owner = new()
            {
                OwnerStatusTypeId = (int)statusId,
                CglCompany = item.CglCompany,
                CglPolicyNumber = item.CglPolicyNumber,
                LocalAreaId = item.LocalArea.LocalAreaId,
                WorkSafeBcpolicyNumber = item.WorkSafeBcpolicyNumber,
                IsMaintenanceContractor = item.IsMaintenanceContractor ?? false,
                OrganizationName = item.OrganizationName,
                OwnerCode = item.OwnerCode,
                DoingBusinessAs = item.DoingBusinessAs,
                Address1 = item.Address1,
                Address2 = item.Address2,
                City = item.City,
                PostalCode = item.PostalCode,
                Province = item.Province,
                GivenName = item.GivenName,
                Surname = item.Surname,
                ArchiveCode = "N",
                MeetsResidency = item.MeetsResidency
            };

            if (item.CglendDate is DateTime cglEndDateUtc)
            {
                owner.CglendDate = DateUtils.AsUTC(cglEndDateUtc);
            }

            if (item.WorkSafeBcexpiryDate is DateTime workSafeBcExpiryDateUtc)
            {
                owner.WorkSafeBcexpiryDate = DateUtils.AsUTC(workSafeBcExpiryDateUtc);
            }

            if (!string.IsNullOrEmpty(item.RegisteredCompanyNumber))
            {
                owner.RegisteredCompanyNumber = item.RegisteredCompanyNumber;
            }

            // get new Secret Key
            string key = SecretKeyHelper.RandomString(8, item.OrganizationName.Length);

            string temp = owner.OwnerCode;

            if (string.IsNullOrEmpty(temp))
            {
                temp = SecretKeyHelper.RandomString(4, item.OrganizationName.Length);
            }

            key = temp + "-" + DateTime.UtcNow.Year + "-" + key;

            owner.SharedKey = key;

            // create a new primary contact record
            if (string.IsNullOrEmpty(item.PrimaryContactRole))
            {
                item.PrimaryContactRole = "Contact";
            }

            HetContact primaryContact = new HetContact
            {
                Role = item.PrimaryContactRole,
                Province = "BC",
                WorkPhoneNumber = item.PrimaryContactPhone,
                Surname = item.PrimaryContactSurname,
                GivenName = item.PrimaryContactGivenName
            };

            owner.PrimaryContact = primaryContact;

            // save record
            _context.HetOwners.Add(owner);
            _context.SaveChanges();

            int id = owner.OwnerId;

            // update contact record
            int? contactId = owner.PrimaryContact.ContactId;

            HetContact contact = _context.HetContacts.FirstOrDefault(x => x.ContactId == contactId);

            if (contact != null)
            {
                contact.OwnerId = id;
                _context.SaveChanges();
            }

            // retrieve updated owner record to return to ui
            return new ObjectResult(new HetsResponse(_ownerRepo.GetRecord(id)));
        }

        #region Owner Search

        /// <summary>
        /// Searches Owners
        /// </summary>
        /// <remarks>Used for the owner search page.</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="equipmentTypes">Equipment Types (comma separated list of id numbers)</param>
        /// <param name="owner">Id for a specific Owner</param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="ownerName"></param>
        /// <param name="ownerCode"></param>
        [HttpGet]
        [Route("search")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<OwnerLite> OwnersSearchGet([FromQuery]string localAreas,
            [FromQuery]string status, [FromQuery]string ownerName = null, [FromQuery]string ownerCode = null)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            IQueryable<HetOwner> data = _context.HetOwners.AsNoTracking()
                .Include(x => x.LocalArea)
                    .ThenInclude(y => y.ServiceArea)
                        .ThenInclude(z => z.District)
                .Include(x => x.HetEquipments)
                    .ThenInclude(y => y.EquipmentStatusType)
                .Include(x => x.PrimaryContact)
                .Include(x => x.OwnerStatusType)
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId));

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.LocalArea.LocalAreaId));
            }

            if (status != null)
            {
                int? statusId = StatusHelper.GetStatusId(status, "ownerStatus", _context);

                if (statusId != null)
                {
                    data = data.Where(x => x.OwnerStatusTypeId == statusId);
                }
            }

            if (ownerName != null)
            {
                data = data.Where(x => 
                    x.OrganizationName.ToLower().Contains(ownerName.ToLower()) || 
                    x.DoingBusinessAs.ToLower().Contains(ownerName.ToLower())
                );
            }

            if (ownerCode != null)
            {
                data = data.Where(x => x.OwnerCode.ToLower().Contains(ownerCode.ToLower()));
            }

            // convert Owner Model to the "OwnerLite" Model
            List<OwnerLite> result = new List<OwnerLite>();

            foreach (HetOwner item in data)
            {
                result.Add(OwnerHelper.ToLiteModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region Get Verification Report

        /// <summary>
        /// Get owner verification report
        /// </summary>
        /// <remarks>Returns an OpenXml Document version of the owner verification notices</remarks>
        /// <param name="parameters">Array of local area and owner id numbers to generate notices for</param>
        [HttpPost]
        [Route("verificationDoc")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdVerificationPost([FromBody]ReportParameters parameters)
        {
            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get equipment status
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // HETS-1109: Archived Owners coming in Status Letters
            // get owner status (only show Active owners)
            int? ownerStatusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (ownerStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get owner report data
            OwnerVerificationReportModel reportModel 
                = _ownerRepo.GetOwnerVerificationLetterData(parameters.LocalAreas, parameters.Owners, statusId, ownerStatusId, districtId);

            // convert to open xml document
            string documentName = $"OwnerVerification-{DateTime.Now:yyyy-MM-dd}.docx";
            byte[] document = OwnerVerification.GetOwnerVerification(reportModel, documentName, (errMessage, ex) => {
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

        #endregion

        #region Get Mailing Labels Pdfs

        /// <summary>
        /// Get owner mailing labels pdf
        /// </summary>
        /// <remarks>Returns a PDF version of the owner mailing labels</remarks>
        /// <param name="parameters">Array of local area and owner ids to generate labels for</param>
        [HttpPost]
        [Route("mailingLabelsPdf")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdMailingLabelsPdfPost([FromBody]ReportParameters parameters)
        {
            _logger.LogInformation("Owner Mailing Labels Pdf");

            // HETS-1041 - Mailing Labels return also Inactive Owners
            // ** Only return Active owner records
            // get active status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) 
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get owner records
            IQueryable<HetOwner> ownerRecords = _context.HetOwners.AsNoTracking()
                .Include(x => x.PrimaryContact)
                .Include(x => x.LocalArea)
                    .ThenInclude(s => s.ServiceArea)
                        .ThenInclude(d => d.District)
                .Where(x => x.OwnerStatusTypeId == statusId)
                .OrderBy(x => x.LocalArea.Name).ThenBy(x => x.OrganizationName);

            if (parameters.Owners.Length > 0)
            {
                ownerRecords = ownerRecords.Where(x => parameters.Owners.Contains(x.OwnerId));
            }

            if (parameters.LocalAreas?.Length > 0)
            {
                ownerRecords = ownerRecords.Where(x => parameters.LocalAreas.Contains(x.LocalAreaId));
            }

            // convert to list
            List<HetOwner> ownerList = ownerRecords.ToList();

            if (!ownerList.Any())
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration))); // record not found

            // generate pdf document name [unique portion only]
            string fileName = "MailingLabels";

            // setup model for submission to the Pdf service
            MailingLabelPdfViewModel model = new()
            {
                ReportDate = DateTime.Now.ToString("yyyy-MM-dd"),
                Title = fileName,
                DistrictId = ownerList[0].LocalArea.ServiceArea.District.DistrictId,
                LabelRow = new List<MailingLabelRowModel>()
            };

            // add owner records
            int column = 1;

            foreach (HetOwner owner in ownerRecords)
            {
                if (!IsDistrictSame(owner.LocalArea.ServiceArea.District, model.DistrictId))
                {
                    // missing district - data error [HETS-16]
                    return new BadRequestObjectResult(
                        new HetsResponse("HETS-16", ErrorViewModel.GetDescription("HETS-16", _configuration)));
                }

                owner.ReportDate = model.ReportDate;
                owner.Title = model.Title;
                owner.DistrictId = model.DistrictId;

                switch (column)
                {
                    case 1:
                        model.LabelRow.Add(new MailingLabelRowModel());
                        model.LabelRow.Last().OwnerColumn1 = _mapper.Map<OwnerDto>(owner);
                        break;
                    default:
                        model.LabelRow.Last().OwnerColumn2 = _mapper.Map<OwnerDto>(owner);
                        column = 0;
                        break;
                }

                column++;
            }

            // setup payload
            string payload = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            _logger.LogInformation("Owner Mailing Labels Pdf - Payload Length: {payloadLength}", payload.Length);

            // pass the request on to the Pdf Micro Service
            string pdfHost = _configuration["PDF_SERVICE_NAME"];
            string pdfUrl = _configuration.GetSection("Constants:OwnerMailingLabelsPdfUrl").Value;
            string targetUrl = pdfHost + pdfUrl;

            targetUrl = targetUrl + "/" + fileName;

            _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Service Url: {targetUrl}", targetUrl);

            // call the MicroService
            try
            {
                HttpClient client = new();
                StringContent stringContent = new(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                _logger.LogInformation("Owner Mailing Labels Pdf - Calling HETS Pdf Service");
                HttpResponseMessage response = client.PostAsync(targetUrl, stringContent).Result;

                // success
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Service Response: OK");

                    var pdfResponseBytes = GetPdf(response, (errMessage, ex) => {
                        _logger.LogError("{errMessage}", errMessage);
                        _logger.LogError("{exception}", ex);
                    });

                    // convert to string and log
                    string pdfResponse = Encoding.Default.GetString(pdfResponseBytes);

                    fileName = fileName + $"-{DateTime.Now:yyyy-MM-dd-H-mm}" + ".pdf";

                    _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Filename: {fileName}", fileName);
                    _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Size: {pdfResponseLength}", pdfResponse.Length);

                    // return content
                    FileContentResult result = new(pdfResponseBytes, "application/pdf")
                    {
                        FileDownloadName = fileName
                    };

                    Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);

                    return result;
                }

                _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Service Response: {statusCode}", response.StatusCode);
            }
            catch (Exception ex)
            {
                Debug.Write("Error generating pdf: " + ex.Message);
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-15", ErrorViewModel.GetDescription("HETS-15", _configuration)));
            }

            // problem occured
            return new BadRequestObjectResult(
                new HetsResponse("HETS-15", ErrorViewModel.GetDescription("HETS-15", _configuration)));
        }

        private static bool IsDistrictSame(HetDistrict district, int districtId)
        {
            return district is not null && district.DistrictId == districtId;
        }

        private static byte[] GetPdf(HttpResponseMessage response, Action<string, Exception> logErrorAction)
        {
            try
            {
                var pdfResponseBytes = response.Content.ReadAsByteArrayAsync();
                pdfResponseBytes.Wait();

                return pdfResponseBytes.Result;
            }
            catch (Exception e)
            {
                
                logErrorAction("GetPdf exception: ", e);
                throw;
            }
        }

        [HttpPost]
        [Route("mailingLabelsDoc")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdMailingLabelsDocPost([FromBody]ReportParameters parameters)
        {
            _logger.LogInformation("Owner Mailing Labels");

            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            IQueryable<HetOwner> ownerRecords = _context.HetOwners.AsNoTracking()
                .Include(x => x.PrimaryContact)
                .Include(x => x.LocalArea)
                    .ThenInclude(s => s.ServiceArea)
                        .ThenInclude(d => d.District)
                .Where(x => x.OwnerStatusTypeId == statusId)
                .OrderBy(x => x.LocalArea.Name).ThenBy(x => x.OrganizationName);

            if (parameters.Owners.Length > 0)
            {
                ownerRecords = ownerRecords.Where(x => parameters.Owners.Contains(x.OwnerId));
            }

            if (parameters.LocalAreas?.Length > 0)
            {
                ownerRecords = ownerRecords.Where(x => parameters.LocalAreas.Contains(x.LocalAreaId));
            }

            List<HetOwner> owners = ownerRecords.ToList();

            if (!owners.Any())
            {
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            var fileName = $"MailingLabels-{DateTime.Now:yyyy-MM-dd-H-mm}.docx";
            var file = MailingLabel.GetMailingLabel(owners, (errMessage, ex) => {
                _logger.LogError(errMessage);
                _logger.LogError(ex.ToString());
            });

            FileContentResult result = new(file, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {
                FileDownloadName = fileName
            };

            Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);

            return result;
        }

        #endregion

        #region Owner Equipment Records

        /// <summary>
        /// Get equipment associated with an owner
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to fetch Equipment for</param>
        [HttpGet]
        [Route("{id}/equipment")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<EquipmentDto>> OwnersIdEquipmentGet([FromRoute]int id)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get archive status
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _context);
            if (statusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            HetOwner owner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.DistrictEquipmentType)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetEquipmentAttachments)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetNotes)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetDigitalFiles)
                .Include(x => x.HetEquipments)
                    .ThenInclude(x => x.HetHistories)
                .First(a => a.OwnerId == id);

            // HETS-701: Archived pieces of equipment should not show up on the Owner's
            //           equipment list on the Owner edit screen
            var equipments = _mapper.Map<List<EquipmentDto>>(owner.HetEquipments.Where(x => x.EquipmentStatusTypeId != statusId).ToList());

            return new ObjectResult(new HetsResponse(equipments));
        }

        /// <summary>
        /// Owner Verification
        /// </summary>
        /// <remarks>Replaces an Owners Equipment</remarks>
        /// <param name="id">id of Owner to verify Equipment for</param>
        /// <param name="items">equipment to verify</param>
        [HttpPut]
        [Route("{id}/equipment")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<EquipmentDto>> OwnersIdEquipmentPut(
            [FromRoute]int id, 
            [FromBody]EquipmentDto[] items)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists || items == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            var entities = new List<HetEquipment>();

            // adjust the incoming list
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                if (item != null)
                {
                    DateTime lastVerifiedDate = DateUtils.AsUTC(item.LastVerifiedDate);

                    bool equipmentExists = _context.HetEquipments
                        .Any(x => x.EquipmentId == item.EquipmentId && item.OwnerId == id);

                    if (equipmentExists)
                    {
                        var equipment = _context.HetEquipments
                            .First(x => x.EquipmentId == item.EquipmentId);

                        if (equipment.LastVerifiedDate != lastVerifiedDate)
                        {
                            equipment.LastVerifiedDate = lastVerifiedDate;
                        }

                        entities.Add(equipment);
                    }
                }
            }

            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<EquipmentDto>>(entities)));
        }

        #endregion

        #region Transfer Equipment Records

        /// <summary>
        /// Equipment Transfer
        /// </summary>
        /// <remarks>Transfers an Owners Equipment</remarks>
        /// <param name="id">id of Owner to transfer Equipment from</param>
        /// <param name="targetOwnerId">id owner to transfer equipment to</param>
        /// <param name="includeSeniority"></param>
        /// <param name="items">equipment to transfer</param>
        [HttpPost]
        [Route("{id}/equipmentTransfer/{targetOwnerId}/{includeSeniority}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<EquipmentDto>> OwnersIdEquipmentTransferPost(
            [FromRoute]int id, 
            [FromRoute]int targetOwnerId,
            [FromRoute]bool includeSeniority, 
            [FromBody]EquipmentDto[] items)
        {
            bool ownerExists = _context.HetOwners.Any(a => a.OwnerId == id);
            bool targetOwnerExists = _context.HetOwners.Any(a => a.OwnerId == targetOwnerId);

            // not found
            if (OwnerOrEquipmentNotFound(ownerExists, targetOwnerExists, items)) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            if (id == targetOwnerId) 
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-34", ErrorViewModel.GetDescription("HETS-34", _configuration)));

            // get active owner status type
            int? ownerStatusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (ownerStatusId == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get active equipment status type
            int? equipmentStatusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (equipmentStatusId == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get archive equipment status type
            int? equipmentArchiveStatusId = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _context);
            if (equipmentArchiveStatusId == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            //***************************************************************
            // HETS-706: BVT Bulk Transfer
            //***************************************************************
            // Equipment Transfer Only:
            // * Equipment Transfer only can only happen between two "Approved"
            //   owner codes in the same district
            // * The selected equipment will be archived for the old owner,
            //   reason for change of status - "Bulk transfer to <new owner code>
            // * The selected equipment will be added to the new owner, with the
            //   Equip ID <newOwner code>-XXXX, as a brand new "Approved" equipment
            // * The seniority will re-start from this point
            // * The seniority and Hire Rotation List will be adjusted accordingly
            // * Set the registration date to the current registration date
            //***************************************************************
            // Equipment and Seniority Transfer:
            // * All of the above (plus)
            // * This newly added equipment will also retain the
            //   seniority information:
            //     YTD / YTD1 / YTD2 / YTD3 / YTD3 /
            //     Years Registered / Seniority
            //***************************************************************

            // get owner and new owner
            HetOwner currentOwner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetEquipments)
                .Include(x => x.LocalArea.ServiceArea.District)
                .First(a => a.OwnerId == id);

            HetOwner targetOwner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetEquipments)
                .Include(x => x.LocalArea.ServiceArea.District)
                .First(a => a.OwnerId == targetOwnerId);

            // check they are in the same district
            if (OwnersInDifferentDistricts(currentOwner, targetOwner))
            {
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-31", ErrorViewModel.GetDescription("HETS-31", _configuration)));
            }

            // check they are both active
            if (OwnersNotActive(currentOwner, targetOwner, ownerStatusId))
            {
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-32", ErrorViewModel.GetDescription("HETS-32", _configuration)));
            }

            var equipmentsToTransfer = new List<HetEquipment>();

            // check all pieces of equipment in the provided list belong to this owner
            foreach (var item in items)
            {
                // get full equipment record
                HetEquipment equipmentToTransfer = _context.HetEquipments
                    .Include(x => x.HetEquipmentAttachments)
                    .FirstOrDefault(x => x.EquipmentId == item.EquipmentId);

                if (EquipmentDoesntBelongToOwner(equipmentToTransfer, currentOwner))
                {
                    return new BadRequestObjectResult(
                        new HetsResponse("HETS-33", ErrorViewModel.GetDescription("HETS-33", _configuration)));
                }

                equipmentsToTransfer.Add(equipmentToTransfer);
            }

            //***************************************************************
            // get fiscal year
            //***************************************************************
            HetDistrictStatus district = _context.HetDistrictStatuses.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId == currentOwner.LocalArea.ServiceArea.District.DistrictId);

            if (district?.CurrentFiscalYear == null) 
                return new NotFoundObjectResult(
                    new HetsResponse("HETS-30", ErrorViewModel.GetDescription("HETS-30", _configuration)));

            int fiscalYear = (int)district.CurrentFiscalYear; // status table uses the start of the year
            DateTime fiscalStart = DateUtils.ConvertPacificToUtcTime(
                new DateTime(fiscalYear, 4, 1, 0, 0, 0, DateTimeKind.Unspecified));

            //***************************************************************
            // process each piece of equipment in the provided list
            //***************************************************************

            // need to start transaction so that old equipments can be replaced by new equipments in agreements
            using var transaction = _context.Database.BeginTransaction();

            foreach (var equipmentToTransfer in equipmentsToTransfer)
            {
                // get new owner code
                string newEquipmentCode = EquipmentHelper.GetEquipmentCode(targetOwner.OwnerId, _context);

                // create a "copy" of the record
                HetEquipment newEquipment = new()
                {
                    OwnerId = targetOwnerId,
                    LocalAreaId = equipmentToTransfer.LocalAreaId,
                    EquipmentStatusTypeId = (int)equipmentStatusId,
                    ApprovedDate = DateTime.UtcNow,
                    EquipmentCode = newEquipmentCode,
                    DistrictEquipmentTypeId = equipmentToTransfer.DistrictEquipmentTypeId,
                    Make = equipmentToTransfer.Make,
                    Model = equipmentToTransfer.Model,
                    Operator = equipmentToTransfer.Operator,
                    LicencePlate = equipmentToTransfer.LicencePlate,
                    SerialNumber = equipmentToTransfer.SerialNumber,
                    Size = equipmentToTransfer.Size,
                    PayRate = equipmentToTransfer.PayRate,
                    RefuseRate = equipmentToTransfer.RefuseRate,
                    YearsOfService = 0,
                    Year = equipmentToTransfer.Year,
                    LastVerifiedDate = DateUtils.AsUTC(equipmentToTransfer.LastVerifiedDate),
                    IsSeniorityOverridden = false,
                    SeniorityOverrideReason = "",
                    Type = equipmentToTransfer.Type,
                    ServiceHoursLastYear = 0,
                    ServiceHoursTwoYearsAgo = 0,
                    ServiceHoursThreeYearsAgo = 0,
                    SeniorityEffectiveDate = DateTime.UtcNow,
                    LicencedGvw = equipmentToTransfer.LicencedGvw,
                    LegalCapacity = equipmentToTransfer.LegalCapacity,
                    PupLegalCapacity = equipmentToTransfer.PupLegalCapacity
                };

                if (equipmentToTransfer.ReceivedDate is DateTime receivedDateUtc)
                {
                    newEquipment.ReceivedDate = DateUtils.AsUTC(receivedDateUtc);
                }

                newEquipment.HetEquipmentAttachments =
                    GetEquipmentAttachments(newEquipment.HetEquipmentAttachments, equipmentToTransfer);

                // seniority information:
                //   YTD / YTD1 / YTD2 / YTD3 / YTD3 /
                //   Years Registered / Seniority
                newEquipment = IncludeSeniorityWithEquipment(includeSeniority, newEquipment, equipmentToTransfer);

                _context.HetEquipments.Add(newEquipment);

                // update original equipment record
                equipmentToTransfer.EquipmentStatusTypeId = (int)equipmentArchiveStatusId;
                equipmentToTransfer.ArchiveCode = "Y";
                equipmentToTransfer.ArchiveDate = DateTime.UtcNow;
                equipmentToTransfer.ArchiveReason = "Bulk transfer to " + targetOwner.OwnerCode;

                _context.SaveChanges(); // this ensures newEquipment.EquimentId is assigned.

                UpdateRentalAgreements(includeSeniority, newEquipment.EquipmentId, equipmentToTransfer, fiscalStart);

                _context.SaveChanges();
            }

            //***************************************************************
            // we need to update seniority for all local areas and
            // district equipment types
            //***************************************************************
            equipmentsToTransfer = UpdateSeniorityForEquipmentTypes(equipmentsToTransfer);

            _context.SaveChanges();

            transaction.Commit();

            // return original items
            return new ObjectResult(
                new HetsResponse(
                    _mapper.Map<List<EquipmentDto>>(equipmentsToTransfer)));
        }

        private static bool EquipmentDoesntBelongToOwner(HetEquipment equipmentToTransfer, HetOwner currentOwner)
        {
            return equipmentToTransfer == null || equipmentToTransfer.OwnerId != currentOwner.OwnerId;
        }

        private static bool OwnerOrEquipmentNotFound(bool ownerExists, bool targetOwnerExists, EquipmentDto[] items)
        {
            return !ownerExists || !targetOwnerExists || items == null;
        }

        private static bool OwnersInDifferentDistricts(HetOwner currentOwner, HetOwner targetOwner)
        {
            return currentOwner.LocalArea.ServiceArea.District.DistrictId 
                != targetOwner.LocalArea.ServiceArea.District.DistrictId;
        }

        private static bool OwnersNotActive(HetOwner currentOwner, HetOwner targetOwner, int? ownerStatusId)
        {
            return currentOwner.OwnerStatusTypeId != ownerStatusId 
                || targetOwner.OwnerStatusTypeId != ownerStatusId;
        }

        private List<HetEquipment> UpdateSeniorityForEquipmentTypes(List<HetEquipment> equipmentsToTransfer)
        {
            List<int> districtEquipmentTypes = new();

            foreach (var equipmentToTransfer in equipmentsToTransfer)
            {
                if (equipmentToTransfer.LocalAreaId != null &&
                    equipmentToTransfer.DistrictEquipmentTypeId != null)
                {
                    int localAreaId = (int)equipmentToTransfer.LocalAreaId;
                    int districtEquipmentTypeId = (int)equipmentToTransfer.DistrictEquipmentTypeId;

                    // check whether we've processed this district already
                    if (districtEquipmentTypes.Contains(districtEquipmentTypeId))
                        continue;

                    districtEquipmentTypes.Add(districtEquipmentTypeId);

                    // recalculate seniority
                    EquipmentHelper.RecalculateSeniority(localAreaId, districtEquipmentTypeId, _context, _configuration, (errMessage, ex) => {
                        _logger.LogError("{errMessage}", errMessage);
                        _logger.LogError("{exception}", ex);
                    });
                }
            }

            return equipmentsToTransfer;
        }

        private static ICollection<HetEquipmentAttachment> GetEquipmentAttachments(
            ICollection<HetEquipmentAttachment> existingAttachments, HetEquipment equipmentToTransfer)
        {
            var attachments = existingAttachments ?? new List<HetEquipmentAttachment>();
            var newAttachments = equipmentToTransfer.HetEquipmentAttachments
                .Select(attachment => new HetEquipmentAttachment
                    {
                        Description = attachment.TypeName,
                        TypeName = attachment.TypeName
                    });

            foreach (HetEquipmentAttachment attachment in newAttachments)
            {
                attachments.Add(attachment);
            }
            return attachments;
        }

        private static HetEquipment IncludeSeniorityWithEquipment(
            bool includeSeniority, HetEquipment newEquipment, HetEquipment equipmentToTransfer)
        {
            if (includeSeniority)
            {
                newEquipment.ServiceHoursLastYear = equipmentToTransfer.ServiceHoursLastYear;
                newEquipment.ServiceHoursTwoYearsAgo = equipmentToTransfer.ServiceHoursTwoYearsAgo;
                newEquipment.ServiceHoursThreeYearsAgo = equipmentToTransfer.ServiceHoursThreeYearsAgo;
                newEquipment.YearsOfService = equipmentToTransfer.YearsOfService;
                newEquipment.Seniority = equipmentToTransfer.Seniority;
                if (equipmentToTransfer.ApprovedDate is DateTime approvedDateUtc)
                {
                    newEquipment.ApprovedDate = DateUtils.AsUTC(approvedDateUtc);
                }
            }
            return newEquipment;
        }

        private void UpdateRentalAgreements(
            bool includeSeniority, int newEquipmentId, HetEquipment equipmentToTransfer, DateTime fiscalStart)
        {
            if (!includeSeniority) return;

            // we also need to update all of the associated rental agreements
            // (for this fiscal year)
            DateTime fiscalStartUtc = DateUtils.AsUTC(fiscalStart);

            IQueryable<HetRentalAgreement> agreements = _context.HetRentalAgreements
                .Where(x => x.EquipmentId == equipmentToTransfer.EquipmentId &&
                            x.DatedOn >= fiscalStartUtc);

            foreach (HetRentalAgreement agreement in agreements)
            {
                agreement.EquipmentId = newEquipmentId;
            }
        }

        #endregion

        #region Owner Attachments

        /// <summary>
        /// Get attachments associated with an owner
        /// </summary>
        /// <remarks>Returns attachments for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch attachments for</param>
        [HttpGet]
        [Route("{id}/attachments")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<DigitalFileDto>> OwnersIdAttachmentsGet([FromRoute]int id)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetOwner owner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetDigitalFiles)
                .First(a => a.OwnerId == id);

            // extract the attachments and update properties for UI
            List<HetDigitalFile> attachments = new List<HetDigitalFile>();

            foreach (HetDigitalFile attachment in owner.HetDigitalFiles)
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

            return new ObjectResult(new HetsResponse(_mapper.Map<List<DigitalFileDto>>(attachments)));
        }

        #endregion

        #region Owner Contact Records

        /// <summary>
        /// Get contacts associated with an owner
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        [HttpGet]
        [Route("{id}/contacts")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<ContactDto>> OwnersIdContactsGet([FromRoute]int id)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetOwner owner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetContacts)
                .First(a => a.OwnerId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<List<ContactDto>>(owner.HetContacts.ToList())));
        }

        /// <summary>
        /// Create owner contact
        /// </summary>
        /// <remarks>Adds Owner Contact</remarks>
        /// <param name="id">id of Owner to add a contact for</param>
        /// <param name="primary"></param>
        /// <param name="item">Adds to Owner Contact</param>
        [HttpPost]
        [Route("{id}/contacts/{primary}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<ContactDto> OwnersIdContactsPost([FromRoute]int id, [FromRoute]bool primary, [FromBody]ContactDto item)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists || item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int contactId;

            // get owner record
            HetOwner owner = _context.HetOwners
                .Include(x => x.HetContacts)
                .First(a => a.OwnerId == id);

            using var transaction = _context.Database.BeginTransaction();

            // add or update contact
            if (item.ContactId > 0)
            {
                HetContact contact = owner.HetContacts.FirstOrDefault(a => a.ContactId == item.ContactId);

                // not found
                if (contact == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                contactId = item.ContactId;

                contact.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
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
                    owner.PrimaryContactId = contactId;
                }
            }
            else  // add contact
            {
                HetContact contact = new HetContact
                {
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
                    Role = item.Role,
                    OwnerId = id
                };

                _context.HetContacts.Add(contact);
                _context.SaveChanges();

                contactId = contact.ContactId;

                if (primary)
                {
                    owner.PrimaryContactId = contactId;
                }
            }

            _context.SaveChanges();
            transaction.Commit();

            // get updated contact record
            HetOwner updatedOwner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetContacts)
                .First(a => a.OwnerId == id);

            HetContact updatedContact = updatedOwner.HetContacts
                .FirstOrDefault(a => a.ContactId == contactId);

            return new ObjectResult(new HetsResponse(_mapper.Map<ContactDto>(updatedContact)));
        }

        /// <summary>
        /// Update owner contacts
        /// </summary>
        /// <remarks>Replaces an Owners Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="items">Replacement Owner contacts</param>
        [HttpPut]
        [Route("{id}/contacts")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<ContactDto>> OwnersIdContactsPut([FromRoute]int id, [FromBody]ContactDto[] items)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists || items == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get owner record
            HetOwner owner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetContacts)
                .First(a => a.OwnerId == id);

            var contactIds = new List<int>();

            // adjust the incoming list
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                if (item == null)
                    continue;

                bool contactExists = _context.HetContacts.Any(x => x.ContactId == item.ContactId);

                if (contactExists)
                {
                    HetContact temp = _context.HetContacts.First(x => x.ContactId == item.ContactId);

                    temp.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                    temp.OwnerId = id;
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

                    contactIds.Add(temp.ContactId);
                }
                else
                {
                    HetContact temp = new HetContact
                    {
                        OwnerId = id,
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

                    owner.HetContacts.Add(temp);
                    contactIds.Add(temp.ContactId);
                }
            }

            // remove contacts that are no longer attached.
            foreach (HetContact contact in owner.HetContacts)
            {
                if (contact != null && contactIds.All(x => x != contact.ContactId))
                {
                    _context.HetContacts.Remove(contact);
                }
            }

            // save changes
            _context.SaveChanges();

            // get updated contact records
            HetOwner updatedOwner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetContacts)
                .First(a => a.OwnerId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<List<ContactDto>>(updatedOwner.HetContacts.ToList())));
        }

        #endregion

        #region Owner History Records

        /// <summary>
        /// Get history associated with owner
        /// </summary>
        /// <remarks>Returns History for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned</param>
        [HttpGet]
        [Route("{id}/history")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<History>> OwnersIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            return new ObjectResult(new HetsResponse(_ownerRepo.GetHistoryRecords(id, offset, limit)));
        }

        /// <summary>
        /// Create owner history
        /// </summary>
        /// <remarks>Add a History record to the Owner</remarks>
        /// <param name="id">id of Owner to add History for</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/history")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<History>> OwnersIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            if (exists)
            {
                HetHistory history = new()
                {
                    HistoryId = 0,
                    HistoryText = item.HistoryText,
                    CreatedDate = DateTime.UtcNow,
                    OwnerId = id
                };

                _context.HetHistories.Add(history);
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(_ownerRepo.GetHistoryRecords(id, null, null)));
        }

        #endregion

        #region Owner Note Records

        /// <summary>
        /// Get note records associated with owner
        /// </summary>
        /// <param name="id">id of Owner to fetch Notes for</param>
        [HttpGet]
        [Route("{id}/notes")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<NoteDto>> OwnersIdNotesGet([FromRoute]int id)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetOwner owner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetNotes)
                .First(x => x.OwnerId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in owner.HetNotes)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<NoteDto>>(notes)));
        }

        /// <summary>
        /// Update or create a note associated with a owner
        /// </summary>
        /// <remarks>Update an Owners Notes</remarks>
        /// <param name="id">id of Owner to update Notes for</param>
        /// <param name="item">Owner Note</param>
        [HttpPost]
        [Route("{id}/note")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<NoteDto>> OwnersIdNotePost([FromRoute]int id, [FromBody]NoteDto item)
        {
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

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
                    OwnerId = id,
                    Text = item.Text,
                    IsNoLongerRelevant = item.IsNoLongerRelevant
                };

                _context.HetNotes.Add(note);
            }

            _context.SaveChanges();

            // return updated note records
            HetOwner owner = _context.HetOwners.AsNoTracking()
                .Include(x => x.HetNotes)
                .First(x => x.OwnerId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in owner.HetNotes)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<List<NoteDto>>(notes)));
        }

        #endregion

        #region Generate Shared Keys

        /// <summary>
        /// Generate shared keys for all owners
        /// that don't have an associated business yet
        /// </summary>
        [HttpPost]
        [Route("GenerateKeys")]
        [RequiresPermission(HetPermission.Admin, HetPermission.WriteAccess)]
        public virtual IActionResult OwnersGenerateKeysPost()
        {
            // get records
            List<HetOwner> owners = _context.HetOwners.AsNoTracking()
                .Where(x => x.BusinessId == null)
                .ToList();

            int i = 0;

            foreach (HetOwner owner in owners)
            {
                i++;
                string key = SecretKeyHelper.RandomString(8, owner.OwnerId);

                string temp = owner.OwnerCode;

                if (string.IsNullOrEmpty(temp))
                {
                    temp = SecretKeyHelper.RandomString(4, owner.OwnerId);
                }

                key = temp + "-" + DateTime.UtcNow.Year + "-" + key;

                // get owner and update
                HetOwner ownerRecord = _context.HetOwners.First(x => x.OwnerId == owner.OwnerId);
                ownerRecord.SharedKey = key;

                if (i % 500 == 0)
                {
                    _context.SaveChangesForImport();
                }
            }

            // save remaining updates
            _context.SaveChangesForImport();

            // return ok
            return new ObjectResult(new HetsResponse(""));
        }

        /// <summary>
        /// Generate shared keys for all owners
        /// that don't have an associated business yet
        /// </summary>
        [HttpPost]
        [Route("GenerateKeysApi")]
        [RequiresPermission(HetPermission.Admin, HetPermission.WriteAccess)]
        public virtual IActionResult GenerateKeysApiPost()
        {
            // security...
            if (!_httpContext.Connection.RemoteIpAddress.ToString().StartsWith("::1") &&
                !_httpContext.Connection.RemoteIpAddress.ToString().StartsWith("::ffff:127.0.0.1"))
            {
                // return ok
                return new JsonResult("Secret Keys can only be updated from the server");
            }

            // get records
            List<HetOwner> owners = _context.HetOwners.AsNoTracking()
                .Where(x => x.BusinessId == null)
                .ToList();

            int i = 0;

            foreach (HetOwner owner in owners)
            {
                i++;
                string key = SecretKeyHelper.RandomString(8, owner.OwnerId);

                string temp = owner.OwnerCode;

                if (string.IsNullOrEmpty(temp))
                {
                    temp = SecretKeyHelper.RandomString(4, owner.OwnerId);
                }

                key = temp + "-" + DateTime.UtcNow.Year + "-" + key;

                // get owner and update
                HetOwner ownerRecord = _context.HetOwners.First(x => x.OwnerId == owner.OwnerId);
                ownerRecord.SharedKey = key;

                if (i % 500 == 0)
                {
                    _context.SaveChangesForImport();
                }
            }

            // save remaining updates
            _context.SaveChangesForImport();

            // return ok
            return new JsonResult("Secret Keys Updated");
        }

        #endregion

        #region Wcb / Cgl Report

        /// <summary>
        /// Owner Wcb / Cgl Report
        /// </summary>
        /// <remarks>Used for the rental request search page.</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="owners">Owners (comma separated list of id numbers)</param>
        /// <param name="wcbExpiry">Find owners whose WCB policy expires before this date</param>
        /// <param name="cglExpiry">Find owners whose CGL policy expires before this date</param>
        [HttpGet]
        [Route("wcbCglReport")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<OwnerWcbCgl>> OwnerWcbCglGet(
            [FromQuery]string localAreas, 
            [FromQuery]string owners,
            [FromQuery]DateTime? wcbExpiry, 
            [FromQuery]DateTime? cglExpiry)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] ownerArray = ArrayHelper.ParseIntArray(owners);

            // owner status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) 
                return new BadRequestObjectResult(
                    new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            DateTime? wcbExpiryUTC = wcbExpiry is DateTime wcbExpiryDt ? DateUtils.AsUTC(wcbExpiryDt) : null;
            DateTime? cglExpiryUTC = cglExpiry is DateTime cglExpiryDt ? DateUtils.AsUTC(cglExpiryDt) : null;

            IQueryable<HetOwner> data = _context.HetOwners.AsNoTracking()
                .Include(y => y.LocalArea.ServiceArea)
                .Include(x => x.PrimaryContact)
                .Where(x => 
                    x.LocalArea.ServiceArea.DistrictId.Equals(districtId) 
                    && x.OwnerStatusTypeId == statusId 
                    && (
                        x.WorkSafeBcexpiryDate == null 
                        || wcbExpiry == null 
                        || wcbExpiryUTC == null
                        || x.WorkSafeBcexpiryDate < wcbExpiryUTC) 
                    && (
                        x.CglendDate == null 
                        || cglExpiry == null 
                        || cglExpiryUTC == null
                        || x.CglendDate < cglExpiryUTC));

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.LocalAreaId));
            }

            if (ownerArray != null && ownerArray.Length > 0)
            {
                data = data.Where(x => ownerArray.Contains(x.OwnerId));
            }

            // convert Rental Request Model to the "RentalRequestHires" Model
            List<OwnerWcbCgl> result = new();

            foreach (HetOwner item in data)
            {
                result.Add(OwnerHelper.ToWcbCglModel(item));
            }

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

        #endregion
    }
}
