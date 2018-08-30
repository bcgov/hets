using System.Collections.Generic;
using System.IO;
using System.Linq;
using HetsApi.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Helpers;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Attachment Upload Controller
    /// </summary>
    [Route("api")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AttachmentUploadController : Controller
    {
        private readonly DbAppContext _context;
        
        public AttachmentUploadController(DbAppContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            // set context data
            HetUser user = UserHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>
        /// Associate an attachment with a piece of equipment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        [HttpPost]
        [Route("equipment/{id}/attachments")]
        [SwaggerOperation("EquipmentIdAttachmentsPost")]
        [SwaggerResponse(200, type: typeof(List<HetAttachment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdAttachmentsPost([FromRoute] int id, [FromForm]IList<IFormFile> files)
        {
            // validate the id            
            bool exists = _context.HetEquipment.Any(a => a.EquipmentId == id);

            if (!exists) return new StatusCodeResult(404);
            
            HetEquipment equipment = _context.HetEquipment
                .Include(x => x.HetAttachment)
                .First(a => a.EquipmentId == id);

            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    HetAttachment attachment = new HetAttachment();

                    // strip out extra info in the file name                   
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        attachment.FileName = Path.GetFileName(file.FileName);
                    }

                    // allocate storage for the file and create a memory stream
                    attachment.FileContents = new byte[file.Length];

                    using (MemoryStream fileStream = new MemoryStream(attachment.FileContents))
                    {
                        file.CopyTo(fileStream);
                    }

                    attachment.Type = GetType(attachment.FileName);
                    attachment.MimeType = GetMimeType(attachment.Type);

                    equipment.HetAttachment.Add(attachment);
                }
            }

            _context.SaveChanges();
            
            return new ObjectResult(equipment.HetAttachment);            
        }

        /// <summary>
        /// Get attachments associated with a piece of equipment
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("equipment/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult EquipmentIdAttachmentsFormGet([FromRoute] int id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/equipment/" + id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        /// <summary>
        /// Associate an attachment with a project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        [HttpPost]
        [Route("projects/{id}/attachments")]
        [SwaggerOperation("ProjectIdAttachmentsPost")]
        [SwaggerResponse(200, type: typeof(List<HetAttachment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            // validate the id            
            bool exists = _context.HetProject.Any(a => a.ProjectId == id);

            if (!exists) return new StatusCodeResult(404);
            
            HetProject project = _context.HetProject
                .Include(x => x.HetAttachment)
                .First(a => a.ProjectId == id);

            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    HetAttachment attachment = new HetAttachment();

                    // strip out extra info in the file name                   
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        attachment.FileName = Path.GetFileName(file.FileName);
                    }

                    // allocate storage for the file and create a memory stream
                    attachment.FileContents = new byte[file.Length];

                    using (MemoryStream fileStream = new MemoryStream(attachment.FileContents))
                    {
                        file.CopyTo(fileStream);
                    }

                    attachment.Type = GetType(attachment.FileName);
                    attachment.MimeType = GetMimeType(attachment.Type);

                    project.HetAttachment.Add(attachment);
                }
            }

            _context.SaveChanges();            

            return new ObjectResult(project.HetAttachment);            
        }

        /// <summary>
        /// Get attachments associated with a project
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("projects/{id}/attachmentsForm")]
        [Produces("text/html")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult ProjectIdAttachmentsFormGet([FromRoute] int id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/projects/" + id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        /// <summary>
        /// Associate an owner with an attachment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        [HttpPost]
        [Route("owners/{id}/attachments")]
        [SwaggerOperation("OwnerIdAttachmentsPost")]
        [SwaggerResponse(200, type: typeof(List<HetAttachment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnerIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            // validate the id            
            bool exists = _context.HetOwner.Any(a => a.OwnerId == id);

            if (!exists) return new StatusCodeResult(404);
            
            HetOwner owner = _context.HetOwner
                .Include(x => x.HetAttachment)
                .First(a => a.OwnerId == id);

            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    HetAttachment attachment = new HetAttachment();

                    // strip out extra info in the file name                   
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        attachment.FileName = Path.GetFileName(file.FileName);
                    }

                    // allocate storage for the file and create a memory stream
                    attachment.FileContents = new byte[file.Length];

                    using (MemoryStream fileStream = new MemoryStream(attachment.FileContents))
                    {
                        file.CopyTo(fileStream);
                    }

                    attachment.Type = GetType(attachment.FileName);
                    attachment.MimeType = GetMimeType(attachment.Type);

                    owner.HetAttachment.Add(attachment);
                }
            }

            _context.SaveChanges();

            return new ObjectResult(owner.HetAttachment);            
        }

        /// <summary>
        /// Get attachments associated with an owner
        /// </summary>
        /// <param name="id"></param>
        [HttpGet]
        [Route("owners/{id}/attachmentsForm")]
        [Produces("text/html")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult OwnerIdAttachmentsFormGet([FromRoute] int id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/owners/" + id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        /// <summary>
        /// Associate an attachment with a rental request
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        [HttpPost]
        [Route("rentalRequests/{id}/attachments")]
        [SwaggerOperation("RentalRequestIdAttachmentsPost")]
        [SwaggerResponse(200, type: typeof(List<HetAttachment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            // validate the id            
            bool exists = _context.HetRentalRequest.Any(a => a.RentalRequestId == id);

            if (!exists) return new StatusCodeResult(404);
            
            HetRentalRequest rentalRequest = _context.HetRentalRequest
                .Include(x => x.HetAttachment)
                .First(a => a.RentalRequestId == id);

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    HetAttachment attachment = new HetAttachment();

                    // strip out extra info in the file name                   
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        attachment.FileName = Path.GetFileName(file.FileName);
                    }

                    // allocate storage for the file and create a memory stream
                    attachment.FileContents = new byte[file.Length];

                    using (MemoryStream fileStream = new MemoryStream(attachment.FileContents))
                    {
                        file.CopyTo(fileStream);
                    }

                    attachment.Type = GetType(attachment.FileName);
                    attachment.MimeType = GetMimeType(attachment.Type);

                    rentalRequest.HetAttachment.Add(attachment);
                }
            }

            _context.SaveChanges();

            return new ObjectResult(rentalRequest.HetAttachment);           
        }

        /// <summary>
        /// Get attachments associated with  rental request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("rentalRequests/{id}/attachmentsForm")]
        [Produces("text/html")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult RentalRequestIdAttachmentsFormGet([FromRoute] int id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/rentalRequests/" + id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        #region Get File Extension / Mime Type

        private string GetType(string fileName)
        {
            // get extension
            int extStart = fileName.LastIndexOf('.');
            if (extStart > 0)
            {
                return fileName.Substring(extStart + 1).ToLower();
            }

            return "";
        }

        private string GetMimeType(string extension)
        {
            switch (extension.ToLower())
            {
                case "txt":
                    return "text/plain";
                case "png":
                    return "image/png";
                case "jpeg":
                    return "image/jpeg";
                case "jpg":
                    return "image/jpeg";
                case "gif":
                    return "image/gif";
                case "tif":
                    return "image/tiff";
                case "tiff":
                    return "image/tiff";
                case "pdf":
                    return "application/pdf";
                case "doc":
                    return "application/msword";
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case "xls":
                    return "application/vnd.ms-excel";
                case "xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }

            return "";
        }

        #endregion
    }
}
