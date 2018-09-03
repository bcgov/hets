using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

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

        public TimeRecordController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;    
            
            // set context data
            HetUser user = UserHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
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
    }
}
