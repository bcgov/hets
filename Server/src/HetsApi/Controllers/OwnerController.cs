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
using Microsoft.EntityFrameworkCore.Storage;

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
    public class OwnerController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private readonly ILogger _logger;

        public OwnerController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = loggerFactory.CreateLogger<OwnerController>();

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }

        /// <summary>
        /// Get owner by id
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("OwnersIdGet")]
        [SwaggerResponse(200, type: typeof(HetOwner))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdGet([FromRoute]int id)
        {
            return new ObjectResult(new HetsResponse(OwnerHelper.GetRecord(id, _context, _configuration)));
        }

        /// <summary>
        /// Get all owners for this district
        /// </summary>
        [HttpGet]
        [Route("lite")]
        [SwaggerOperation("OwnersGetLite")]
        [SwaggerResponse(200, type: typeof(List<OwnerLiteProjects>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersGetLite()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get active status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get all active owners for this district
            IEnumerable<OwnerLiteProjects> owners = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetRentalAgreement)
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
        [SwaggerOperation("OwnersGetLiteHires")]
        [SwaggerResponse(200, type: typeof(List<OwnerLiteProjects>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersGetLiteHires()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get active status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get all active owners for this district (and any projects they're associated with)
            IEnumerable<OwnerLiteProjects> owners = _context.HetRentalRequestRotationList.AsNoTracking()
                .Include(x => x.RentalRequest)
                    .ThenInclude(y => y.LocalArea)
                        .ThenInclude(z => z.ServiceArea)
                .Include(x => x.RentalRequest)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Where(x => x.RentalRequest.LocalArea.ServiceArea.DistrictId.Equals(districtId))
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
        [SwaggerOperation("OwnersGetLiteTs")]
        [SwaggerResponse(200, type: typeof(List<OwnerLiteProjects>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersGetLiteTs()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // get active status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get fiscal year
            HetDistrictStatus status = _context.HetDistrictStatus.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;
            if (fiscalYear == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // fiscal year in the status table stores the "start" of the year
            DateTime fiscalYearStart = new DateTime((int)fiscalYear, 3, 31);

            // get all active owners for this district (and any projects they're associated with)
            IEnumerable<OwnerLiteProjects> owners = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Project)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Where(x => x.Equipment.LocalArea.ServiceArea.DistrictId == districtId &&
                            x.Equipment.Owner.OwnerStatusTypeId == statusId &&
                            x.Project.AppCreateTimestamp > fiscalYearStart)
                .GroupBy(x => x.Equipment.Owner, (o, agreements) => new OwnerLiteProjects
                {
                    OwnerCode = o.OwnerCode,
                    OrganizationName = o.OrganizationName,
                    Id = o.OwnerId,
                    LocalAreaId = o.LocalAreaId,
                    ProjectIds = agreements.Select(y => y.ProjectId).ToList()
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
        [SwaggerOperation("OwnersIdPut")]
        [SwaggerResponse(200, type: typeof(HetOwner))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdPut([FromRoute]int id, [FromBody]HetOwner item)
        {
            if (item == null || id != item.OwnerId)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetOwner owner = _context.HetOwner.First(x => x.OwnerId == item.OwnerId);

            int? oldLocalArea = owner.LocalAreaId;
            bool? oldIsMaintenanceContractor = owner.IsMaintenanceContractor;

            if (item.RegisteredCompanyNumber == "") item.RegisteredCompanyNumber = null;
            if (item.WorkSafeBcpolicyNumber == "") item.WorkSafeBcpolicyNumber = null;

            owner.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            owner.CglCompanyName = item.CglCompanyName;
            owner.CglendDate = item.CglendDate;
            owner.CglPolicyNumber = item.CglPolicyNumber;
            owner.LocalAreaId = item.LocalArea.LocalAreaId;
            owner.WorkSafeBcexpiryDate = item.WorkSafeBcexpiryDate;
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
                IQueryable<HetEquipment> equipmentList = _context.HetEquipment
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
                if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                // get processing rules
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);

                // get active equipment
                IQueryable<HetEquipment> equipmentListB = _context.HetEquipment
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
                    SeniorityListHelper.AssignBlocks(localAreaId, districtEquipmentTypeId, blockSize, totalBlocks, _context);

                    // update old area block assignments
                    if (oldLocalArea != null)
                    {
                        SeniorityListHelper.AssignBlocks((int)oldLocalArea, districtEquipmentTypeId, blockSize, totalBlocks, _context);
                    }
                }
            }

            // save the changes
            _context.SaveChanges();

            // retrieve updated owner record to return to ui
            return new ObjectResult(new HetsResponse(OwnerHelper.GetRecord(id, _context, _configuration)));
        }

        /// <summary>
        /// Update owner status
        /// </summary>
        /// <param name="id">id of Owner to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}/status")]
        [SwaggerOperation("OwnersIdStatusPut")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdStatusPut([FromRoute]int id, [FromBody]OwnerStatus item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetOwner owner = _context.HetOwner
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.LocalArea)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .First(x => x.OwnerId == id);

            // get status id
            int? statusId = StatusHelper.GetStatusId(item.Status, "ownerStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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
                foreach (HetEquipment equipment in owner.HetEquipment)
                {
                    // used for seniority recalculation
                    int localAreaId = equipment.LocalArea.LocalAreaId;
                    int districtEquipmentTypeId = equipment.DistrictEquipmentType.DistrictEquipmentTypeId;

                    // get status id
                    int? eqStatusId = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _context);
                    if (eqStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                    // if the equipment is already archived - leave it archived
                    // if the equipment is already in the same state as the owner's new state - then ignore
                    if (!equipment.EquipmentStatusTypeId.Equals(eqStatusId) && !equipment.EquipmentStatusTypeId.Equals(statusId))
                    {
                        eqStatusId = StatusHelper.GetStatusId(item.Status, "equipmentStatus", _context);
                        if (eqStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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
                        EquipmentHelper.RecalculateSeniority(localAreaId, districtEquipmentTypeId, _context, _configuration);
                    }
                }
            }

            // save the changes
            _context.SaveChanges();

            // retrieve updated owner record to return to ui
            return new ObjectResult(new HetsResponse(OwnerHelper.GetRecord(id, _context, _configuration)));
        }

        /// <summary>
        /// Create owner
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("OwnersPost")]
        [SwaggerResponse(200, type: typeof(HetOwner))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersPost([FromBody]HetOwner item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get status id
            int? statusId = StatusHelper.GetStatusId(item.Status, "ownerStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // create record
            HetOwner owner = new HetOwner
            {
                OwnerStatusTypeId = (int)statusId,
                CglCompanyName = item.CglCompanyName,
                CglendDate = item.CglendDate,
                CglPolicyNumber = item.CglPolicyNumber,
                LocalAreaId = item.LocalArea.LocalAreaId,
                WorkSafeBcexpiryDate = item.WorkSafeBcexpiryDate,
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
            _context.HetOwner.Add(owner);
            _context.SaveChanges();

            int id = owner.OwnerId;

            // update contact record
            int? contactId = owner.PrimaryContact.ContactId;

            HetContact contact = _context.HetContact.FirstOrDefault(x => x.ContactId == contactId);

            if (contact != null)
            {
                contact.OwnerId = id;
                _context.SaveChanges();
            }

            // retrieve updated owner record to return to ui
            return new ObjectResult(new HetsResponse(OwnerHelper.GetRecord(id, _context, _configuration)));
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
        [SwaggerOperation("OwnersSearchGet")]
        [SwaggerResponse(200, type: typeof(List<OwnerLite>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersSearchGet([FromQuery]string localAreas,
            [FromQuery]string equipmentTypes, [FromQuery]int? owner, [FromQuery]string status,
            [FromQuery]bool? hired, [FromQuery]string ownerName = null, [FromQuery]string ownerCode = null)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] equipmentTypesArray = ArrayHelper.ParseIntArray(equipmentTypes);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            IQueryable<HetOwner> data = _context.HetOwner.AsNoTracking()
                .Include(x => x.LocalArea)
                    .ThenInclude(y => y.ServiceArea)
                        .ThenInclude(z => z.District)
                .Include(x => x.HetEquipment)
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

            if (hired == true)
            {
                IQueryable<int?> hiredOwnersQuery = _context.HetRentalAgreement
                                    .Where(agreement => agreement.Status == "Active")
                                    .Join(
                                        _context.HetEquipment,
                                        agreement => agreement.EquipmentId,
                                        equipment => equipment.EquipmentId,
                                        (agreement, equipment) => new
                                        {
                                            tempAgreement = agreement,
                                            tempEqiupment = equipment
                                        }
                                    )
                                    .Where(projection => projection.tempEqiupment.OwnerId != null)
                                    .Select(projection => projection.tempEqiupment.OwnerId)
                                    .Distinct();

                data = data.Where(o => hiredOwnersQuery.Contains(o.OwnerId));
            }

            if (equipmentTypesArray != null)
            {
                IQueryable<int?> equipmentTypeQuery = _context.HetEquipment
                    .Where(x => equipmentTypesArray.Contains(x.DistrictEquipmentTypeId))
                    .Select(x => x.OwnerId)
                    .Distinct();

                data = data.Where(x => equipmentTypeQuery.Contains(x.OwnerId));
            }

            if (owner != null)
            {
                data = data.Where(x => x.OwnerId == owner);
            }

            if (ownerName != null)
            {
                data = data.Where(x => x.OrganizationName.ToLower().Contains(ownerName.ToLower()));
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

        #region Get Verification Pdfs

        /// <summary>
        /// Get owner verification pdf
        /// </summary>
        /// <remarks>Returns a PDF version of the owner verification notices</remarks>
        /// <param name="parameters">Array of local area and owner id numbers to generate notices for</param>
        [HttpPost]
        [Route("verificationPdf")]
        [SwaggerOperation("OwnersIdVerificationPdfPost")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdVerificationPdfPost([FromBody]ReportParameters parameters)
        {
            // get equipment status
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            _logger.LogInformation("Owner Verification Notices Pdf");

            // get owner records
            IQueryable<HetOwner> ownerRecords = _context.HetOwner.AsNoTracking()
                .Include(x => x.PrimaryContact)
                .Include(x => x.Business)
                .Include(x => x.HetEquipment)
                    .ThenInclude(a => a.HetEquipmentAttachment)
                .Include(x => x.HetEquipment)
                    .ThenInclude(l => l.LocalArea)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.LocalArea)
                    .ThenInclude(s => s.ServiceArea)
                        .ThenInclude(d => d.District)
                .OrderBy(x => x.LocalArea.Name).ThenBy(x => x.OrganizationName);

            if (parameters.Owners?.Length > 0)
            {
                ownerRecords = ownerRecords.Where(x => parameters.Owners.Contains(x.OwnerId));
            }

            if (parameters.LocalAreas?.Length > 0)
            {
                ownerRecords = ownerRecords.Where(x => parameters.LocalAreas.Contains(x.LocalAreaId));
            }

            // convert to list
            List<HetOwner> ownerList = ownerRecords.ToList();

            // strip out inactive and archived equipment
            foreach (HetOwner owner in ownerList)
            {
                owner.HetEquipment = owner.HetEquipment.Where(x => x.EquipmentStatusTypeId == statusId).ToList();
            }

            if (ownerList.Any())
            {
                // get address and contact info
                string address = ownerList[0].LocalArea.ServiceArea.Address;

                if (!string.IsNullOrEmpty(address))
                {
                    address = address.Replace("  ", " ");
                    address = address.Replace("amp;", "and");
                    address = address.Replace("|", " | ");
                }
                else
                {
                    address = "";
                }

                string contact = $"Phone: {ownerList[0].LocalArea.ServiceArea.Phone} | Fax: {ownerList[0].LocalArea.ServiceArea.Fax}";

                // generate pdf document name [unique portion only]
                string fileName = "OwnerVerification";

                // setup model for submission to the Pdf service
                OwnerVerificationPdfViewModel model = new OwnerVerificationPdfViewModel
                {
                    ReportDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    Title = fileName,
                    DistrictId = ownerList[0].LocalArea.ServiceArea.District.DistrictId,
                    MinistryDistrictId = ownerList[0].LocalArea.ServiceArea.District.MinistryDistrictId,
                    DistrictName = ownerList[0].LocalArea.ServiceArea.District.Name,
                    DistrictAddress = address,
                    DistrictContact = contact,
                    LocalAreaName = ownerList[0].LocalArea.Name,
                    Owners = new List<HetOwner>()
                };

                // add owner records - must verify district ids too
                foreach (HetOwner owner in ownerRecords)
                {
                    if (owner.LocalArea.ServiceArea.District == null ||
                        owner.LocalArea.ServiceArea.District.DistrictId != model.DistrictId)
                    {
                        // missing district - data error [HETS-16]
                        return new ObjectResult(new HetsResponse("HETS-16", ErrorViewModel.GetDescription("HETS-16", _configuration)));
                    }

                    owner.ReportDate = model.ReportDate;
                    owner.Title = model.Title;
                    owner.DistrictId = model.DistrictId;
                    owner.MinistryDistrictId = model.MinistryDistrictId;
                    owner.DistrictName = model.DistrictName;
                    owner.DistrictAddress = model.DistrictAddress;
                    owner.DistrictContact = model.DistrictContact;
                    owner.LocalAreaName = model.LocalAreaName;

                    if (!string.IsNullOrEmpty(owner.SharedKey))
                    {
                        owner.SharedKeyHeader = "Secret Key: ";
                    }
                    else
                    {
                        //"Business Name: label and value instead"
                        owner.SharedKeyHeader = "Business Name: ";

                        if (owner.Business != null && !string.IsNullOrEmpty(owner.Business.BceidLegalName))
                        {
                            owner.SharedKeyHeader = "Business Name: " + owner.Business.BceidLegalName;
                        }
                    }

                    model.Owners.Add(owner);
                }

                // setup payload
                string payload = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                });

                _logger.LogInformation("Owner Verification Notices Pdf - Payload Length: {0}", payload.Length);

                // pass the request on to the Pdf Micro Service
                string pdfHost = _configuration["PDF_SERVICE_NAME"];
                string pdfUrl = _configuration.GetSection("Constants:OwnerVerificationPdfUrl").Value;
                string targetUrl = pdfHost + pdfUrl;

                targetUrl = targetUrl + "/" + fileName;

                _logger.LogInformation("Owner Verification Notices Pdf - HETS Pdf Service Url: {0}", targetUrl);

                // call the MicroService
                try
                {
                    HttpClient client = new HttpClient();
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    _logger.LogInformation("Owner Verification Notices Pdf - Calling HETS Pdf Service");
                    HttpResponseMessage response = client.PostAsync(targetUrl, stringContent).Result;

                    // success
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        _logger.LogInformation("Owner Verification Notices Pdf - HETS Pdf Service Response: OK");

                        var pdfResponseBytes = GetPdf(response);

                        // convert to string and log
                        string pdfResponse = Encoding.Default.GetString(pdfResponseBytes);

                        fileName = fileName + $"-{DateTime.Now:yyyy-MM-dd-H-mm}" + ".pdf";

                        _logger.LogInformation("Owner Verification Notices Pdf - HETS Pdf Filename: {0}", fileName);
                        _logger.LogInformation("Owner Verification Notices Pdf - HETS Pdf Size: {0}", pdfResponse.Length);

                        // return content
                        FileContentResult result = new FileContentResult(pdfResponseBytes, "application/pdf")
                        {
                            FileDownloadName = fileName
                        };

                        Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);

                        return result;
                    }

                    _logger.LogInformation("Owner Verification Notices Pdf - HETS Pdf Service Response: {0}", response.StatusCode);
                }
                catch (Exception ex)
                {
                    Debug.Write("Error generating pdf: " + ex.Message);
                    return new ObjectResult(new HetsResponse("HETS-15", ErrorViewModel.GetDescription("HETS-15", _configuration)));
                }

                // problem occured
                return new ObjectResult(new HetsResponse("HETS-15", ErrorViewModel.GetDescription("HETS-15", _configuration)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
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

        #region Get Mailing Labels Pdfs

        /// <summary>
        /// Get owner mailing labels pdf
        /// </summary>
        /// <remarks>Returns a PDF version of the owner mailing labels</remarks>
        /// <param name="parameters">Array of local area and owner ids to generate labels for</param>
        [HttpPost]
        [Route("mailingLabelsPdf")]
        [SwaggerOperation("OwnersIdMailingLabelsPdfPost")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdMailingLabelsPdfPost([FromBody]ReportParameters parameters)
        {
            _logger.LogInformation("Owner Mailing Labels Pdf");

            // HETS-1041 - Mailing Labels return also Inactive Owners
            // ** Only return Active owner records
            // get active status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get owner records
            IQueryable<HetOwner> ownerRecords = _context.HetOwner.AsNoTracking()
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

            if (ownerList.Any())
            {
                // generate pdf document name [unique portion only]
                string fileName = "MailingLabels";

                // setup model for submission to the Pdf service
                MailingLabelPdfViewModel model = new MailingLabelPdfViewModel
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
                    if (owner.LocalArea.ServiceArea.District == null ||
                        owner.LocalArea.ServiceArea.District.DistrictId != model.DistrictId)
                    {
                        // missing district - data error [HETS-16]
                        return new ObjectResult(new HetsResponse("HETS-16", ErrorViewModel.GetDescription("HETS-16", _configuration)));
                    }

                    owner.ReportDate = model.ReportDate;
                    owner.Title = model.Title;
                    owner.DistrictId = model.DistrictId;

                    switch (column)
                    {
                        case 1:
                            model.LabelRow.Add(new MailingLabelRowModel());
                            model.LabelRow.Last().OwnerColumn1 = owner;
                            break;
                        default:
                            model.LabelRow.Last().OwnerColumn2 = owner;
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
                    Formatting = Formatting.Indented,
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                });

                _logger.LogInformation("Owner Mailing Labels Pdf - Payload Length: {0}", payload.Length);

                // pass the request on to the Pdf Micro Service
                string pdfHost = _configuration["PDF_SERVICE_NAME"];
                string pdfUrl = _configuration.GetSection("Constants:OwnerMailingLabelsPdfUrl").Value;
                string targetUrl = pdfHost + pdfUrl;

                targetUrl = targetUrl + "/" + fileName;

                _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Service Url: {0}", targetUrl);

                // call the MicroService
                try
                {
                    HttpClient client = new HttpClient();
                    StringContent stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                    _logger.LogInformation("Owner Mailing Labels Pdf - Calling HETS Pdf Service");
                    HttpResponseMessage response = client.PostAsync(targetUrl, stringContent).Result;

                    // success
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Service Response: OK");

                        var pdfResponseBytes = GetPdf(response);

                        // convert to string and log
                        string pdfResponse = Encoding.Default.GetString(pdfResponseBytes);

                        fileName = fileName + $"-{DateTime.Now:yyyy-MM-dd-H-mm}" + ".pdf";

                        _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Filename: {0}", fileName);
                        _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Size: {0}", pdfResponse.Length);

                        // return content
                        FileContentResult result = new FileContentResult(pdfResponseBytes, "application/pdf")
                        {
                            FileDownloadName = fileName
                        };

                        Response.Headers.Add("Content-Disposition", "inline; filename=" + fileName);

                        return result;
                    }

                    _logger.LogInformation("Owner Mailing Labels Pdf - HETS Pdf Service Response: {0}", response.StatusCode);
                }
                catch (Exception ex)
                {
                    Debug.Write("Error generating pdf: " + ex.Message);
                    return new ObjectResult(new HetsResponse("HETS-15", ErrorViewModel.GetDescription("HETS-15", _configuration)));
                }

                // problem occured
                return new ObjectResult(new HetsResponse("HETS-15", ErrorViewModel.GetDescription("HETS-15", _configuration)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
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
        [SwaggerOperation("OwnersIdEquipmentGet")]
        [SwaggerResponse(200, type: typeof(List<HetEquipment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdEquipmentGet([FromRoute]int id)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get archive status
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.DistrictEquipmentType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.Owner)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetEquipmentAttachment)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetNote)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetDigitalFile)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.HetHistory)
                .First(a => a.OwnerId == id);

            // HETS-701: Archived pieces of equipment should not show up on the Owner's
            //           equipment list on the Owner edit screen
            List<HetEquipment> equipments = owner.HetEquipment.Where(x => x.EquipmentStatusTypeId != statusId).ToList();

            return new ObjectResult(new HetsResponse(equipments));
        }

        /// <summary>
        /// Create owner equipment
        /// </summary>
        /// <remarks>Replaces an Owners Equipment</remarks>
        /// <param name="id">id of Owner to replace Equipment for</param>
        /// <param name="items">replacement owner equipment</param>
        [HttpPut]
        [Route("{id}/equipment")]
        [SwaggerOperation("OwnersIdEquipmentPut")]
        [SwaggerResponse(200, type: typeof(List<HetEquipment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdEquipmentPut([FromRoute]int id, [FromBody]HetEquipment[] items)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists || items == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetOwner owner = _context.HetOwner
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.Owner)
                .Include(x => x.HetNote)
                .Include(x => x.HetDigitalFile)
                .Include(x => x.HetHistory)
                .Include(x => x.HetContact)
                .First(x => x.OwnerId == id);

            // adjust the incoming list
            for (int i = 0; i < items.Length; i++)
            {
                HetEquipment item = items[i];

                if (item != null)
                {
                    DateTime lastVerifiedDate = item.LastVerifiedDate;

                    bool equipmentExists = _context.HetEquipment.Any(x => x.EquipmentId == item.EquipmentId);

                    if (equipmentExists)
                    {
                        items[i] = _context.HetEquipment
                            .Include(x => x.LocalArea.ServiceArea.District.Region)
                            .Include(x => x.DistrictEquipmentType)
                            .Include(x => x.Owner)
                            .Include(x => x.HetEquipmentAttachment)
                            .Include(x => x.HetNote)
                            .Include(x => x.HetDigitalFile)
                            .Include(x => x.HetHistory)
                            .First(x => x.EquipmentId == item.EquipmentId);

                        if (items[i].LastVerifiedDate != lastVerifiedDate)
                        {
                            items[i].LastVerifiedDate = lastVerifiedDate;
                            _context.HetEquipment.Update(items[i]);
                        }
                    }
                    else
                    {
                        _context.Add(item);
                        items[i] = item;
                    }
                }
            }

            // remove equipment that are no longer attached
            List<HetEquipment> equipmentToRemove = new List<HetEquipment>();

            foreach (HetEquipment equipment in owner.HetEquipment)
            {
                if (equipment != null && items.All(x => x.EquipmentId != equipment.EquipmentId))
                {
                    equipmentToRemove.Add(equipment);
                }
            }

            if (equipmentToRemove.Count > 0)
            {
                foreach (HetEquipment equipment in equipmentToRemove)
                {
                    owner.HetEquipment.Remove(equipment);
                }
            }

            // replace Equipment List
            owner.HetEquipment = items.ToList();
            _context.HetOwner.Update(owner);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(items));
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
        [SwaggerOperation("OwnersIdEquipmentTransferPost")]
        [SwaggerResponse(200, type: typeof(List<HetEquipment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdEquipmentTransferPost([FromRoute]int id, [FromRoute]int targetOwnerId,
            [FromRoute]bool includeSeniority, [FromBody]HetEquipment[] items)
        {
            bool ownerExists = _context.HetOwner.Any(a => a.OwnerId == id);
            bool targetOwnerExists = _context.HetOwner.Any(a => a.OwnerId == targetOwnerId);

            // not found
            if (!ownerExists || !targetOwnerExists || items == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            if (id == targetOwnerId) return new ObjectResult(new HetsResponse("HETS-34", ErrorViewModel.GetDescription("HETS-34", _configuration)));

            // get active owner status type
            int? ownerStatusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (ownerStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get active equipment status type
            int? equipmentStatusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (equipmentStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get archive equipment status type
            int? equipmentArchiveStatusId = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _context);
            if (equipmentArchiveStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

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
            HetOwner currentOwner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetEquipment)
                .Include(x => x.LocalArea.ServiceArea.District)
                .First(a => a.OwnerId == id);

            HetOwner targetOwner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetEquipment)
                .Include(x => x.LocalArea.ServiceArea.District)
                .First(a => a.OwnerId == targetOwnerId);

            // check they are in the same district
            if (currentOwner.LocalArea.ServiceArea.District.DistrictId !=
                targetOwner.LocalArea.ServiceArea.District.DistrictId)
            {
                return new ObjectResult(new HetsResponse("HETS-31", ErrorViewModel.GetDescription("HETS-31", _configuration)));
            }

            // check they are both active
            if (currentOwner.OwnerStatusTypeId != ownerStatusId &&
                targetOwner.OwnerStatusTypeId != ownerStatusId)
            {
                return new ObjectResult(new HetsResponse("HETS-32", ErrorViewModel.GetDescription("HETS-32", _configuration)));
            }

            // check all pieces of equipment in the provided list belong to this owner
            foreach (HetEquipment equipmentToTransfer in items)
            {
                if (equipmentToTransfer.OwnerId != currentOwner.OwnerId)
                {
                    return new ObjectResult(new HetsResponse("HETS-33", ErrorViewModel.GetDescription("HETS-33", _configuration)));
                }
            }

            //***************************************************************
            // get fiscal year
            //***************************************************************
            HetDistrictStatus district = _context.HetDistrictStatus.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId == currentOwner.LocalArea.ServiceArea.District.DistrictId);

            if (district?.CurrentFiscalYear == null) return new ObjectResult(new HetsResponse("HETS-30", ErrorViewModel.GetDescription("HETS-30", _configuration)));

            int fiscalYear = (int)district.CurrentFiscalYear; // status table uses the start of the year
            DateTime fiscalStart = new DateTime(fiscalYear, 4, 1);

            //***************************************************************
            // process each piece of equipment in the provided list
            //***************************************************************
            foreach (HetEquipment item in items)
            {
                // get full equipment record
                HetEquipment equipmentToTransfer = _context.HetEquipment
                    .Include(x => x.HetEquipmentAttachment)
                    .First(x => x.EquipmentId == item.EquipmentId);

                // get new owner code
                string newEquipmentCode = EquipmentHelper.GetEquipmentCode(targetOwner.OwnerId, _context);

                // create a "copy" of the record
                HetEquipment newEquipment = new HetEquipment
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
                    ReceivedDate = equipmentToTransfer.ReceivedDate,
                    LicencePlate = equipmentToTransfer.LicencePlate,
                    SerialNumber = equipmentToTransfer.SerialNumber,
                    Size = equipmentToTransfer.Size,
                    PayRate = equipmentToTransfer.PayRate,
                    RefuseRate = equipmentToTransfer.RefuseRate,
                    YearsOfService = 0,
                    Year = equipmentToTransfer.Year,
                    LastVerifiedDate = equipmentToTransfer.LastVerifiedDate,
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

                foreach (HetEquipmentAttachment attachment in equipmentToTransfer.HetEquipmentAttachment)
                {
                    if (newEquipment.HetEquipmentAttachment == null)
                    {
                        newEquipment.HetEquipmentAttachment = new List<HetEquipmentAttachment>();
                    }

                    HetEquipmentAttachment newAttachment = new HetEquipmentAttachment
                    {
                        Description = attachment.TypeName,
                        TypeName = attachment.TypeName
                    };

                    newEquipment.HetEquipmentAttachment.Add(newAttachment);
                }

                // seniority information:
                //   YTD / YTD1 / YTD2 / YTD3 / YTD3 /
                //   Years Registered / Seniority
                if (includeSeniority)
                {
                    newEquipment.ServiceHoursLastYear = equipmentToTransfer.ServiceHoursLastYear;
                    newEquipment.ServiceHoursTwoYearsAgo = equipmentToTransfer.ServiceHoursTwoYearsAgo;
                    newEquipment.ServiceHoursThreeYearsAgo = equipmentToTransfer.ServiceHoursThreeYearsAgo;
                    newEquipment.YearsOfService = equipmentToTransfer.YearsOfService;
                    newEquipment.Seniority = equipmentToTransfer.Seniority;
                    newEquipment.ApprovedDate = equipmentToTransfer.ApprovedDate;
                }

                using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
                {

                    // update new record
                    _context.HetEquipment.Add(newEquipment);
                    _context.SaveChanges();

                    if (includeSeniority)
                    {
                        int newEquipmentId = newEquipment.EquipmentId;

                        // we also need to update all of the associated rental agreements
                        // (for this fiscal year)
                        IQueryable<HetRentalAgreement> agreements = _context.HetRentalAgreement
                            .Where(x => x.EquipmentId == item.EquipmentId &&
                                        x.DatedOn >= fiscalStart);

                        foreach (HetRentalAgreement agreement in agreements)
                        {
                            agreement.EquipmentId = newEquipmentId;
                            _context.HetRentalAgreement.Update(agreement);
                        }
                    }

                    // update original equipment record
                    equipmentToTransfer.EquipmentStatusTypeId = (int) equipmentArchiveStatusId;
                    equipmentToTransfer.ArchiveCode = "Y";
                    equipmentToTransfer.ArchiveDate = DateTime.UtcNow;
                    equipmentToTransfer.ArchiveReason = "Bulk transfer to " + targetOwner.OwnerCode;

                    // save archived equipment record
                    _context.HetEquipment.Update(equipmentToTransfer);
                    _context.SaveChanges();

                    transaction.Commit();
                }
            }

            //***************************************************************
            // we need to update seniority for all local areas and
            // district equipment types
            //***************************************************************
            List<int> districtEquipmentTypes = new List<int>();

            foreach (HetEquipment equipmentToTransfer in items)
            {
                if (equipmentToTransfer.LocalAreaId != null &&
                    equipmentToTransfer.DistrictEquipmentTypeId != null)
                {
                    int localAreaId = (int)equipmentToTransfer.LocalAreaId;
                    int districtEquipmentTypeId = (int)equipmentToTransfer.DistrictEquipmentTypeId;

                    // check whether we've processed this district already
                    if (districtEquipmentTypes.Contains(districtEquipmentTypeId)) continue;
                    districtEquipmentTypes.Add(districtEquipmentTypeId);

                    // recalculate seniority
                    EquipmentHelper.RecalculateSeniority(localAreaId, districtEquipmentTypeId, _context, _configuration);
                }
            }

            // return original items
            return new ObjectResult(new HetsResponse(items));
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
        [SwaggerOperation("OwnersIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<HetDigitalFile>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdAttachmentsGet([FromRoute]int id)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetDigitalFile)
                .First(a => a.OwnerId == id);

            // extract the attachments and update properties for UI
            List<HetDigitalFile> attachments = new List<HetDigitalFile>();

            foreach (HetDigitalFile attachment in owner.HetDigitalFile)
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

        #region Owner Contact Records

        /// <summary>
        /// Get contacts associated with an owner
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        [HttpGet]
        [Route("{id}/contacts")]
        [SwaggerOperation("OwnersIdContactsGet")]
        [SwaggerResponse(200, type: typeof(List<HetContact>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdContactsGet([FromRoute]int id)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetContact)
                .First(a => a.OwnerId == id);

            return new ObjectResult(new HetsResponse(owner.HetContact.ToList()));
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
        [SwaggerOperation("OwnersIdContactsPost")]
        [SwaggerResponse(200, type: typeof(HetContact))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdContactsPost([FromRoute]int id, [FromRoute]bool primary, [FromBody]HetContact item)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists || item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int contactId;

            // get owner record
            HetOwner owner = _context.HetOwner
                .Include(x => x.HetContact)
                .First(a => a.OwnerId == id);

            // add or update contact
            if (item.ContactId > 0)
            {
                HetContact contact = owner.HetContact.FirstOrDefault(a => a.ContactId == item.ContactId);

                // not found
                if (contact == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

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

                _context.HetContact.Add(contact);
                _context.SaveChanges();

                contactId = contact.ContactId;

                if (primary)
                {
                    owner.PrimaryContactId = contactId;
                }
            }

            _context.SaveChanges();

            // get updated contact record
            HetOwner updatedOwner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetContact)
                .First(a => a.OwnerId == id);

            HetContact updatedContact = updatedOwner.HetContact
                .FirstOrDefault(a => a.ContactId == contactId);

            return new ObjectResult(new HetsResponse(updatedContact));
        }

        /// <summary>
        /// Update owner contacts
        /// </summary>
        /// <remarks>Replaces an Owners Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="items">Replacement Owner contacts</param>
        [HttpPut]
        [Route("{id}/contacts")]
        [SwaggerOperation("OwnersIdContactsPut")]
        [SwaggerResponse(200, type: typeof(List<HetContact>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdContactsPut([FromRoute]int id, [FromBody]HetContact[] items)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists || items == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get owner record
            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetContact)
                .First(a => a.OwnerId == id);

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

                        items[i] = temp;
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

                        owner.HetContact.Add(temp);
                        items[i] = temp;
                    }
                }
            }

            // remove contacts that are no longer attached.
            foreach (HetContact contact in owner.HetContact)
            {
                if (contact != null && items.All(x => x.ContactId != contact.ContactId))
                {
                    _context.HetContact.Remove(contact);
                }
            }

            // save changes
            _context.SaveChanges();

            // get updated contact records
            HetOwner updatedOwner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetContact)
                .First(a => a.OwnerId == id);

            return new ObjectResult(new HetsResponse(updatedOwner.HetContact.ToList()));
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
        [SwaggerOperation("OwnersIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HetHistory>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            return new ObjectResult(new HetsResponse(OwnerHelper.GetHistoryRecords(id, offset, limit, _context)));
        }

        /// <summary>
        /// Create owner history
        /// </summary>
        /// <remarks>Add a History record to the Owner</remarks>
        /// <param name="id">id of Owner to add History for</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/history")]
        [SwaggerOperation("OwnersIdHistoryPost")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdHistoryPost([FromRoute]int id, [FromBody]HetHistory item)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            if (exists)
            {
                HetHistory history = new HetHistory
                {
                    HistoryId = 0,
                    HistoryText = item.HistoryText,
                    CreatedDate = item.CreatedDate,
                    OwnerId = id
                };

                _context.HetHistory.Add(history);
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(OwnerHelper.GetHistoryRecords(id, null, null, _context)));
        }

        #endregion

        #region Owner Note Records

        /// <summary>
        /// Get note records associated with owner
        /// </summary>
        /// <param name="id">id of Owner to fetch Notes for</param>
        [HttpGet]
        [Route("{id}/notes")]
        [SwaggerOperation("OwnersIdNotesGet")]
        [SwaggerResponse(200, type: typeof(List<HetNote>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdNotesGet([FromRoute]int id)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.OwnerId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in owner.HetNote)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(notes));
        }

        /// <summary>
        /// Update or create a note associated with a owner
        /// </summary>
        /// <remarks>Update an Owners Notes</remarks>
        /// <param name="id">id of Owner to update Notes for</param>
        /// <param name="item">Owner Note</param>
        [HttpPost]
        [Route("{id}/note")]
        [SwaggerOperation("OwnersIdNotePost")]
        [SwaggerResponse(200, type: typeof(HetNote))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdNotePost([FromRoute]int id, [FromBody]HetNote item)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

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
                    OwnerId = id,
                    Text = item.Text,
                    IsNoLongerRelevant = item.IsNoLongerRelevant
                };

                _context.HetNote.Add(note);
            }

            _context.SaveChanges();

            // return updated note records
            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.OwnerId == id);

            List<HetNote> notes = new List<HetNote>();

            foreach (HetNote note in owner.HetNote)
            {
                if (note.IsNoLongerRelevant == false)
                {
                    notes.Add(note);
                }
            }

            return new ObjectResult(new HetsResponse(notes));
        }

        #endregion

        #region Generate Shared Keys

        /// <summary>
        /// Generate shared keys for all owners
        /// that don't have an associated business yet
        /// </summary>
        [HttpPost]
        [Route("GenerateKeys")]
        [SwaggerOperation("OwnersGenerateKeysPost")]
        [RequiresPermission(HetPermission.Admin)]
        public virtual IActionResult OwnersGenerateKeysPost()
        {
            // get records
            List<HetOwner> owners = _context.HetOwner.AsNoTracking()
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
                HetOwner ownerRecord = _context.HetOwner.First(x => x.OwnerId == owner.OwnerId);
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
        [SwaggerOperation("GenerateKeysApiPost")]
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
            List<HetOwner> owners = _context.HetOwner.AsNoTracking()
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
                HetOwner ownerRecord = _context.HetOwner.First(x => x.OwnerId == owner.OwnerId);
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
        [SwaggerOperation("OwnerWcbCglGet")]
        [SwaggerResponse(200, type: typeof(List<OwnerWcbCgl>))]
        public virtual IActionResult OwnerWcbCglGet([FromQuery]string localAreas, [FromQuery]string owners,
            [FromQuery]DateTime? wcbExpiry, [FromQuery]DateTime? cglExpiry)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] ownerArray = ArrayHelper.ParseIntArray(owners);

            // owner status
            int? statusId = StatusHelper.GetStatusId(HetOwner.StatusApproved, "ownerStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            IQueryable<HetOwner> data = _context.HetOwner.AsNoTracking()
                .Include(y => y.LocalArea.ServiceArea)
                .Include(x => x.PrimaryContact)
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.OwnerStatusTypeId == statusId &&
                            (x.WorkSafeBcexpiryDate == null || wcbExpiry == null || x.WorkSafeBcexpiryDate < wcbExpiry) &&
                            (x.CglendDate == null || cglExpiry == null || x.CglendDate < cglExpiry));

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.LocalAreaId));
            }

            if (ownerArray != null && ownerArray.Length > 0)
            {
                data = data.Where(x => ownerArray.Contains(x.OwnerId));
            }

            // convert Rental Request Model to the "RentalRequestHires" Model
            List<OwnerWcbCgl> result = new List<OwnerWcbCgl>();

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
