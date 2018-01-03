using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.AspNetCore.Http;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    ///
    /// </summary>
    public class EquipmentService : ServiceBase, IEquipmentService
    {
        private readonly DbAppContext _context;

        /// <summary>
        /// Create a service and set the database context
        /// </summary>
        public EquipmentService(IHttpContextAccessor httpContextAccessor, DbAppContext context) : base(httpContextAccessor, context)
        {
            _context = context;
        }

        private void AdjustRecord(Equipment item)
        {
            if (item != null)
            {
                // Adjust the record to allow it to be updated / inserted
                if (item.LocalArea != null)
                {
                    item.LocalArea = _context.LocalAreas.FirstOrDefault(a => a.Id == item.LocalArea.Id);
                }

                // DistrictEquiptmentType
                if (item.DistrictEquipmentType != null)
                {
                    item.DistrictEquipmentType = _context.DistrictEquipmentTypes.FirstOrDefault(a => a.Id == item.DistrictEquipmentType.Id);
                }

                // dump truck details
                if (item.DumpTruck != null)
                {
                    item.DumpTruck = _context.DumpTrucks.FirstOrDefault(a => a.Id == item.DumpTruck.Id);
                }

                // owner
                if (item.Owner != null)
                {
                    item.Owner = _context.Owners
                        .Include(x => x.EquipmentList)
                        .FirstOrDefault(a => a.Id == item.Owner.Id);
                }


                // EquipmentAttachments is a list
                if (item.EquipmentAttachments != null)
                {
                    for (int i = 0; i < item.EquipmentAttachments.Count; i++)
                    {
                        if (item.EquipmentAttachments[i] != null)
                        {
                            item.EquipmentAttachments[i] = _context.EquipmentAttachments.FirstOrDefault(a => a.Id == item.EquipmentAttachments[i].Id);
                        }
                    }
                }

                // Attachments is a list
                if (item.Attachments != null)
                {
                    for (int i = 0; i < item.Attachments.Count; i++)
                    {
                        if (item.Attachments[i] != null)
                        {
                            item.Attachments[i] = _context.Attachments.FirstOrDefault(a => a.Id == item.Attachments[i].Id);
                        }
                    }
                }

                // Notes is a list
                if (item.Notes != null)
                {
                    for (int i = 0; i < item.Notes.Count; i++)
                    {
                        if (item.Notes[i] != null)
                        {
                            item.Notes[i] = _context.Notes.FirstOrDefault(a => a.Id == item.Notes[i].Id);
                        }
                    }
                }

                // History is a list
                if (item.History != null)
                {
                    for (int i = 0; i < item.History.Count; i++)
                    {
                        if (item.History[i] != null)
                        {
                            item.History[i] = _context.Historys.FirstOrDefault(a => a.Id == item.History[i].Id);
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
                    .Include(x => x.DistrictEquipmentType)
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
        /// Remove seniority audits associated with an equipment ID
        /// </summary>
        /// <param name="equipmentId"></param>
        private void RemoveSeniorityAudits(int equipmentId)
        {
            var seniorityAudits = _context.SeniorityAudits
                    .Include(x => x.Equipment)
                    .Where(x => x.Equipment.Id == equipmentId)
                    .ToList();
            if (seniorityAudits != null)
            {
                foreach (SeniorityAudit seniorityAudit in seniorityAudits)
                {
                    _context.SeniorityAudits.Remove(seniorityAudit);
                }
            }
            _context.SaveChanges();
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks>Returns attachments for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>

        public virtual IActionResult EquipmentIdAttachmentsGetAsync(int id)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);
            if (exists)
            {
                Equipment equipment = _context.Equipments
                    .Include(x => x.Attachments)
                    .First(a => a.Id == id);
                var result = MappingExtensions.GetAttachmentListAsViewModel(equipment.Attachments);
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
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentIdDeletePostAsync(int id)
        {
            var exists = _context.Equipments.Any(a => a.Id == id);
            if (exists)
            {
                // remove associated seniority audits.
                RemoveSeniorityAudits(id);

                var item = _context.Equipments
                    .Include(x => x.LocalArea)
                    .Include(x => x.DistrictEquipmentType.EquipmentType)
                    .First(a => a.Id == id);

                int localAreaId = -1;
                int equipmentTypeId = -1;

                if (item.LocalArea != null && item.DistrictEquipmentType != null && item.DistrictEquipmentType.EquipmentType != null)
                {
                    localAreaId = item.LocalArea.Id;
                    equipmentTypeId = item.DistrictEquipmentType.EquipmentType.Id;
                }

                _context.Equipments.Remove(item);
                // Save the changes

                _context.SaveChanges();
                // update the seniority list
                if (localAreaId != -1 && equipmentTypeId != -1)
                {
                    _context.CalculateSeniorityList(localAreaId, equipmentTypeId);
                }

                return new ObjectResult(item);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }


        ///  <summary>
        /// 
        ///  </summary>
        ///  <remarks>Returns History for a particular Equipment</remarks>
        ///  <param name="id">id of SchoolBus to fetch History for</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdHistoryGetAsync(int id, int? offset, int? limit)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);
            if (exists)
            {
                Equipment schoolBus = _context.Equipments
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                List<History> data = schoolBus.History.OrderByDescending(y => y.LastUpdateTimestamp).ToList();

                if (offset == null)
                {
                    offset = 0;
                }
                if (limit == null)
                {
                    limit = data.Count() - offset;
                }
                List<HistoryViewModel> result = new List<HistoryViewModel>();

                for (int i = (int)offset; i < data.Count() && i < offset + limit; i++)
                {
                    result.Add(data[i].ToViewModel(id));
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
        /// <remarks>Add a History record to the Equipment</remarks>
        /// <param name="id">id of Equipment to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        public virtual IActionResult EquipmentIdHistoryPostAsync(int id, History item)
        {
            HistoryViewModel result = new HistoryViewModel();

            bool exists = _context.Equipments.Any(a => a.Id == id);
            if (exists)
            {
                Equipment equipment = _context.Equipments
                    .Include(x => x.History)
                    .First(a => a.Id == id);
                if (equipment.History == null)
                {
                    equipment.History = new List<History>();
                }
                // force add
                item.Id = 0;
                equipment.History.Add(item);
                _context.Equipments.Update(equipment);
                _context.SaveChanges();
            }

            result.HistoryText = item.HistoryText;
            result.Id = item.Id;
            result.LastUpdateTimestamp = item.LastUpdateTimestamp;
            result.LastUpdateUserid = item.LastUpdateUserid;
            result.AffectedEntityId = id;

            return new ObjectResult(result);
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
                    .Include(x => x.DistrictEquipmentType.EquipmentType)
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

                DateTime? originalSeniorityEffectiveDate = _context.GetEquipmentSeniorityEffectiveDate(id);

                var exists = _context.Equipments
                    .Any(a => a.Id == id);

                if (exists && id == item.Id)
                {
                    _context.Equipments.Update(item);
                    // Save the changes
                    _context.SaveChanges();

                    // if seniority has changed, update blocks.
                    if ((originalSeniorityEffectiveDate == null && item.SeniorityEffectiveDate != null) ||
                        (originalSeniorityEffectiveDate != null && item.SeniorityEffectiveDate != null
                            && originalSeniorityEffectiveDate < item.SeniorityEffectiveDate))
                    {
                        _context.UpdateBlocksFromEquipment(item);
                    }

                    var result = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType)
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

        private void CalculateViewModel(EquipmentViewModel result)
        {
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

            result.HasDuplicates = _context.Equipments.Any(x => x.SerialNumber == result.SerialNumber && x.Status == "Active");

            // duplicate Equipment uses the same criteria as hasDuplicates.

            if (result.HasDuplicates == true)
            {
                result.DuplicateEquipment = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Where(x => x.SerialNumber == result.SerialNumber && x.Status == "Active")
                    .ToList();
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
                    .Include(x => x.DistrictEquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .First(a => a.Id == id);
                var result = equipment.ToViewModel();

                CalculateViewModel(result);

                return new ObjectResult(result);
            }
            else
            {
                // record not found
                return new StatusCodeResult(404);
            }
        }

        private string GenerateEquipmentCode(string ownerEquipmentCodePrefix, int equipmentNumber)
        {
            string result = ownerEquipmentCodePrefix + "-" + equipmentNumber.ToString("D4");
            return result;
        }

        /// <summary>
        /// Set the Equipment fields for a new record for fields that are not provided by the front end.
        /// </summary>
        /// <param name="item"></param>
        private void SetNewRecordFields(Equipment item)
        {
            item.ReceivedDate = DateTime.UtcNow;
            item.LastVerifiedDate = DateTime.UtcNow;
            // generate a new equipment code.
            if (item.Owner != null)
            {
                int equipmentNumber = 1;
                if (item.Owner.EquipmentList != null)
                {
                    bool looking = true;
                    equipmentNumber = item.Owner.EquipmentList.Count + 1;

                    // generate a unique equipment number
                    while (looking)
                    {
                        string candidate = GenerateEquipmentCode(item.Owner.OwnerEquipmentCodePrefix, equipmentNumber);
                        if ((item.Owner.EquipmentList).Any(x => x.EquipmentCode == candidate))
                        {
                            equipmentNumber++;
                        }
                        else
                        {
                            looking = false;
                        }
                    }
                }
                // set the equipment code
                item.EquipmentCode = GenerateEquipmentCode(item.Owner.OwnerEquipmentCodePrefix, equipmentNumber);
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
                    _context.SaveChanges();
                }
                else
                {
                    // record not found
                    // Certain fields are set on new record.
                    SetNewRecordFields(item);
                    _context.Equipments.Add(item);
                    _context.SaveChanges();
                    // add the equipment to the Owner's equipment list.
                    Owner owner = item.Owner;
                    if (owner != null)
                    {
                        if (owner.EquipmentList == null)
                        {
                            owner.EquipmentList = new List<Equipment>();
                        }
                        if (!owner.EquipmentList.Contains(item))
                        {
                            owner.EquipmentList.Add(item);
                            _context.Owners.Update(owner);
                        }
                    }
                    _context.SaveChanges();
                    // now update the seniority list.
                    if (item.LocalArea != null && item.DistrictEquipmentType != null && item.DistrictEquipmentType.EquipmentType != null)
                    {
                        _context.CalculateSeniorityList(item.LocalArea.Id, item.DistrictEquipmentType.EquipmentType.Id);
                    }



                }
                // return the full object for the client side code.

                int item_id = item.Id;
                var result = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType)
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
        /// Recalculates seniority for the database
        /// </summary>
        /// <remarks>Used to calculate seniority for all database records.</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentRecalcSeniorityGetAsync(int region)
        {
            // calculate all of the rotation lists.

            var localAreas = _context.LocalAreas
                .Where(x => x.ServiceArea.District.Region.Id == region)
                .Select(x => x)
                .ToList();
            var equipmentTypes = _context.EquipmentTypes.Select(x => x).ToList();

            foreach (LocalArea localArea in localAreas)
            {
                foreach (EquipmentType equipmentType in equipmentTypes)
                {
                    _context.CalculateSeniorityList(localArea.Id, equipmentType.Id);
                }
            }

            return new ObjectResult("Done Recalc.");
        }

        /// <summary>
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localareasCSV">Local Areas (array of id numbers)</param>
        /// <param name="typesCSV">Equipment Types (array of id numbers)</param>
        /// <param name="equipmentAttachment">Equipment Attachments </param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notverifiedsincedate">Not Verified Since Date</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentSearchGetAsync(string localareasCSV, string typesCSV, string equipmentAttachment, int? owner, string status, bool? hired, DateTime? notverifiedsincedate)
        {
            int?[] localareas = ParseIntArray(localareasCSV);
            int?[] types = ParseIntArray(typesCSV);

            var data = _context.Equipments
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Select(x => x);

            // Default search results must be limited to user
            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();
            data = data.Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId));

            if (localareas != null && localareas.Length > 0)
            {
                data = data.Where(x => localareas.Contains(x.LocalArea.Id));
            }

            if (equipmentAttachment != null)
            {
                data = data.Where(x => x.EquipmentAttachments.Any(y => y.TypeName.ToLower().Contains(equipmentAttachment.ToLower())));
            }

            if (owner != null)
            {
                data = data.Where(x => x.Owner.Id == owner);
            }

            if (status != null)
            {
                // TODO: Change to enumerated type
                data = data.Where(x => x.Status.ToLower() == status.ToLower());
            }

            if (hired == true)
            {
                IQueryable<int?> hiredEquipmentQuery = _context.RentalAgreements
                                    .Where(agreement => agreement.Status == "Active")
                                    .Select(agreement => agreement.EquipmentId)
                                    .Distinct();

                data = data.Where(e => hiredEquipmentQuery.Contains(e.Id));
            }

            if (types != null && types.Length > 0)
            {
                data = data.Where(x => types.Contains(x.DistrictEquipmentType.Id));
            }

            if (notverifiedsincedate != null)
            {
                data = data.Where(x => x.LastVerifiedDate >= notverifiedsincedate);
            }

            List<EquipmentViewModel> result = new List<EquipmentViewModel>();
            foreach (var item in data)
            {
                EquipmentViewModel newItem = item.ToViewModel();
                result.Add(newItem);
            }

            // second pass to do calculated fields.
            foreach (var equipmentViewModel in result)
            {
                CalculateViewModel(equipmentViewModel);
            }

            return new ObjectResult(result);

        }
    }
}
