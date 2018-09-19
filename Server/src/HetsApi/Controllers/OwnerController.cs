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
    #region Parameter Models

    /// <summary>
    /// Owner Status Class - required to update the status record only
    /// </summary>
    public class OwnerStatus
    {
        public string Status { get; set; }
        public string StatusComment { get; set; }
    }

    #endregion

    /// <summary>
    /// Note Controller
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
            HetOwner owner = _context.HetOwner.First(x => x.OwnerId != item.OwnerId);

            int? oldLocalArea = owner.LocalAreaId;

            owner.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
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

            // we need to update the equipment records to match any change in local area
            if (oldLocalArea != item.LocalArea.LocalAreaId)
            {
                IQueryable<HetEquipment> equipmentList = _context.HetEquipment
                    .Include(x => x.Owner)
                    .Include(x => x.LocalArea)
                    .Where(x => x.Owner.OwnerId == id);

                foreach (HetEquipment equipment in equipmentList)
                {
                    equipment.LocalAreaId = item.LocalArea.LocalAreaId;
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
            HetOwner owner = _context.HetOwner.First(x => x.OwnerId != id);

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

                    // if the equipment is already archived - leave it archived
                    // if the equipment is already in the same state as the owner's new state - then ignore
                    if (!equipment.Status.Equals(HetEquipment.StatusArchived, StringComparison.CurrentCultureIgnoreCase) &&
                        !equipment.Status.Equals(item.Status, StringComparison.InvariantCultureIgnoreCase))
                    {
                        int? eqStatusId = StatusHelper.GetStatusId(item.Status, "equipmentStatus", _context);
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
        [Route("/api/owners")]
        [SwaggerOperation("OwnersPost")]
        [SwaggerResponse(200, type: typeof(HetOwner))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersPost([FromBody]HetOwner item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            // create record
            HetOwner owner = new HetOwner
            {
                CglendDate = item.CglendDate,
                CglPolicyNumber = item.CglPolicyNumber,
                LocalAreaId = item.LocalArea.LocalAreaId,
                WorkSafeBcexpiryDate = item.WorkSafeBcexpiryDate,
                WorkSafeBcpolicyNumber = item.WorkSafeBcpolicyNumber,
                IsMaintenanceContractor = item.IsMaintenanceContractor,
                OrganizationName = item.OrganizationName,
                OwnerCode = item.OwnerCode,
                DoingBusinessAs = item.DoingBusinessAs,
                RegisteredCompanyNumber = item.RegisteredCompanyNumber,
                Address1 = item.Address1,
                Address2 = item.Address2,
                City = item.City,
                PostalCode = item.PostalCode,
                Province = item.Province,
                GivenName = item.GivenName,
                Surname = item.Surname
            };

            _context.HetOwner.Add(owner);

            // save record
            _context.SaveChanges();

            int id = owner.OwnerId;

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
        [HttpGet]
        [Route("search")]
        [SwaggerOperation("OwnersSearchGet")]
        [SwaggerResponse(200, type: typeof(List<HetOwner>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersSearchGet([FromQuery]string localAreas, 
            [FromQuery]string equipmentTypes, [FromQuery]int? owner, [FromQuery]string status, 
            [FromQuery]bool? hired)
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
        /// <param name="items">Array of owner id numbers to generate notices for</param>
        [HttpPost]
        [Route("verificationPdf")]
        [SwaggerOperation("OwnersIdVerificationPdfPost")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdVerificationPdfPost([FromBody]List<int> items)
        {
            if (items == null || items.Count <= 0)
            {
                // verification array is empty [HETS-14]
                return new ObjectResult(new HetsResponse("HETS-14", ErrorViewModel.GetDescription("HETS-14", _configuration)));
            }

            _logger.LogInformation("Owner Verification Notices Pdf [Owner Count: {0}]", items.Count);

            // get owner records
            List<HetOwner> owners = _context.HetOwner.AsNoTracking()
                .Include(x => x.PrimaryContact)
                .Include(x => x.HetEquipment)
                    .ThenInclude(a => a.HetEquipmentAttachment)
                .Include(x => x.HetEquipment)
                    .ThenInclude(l => l.LocalArea)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                .Include(x => x.LocalArea)
                    .ThenInclude(s => s.ServiceArea)
                        .ThenInclude(d => d.District)
                .Where(x => items.Contains(x.OwnerId))
                .ToList();

            // strip out inactive and archived equipment
            foreach (HetOwner owner in owners)
            {
                owner.HetEquipment = owner.HetEquipment.Where(x => x.Status == HetEquipment.StatusApproved).ToList();
            }

            if (owners.Count > 0)
            {
                if (owners[0].LocalArea.ServiceArea.District == null)
                {
                    // missing district - data error [HETS-16]
                    return new ObjectResult(new HetsResponse("HETS-16", ErrorViewModel.GetDescription("HETS-16", _configuration)));
                }

                // generate pdf document name [unique portion only]
                string fileName = "OwnerVerification_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day;

                // setup model for submission to the Pdf service
                OwnerVerificationPdfViewModel model = new OwnerVerificationPdfViewModel
                {
                    ReportDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    Title = fileName,
                    DistrictId = owners[0].LocalArea.ServiceArea.District.DistrictId,
                    MinistryDistrictId = owners[0].LocalArea.ServiceArea.District.MinistryDistrictId,
                    DistrictName = owners[0].LocalArea.ServiceArea.District.Name,
                    DistrictAddress = "to be completed",
                    DistrictContact = "to be completed",
                    Owners = new List<HetOwner>()
                };

                // add owner records - must verify district ids too
                foreach (HetOwner owner in owners)
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
                    owner.DistrictContact = model.DistrictAddress;

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

                        fileName = fileName + ".pdf";

                        _logger.LogInformation("Owner Verification Notices Pdf - HETS Pdf Filename: {0}", fileName);
                        _logger.LogInformation("Owner Verification Notices Pdf - HETS Pdf Size: {0}", pdfResponse.Length);

                        // return content
                        FileContentResult result = new FileContentResult(pdfResponseBytes, "application/pdf")
                        {
                            FileDownloadName = fileName
                        };

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

            return new ObjectResult(new HetsResponse(owner.HetEquipment));            
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
        /// <param name="item">Adds to Owner Contact</param>
        /// <param name="primary"></param>
        [HttpPost]
        [Route("{id}/contacts/{primary}")]
        [SwaggerOperation("OwnersIdContactsPost")]
        [SwaggerResponse(200, type: typeof(HetContact))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnersIdContactsPost([FromRoute]int id, [FromBody]HetContact item, bool primary)
        {
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            // not found
            if (!exists || item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            int contactId;

            // get owner record
            HetOwner owner = _context.HetOwner.AsNoTracking()
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
                    FaxPhoneNumber = item.FaxPhoneNumber,
                    GivenName = item.GivenName,
                    MobilePhoneNumber = item.MobilePhoneNumber,
                    PostalCode = item.PostalCode,
                    Province = item.Province,
                    Surname = item.Surname,
                    Role = item.Role
                };

                owner.HetContact.Add(contact);

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
                HetOwner owner = _context.HetOwner.AsNoTracking()
                    .First(a => a.OwnerId == id);

                HetHistory history = new HetHistory
                {
                    HistoryId = 0,
                    HistoryText = item.HistoryText,
                    CreatedDate = item.CreatedDate,
                    OwnerId = owner.OwnerId
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

            HetOwner owner = _context.HetOwner.AsNoTracking()
                .Include(x => x.HetNote)
                .First(x => x.OwnerId == id);

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
                    Text = item.Text,
                    IsNoLongerRelevant = item.IsNoLongerRelevant
                };

                owner.HetNote.Add(note);
            }

            _context.SaveChanges();

            // return updated note records
            owner = _context.HetOwner.AsNoTracking()
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
    }
}
