/*
 * REST API Documentation for the MOTI Hired Equipment Tracking System (HETS) Application
 *
 * The Hired Equipment Program is for owners/operators who have a dump truck, bulldozer, backhoe or  other piece of equipment they want to hire out to the transportation ministry for day labour and  emergency projects.  The Hired Equipment Program distributes available work to local equipment owners. The program is  based on seniority and is designed to deliver work to registered users fairly and efficiently  through the development of local area call-out lists. 
 *
 * OpenAPI spec version: v1
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class EquipmentService : IEquipmentService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public EquipmentService(DbAppContext context)
        {
            _context = context;
        }

        private void AdjustRecord (Equipment item)
        {
            // Adjust the record to allow it to be updated / inserted
            if (item.LocalArea != null)
            {
                int localarea_id = item.LocalArea.Id;
                bool localarea_exists = _context.LocalAreas.Any(a => a.Id == localarea_id);
                if (localarea_exists)
                {
                    LocalArea localarea = _context.LocalAreas.First(a => a.Id == localarea_id);
                    item.LocalArea = localarea;
                }
                else
                {
                    item.LocalArea = null;
                }
            }            

            // EquiptmentType
            if (item.EquipmentType != null)
            {
                int equipment_type_id = item.EquipmentType.Id;
                bool equipment_type_exists = _context.EquipmentTypes.Any(a => a.Id == equipment_type_id);
                if (equipment_type_exists)
                {
                    EquipmentType equipmentType = _context.EquipmentTypes.First(a => a.Id == equipment_type_id);
                    item.EquipmentType = equipmentType;
                }
                else
                {
                    item.EquipmentType = null;
                }
            }
            
            // dump truck details
            if (item.DumpTruck != null)
            {
                int dump_truck_details_id = item.DumpTruck.Id;
                bool dump_truck_details_exists = _context.DumpTrucks.Any(a => a.Id == dump_truck_details_id);
                if (dump_truck_details_exists)
                {
                    DumpTruck dumptruck = _context.DumpTrucks.First(a => a.Id == dump_truck_details_id);
                    item.DumpTruck = dumptruck;
                }
                else
                {
                    item.DumpTruck = null;
                }
            }
            
            // owner
            if (item.Owner != null)
            {
                int owner_id = item.Owner.Id;
                bool owner_exists = _context.Owners.Any(a => a.Id == owner_id);
                if (owner_exists)
                {
                    Owner owner = _context.Owners.First(a => a.Id == owner_id);
                    item.Owner = owner;
                }
                else
                {
                    item.Owner = null;
                }
            }


            // EquipmentAttachments is a list     
            if (item.EquipmentAttachments != null)
            {
                for (int i = 0; i < item.EquipmentAttachments.Count; i++)
                {
                    EquipmentAttachment equipmentAttachment = item.EquipmentAttachments[i];
                    if (equipmentAttachment != null)
                    {
                        int equipmentAttachment_id = equipmentAttachment.Id;
                        bool equipmentAttachment_exists = _context.EquipmentAttachments.Any(a => a.Id == equipmentAttachment_id);
                        if (equipmentAttachment_exists)
                        {
                            equipmentAttachment = _context.EquipmentAttachments.First(a => a.Id == equipmentAttachment_id);
                            item.EquipmentAttachments[i] = equipmentAttachment;
                        }
                        else
                        {
                            item.EquipmentAttachments[i] = null;
                        }
                    }
                }
            }

            // Attachments is a list     
            if (item.Attachments != null)
            {
                for (int i = 0; i < item.Attachments.Count; i++)
                {
                    Attachment attachment = item.Attachments[i];
                    if (attachment != null)
                    {
                        int attachment_id = attachment.Id;
                        bool attachment_exists = _context.Attachments.Any(a => a.Id == attachment_id);
                        if (attachment_exists)
                        {
                            attachment = _context.Attachments.First(a => a.Id == attachment_id);
                            item.Attachments[i] = attachment;
                        }
                        else
                        {
                            item.Attachments[i] = null;
                        }
                    }
                }
            }

            // Notes is a list     
            if (item.Notes != null)
            {
                for (int i = 0; i < item.Notes.Count; i++)
                {
                    Note note = item.Notes[i];
                    if (note != null)
                    {
                        int note_id = note.Id;
                        bool note_exists = _context.Notes.Any(a => a.Id == note_id);
                        if (note_exists)
                        {
                            note = _context.Notes.First(a => a.Id == note_id);
                            item.Notes[i] = note;
                        }
                        else
                        {
                            item.Notes[i] = null;
                        }
                    }
                }
            }

            // History is a list     
            if (item.History != null)
            {
                for (int i = 0; i < item.History.Count; i++)
                {
                    History history = item.History[i];
                    if (history != null)
                    {
                        int history_id = history.Id;
                        bool history_exists = _context.Historys.Any(a => a.Id == history_id);
                        if (history_exists)
                        {
                            history = _context.Historys.First(a => a.Id == history_id);
                            item.History[i] = history;
                        }
                        else
                        {
                            item.History[i] = null;
                        }
                    }
                }
            }

            // Seniority Audit is a list     
            if (item.SeniorityAudit != null)
            {
                for (int i = 0; i < item.SeniorityAudit.Count; i++)
                {
                    SeniorityAudit seniorityaudit = item.SeniorityAudit[i];
                    if (seniorityaudit != null)
                    {
                        int seniorityaudit_id = seniorityaudit.Id;
                        bool seniorityaudit_exists = _context.SeniorityAudits.Any(a => a.Id == seniorityaudit_id);
                        if (seniorityaudit_exists)
                        {
                            seniorityaudit = _context.SeniorityAudits.First(a => a.Id == seniorityaudit_id);
                            item.SeniorityAudit[i] = seniorityaudit;
                        }
                        else
                        {
                            item.SeniorityAudit[i] = null;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Equipment created</response>
        public virtual IActionResult EquipmentBulkPostAsync(Equipment[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }
            foreach (Equipment item in items)
            {
                AdjustRecord(item);

                // determine if this is an insert or an update            
                bool exists = _context.Equipments.Any(a => a.Id == item.Id);
                if (exists)                
                {
                    _context.Update(item);
                }
                else
                {
                    _context.Add(item);
                }
            }
            // Save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentGetAsync()
        {            
            var result = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .ToList();
            return new ObjectResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentIdDeletePostAsync(int id)
        {
            var exists = _context.Equipments.Any(a => a.Id == id);
            if (exists)
            {
                var item = _context.Equipments.First(a => a.Id == id);
                _context.Equipments.Remove(item);
                // Save the changes
                _context.SaveChanges();
                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentAttachments for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdEquipmentattachmentsGetAsync(int id)
        {
            bool exists = _context.Equipments.Any(x => x.Id == id);
            if (exists)
            {
                var result = _context.EquipmentAttachments
                    .Include(x => x.Equipment)
                    .Include(x => x.Type)
                    .Where(x => x.Equipment.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentIdGetAsync(int id)
        {
            var exists = _context.Equipments.Any(a => a.Id == id);
            if (exists)
            {
                var result = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .First(a => a.Id == id);
                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentIdPutAsync(int id, Equipment item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                var exists = _context.Equipments                    
                    .Any(a => a.Id == id);
                if (exists && id == item.Id)
                {
                    _context.Equipments.Update(item);
                    // Save the changes
                    _context.SaveChanges();

                    var result = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .First(a => a.Id == id);
                    
                    return new ObjectResult(result);
                }
                else
                {
                    // record not found
                    return new StatusCodeResult(404);
                }
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentViewModel for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdViewGetAsync(int id)
        {
            var exists = _context.Equipments.Any(a => a.Id == id);
            if (exists)
            {
                var equipment = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .First(a => a.Id == id);
                var result = equipment.ToViewModel();

                // populate the calculated fields.

                // ServiceHoursThisYear is the sum of TimeCard hours for the current fiscal year (April 1 - March 31) for the equipment.

                // At this time the structure for timecard hours is not set, so it is set to a constant.

                // TODO: change to a real calculation once the structure for timecard hours is established.

                result.ServiceHoursThisYear = 99;

                // lastTimeRecordDateThisYear is the most recent time card date this year.  Can be null.

                // TODO: change to a real calculation once the structure for timecard hours is established.

                result.LastTimeRecordDateThisYear = null;

                // isWorking is true if there is an active Rental Agreements for the equipment. 

                result.IsWorking = _context.RentalAgreements
                    .Include(x => x.Equipment)
                    .Any(x => x.Equipment.Id == result.Id);

               // hasDuplicates is true if there is other equipment with the same serial number.

                result.HasDuplicates = _context.Equipments.Any(x => x.SerialNum == result.SerialNum && x.Status == "Active");
                
                // duplicate Equipment uses the same criteria as hasDuplicates.

                if (result.HasDuplicates == true)
                {
                    result.DuplicateEquipment = _context.Equipments
                        .Include(x => x.LocalArea.ServiceArea.District.Region)
                        .Include(x => x.EquipmentType)
                        .Include(x => x.DumpTruck)
                        .Include(x => x.Owner)
                        .Include(x => x.EquipmentAttachments)
                        .Include(x => x.Notes)
                        .Include(x => x.Attachments)
                        .Include(x => x.History)
                        .Where(x => x.SerialNum == result.SerialNum && x.Status == "Active")
                        .ToList();
                }

                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Equipment created</response>
        public virtual IActionResult EquipmentPostAsync(Equipment item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.Equipments.Any(a => a.Id == item.Id);
                if (exists)
                {
                    _context.Equipments.Update(item);                    
                }
                else
                {
                    // record not found
                    _context.Equipments.Add(item);
                }
                // Save the changes                    
                _context.SaveChanges();
                int item_id = item.Id;
                var result = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .First(a => a.Id == item_id);

                return new ObjectResult(result);                
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }

        }

        /// <summary>
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localareas">Local Areas (array of id numbers)</param>
        /// <param name="types">Equipment Types (array of id numbers)</param>
        /// <param name="attachments">Equipment Attachments (array of id numbers)</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notverifiedsincedate">Not Verified Since Date</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentSearchGetAsync(int?[] localareas, int?[] types, int?[] attachments, int? owner, string status, bool? hired, DateTime? notverifiedsincedate)
        {
            var data = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Select(x => x);

            if (localareas != null)
            {
                foreach (int? localarea in localareas)
                {
                    if (localarea != null)
                    {
                        data = data.Where(x => x.LocalArea.Id == localarea);
                    }
                }                
            }

            if (types != null)
            {
                foreach (int? equipmenttype in types)
                {
                    if (equipmenttype != null)
                    {
                        data = data.Where(x => x.EquipmentType.Id == equipmenttype);
                    }
                }
            }

            if (attachments != null)
            {
                foreach (int? attachment in attachments)
                {
                    if (attachment != null)
                    {
                        data = data.Where(x => x.EquipmentAttachments.Any(y => y.Id == attachment));
                    }
                }
            }

            if (owner != null)
            {
                data = data.Where(x => x.Owner.Id == owner);                
            }

            if (status != null)
            {
                data = data.Where(x => x.Status == status);
            }

            if (hired != null)
            {
                // hired is not currently implemented. 
            }

            if (notverifiedsincedate != null)
            {
                data = data.Where(x => x.LastVerifiedDate >= notverifiedsincedate);
            }

            var result = data.ToList();
            return new ObjectResult(result);

        }
    }
}
