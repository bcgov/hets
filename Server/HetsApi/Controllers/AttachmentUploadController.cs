using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HetsApi.Authorization;
using HetsData.Helpers;
using HetsData.Entities;
using HetsData.Dtos;
using AutoMapper;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Attachment Upload Controller
    /// </summary>
    [Route("api")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AttachmentUploadController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IMapper _mapper;

        public AttachmentUploadController(DbAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Associate an attachment with a piece of equipment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        [HttpPost]
        [Route("equipment/{id}/attachments")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<DigitalFileDto>> EquipmentIdAttachmentsPost([FromRoute] int id, [FromForm]IList<IFormFile> files)
        {
            // validate the id
            bool exists = _context.HetEquipments.Any(a => a.EquipmentId == id);

            if (!exists) return new StatusCodeResult(404);

            HetEquipment equipment = _context.HetEquipments
                .Include(x => x.HetDigitalFiles)
                .First(a => a.EquipmentId == id);

            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    HetDigitalFile attachment = new HetDigitalFile();

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

                    // set the mime type id
                    int? mimeTypeId = StatusHelper.GetMimeTypeId(attachment.Type, _context);
                    if (mimeTypeId == null) throw new DataException("Mime Type Id cannot be null");

                    attachment.MimeTypeId = (int)mimeTypeId;

                    equipment.HetDigitalFiles.Add(attachment);
                }
            }

            _context.SaveChanges();

            return new ObjectResult(_mapper.Map<List<DigitalFileDto>>(equipment.HetDigitalFiles));
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
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<DigitalFileDto>> ProjectIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            // validate the id
            bool exists = _context.HetProjects.Any(a => a.ProjectId == id);

            if (!exists) return new StatusCodeResult(404);

            HetProject project = _context.HetProjects
                .Include(x => x.HetDigitalFiles)
                .First(a => a.ProjectId == id);

            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    HetDigitalFile attachment = new HetDigitalFile();

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

                    // set the mime type id
                    int? mimeTypeId = StatusHelper.GetMimeTypeId(attachment.Type, _context);
                    if (mimeTypeId == null) throw new DataException("Mime Type Id cannot be null");

                    attachment.MimeTypeId = (int)mimeTypeId;

                    project.HetDigitalFiles.Add(attachment);
                }
            }

            _context.SaveChanges();

            return new ObjectResult(_mapper.Map<List<DigitalFileDto>>(project.HetDigitalFiles));
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
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<DigitalFileDto>> OwnerIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            // validate the id
            bool exists = _context.HetOwners.Any(a => a.OwnerId == id);

            if (!exists) return new StatusCodeResult(404);

            HetOwner owner = _context.HetOwners
                .Include(x => x.HetDigitalFiles)
                .First(a => a.OwnerId == id);

            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    HetDigitalFile attachment = new HetDigitalFile();

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

                    // set the mime type id
                    int? mimeTypeId = StatusHelper.GetMimeTypeId(attachment.Type, _context);
                    if (mimeTypeId == null) throw new DataException("Mime Type Id cannot be null");

                    attachment.MimeTypeId = (int)mimeTypeId;

                    owner.HetDigitalFiles.Add(attachment);
                }
            }

            _context.SaveChanges();

            return new ObjectResult(_mapper.Map<List<DigitalFileDto>>(owner.HetDigitalFiles));
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
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<DigitalFileDto>> RentalRequestIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            // validate the id
            bool exists = _context.HetRentalRequests.Any(a => a.RentalRequestId == id);

            if (!exists) return new StatusCodeResult(404);

            HetRentalRequest rentalRequest = _context.HetRentalRequests
                .Include(x => x.HetDigitalFiles)
                .First(a => a.RentalRequestId == id);

            foreach (IFormFile file in files)
            {
                if (file.Length > 0)
                {
                    HetDigitalFile attachment = new HetDigitalFile();

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

                    // set the mime type id
                    int? mimeTypeId = StatusHelper.GetMimeTypeId(attachment.Type, _context);
                    if (mimeTypeId == null) throw new DataException("Mime Type Id cannot be null");

                    attachment.MimeTypeId = (int)mimeTypeId;

                    rentalRequest.HetDigitalFiles.Add(attachment);
                }
            }

            _context.SaveChanges();

            return new ObjectResult(_mapper.Map<List<DigitalFileDto>>(rentalRequest.HetDigitalFiles));
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

        #region Get File Extension

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

        #endregion
    }
}
