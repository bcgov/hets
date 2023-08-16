using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HetsData.Dtos;
using HetsCommon;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Time Record Controller
    /// </summary>
    [Route("api/timeRecords")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class TimeRecordController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TimeRecordController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete a time record
        /// </summary>
        /// <param name="id">id of TimeRecord to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<TimeRecordDto> TimeRecordsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetTimeRecords.Any(a => a.TimeRecordId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // get record
            HetTimeRecord item = _context.HetTimeRecords.First(a => a.TimeRecordId == id);

            if (item != null)
            {
                _context.HetTimeRecords.Remove(item);

                // save the changes
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<TimeRecordDto>(item)));
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
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<TimeRecordSearchLite>> TimeRecordSearchGet([FromQuery]string localAreas,
            [FromQuery]string projects, [FromQuery]string owners, [FromQuery]string equipment)
        {
            int?[] localAreasArray = ArrayHelper.ParseIntArray(localAreas);
            int?[] projectArray = ArrayHelper.ParseIntArray(projects);
            int?[] ownerArray = ArrayHelper.ParseIntArray(owners);
            int?[] equipmentArray = ArrayHelper.ParseIntArray(equipment);

            // get initial results - must be limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            // get fiscal year
            HetDistrictStatus district = _context.HetDistrictStatuses.AsNoTracking()
                .FirstOrDefault(x => x.DistrictId == districtId);

            if (district?.CurrentFiscalYear == null) return new BadRequestObjectResult(new HetsResponse("HETS-30", ErrorViewModel.GetDescription("HETS-30", _configuration)));

            int fiscalYear = (int)district.CurrentFiscalYear; // status table uses the start of the year
            DateTime fiscalStart = DateUtils.ConvertPacificToUtcTime(
                new DateTime(fiscalYear, 3, 31, 0, 0, 0)); // look for all records AFTER the 31st

            // only return active equipment / projects and agreements
            IQueryable<HetTimeRecord> data = _context.HetTimeRecords.AsNoTracking()
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
                .Where(x => 
                    x.RentalAgreement.Equipment.LocalArea.ServiceArea.DistrictId.Equals(districtId) 
                    && x.WorkedDate > fiscalStart);

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
