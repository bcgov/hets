using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Attachment Controller
    /// </summary>
    [Route("api/attachments")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AttachmentController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AttachmentController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete attachment
        /// </summary>
        /// <param name="id">id of Attachment to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<DigitalFileDto> AttachmentsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetDigitalFiles.Any(a => a.DigitalFileId == id);

            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetDigitalFile item = _context.HetDigitalFiles.First(a => a.DigitalFileId == id);

            _context.HetDigitalFiles.Remove(item);
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<DigitalFileDto>(item)));
        }

        /// <summary>
        /// Return the binary file
        /// </summary>
        /// <param name="id">Attachment Id</param>
        [HttpGet]
        [Route("{id}/download")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult AttachmentsIdDownloadGet([FromRoute]int id)
        {
            bool exists = _context.HetDigitalFiles.Any(a => a.DigitalFileId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetDigitalFile attachment = _context.HetDigitalFiles.First(a => a.DigitalFileId == id);

            FileContentResult result = new FileContentResult(attachment.FileContents, "application/octet-stream")
            {
                FileDownloadName = attachment.FileName
            };

            return result;
        }
    }
}
