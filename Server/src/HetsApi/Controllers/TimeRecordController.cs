using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Time Record Controller
    /// </summary>
    [Route("api/timeRecords")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class TimeRecordController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;

        public TimeRecordController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        /// Delete a time record	
        /// </summary>	
        /// <param name="id">id of TimeRecord to delete</param>	
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("TimeRecordsIdDeletePost")]
        [SwaggerResponse(200, type: typeof(List<HetTimeRecord>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult TimeRecordsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetTimeRecord.Any(a => a.TimeRecordId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetTimeRecord item = _context.HetTimeRecord.First(a => a.TimeRecordId == id);

            if (item != null)
            {
                _context.HetTimeRecord.Remove(item);

                // save the changes
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(item));            
        }

        /// <summary>
        /// Time Record Owners
        /// </summary>
        /// <remarks>Used for the time entry search page</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="projects">Projects (comma separated list of id numbers)</param>
        /// <param name="owners">Owners (comma separated list of id numbers)</param>
        /// <param name="equipment">Equipment (comma separated list of id numbers)</param>
        [HttpGet]
        [Route("search")]
        [SwaggerOperation("TimeRecordSearchGet")]
        [SwaggerResponse(200, type: typeof(List<HetTimeRecord>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult TimeRecordSearchGet([FromQuery]string localAreas,
            [FromQuery]string projects, [FromQuery]string owners, [FromQuery]string equipment)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] projectArray = ArrayHelper.ParseIntArray(projects);
            int?[] ownerArray = ArrayHelper.ParseIntArray(owners);
            int?[] equipmentArray = ArrayHelper.ParseIntArray(equipment);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (statusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            int? projectStatusId = StatusHelper.GetStatusId(HetProject.StatusActive, "projectStatus", _context);
            if (projectStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            int? agreementStatusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (agreementStatusId == null) return new ObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            // get fiscal year
            HetDistrictStatus district = _context.HetDistrictStatus.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId == districtId);

            if (district?.CurrentFiscalYear == null) return new ObjectResult(new HetsResponse("HETS-30", ErrorViewModel.GetDescription("HETS-30", _configuration)));

            int fiscalYear = (int)district.CurrentFiscalYear; // status table uses the start of the year
            DateTime fiscalStart = new DateTime(fiscalYear, 3, 31); // look for all records AFTER the 31st

            // only return active equipment / projects and agreements
            IQueryable<HetTimeRecord> data = _context.HetTimeRecord.AsNoTracking()
                .Include(x => x.RentalAgreement)
                    .ThenInclude(x => x.Project)
                .Include(x => x.RentalAgreement)
                    .ThenInclude(x => x.Equipment)
                        .ThenInclude(y => y.EquipmentStatusType)
                .Include(x => x.RentalAgreement)
                    .ThenInclude(x => x.Equipment)
                        .ThenInclude(z => z.LocalArea)
                            .ThenInclude(a => a.ServiceArea)
                .Include(x => x.RentalAgreement)
                    .ThenInclude(x => x.Equipment)
                        .ThenInclude(z => z.Owner)
                .Where(x => x.RentalAgreement.Equipment.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.RentalAgreement.Equipment.EquipmentStatusTypeId == statusId &&
                            x.RentalAgreement.Project.ProjectStatusTypeId == projectStatusId &&
                            x.RentalAgreement.RentalAgreementStatusTypeId == agreementStatusId &&
                            x.WorkedDate > fiscalStart);

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.RentalAgreement.Equipment.LocalArea.LocalAreaId));
            }

            if (projectArray != null && projectArray.Length > 0)
            {
                data = data.Where(x => projectArray.Contains(x.RentalAgreement.ProjectId));
            }

            if (ownerArray != null && ownerArray.Length > 0)
            {
                data = data.Where(x => ownerArray.Contains(x.RentalAgreement.Equipment.OwnerId));
            }

            if (equipmentArray != null && equipmentArray.Length > 0)
            {
                data = data.Where(x => equipmentArray.Contains(x.RentalAgreement.EquipmentId));
            }

            // convert Time Model to the "TimeLite" Model
            List<TimeRecordSearchLite> result = new List<TimeRecordSearchLite>();

            foreach (HetTimeRecord item in data)
            {
                result.Add(TimeRecordHelper.ToLiteModel(item));
            }

            // return to the client            
            return new ObjectResult(new HetsResponse(result));
        }
    }
}
