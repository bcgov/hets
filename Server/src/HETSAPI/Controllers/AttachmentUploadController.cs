using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HETSAPI.Models;
using HETSAPI.Services.Impl;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;

namespace HETSAPI.Controllers
{
    /// <summary>
    /// Attachment Upload Controller
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AttachmentUploadController
    {
        private readonly IAttachmentUploadService _service;

        /// <summary>
        /// Attachment Upload Constructor
        /// </summary>
        /// <param name="service"></param>
        public AttachmentUploadController(IAttachmentUploadService service)
        {
            _service = service;
        }

        /// <summary>
        /// Associate an attachment with a piece of equipment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/equipment/{id}/attachmentsUpload")]
        public virtual IActionResult EquipmentIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            return _service.EquipmentIdAttachmentsPostAsync(id, files);
        }

        /// <summary>
        /// Get attachments associated with a piece of equipment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/equipment/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult EquipmentIdAttachmentsFormGet([FromRoute] int id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/equipment/"+id+"/attachments/Upload\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        /// <summary>
        /// Associate an attachment with a project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/projects/{id}/attachments")]
        public virtual IActionResult ProjectIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            return _service.ProjectIdAttachmentsPostAsync(id, files);
        }

        /// <summary>
        /// Get attachments associated with a project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/projects/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult ProjectIdAttachmentsFormGet([FromRoute] int id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/projects/" + id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        /// <summary>
        /// Associate an owner with an attachment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/owners/{id}/attachments")]
        public virtual IActionResult OwnerIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            return _service.OwnerIdAttachmentsPostAsync(id, files);
        }

        /// <summary>
        /// Get attchments associated with an owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/owners/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult OwnerIdAttachmentsFormGet([FromRoute] int id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/owners/" + id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        /// <summary>
        /// Associate an attchment with a rental request
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/rentalrequests/{id}/attachments")]
        public virtual IActionResult RentalRequestIdAttachmentsPost([FromRoute] int id, [FromForm] IList<IFormFile> files)
        {
            return _service.RentalRequestIdAttachmentsPostAsync(id, files);
        }

        /// <summary>
        /// Get attachments associated with  rental request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/rentalrequests/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult RentalRequestIdAttachmentsFormGet([FromRoute] int id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/rentalrequests/" + id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }
    }

    /// <summary>
    /// Attachment Upload Service Interface
    /// </summary>
    public interface IAttachmentUploadService
    {
        /// <summary>
        /// Basic file receiver for .NET Core
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        IActionResult EquipmentIdAttachmentsPostAsync(int id, IList<IFormFile> files);

        /// <summary>
        /// Basic file receiver for .NET Core
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        IActionResult ProjectIdAttachmentsPostAsync(int id, IList<IFormFile> files);

        /// <summary>
        /// Basic file receiver for .NET Core
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        IActionResult OwnerIdAttachmentsPostAsync(int id, IList<IFormFile> files);

        /// <summary>
        /// Basic file receiver for .NET Core
        /// </summary>
        /// <param name="id"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        IActionResult RentalRequestIdAttachmentsPostAsync(int id, IList<IFormFile> files);
    }

    /// <summary>
    /// Attachment Upload Service
    /// </summary>
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AttachmentUploadService : ServiceBase, IAttachmentUploadService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Attachment Upload Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="context"></param>
        public AttachmentUploadService(IHttpContextAccessor httpContextAccessor, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
        }

        /// <summary>
        /// Utility function used by the various attachment upload functions
        /// </summary>
        /// <param name="attachments">A list of attachments to add to</param>
        /// <param name="files">The files to add to the attachments</param>
        private void AddFilesToAttachments( List<Attachment> attachments, IList<IFormFile> files)
        {
            if (attachments == null)
            {
                attachments = new List<Attachment>();
            }

            //
            // MOTI has requested that files be stored in the database.
            //                                         
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    Attachment attachment = new Attachment();
                    // strip out extra info in the file name.                        
                    if (!string.IsNullOrEmpty(file.FileName))
                    {
                        attachment.FileName = Path.GetFileName(file.FileName);
                    }

                    // allocate storage for the file and create a memory stream
                    attachment.FileContents = new byte[file.Length];                   
                    MemoryStream fileStream = new MemoryStream(attachment.FileContents);
                    file.CopyTo(fileStream);
                    _context.Attachments.Add(attachment);
                    attachments.Add(attachment);
                }
            }
        }
        
        /// <summary>
        /// Basic file receiver for .NET Core
        /// </summary>
        /// <param name="id">Schoolbus Id</param>
        /// <param name="files">Files to add to attachments</param>
        /// <returns></returns>
        public IActionResult EquipmentIdAttachmentsPostAsync(int id, IList<IFormFile> files)
        {
            // validate the bus id            
            bool exists = _context.Equipments.Any(a => a.Id == id);
            if (exists)
            {
                Equipment equipment = _context.Equipments
                    .Include(x => x.Attachments)     
                    .First(a => a.Id == id);

                AddFilesToAttachments(equipment.Attachments, files);

                _context.Equipments.Update(equipment);
                _context.SaveChanges();

                List<AttachmentViewModel> result = MappingExtensions.GetAttachmentListAsViewModel(equipment.Attachments);

                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// Basic file receiver for .NET Core
        /// </summary>
        /// <param name="id">Schoolbus Id</param>
        /// <param name="files">Files to add to attachments</param>
        /// <returns></returns>
        public IActionResult ProjectIdAttachmentsPostAsync(int id, IList<IFormFile> files)
        {
            // validate the bus id            
            bool exists = _context.Projects.Any(a => a.Id == id);
            if (exists)
            {
                Project project = _context.Projects
                    .Include(x => x.Attachments)
                    .First(a => a.Id == id);

                AddFilesToAttachments(project.Attachments, files);

                _context.Projects.Update(project);
                _context.SaveChanges();

                List<AttachmentViewModel> result = MappingExtensions.GetAttachmentListAsViewModel(project.Attachments);

                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// Basic file receiver for .NET Core
        /// </summary>
        /// <param name="id">Schoolbus Id</param>
        /// <param name="files">Files to add to attachments</param>
        /// <returns></returns>
        public IActionResult RentalRequestIdAttachmentsPostAsync(int id, IList<IFormFile> files)
        {
            // validate the bus id            
            bool exists = _context.Equipments.Any(a => a.Id == id);
            if (exists)
            {
                RentalRequest rentalRequest = _context.RentalRequests
                    .Include(x => x.Attachments)
                    .First(a => a.Id == id);

                AddFilesToAttachments(rentalRequest.Attachments, files);

                _context.RentalRequests.Update(rentalRequest);
                _context.SaveChanges();

                List<AttachmentViewModel> result = MappingExtensions.GetAttachmentListAsViewModel(rentalRequest.Attachments);

                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// Basic file receiver for .NET Core
        /// </summary>
        /// <param name="id">Owner Id</param>
        /// <param name="files">Files to add to attachments</param>
        /// <returns></returns>
        public IActionResult OwnerIdAttachmentsPostAsync(int id, IList<IFormFile> files)
        {
            // validate the bus id            
            bool exists = _context.Owners.Any(a => a.Id == id);
            if (exists)
            {
                Owner owner = _context.Owners
                    .Include(x => x.Attachments)
                    .First(a => a.Id == id);

                AddFilesToAttachments(owner.Attachments, files);

                _context.Owners.Update(owner);
                _context.SaveChanges();

                List<AttachmentViewModel> result = MappingExtensions.GetAttachmentListAsViewModel(owner.Attachments);

                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }
    }
}
