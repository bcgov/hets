using System;
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
    /// Equipment Controller
    /// </summary>
    [Route("/api/equipment")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EquipmentController : Controller
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        public EquipmentController(DbAppContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        /// Get equipment by id
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation("EquipmentIdGet")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdGet([FromRoute]int id)
        {
            HetEquipment equipment = _context.HetEquipment.AsNoTracking()
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Owner)
                .Include(x => x.HetEquipmentAttachment)
                .Include(x => x.HetNote)
                .Include(x => x.HetAttachment)
                .Include(x => x.HetHistory)
                .FirstOrDefault(a => a.EquipmentId == id);                                    

            //if (equipment != null)
            //{
              //  response.IsHired = EquipmentHelper.IsHired(id, _context);
              //  response.NumberOfBlocks = EquipmentHelper.GetNumberOfBlocks(equipment, _configuration);
              //  response.HoursYtd = EquipmentHelper.GetYtdServiceHours(id, _context);
            //}

            return new ObjectResult(equipment);
        }

        /// <summary>
        /// Update equipment
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation("EquipmentIdPut")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdPut([FromRoute]int id, [FromBody]HetEquipment item)
        {
            return null;
            //return _service.EquipmentIdPutAsync(id, item);
        }

        /// <summary>
        /// Update equipment status
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        [HttpPut]
        [Route("{id}/status")]
        [SwaggerOperation("EquipmentIdStatusPut")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdStatusPut([FromRoute]int id, [FromBody]EquipmentStatus item)
        {
            return null;
            //return _service.EquipmentIdStatusPutAsync(id, item);
        }

        /// <summary>
        /// Create equipment
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        [Route("/api/equipment")]
        [SwaggerOperation("EquipmentPost")]
        [SwaggerResponse(200, type: typeof(HetEquipment))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentPost([FromBody]HetEquipment item)
        {
            return null;
            //return _service.EquipmentPostAsync(item);
        }

        /// <summary>
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="types">Equipment Types (comma separated list of id numbers)</param>
        /// <param name="equipmentAttachment">Searches equipmentAttachment type</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notVerifiedSinceDate">Not Verified Since Date</param>
        /// <param name="equipmentId">Equipment Code</param>
        [HttpGet]
        [Route("search")]
        [SwaggerOperation("EquipmentSearchGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentLite>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentSearchGet([FromQuery]string localAreas, [FromQuery]string types, 
            [FromQuery]string equipmentAttachment, [FromQuery]int? owner, [FromQuery]string status, 
            [FromQuery]bool? hired, [FromQuery]DateTime? notVerifiedSinceDate, [FromQuery]string equipmentId = null)
        {
            return null;
            //return _service.EquipmentSearchGetAsync(localAreas, types, equipmentAttachment, owner, status, hired, notVerifiedSinceDate, equipmentId);
        }

        /*

        #region Clone Project Agreements

        /// <summary>
        /// Get rental agreements associated with an equipment id
        /// </summary>
        /// <remarks>Gets as Equipment's Rental Agreements</remarks>
        /// <param name="id">id of Equipment to fetch agreements for</param>
        [HttpGet]
        [Route("{id}/rentalAgreements")]
        [SwaggerOperation("EquipmentIdRentalAgreementsGet")]
        [SwaggerResponse(200, type: typeof(List<RentalAgreement>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdRentalAgreementsGet([FromRoute]int id)
        {
            return _service.EquipmentIdGetAgreementsAsync(id);
        }

        /// <summary>
        /// Update a rental agreement by cloning a previous equipment rental agreement
        /// </summary>
        /// <param name="id">Project id</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/rentalAgreementClone")]
        [SwaggerOperation("EquipmentRentalAgreementClonePost")]
        [SwaggerResponse(200, type: typeof(RentalAgreement))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentRentalAgreementClonePost([FromRoute]int id, [FromBody]EquipmentRentalAgreementClone item)
        {
            return _service.EquipmentRentalAgreementClonePostAsync(id, item);
        }

        #endregion

        #region Duplicate Equipment Records

        /// <summary>
        /// Get all duplicate equipment records
        /// </summary>
        /// <param name="id">id of Equipment to fetch Equipment Attachments for</param>
        /// <param name="serialNumber"></param>
        [HttpGet]
        [Route("{id}/duplicates/{serialNumber}")]
        [SwaggerOperation("EquipmentIdEquipmentDuplicatesGet")]
        [SwaggerResponse(200, type: typeof(List<DuplicateEquipmentViewModel>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdEquipmentDuplicatesGet([FromRoute]int id, [FromRoute]string serialNumber)
        {
            return _service.EquipmentIdEquipmentDuplicatesGetAsync(id, serialNumber);
        }

        #endregion

        #region Equipment Attachment Records

        /// <summary>
        /// Get all equipment attachments for an equipment record
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentAttachments for</param>
        [HttpGet]
        [Route("{id}/equipmentAttachments")]
        [SwaggerOperation("EquipmentIdEquipmentAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<EquipmentAttachment>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdEquipmentAttachmentsGet([FromRoute]int id)
        {
            return _service.EquipmentIdEquipmentAttachmentsGetAsync(id);
        }

        #endregion

        #region Attachments

        /// <summary>
        /// Get all attachments associated with an equipment record
        /// </summary>
        /// <remarks>Returns attachments for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch attachments for</param>
        [HttpGet]
        [Route("{id}/attachments")]
        [SwaggerOperation("EquipmentIdAttachmentsGet")]
        [SwaggerResponse(200, type: typeof(List<AttachmentViewModel>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdAttachmentsGet([FromRoute]int id)
        {
            return _service.EquipmentIdAttachmentsGetAsync(id);
        }

        #endregion

        #region Equipment History Records

        /// <summary>
        /// Get equipment history
        /// </summary>
        /// <remarks>Returns History for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch History for</param>
        /// <param name="offset">offset for records that are returned</param>
        /// <param name="limit">limits the number of records returned.</param>
        [HttpGet]
        [Route("{id}/history")]
        [SwaggerOperation("EquipmentIdHistoryGet")]
        [SwaggerResponse(200, type: typeof(List<HistoryViewModel>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdHistoryGet([FromRoute]int id, [FromQuery]int? offset, [FromQuery]int? limit)
        {
            return _service.EquipmentIdHistoryGetAsync(id, offset, limit);
        }

        /// <summary>
        /// Create equipment history
        /// </summary>
        /// <remarks>Add a History record to the Equipment</remarks>
        /// <param name="id">id of Equipment to add History for</param>
        /// <param name="item"></param>
        [HttpPost]
        [Route("{id}/history")]
        [SwaggerOperation("EquipmentIdHistoryPost")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdHistoryPost([FromRoute]int id, [FromBody]History item)
        {
            return _service.EquipmentIdHistoryPostAsync(id, item);
        }

        #endregion

        #region Equipment Note Records

        /// <summary>
        /// Get note records associated with equipment
        /// </summary>
        /// <param name="id">id of Equipment to fetch Notes for</param>
        [HttpGet]
        [Route("{id}/notes")]
        [SwaggerOperation("EquipmentsIdNotesGet")]
        [SwaggerResponse(200, type: typeof(List<Note>))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdNotesGet([FromRoute]int id)
        {
            return _service.EquipmentIdNotesGetAsync(id);
        }

        /// <summary>
        /// Update or create a note associated with a equipment
        /// </summary>
        /// <remarks>Update a Equipment&#39;s Notes</remarks>
        /// <param name="id">id of Equipment to update Notes for</param>
        /// <param name="item">Equipment Note</param>
        [HttpPost]
        [Route("{id}/note")]
        [SwaggerOperation("EquipmentIdNotePost")]
        [SwaggerResponse(200, type: typeof(Note))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdNotePost([FromRoute]int id, [FromBody]Note item)
        {
            return _service.EquipmentIdNotesPostAsync(id, item);
        }

        /// <summary>
        /// Update or create an array of notes associated with a equipment
        /// </summary>
        /// <remarks>Adds Note Records</remarks>
        /// <param name="id">id of Equipment to add notes for</param>
        /// <param name="items">Array of Equipment Notes</param>
        [HttpPost]
        [Route("{id}/notes")]
        [SwaggerOperation("EquipmentIdNotesBulkPostAsync")]
        [SwaggerResponse(200, type: typeof(TimeRecord))]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentIdNotesBulkPostAsync([FromRoute]int id, [FromBody]Note[] items)
        {
            return _service.EquipmentIdNotesBulkPostAsync(id, items);
        }

        #endregion

        #region Seniority List Pdf

        /// <summary>
        /// Get a pdf version of the seniority list
        /// </summary>
        /// <remarks>Returns a PDF version of the seniority list</remarks>
        /// <param name="localAreas">Local Areas (comma separated list of id numbers)</param>
        /// <param name="types">Equipment Types (comma separated list of id numbers)</param>
        [HttpGet]
        [Route("seniorityListPdf")]
        [SwaggerOperation("EquipmentSeniorityListPdfGet")]
        [RequiresPermission(HetPermission.Login)]
        public virtual IActionResult EquipmentSeniorityListPdfGet([FromQuery]string localAreas, [FromQuery]string types)
        {
            return _service.EquipmentSeniorityListPdfGetAsync(localAreas, types);
        }

        #endregion

    */        
    }
}
