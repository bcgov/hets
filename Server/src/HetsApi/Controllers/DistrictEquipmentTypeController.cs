using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// District Equipment Type Controller
    /// </summary>
    [Route("api/districtEquipmentTypes")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class DistrictEquipmentTypeController : Controller
    {
        private readonly Object _thisLock = new Object();

        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public DistrictEquipmentTypeController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;

            // set context data
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }

        /// <summary>
        /// Get all district equipment types
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerOperation("DistrictEquipmentTypesGet")]
        [SwaggerResponse(200, type: typeof(List<HetDistrictEquipmentType>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult DistrictEquipmentTypesGet()
        {
            // get current users district id
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, HttpContext);

            // not found
            if (districtId == null) return new ObjectResult(new List<HetDistrictEquipmentType>());

            List<HetDistrictEquipmentType> equipmentTypes = _context.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.District)
                .Include(x => x.EquipmentType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.LocalArea)
                .Include(x => x.HetEquipment)
                    .ThenInclude(x => x.EquipmentStatusType)
                .Where(x => x.District.DistrictId == districtId &&
                            !x.Deleted)
                .OrderBy(x => x.DistrictEquipmentName)
                .ToList();

            foreach (HetDistrictEquipmentType equipmentType in equipmentTypes)
            {
                IEnumerable<HetEquipment> approvedEquipment = equipmentType.HetEquipment
                    .Where(x => x.EquipmentStatusType.EquipmentStatusTypeCode == HetEquipment.StatusApproved);

                List<HetEquipment> hetEquipments = approvedEquipment.ToList();
                equipmentType.EquipmentCount = hetEquipments.Count;

                foreach(HetEquipment equipment in hetEquipments)
                {
                    LocalAreaEquipment localAreaEquipment = equipmentType.LocalAreas
                        .FirstOrDefault(x => x.Id == equipment.LocalAreaId);

                    if (localAreaEquipment == null)
                    {
                        localAreaEquipment = new LocalAreaEquipment
                        {
                            Id = equipment.LocalArea.LocalAreaId,
                            Name = equipment.LocalArea.Name,
                            EquipmentCount = 1
                        };

                        equipmentType.LocalAreas.Add(localAreaEquipment);
                    }
                    else
                    {
                        localAreaEquipment.EquipmentCount = localAreaEquipment.EquipmentCount + 1;
                    }
                }

                // remove unnecessary data
                equipmentType.HetEquipment = null;
            }

            return new ObjectResult(new HetsResponse(equipmentTypes));
        }

        /// <summary>
        /// Get all district equipment types by district for rental agreement summary filtering
        /// </summary>
        [HttpGet]
        [Route("agreementSummary")]
        [SwaggerOperation("DistrictEquipmentTypesGetAgreementSummary")]
        [SwaggerResponse(200, type: typeof(List<DistrictEquipmentTypeAgreementSummary>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult DistrictEquipmentTypesGetAgreementSummary()
        {
            // get user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, HttpContext);

            IEnumerable<DistrictEquipmentTypeAgreementSummary> equipmentTypes = _context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Equipment.DistrictEquipmentType)
                .Where(x => x.DistrictId == districtId &&
                            !x.Number.StartsWith("BCBid"))
                .GroupBy(x => x.Equipment.DistrictEquipmentType, (t, agreements) => new DistrictEquipmentTypeAgreementSummary
                {
                    Id = t.DistrictEquipmentTypeId,
                    Name = t.DistrictEquipmentName,
                    AgreementIds = agreements.Select(y => y.RentalAgreementId).Distinct().ToList(),
                    ProjectIds = agreements.Select(y => y.ProjectId).Distinct().ToList(),
                })
                .ToList();

            return new ObjectResult(new HetsResponse(equipmentTypes));
        }

        /// <summary>
        /// Delete district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("DistrictEquipmentTypesIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetDistrictEquipmentType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement, HetPermission.WriteAccess)]
        public virtual IActionResult DistrictEquipmentTypesIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetDistrictEquipmentType.Any(a => a.DistrictEquipmentTypeId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetDistrictEquipmentType item = _context.HetDistrictEquipmentType.First(a => a.DistrictEquipmentTypeId == id);

            // HETS-978 - Give a clear error message when deleting equipment type fails
            int? archiveStatus = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _context);
            if (archiveStatus == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            int? pendingStatus = StatusHelper.GetStatusId(HetEquipment.StatusPending, "equipmentStatus", _context);
            if (pendingStatus == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            HetEquipment equipment = _context.HetEquipment.AsNoTracking()
                .FirstOrDefault(x => x.DistrictEquipmentTypeId == item.DistrictEquipmentTypeId &&
                                     x.EquipmentStatusTypeId != archiveStatus);

            if (equipment != null)
            {
                return new BadRequestObjectResult(new HetsResponse("HETS-37", ErrorViewModel.GetDescription("HETS-37", _configuration)));
            }

            bool softDelete = false;

            // check for foreign key relationships - equipment
            equipment = _context.HetEquipment.AsNoTracking()
                .FirstOrDefault(x => x.DistrictEquipmentTypeId == item.DistrictEquipmentTypeId);

            if (equipment != null) softDelete = true;

            // check for foreign key relationships - local area rotation lists
            HetLocalAreaRotationList rotationList = _context.HetLocalAreaRotationList.AsNoTracking()
                .FirstOrDefault(x => x.DistrictEquipmentTypeId == item.DistrictEquipmentTypeId);

            if (rotationList != null) softDelete = true;

            // check for foreign key relationships - rental requests
            HetRentalRequest request = _context.HetRentalRequest.AsNoTracking()
                .FirstOrDefault(x => x.DistrictEquipmentTypeId == item.DistrictEquipmentTypeId);

            if (request != null) softDelete = true;

            // delete the record
            if (!softDelete)
            {
                _context.HetDistrictEquipmentType.Remove(item);
            }
            else
            {
                // else "SOFT" delete record
                item.Deleted = true;
            }

            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }

        /// <summary>
        /// Get district equipment type by id
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("DistrictEquipmentTypesIdGet")]
        [SwaggerResponse(200, type: typeof(HetDistrictEquipmentType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement)]
        public virtual IActionResult DistrictEquipmentTypesIdGet([FromRoute]int id)
        {
            HetDistrictEquipmentType equipmentType = _context.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.District)
                    .ThenInclude(y => y.Region)
                .Include(x => x.EquipmentType)
                .FirstOrDefault(a => a.DistrictEquipmentTypeId == id);

            return new ObjectResult(new HetsResponse(equipmentType));
        }

        /// <summary>
        /// Create or update district equipment type
        /// </summary>
        /// <param name="id">id of DistrictEquipmentType to update (0 to create)</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}")]
        [SwaggerOperation("DistrictEquipmentTypesIdPost")]
        [SwaggerResponse(200, type: typeof(HetDistrictEquipmentType))]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement, HetPermission.WriteAccess)]
        public virtual IActionResult DistrictEquipmentTypesIdPost([FromRoute]int id, [FromBody]HetDistrictEquipmentType item)
        {
            if (id != item.DistrictEquipmentTypeId)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // add or update equipment type
            if (item.DistrictEquipmentTypeId > 0)
            {
                bool exists = _context.HetDistrictEquipmentType.Any(a => a.DistrictEquipmentTypeId == id);

                // not found
                if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                // get equipment status
                int? equipmentStatusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);

                if (equipmentStatusId == null) return new NotFoundObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

                // get record
                HetDistrictEquipmentType equipment = _context.HetDistrictEquipmentType
                    .Include(x => x.EquipmentType)
                    .First(x => x.DistrictEquipmentTypeId == id);

                // HETS-1163 - Recalculate seniority and Blk assignment
                // for change in Blue book section number to and from
                bool currentIsDumpTruck = equipment.EquipmentType.IsDumpTruck;

                HetEquipmentType newEquipmentType = _context.HetEquipmentType.AsNoTracking()
                    .FirstOrDefault(x => x.EquipmentTypeId == item.EquipmentType.EquipmentTypeId);

                if (newEquipmentType == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

                bool updateSeniority = currentIsDumpTruck != newEquipmentType.IsDumpTruck;

                // modify record
                equipment.DistrictEquipmentName = item.DistrictEquipmentName;
                equipment.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                equipment.DistrictId = item.District.DistrictId;
                equipment.EquipmentTypeId = item.EquipmentType.EquipmentTypeId;

                // update seniority and assignments for this District Equipment Type (HETS-1163)
                if (updateSeniority)
                {
                    IConfigurationSection scoringRules = _configuration.GetSection("SeniorityScoringRules");
                    string seniorityScoringRules = GetConfigJson(scoringRules);

                    // update the seniority and block assignments for the master record
                    List<HetLocalArea> localAreas = _context.HetEquipment.AsNoTracking()
                        .Include(x => x.LocalArea)
                        .Where(x => x.EquipmentStatusTypeId == equipmentStatusId &&
                                    x.DistrictEquipmentTypeId == equipment.DistrictEquipmentTypeId)
                        .Select(x => x.LocalArea)
                        .Distinct()
                        .ToList();

                    foreach (HetLocalArea localArea in localAreas)
                    {
                        EquipmentHelper.RecalculateSeniority(localArea.LocalAreaId,                             equipment.DistrictEquipmentTypeId, _context, seniorityScoringRules);
                    }
                }
            }
            else
            {
                HetDistrictEquipmentType equipment = new HetDistrictEquipmentType
                {
                    DistrictEquipmentName = item.DistrictEquipmentName,
                    DistrictId = item.District.DistrictId,
                    EquipmentTypeId = item.EquipmentType.EquipmentTypeId
                };

                _context.HetDistrictEquipmentType.Add(equipment);
            }

            // save the changes
            _context.SaveChanges();

            // get the id (in the case of new records)
            id = item.DistrictEquipmentTypeId;

            // return the updated equipment type record
            HetDistrictEquipmentType equipmentType = _context.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.District)
                    .ThenInclude(y => y.Region)
                .Include(x => x.EquipmentType)
                .FirstOrDefault(a => a.DistrictEquipmentTypeId == id);

            return new ObjectResult(new HetsResponse(equipmentType));
        }

        /// <summary>
        /// Merge district equipment types with the same starting acronym (e.g. "CLM - xxx")
        /// * must be in the same district
        /// * and be of the same equipment type
        /// </summary>
        [HttpPost]
        [Route("merge")]
        [SwaggerOperation("MergeDistrictEquipmentTypesPost")]
        [RequiresPermission(HetPermission.DistrictCodeTableManagement, HetPermission.WriteAccess)]
        public virtual IActionResult MergeDistrictEquipmentTypesPost()
        {
            string connectionString = GetConnectionString();

            IConfigurationSection scoringRules = _configuration.GetSection("SeniorityScoringRules");
            string seniorityScoringRules = GetConfigJson(scoringRules);

            // queue the job
            BackgroundJob.Enqueue(() => DistrictEquipmentTypeHelper.MergeDistrictEquipmentTypes(null,
                seniorityScoringRules, connectionString));

            // return ok
            return new ObjectResult(new HetsResponse("Merge job added to hangfire"));
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
    }
}
