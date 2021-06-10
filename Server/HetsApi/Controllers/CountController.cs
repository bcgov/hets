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
    #region Count Model

    public class CountModel
    {
        public int UnapprovedOwners { get; set; }
        public int UnapprovedEquipment { get; set; }
        public int HiredEquipment { get; set; }
        public int InProgressRentalRequests { get; set; }
    }

    #endregion

    /// <summary>
    /// Count Controller
    /// </summary>
    [Route("api/counts")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CountController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;

        public CountController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation("CountsGet")]
        [SwaggerResponse(200, type: typeof(CountModel))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult CountsGet()
        {
            CountModel result = new CountModel();

            // limited to user's district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context, _httpContext);

            // Count 1: Unapproved Owners
            int? pendingStatusId = StatusHelper.GetStatusId(HetOwner.StatusPending, "ownerStatus", _context);
            if (pendingStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            result.UnapprovedOwners = _context.HetOwner
                .AsNoTracking()
                .Count(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.OwnerStatusTypeId.Equals(pendingStatusId));

            // Count 2: Unapproved Equipment
            int? unapprovedStatusId = StatusHelper.GetStatusId(HetEquipment.StatusUnapproved, "equipmentStatus", _context);
            if (unapprovedStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            result.UnapprovedEquipment = _context.HetEquipment
                .AsNoTracking()
                .Count(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.EquipmentStatusTypeId.Equals(unapprovedStatusId));

            // Count 3: Hired Equipment
            int? activeEquipmentStatusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _context);
            if (activeEquipmentStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            var approvedEquipmentQuery = _context.HetEquipment
                .AsNoTracking()
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.EquipmentStatusTypeId.Equals(activeEquipmentStatusId));

            int? agreementStatusId = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", _context);
            if (agreementStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            var hiredEquipmentQuery = _context.HetRentalAgreement.AsNoTracking()
                .Where(x => x.Equipment.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.RentalAgreementStatusTypeId == agreementStatusId)
                .Select(x => x.EquipmentId)
                .Distinct();

            result.HiredEquipment = approvedEquipmentQuery.Count(e => hiredEquipmentQuery.Contains(e.EquipmentId));

            // Count 4: In Progress Rental Requests
            int? requestStatusId = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", _context);
            if (requestStatusId == null) return new BadRequestObjectResult(new HetsResponse("HETS-23", ErrorViewModel.GetDescription("HETS-23", _configuration)));

            result.InProgressRentalRequests = _context.HetRentalRequest
                .AsNoTracking()
                .Count(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.RentalRequestStatusTypeId.Equals(requestStatusId));

            // return to the client
            return new ObjectResult(new HetsResponse(result));
        }

    }
}
