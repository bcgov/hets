using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HETSAPI.Models;
using HETSAPI.Services.Impl;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;

namespace HETSAPI.Controllers
{
    public class AttachmentUploadController
    {
        private readonly IAttachmentUploadService _service;
        public AttachmentUploadController(IAttachmentUploadService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("/api/equipment/{id}/attachments")]
        public virtual IActionResult EquipmentIdAttachmentsPost([FromRoute] int Id, [FromForm] IList<IFormFile> files)
        {
            return _service.EquipmentIdAttachmentsPostAsync(Id, files);
        }

        [HttpGet]
        [Route("/api/equipment/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult EquipmentIdAttachmentsFormGet([FromRoute] int Id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/equipment/"+Id+"/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        [HttpPost]
        [Route("/api/projects/{id}/attachments")]
        public virtual IActionResult ProjectIdAttachmentsPost([FromRoute] int Id, [FromForm] IList<IFormFile> files)
        {
            return _service.ProjectIdAttachmentsPostAsync(Id, files);
        }

        [HttpGet]
        [Route("/api/projects/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult ProjectIdAttachmentsFormGet([FromRoute] int Id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/projects/" + Id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        [HttpPost]
        [Route("/api/owners/{id}/attachments")]
        public virtual IActionResult OwnerIdAttachmentsPost([FromRoute] int Id, [FromForm] IList<IFormFile> files)
        {
            return _service.OwnerIdAttachmentsPostAsync(Id, files);
        }

        [HttpGet]
        [Route("/api/owners/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult OwnerIdAttachmentsFormGet([FromRoute] int Id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/owners/" + Id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }

        [HttpPost]
        [Route("/api/rentalrequests/{id}/attachments")]
        public virtual IActionResult RentalRequestIdAttachmentsPost([FromRoute] int Id, [FromForm] IList<IFormFile> files)
        {
            return _service.RentalRequestIdAttachmentsPostAsync(Id, files);
        }

        [HttpGet]
        [Route("/api/rentalrequests/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult RentalRequestIdAttachmentsFormGet([FromRoute] int Id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/rentalrequests/" + Id + "/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }
    }

    public interface IAttachmentUploadService
    {
        IActionResult EquipmentIdAttachmentsPostAsync(int Id, IList<IFormFile> files);
        IActionResult ProjectIdAttachmentsPostAsync(int Id, IList<IFormFile> files);
        IActionResult OwnerIdAttachmentsPostAsync(int Id, IList<IFormFile> files);
        IActionResult RentalRequestIdAttachmentsPostAsync(int Id, IList<IFormFile> files);
    }

    public class AttachmentUploadService : ServiceBase, IAttachmentUploadService
    {
        private readonly IConfiguration Configuration;
        private readonly DbAppContext _context;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="configuration"></param>
        /// <param name="context"></param>
        public AttachmentUploadService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, DbAppContext context) : base(httpContextAccessor, context)
        {
            Configuration = configuration;
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
                    if (file.FileName != null && file.FileName.Length > 0)
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
        ///  Basic file receiver for .NET Core
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
        ///  Basic file receiver for .NET Core
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
        ///  Basic file receiver for .NET Core
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
        ///  Basic file receiver for .NET Core
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
