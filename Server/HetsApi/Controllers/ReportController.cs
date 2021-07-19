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
using HetsData.Dtos;
using AutoMapper;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Contact Controller
    /// </summary>
    [Route("api/reports")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class ReportController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ReportController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all batch reports for the current district
        /// </summary>
        [HttpGet]
        [Route("")]
        [RequiresPermission(HetPermission.Login)]
        public virtual ActionResult<List<BatchReportDto>> BatchReportGet()
        {
            // get users district
            int? districtId = UserAccountHelper.GetUsersDistrictId(_context);

            var reports = _context.HetBatchReports.AsNoTracking()
                .Where(x => x.DistrictId == districtId)
                .OrderByDescending(x => x.StartDate)
                .ToList()
                .Select(x => new BatchReportDto
                {
                    ReportId = x.ReportId,
                    DistrictId = x.DistrictId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Complete = x.Complete ?? false
                }).ToList();

            return new ObjectResult(new HetsResponse(reports));
        }

        /// <summary>
        /// Delete report
        /// </summary>
        /// <param name="id">id of Report to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<BatchReportDto> BatchReportIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetBatchReports.Any(a => a.ReportId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetBatchReport report = _context.HetBatchReports.First(a => a.ReportId == id);

            if (report != null)
            {
                _context.HetBatchReports.Remove(report);

                // delete file
                FileUtility.DeleteFile(report.ReportLink);

                // save the changes
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(_mapper.Map<BatchReportDto>(report)));
        }

        /// <summary>
        /// Return the binary file
        /// </summary>
        /// <param name="id">Report Id</param>
        [HttpGet]
        [Route("{id}/download")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult BatchReportIdDownloadGet([FromRoute]int id)
        {
            bool exists = _context.HetBatchReports.Any(a => a.ReportId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetBatchReport report = _context.HetBatchReports.AsNoTracking().First(a => a.ReportId == id);

            // report name
            string reportName = report.ReportName + ".pdf";

            // get binary
            byte[] reportBinary = FileUtility.FileToByteArray(report.ReportLink);
           
            // return content
            FileContentResult result = new FileContentResult(reportBinary, "application/pdf")
            {
                FileDownloadName = reportName
            };

            Response.Headers.Add("Content-Disposition", "inline; filename=" + reportName);

            return result;
        }
    }
}
