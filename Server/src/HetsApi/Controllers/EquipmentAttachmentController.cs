using System.Collections.Generic;
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
    [Route("api/equipmentAttachments")]
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
            User user = UserAccountHelper.GetUser(context, httpContextAccessor.HttpContext);
            _context.SmUserId = user.SmUserId;
            _context.DirectoryName = user.SmAuthorizationDirectory;
            _context.SmUserGuid = user.UserGuid;
            _context.SmBusinessGuid = user.BusinessGuid;
        }

        /// <summary>
        /// Delete equipment attachment
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [SwaggerOperation("EquipmentAttachmentsIdDeletePost")]
        [SwaggerResponse(200, type: typeof(HetEquipmentAttachment))]
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

            return new ObjectResult(new HetsResponse(item));
        }

        /// <summary>
        /// Update equipment attachment
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("EquipmentAttachmentsIdPut")]
        [SwaggerResponse(200, type: typeof(HetEquipmentAttachment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentAttachmentsIdPut([FromRoute]int id, [FromBody]HetEquipmentAttachment item)
        {
            if (id != item.EquipmentAttachmentId)
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
            equipmentAttachment.Description = item.TypeName;
            equipmentAttachment.TypeName = item.TypeName;
            equipmentAttachment.EquipmentId = item.Equipment.EquipmentId;

            _context.HetEquipmentAttachment.Update(equipmentAttachment);

            // save the changes
            _context.SaveChanges();

            // return the updated condition type record
            HetEquipmentAttachment updEquipmentAttachment = _context.HetEquipmentAttachment.AsNoTracking()
                .Include(x => x.Equipment)
                .FirstOrDefault(a => a.EquipmentAttachmentId == id);

            return new ObjectResult(new HetsResponse(updEquipmentAttachment));
        }

        /// <summary>
        /// Create equipment attachment
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [SwaggerOperation("EquipmentAttachmentsPost")]
        [SwaggerResponse(200, type: typeof(HetEquipmentAttachment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentAttachmentsPost([FromBody]HetEquipmentAttachment item)
        {
            // not found
            if (item == null) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // create record
            HetEquipmentAttachment equipmentAttachment = new HetEquipmentAttachment
            {
                ConcurrencyControlNumber = item.ConcurrencyControlNumber,
                Description = item.TypeName,
                TypeName = item.TypeName,
                EquipmentId = item.Equipment.EquipmentId
            };

            // save the changes
            _context.HetEquipmentAttachment.Add(equipmentAttachment);
            _context.SaveChanges();

            // get the id (in the case of new records)
            int id = equipmentAttachment.EquipmentAttachmentId;

            // return the updated condition type record
            HetEquipmentAttachment updEquipmentAttachment = _context.HetEquipmentAttachment.AsNoTracking()
                .Include(x => x.Equipment)
                .FirstOrDefault(a => a.EquipmentAttachmentId == id);

            return new ObjectResult(new HetsResponse(updEquipmentAttachment));
        }

        /// <summary>	
        /// Create multiple equipment attachments (an array of equipment attachments)
        /// </summary>	
        /// <param name="items"></param>	
        [HttpPost]
        [Route("bulk")]
        [SwaggerOperation("EquipmentAttachmentsBulkPost")]
        [SwaggerResponse(200, type: typeof(List<HetEquipmentAttachment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentAttachmentsBulkPost([FromBody]HetEquipmentAttachment[] items)
        {
            // not found
            if (items == null || items.Length < 1) return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // process each attachment records
            foreach (HetEquipmentAttachment item in items)
            {
                HetEquipmentAttachment equipmentAttachment = new HetEquipmentAttachment
                {
                    ConcurrencyControlNumber = item.ConcurrencyControlNumber,
                    Description = item.TypeName,
                    TypeName = item.TypeName,
                    EquipmentId = item.Equipment.EquipmentId
                };

                // save the changes
                _context.HetEquipmentAttachment.Add(equipmentAttachment);
            }

            _context.SaveChanges();

            // return all equipment attachments
            int id = items[0].Equipment.EquipmentId;

            List<HetEquipmentAttachment> attachments = _context.HetEquipmentAttachment.AsNoTracking()
                .Where(x => x.EquipmentId == id).ToList();

            return new ObjectResult(new HetsResponse(attachments));
        }
    }
}
