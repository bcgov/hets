using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using HetsApi.Authorization;
using HetsApi.Model;
using HetsData.Entities;
using AutoMapper;
using HetsData.Dtos;

namespace HetsApi.Controllers
{
    /// <summary>
    /// Equipment Attachment Controller
    /// </summary>
    [Route("api/equipmentAttachments")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentAttachmentController : ControllerBase
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EquipmentAttachmentController(DbAppContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Delete equipment attachment
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to delete</param>
        [HttpPost]
        [Route("{id}/delete")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<EquipmentAttachmentDto> EquipmentAttachmentsIdDeletePost([FromRoute]int id)
        {
            bool exists = _context.HetEquipmentAttachments.Any(a => a.EquipmentAttachmentId == id);

            // not found
            if (!exists) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            HetEquipmentAttachment item = _context.HetEquipmentAttachments
                .Include(x => x.Equipment)
                .First(a => a.EquipmentAttachmentId == id);

            _context.HetEquipmentAttachments.Remove(item);

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(_mapper.Map<EquipmentAttachmentDto>(item)));
        }

        /// <summary>
        /// Update equipment attachment
        /// </summary>
        /// <param name="id">id of EquipmentAttachment to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<EquipmentAttachmentDto> EquipmentAttachmentsIdPut([FromRoute]int id, [FromBody]EquipmentAttachmentDto item)
        {
            if (id != item.EquipmentAttachmentId)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            bool exists = _context.HetEquipmentAttachments.Any(a => a.EquipmentAttachmentId == id);

            if (!exists)
            {
                // not found
                return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // get record
            HetEquipmentAttachment equipmentAttachment = _context.HetEquipmentAttachments
                .First(x => x.EquipmentAttachmentId == id);

            equipmentAttachment.ConcurrencyControlNumber = item.ConcurrencyControlNumber;
            equipmentAttachment.Description = item.TypeName;
            equipmentAttachment.TypeName = item.TypeName;
            equipmentAttachment.EquipmentId = item.Equipment.EquipmentId;

            // save the changes
            _context.SaveChanges();

            // return the updated condition type record
            HetEquipmentAttachment updEquipmentAttachment = _context.HetEquipmentAttachments.AsNoTracking()
                .Include(x => x.Equipment)
                .FirstOrDefault(a => a.EquipmentAttachmentId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<EquipmentAttachmentDto>(updEquipmentAttachment)));
        }

        /// <summary>
        /// Create equipment attachment
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<EquipmentAttachmentDto> EquipmentAttachmentsPost([FromBody]EquipmentAttachmentDto item)
        {
            // not found
            if (item == null) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // create record
            HetEquipmentAttachment equipmentAttachment = new HetEquipmentAttachment
            {
                ConcurrencyControlNumber = item.ConcurrencyControlNumber,
                Description = item.TypeName,
                TypeName = item.TypeName,
                EquipmentId = item.Equipment.EquipmentId
            };

            // save the changes
            _context.HetEquipmentAttachments.Add(equipmentAttachment);
            _context.SaveChanges();

            // get the id (in the case of new records)
            int id = equipmentAttachment.EquipmentAttachmentId;

            // return the updated condition type record
            HetEquipmentAttachment updEquipmentAttachment = _context.HetEquipmentAttachments.AsNoTracking()
                .Include(x => x.Equipment)
                .FirstOrDefault(a => a.EquipmentAttachmentId == id);

            return new ObjectResult(new HetsResponse(_mapper.Map<EquipmentAttachmentDto>(updEquipmentAttachment)));
        }

        /// <summary>
        /// Create multiple equipment attachments (an array of equipment attachments)
        /// </summary>
        /// <param name="items"></param>
        [HttpPost]
        [Route("bulk")]
        [RequiresPermission(HetPermission.Login, HetPermission.WriteAccess)]
        public virtual ActionResult<List<EquipmentAttachmentDto>> EquipmentAttachmentsBulkPost([FromBody]EquipmentAttachmentDto[] items)
        {
            // not found
            if (items == null || items.Length < 1) return new NotFoundObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));

            // process each attachment records
            foreach (var item in items)
            {
                HetEquipmentAttachment equipmentAttachment = new HetEquipmentAttachment
                {
                    ConcurrencyControlNumber = item.ConcurrencyControlNumber,
                    Description = item.TypeName,
                    TypeName = item.TypeName,
                    EquipmentId = item.Equipment.EquipmentId
                };

                // save the changes
                _context.HetEquipmentAttachments.Add(equipmentAttachment);
            }

            _context.SaveChanges();

            // return all equipment attachments
            int id = items[0].Equipment.EquipmentId;

            List<HetEquipmentAttachment> attachments = _context.HetEquipmentAttachments.AsNoTracking()
                .Where(x => x.EquipmentId == id).ToList();

            return new ObjectResult(new HetsResponse(_mapper.Map<List<EquipmentAttachmentDto>>(attachments)));
        }
    }
}
