
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HETSAPI.Authorization;
using HETSAPI.Models;
using HETSAPI.Services.Impl;
using HETSCommon;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
        [Route("/api/equipments/{id}/attachments")]
        public virtual IActionResult SchoolbusesIdAttachmentsPost([FromRoute] int Id, [FromForm] IList<IFormFile> files)
        {
            return _service.EquipmentIdAttachmentsPostAsync(Id, files);
        }

        [HttpGet]
        [Route("/api/equipments/{id}/attachmentsForm")]
        [Produces("text/html")]
        public virtual IActionResult SchoolbusesIdAttachmentsFormGet([FromRoute] int Id)
        {
            return new ObjectResult("<html><body><form method=\"post\" action=\"/api/schoolbuses/"+Id+"/attachments\" enctype=\"multipart/form-data\"><input type=\"file\" name = \"files\" multiple /><input type = \"submit\" value = \"Upload\" /></body></html>");
        }        
    }

    public interface IAttachmentUploadService
    {
        IActionResult EquipmentIdAttachmentsPostAsync(int Id, IList<IFormFile> files);
      
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
    }
}
