using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using HetsApi.Helpers;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Attachment Controller
    /// </summary>
    [Route("api/attachments")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AttachmentController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        
        public AttachmentController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        /// Delete attachment
        /// </summary>
        /// <param name="id">id of Attachment to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("AttachmentsIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetDigitalFile))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult AttachmentsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetDigitalFile.Any(a => a.DigitalFileId == id);

            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            HetDigitalFile item = _context.HetDigitalFile.First(a => a.DigitalFileId == id);

            if (item != null)
            {
                _context.HetDigitalFile.Remove(item);
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(item));            
        }

        /// <summary>
        /// Return the binary file
        /// </summary>
        /// <param name="id">Attachment Id</param>
        [HttpGet]
        [Route("{id}/download")]
        [SwaggerOperation("AttachmentsIdDownloadGet")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult AttachmentsIdDownloadGet([FromRoute]int id)
        {
            bool exists = _context.HetDigitalFile.Any(a => a.DigitalFileId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            HetDigitalFile attachment = _context.HetDigitalFile.First(a => a.DigitalFileId == id);
                       
            FileContentResult result = new FileContentResult(attachment.FileContents, "application/octet-stream")
            {
                FileDownloadName = attachment.FileName
            };

            return result;            
        }
    }
}
