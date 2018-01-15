using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HETSAPI.Models;
using HETSAPI.ViewModels;
using HETSAPI.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Services.Impl
{
    /// <summary>
    /// Owner Service
    /// </summary>
    public class OwnerService : ServiceBase, IOwnerService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Owner Service Constructor
        /// </summary>
        public OwnerService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
        }

        private void AdjustRecord(Owner item)
        {
            if (item != null)
            {
                if (item.LocalArea != null)
                {
                    item.LocalArea = _context.LocalAreas.FirstOrDefault(a => a.Id == item.LocalArea.Id);
                }

                // Adjust the owner contacts.
                if (item.Contacts != null)
                {
                    for (int i = 0; i < item.Contacts.Count; i++)
                    {
                        if (item.Contacts[i] != null)
                        {
                            item.Contacts[i] = _context.Contacts.FirstOrDefault(a => a.Id == item.Contacts[i].Id);
                        }
                    }
                }

                if (item.PrimaryContact != null)
                {
                    item.PrimaryContact = _context.Contacts.FirstOrDefault(a => a.Id == item.PrimaryContact.Id);
                }

                // Adjust the equipment list.
                if (item.EquipmentList != null)
                {
                    for (int i = 0; i < item.EquipmentList.Count; i++)
                    {
                        if (item.EquipmentList[i] != null)
                        {
                            item.EquipmentList[i] = _context.Equipments
                                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                                    .Include(x => x.DistrictEquipmentType)
                                    .Include(x => x.DumpTruck)
                                    .Include(x => x.Owner)
                                    .Include(x => x.EquipmentAttachments)
                                    .Include(x => x.Notes)
                                    .Include(x => x.Attachments)
                                    .Include(x => x.History)
                                    .FirstOrDefault(a => a.Id == item.EquipmentList[i].Id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create bulk owner records
        /// </summary>
        /// <param name="items"></param>
        /// <response code="201">Owner created</response>
        public virtual IActionResult OwnersBulkPostAsync(Owner[] items)
        {
            if (items == null)
            {
                return new BadRequestResult();
            }

            foreach (Owner item in items)
            {
                AdjustRecord(item);

                // determine if this is an insert or an update
                bool exists = _context.Owners.Any(a => a.Id == item.Id);

                if (exists)
                {
                    _context.Update(item);
                }
                else
                {
                    _context.Add(item);
                }
            }

            // save the changes
            _context.SaveChanges();
            return new NoContentResult();
        }

        /// <summary>
        /// Get all owners
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult OwnersGetAsync()
        {
            List<Owner> result = _context.Owners
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Get all attachments associated with an owner
        /// </summary>
        /// <remarks>Returns attachments for a particular Owner</remarks>
        /// <param name="id">id of Owner to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        public virtual IActionResult OwnersIdAttachmentsGetAsync(int id)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists)
            {
                Owner owner = _context.Owners
                    .Include(x => x.Attachments)
                    .First(a => a.Id == id);

                List<AttachmentViewModel> result = MappingExtensions.GetAttachmentListAsViewModel(owner.Attachments);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get all contacts associated with an owner
        /// </summary>
        /// <param name="id">id of Owner to fetch Contacts for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult OwnersIdContactsGetAsync(int id)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists)
            {
                List<Contact> result = GetOwnerContacts(id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update contact associated with an owner
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="item">Replacement Owner contacts.</param>
        /// <response code="200">OK</response>
        public virtual IActionResult OwnersIdContactsPostAsync(int id, Contact item)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists && item != null)
            {
                Owner owner = _context.Owners
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentList)
                    .ThenInclude(y => y.DistrictEquipmentType)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // adjust the incoming list.
                item.Id = 0;

                _context.Contacts.Add(item);
                owner.Contacts.Add(item);
                _context.Owners.Update(owner);
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create owner contacts
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Contacts</remarks>
        /// <param name="id">id of Owner to replace Contacts for</param>
        /// <param name="items">Replacement Owner contacts.</param>
        /// <response code="200">OK</response>
        public virtual IActionResult OwnersIdContactsPutAsync(int id, Contact[] items)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists && items != null)
            {
                Owner owner = _context.Owners
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentList)
                    .ThenInclude(y => y.DistrictEquipmentType)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // adjust the incoming list
                for (int i = 0; i < items.Count(); i++)
                {
                    Contact item = items[i];

                    if (item != null)
                    {
                        bool contactExists = _context.Contacts.Any(x => x.Id == item.Id);

                        if (contactExists)
                        {
                            items[i] = _context.Contacts
                                .First(x => x.Id == item.Id);
                        }
                        else
                        {
                            _context.Add(item);
                            items[i] = item;
                        }
                    }
                }

                // remove contacts that are no longer attached.
                foreach (Contact contact in owner.Contacts)
                {
                    if (contact != null && items.All(x => x.Id != contact.Id))
                    {
                        _context.Remove(contact);
                    }
                }

                // replace contacts
                owner.Contacts = items.ToList();
                _context.Update(owner);
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(items));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Delete owner
        /// </summary>
        /// <param name="id">id of Owner to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        public virtual IActionResult OwnersIdDeletePostAsync(int id)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists)
            {
                Owner item = _context.Owners.First(a => a.Id == id);

                if (item != null)
                {
                    _context.Owners.Remove(item);

                    // save the changes
                    _context.SaveChanges();
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        ///  <summary>
        ///  Get history associated with an owner
        ///  </summary>
        ///  <remarks>Returns History for a particular Owner</remarks>
        ///  <param name="id">id of Owner to fetch History for</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult OwnersIdHistoryGetAsync(int id, int? offset, int? limit)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists)
            {
                Owner owner = _context.Owners
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                List<History> data = owner.History.OrderByDescending(y => y.LastUpdateTimestamp).ToList();

                if (offset == null)
                {
                    offset = 0;
                }

                if (limit == null)
                {
                    limit = data.Count - offset;
                }

                List<HistoryViewModel> result = new List<HistoryViewModel>();

                for (int i = (int)offset; i < data.Count && i < offset + limit; i++)
                {
                    result.Add(data[i].ToViewModel(id));
                }

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create hoitory associated with an owner
        /// </summary>
        /// <remarks>Add a History record to the Owner</remarks>
        /// <param name="id">id of Owner to add History for</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="201">History created</response>
        public virtual IActionResult OwnersIdHistoryPostAsync(int id, History item)
        {
            HistoryViewModel result = new HistoryViewModel();

            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists)
            {
                Owner owner = _context.Owners
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                if (owner.History == null)
                {
                    owner.History = new List<History>();
                }

                // force add
                item.Id = 0;
                owner.History.Add(item);
                _context.Owners.Update(owner);
                _context.SaveChanges();
            }

            result.HistoryText = item.HistoryText;
            result.Id = item.Id;
            result.LastUpdateTimestamp = item.LastUpdateTimestamp;
            result.LastUpdateUserid = item.LastUpdateUserid;
            result.AffectedEntityId = id;

            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Get equipment associated with an owner
        /// </summary>
        /// <remarks>Gets an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to fetch Equipment for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult OwnersIdEquipmentGetAsync(int id)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists)
            {
                Owner owner = _context.Owners
                    .Include(x => x.EquipmentList)
                        .ThenInclude(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentList)
                        .ThenInclude(x => x.DistrictEquipmentType)
                    .Include(x => x.EquipmentList)
                        .ThenInclude(x => x.DumpTruck)
                    .Include(x => x.EquipmentList)
                        .ThenInclude(x => x.Owner)
                    .Include(x => x.EquipmentList)
                        .ThenInclude(x => x.EquipmentAttachments)
                    .Include(x => x.EquipmentList)
                        .ThenInclude(x => x.Notes)
                    .Include(x => x.EquipmentList)
                        .ThenInclude(x => x.Attachments)
                    .Include(x => x.EquipmentList)
                        .ThenInclude(x => x.History)
                    .First(a => a.Id == id);

                return new ObjectResult(owner.EquipmentList);
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update equipment associated with an owner
        /// </summary>
        /// <remarks>Replaces an Owner&#39;s Equipment</remarks>
        /// <param name="id">id of Owner to replace Equipment for</param>
        /// <param name="items">Replacement Owner Equipment.</param>
        /// <response code="200">OK</response>
        public virtual IActionResult OwnersIdEquipmentPutAsync(int id, Equipment[] items)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists && items != null)
            {
                Owner owner = _context.Owners
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentList)
                    .ThenInclude(y => y.DistrictEquipmentType)
                    .Include(x => x.EquipmentList)
                    .ThenInclude(y => y.Owner)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(x => x.Id == id);

                // adjust the incoming list
                for (int i = 0; i < items.Count(); i++)
                {
                    Equipment item = items[i];

                    if (item != null)
                    {
                        DateTime lastVerifiedDate = item.LastVerifiedDate;

                        bool equipmentExists = _context.Equipments.Any(x => x.Id == item.Id);

                        if (equipmentExists)
                        {
                            items[i] = _context.Equipments
                                .Include(x => x.LocalArea.ServiceArea.District.Region)
                                .Include(x => x.DistrictEquipmentType)
                                .Include(x => x.DumpTruck)
                                .Include(x => x.Owner)
                                .Include(x => x.EquipmentAttachments)
                                .Include(x => x.Notes)
                                .Include(x => x.Attachments)
                                .Include(x => x.History)
                                .First(x => x.Id == item.Id);

                            if (items[i].LastVerifiedDate != lastVerifiedDate)
                            {
                                items[i].LastVerifiedDate = lastVerifiedDate;
                                _context.Equipments.Update(items[i]);
                            }
                        }
                        else
                        {
                            _context.Add(item);
                            items[i] = item;
                        }
                    }
                }

                // remove equipment that are no longer attached
                List<Equipment> equipmentToRemove = new List<Equipment>();

                foreach (Equipment equipment in owner.EquipmentList)
                {
                    if (equipment != null && items.All(x => x.Id != equipment.Id))
                    {
                        equipmentToRemove.Add(equipment);
                    }
                }

                if (equipmentToRemove.Count > 0)
                {
                    foreach (Equipment equipment in equipmentToRemove)
                    {
                        owner.EquipmentList.Remove(equipment);
                    }
                }

                // replace Equipment List.
                owner.EquipmentList = items.ToList();
                _context.Owners.Update(owner);
                _context.SaveChanges();

                return new ObjectResult(new HetsResponse(items));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get owner by id
        /// </summary>
        /// <param name="id">id of Owner to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        public virtual IActionResult OwnersIdGetAsync(int id)
        {
            bool exists = _context.Owners.Any(a => a.Id == id);

            if (exists)
            {
                Owner result = _context.Owners
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentList).ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.EquipmentList).ThenInclude(y => y.DistrictEquipmentType)
                    .Include(x => x.EquipmentList).ThenInclude(y => y.DumpTruck)
                    .Include(x => x.EquipmentList).ThenInclude(y => y.Owner)
                    .Include(x => x.EquipmentList).ThenInclude(y => y.EquipmentAttachments)
                    .Include(x => x.EquipmentList).ThenInclude(y => y.Notes)
                    .Include(x => x.EquipmentList).ThenInclude(y => y.Attachments)
                    .Include(x => x.EquipmentList).ThenInclude(y => y.History)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .Include(x => x.Contacts)
                    .First(a => a.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Returns contacts for a specific owner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<Contact> GetOwnerContacts(int id)
        {
            List<Contact> result = null;

            Owner owner = _context.Owners
                        .Include(x => x.Contacts)
                        .FirstOrDefault(x => x.Id == id);

            if (owner != null)
            {
                result = owner.Contacts;
                _context.Entry(owner).State = EntityState.Detached;
            }            

            return result;
        }

        /// <summary>
        /// Update owner
        /// </summary>
        /// <param name="id">id of Owner to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Owner not found</response>
        public virtual IActionResult OwnersIdPutAsync(int id, Owner item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                // we specifically do not want to change contacts from this service
                item.Contacts = GetOwnerContacts(id);

                bool exists = _context.Owners.Any(a => a.Id == id);

                if (exists && id == item.Id)
                {
                    _context.Owners.Update(item);

                    // save the changes
                    _context.SaveChanges();

                    return new ObjectResult(new HetsResponse(item));
                }

                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        ///Create owner
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Owner created</response>
        public virtual IActionResult OwnersPostAsync(Owner item)
        {
            AdjustRecord(item);

            bool exists = _context.Owners.Any(a => a.Id == item.Id);

            if (exists)
            {
                _context.Owners.Update(item);
            }
            else
            {
                // record not found
                _context.Owners.Add(item);
            }

            // save the changes
            _context.SaveChanges();

            return new ObjectResult(new HetsResponse(item));
        }

        /// <summary>
        /// Search Owners
        /// </summary>
        /// <remarks>Used for the owner search page.</remarks>
        /// <param name="localAreas">Local Areas (array of id numbers)</param>
        /// <param name="equipmentTypes">Equipment Types (array of id numbers)</param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <response code="200">OK</response>
        public virtual IActionResult OwnersSearchGetAsync(string localAreas, string equipmentTypes, int? owner, string status, bool? hired)
        {
            int?[] localAreasArray = ParseIntArray(localAreas);
            int?[] equipmentTypesArray = ParseIntArray(equipmentTypes);

            // default search results must be limited to user
            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();

            IQueryable<Owner> data = _context.Owners
                    .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId))
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.Attachments)
                    .Include(x => x.Contacts)
                    .Select(x => x);            

            if (localAreasArray != null && localAreasArray.Length > 0)
            {
                data = data.Where(x => localAreasArray.Contains(x.LocalArea.Id));
            }

            if (status != null)
            {
                data = data.Where(x => String.Equals(x.Status, status, StringComparison.CurrentCultureIgnoreCase));
            }

            if (hired == true)
            {
                IQueryable<int?> hiredOwnersQuery = _context.RentalAgreements
                                    .Where(agreement => agreement.Status == "Active")
                                    .Join(
                                        _context.Equipments,
                                        agreement => agreement.EquipmentId,
                                        equipment => equipment.Id,
                                        (agreement, equipment) => new
                                        {
                                            tempAgreement = agreement,
                                            tempEqiupment = equipment
                                        }
                                    )
                                    .Where(projection => projection.tempEqiupment.OwnerId != null)
                                    .Select(projection => projection.tempEqiupment.OwnerId)
                                    .Distinct();

                data = data.Where(o => hiredOwnersQuery.Contains(o.Id));
            }

            if (equipmentTypesArray != null)
            {
                var equipmentTypeQuery = _context.Equipments
                    .Where(x => equipmentTypesArray.Contains(x.DistrictEquipmentTypeId))
                    .Select(x => x.OwnerId)
                    .Distinct();

                data = data.Where(x => equipmentTypeQuery.Contains(x.Id));
            }

            if (owner != null)
            {
                data = data.Where(x => x.Id == owner);
            }

            List<Owner> result = data.ToList();
            return new ObjectResult(new HetsResponse(result));
        }
    }
}
