using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using HetsApi.Authorization;
using HetsApi.Helpers;
using HetsApi.Model;
using HetsData.Model;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Equipment Attachment Controller
    /// </summary>
    [Route("/api/equipmentAttachments")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentAttachmentController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public EquipmentAttachmentController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;    
            
            // set context data
            HetUser user = ModelHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.Guid;
        }

        /// <summary>	
        /// Delete equipment attachment	
        /// </summary>	
        /// <param name="id">id of EquipmentAttachment to delete</param>		
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("EquipmentAttachmentsIdDeletePost")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentAttachmentsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetEquipmentAttachment.Any(a => a.EquipmentAttachmentId == id);

            // not found
            if (!exists) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            
            HetEquipmentAttachment item = _context.HetEquipmentAttachment
                .Include(x => x.Equipment)
                .First(a => a.EquipmentAttachmentId == id);

            _context.HetEquipmentAttachment.Remove(item);

            // save the changes
            _context.SaveChanges();

            // convert to UI model
            EquipmentAttachment response = new EquipmentAttachment();
            response = (EquipmentAttachment)ModelHelper.CopyProperties(item, response);

            return new ObjectResult(new HetsResponse(response));
        }

        /// <summary>	
        /// Update equipment attachment	
        /// </summary>	
        /// <param name="id">id of EquipmentAttachment to update</param>	
        /// <param name="item"></param>	
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("EquipmentAttachmentsIdPut")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentAttachmentsIdPut([FromRoute]int id, [FromBody]EquipmentAttachment item)
        {
            if (id != item.Id)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetEquipmentAttachment.Any(a => a.EquipmentAttachmentId == id);

            if (!exists)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // get record
            HetEquipmentAttachment equipmentAttachment = _context.HetEquipmentAttachment
                .First(x => x.EquipmentAttachmentId == id);

            equipmentAttachment.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            equipmentAttachment.Description = item.Description;
            equipmentAttachment.TypeName = item.TypeName;
            equipmentAttachment.EquipmentId = item.EquipmentId;      

            _context.HetEquipmentAttachment.Update(equipmentAttachment);

            // save the changes
            _context.SaveChanges();

            // return the updated condition type record
            HetEquipmentAttachment updEquipmentAttachment = _context.HetEquipmentAttachment.AsNoTracking()
                .Include(x => x.Equipment)
                .FirstOrDefault(a => a.EquipmentAttachmentId == id);

            // convert to UI model
            EquipmentAttachment response = new EquipmentAttachment();
            response = (EquipmentAttachment)ModelHelper.CopyProperties(updEquipmentAttachment, response);

            return new ObjectResult(new HetsResponse(response));
        }

        /// <summary>	
        /// Create equipment attachment	
        /// </summary>	
        /// <param name="item"></param>	
        [HttpPost]
        [Route("")]
        [SwaggerOperation("EquipmentAttachmentsPost")]
        [SwaggerResponse(200, type: typeof(EquipmentAttachment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentAttachmentsPost([FromBody]EquipmentAttachment item)
        {
            if (item != null)
            {
                // not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetEquipmentAttachment.Any(a => a.EquipmentAttachmentId == item.Id);

            if (!exists)
            {
                // update record
                HetEquipmentAttachment equipmentAttachment = _context.HetEquipmentAttachment
                    .First(x => x.EquipmentAttachmentId == item.Id);

                equipmentAttachment.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
                equipmentAttachment.Description = item.Description;
                equipmentAttachment.TypeName = item.TypeName;
                equipmentAttachment.EquipmentId = item.EquipmentId;

                _context.HetEquipmentAttachment.Update(equipmentAttachment);
            }
            else
            {
                // create record
                HetEquipmentAttachment equipmentAttachment = new HetEquipmentAttachment
                {
                    ConcurrencyControlNumber = item.ConcurrencyControlNumber,
                    Description = item.Description,
                    TypeName = item.TypeName,
                    EquipmentId = item.EquipmentId
                };
                
                _context.HetEquipmentAttachment.Add(equipmentAttachment);
            }
            
            // save the changes
            _context.SaveChanges();

            // get the id (in the case of new records)
            int id = item.Id;

            // return the updated condition type record
            HetEquipmentAttachment updEquipmentAttachment = _context.HetEquipmentAttachment.AsNoTracking()
                .Include(x => x.Equipment)
                .FirstOrDefault(a => a.EquipmentAttachmentId == id);

            // convert to UI model
            EquipmentAttachment response = new EquipmentAttachment();
            response = (EquipmentAttachment)ModelHelper.CopyProperties(updEquipmentAttachment, response);

            return new ObjectResult(new HetsResponse(response));
        }
    }
}
