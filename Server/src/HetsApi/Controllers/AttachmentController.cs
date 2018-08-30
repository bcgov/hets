using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using HetsApi.Helpers;
using Microsoft.AspNetCore.Http;

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
            HetUser user = UserHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }        
       
        /// <summary>
        /// Delete attachment
        /// </summary>
        /// <param name="id">id of Attachment to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("AttachmentsIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetAttachment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult AttachmentsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetAttachment.Any(a => a.AttachmentId == id);

            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            HetAttachment item = _context.HetAttachment.First(a => a.AttachmentId == id);

            if (item != null)
            {
                _context.HetAttachment.Remove(item);
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
            bool exists = _context.HetAttachment.Any(a => a.AttachmentId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            HetAttachment attachment = _context.HetAttachment.First(a => a.AttachmentId == id);
                       
            FileContentResult result = new FileContentResult(attachment.FileContents, "application/octet-stream")
            {
                FileDownloadName = attachment.FileName
            };

            return result;            
        }
    }
}
