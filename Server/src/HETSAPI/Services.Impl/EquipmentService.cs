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
    /// Equipment Status Class - required to update the status record only
    /// </summary>
    public class EquipmentStatus
    {
        public string Status { get; set; }
        public string StatusComment { get; set; }
    }

    /// <summary>
    /// Equipment Rental Agreement Clone Class - required to clone a previous agreement
    /// </summary>
    public class EquipmentRentalAgreementClone
    {
        public int EquipmentId { get; set; }
        public int AgreementToCloneId { get; set; }
        public int RentalAgreementId { get; set; }
    }

    /// <summary>
    /// Equipment Service
    /// </summary>
    public class EquipmentService : ServiceBase, IEquipmentService
    {
        private readonly DbAppContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Equipment Service Constructor
        /// </summary>
        public EquipmentService(IHttpContextAccessor httpContextAccessor, DbAppContext context, IConfiguration configuration) : base(httpContextAccessor, context)
        {
            _context = context;
            _configuration = configuration;
        }        

        /// <summary>
        /// Create bulk equipment records
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

            // save the changes
            _context.SaveChanges();

            return new NoContentResult();
        }

        /// <summary>
        /// Get all equipment records
        /// </summary>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentGetAsync()
        {
            List<Equipment> result = _context.Equipments.AsNoTracking()
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .ToList();

            return new ObjectResult(new HetsResponse(result));
        }

        
        /// <summary>
        /// Delete equipment record
        /// </summary>
        /// <param name="id">id of Equipment to delete</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentIdDeletePostAsync(int id)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists)
            {
                // remove associated seniority audits
                RemoveSeniorityAudits(id);

                Equipment item = _context.Equipments
                    .Include(x => x.LocalArea)
                    .Include(x => x.DistrictEquipmentType)
                    .Include(x => x.DistrictEquipmentType.EquipmentType)
                    .First(a => a.Id == id);

                int localAreaId = -1;
                int districtEquipmentTypeId = -1;
                int equipmentTypeId = -1;

                if (item.LocalArea != null && item.DistrictEquipmentType != null && item.DistrictEquipmentType.EquipmentType != null)
                {
                    localAreaId = item.LocalArea.Id;
                    districtEquipmentTypeId = item.DistrictEquipmentType.Id;
                    equipmentTypeId = item.DistrictEquipmentType.EquipmentType.Id;
                }

                _context.Equipments.Remove(item);

                // save the changes
                _context.SaveChanges();

                // update the seniority list
                if (localAreaId != -1 && districtEquipmentTypeId != -1 && equipmentTypeId != -1)
                {
                    _context.CalculateSeniorityList(localAreaId, districtEquipmentTypeId, equipmentTypeId, _configuration);
                }

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Get equipment record
        /// </summary>
        /// <param name="id">id of Equipment to fetch</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentIdGetAsync(int id)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists)
            {
                Equipment result = _context.Equipments.AsNoTracking()
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)                    
                    .First(a => a.Id == id);

                result.IsHired = IsHired(id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        // add an "IsHired" flag to indicate if this equipment is currently in use
        private bool IsHired(int id)
        {
            // add an "IsHired" flag to indicate if this equipment is currently in use
            IQueryable<RentalAgreement> agreements = _context.RentalAgreements.AsNoTracking()
                .Include(x => x.Equipment)
                .Where(x => x.Status == "Active");
            
            if (agreements.Any(x => x.Equipment.Id == id))
            { 
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get equipment record by id
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentViewModel for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdViewGetAsync(int id)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists)
            {
                Equipment equipment = _context.Equipments.AsNoTracking()
                    .Include(x => x.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                    .Include(x => x.DumpTruck)
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentAttachments)
                    .Include(x => x.Notes)
                    .Include(x => x.Attachments)
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                EquipmentViewModel result = equipment.ToViewModel();

                result.IsHired = IsHired(id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update equipment record
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentIdPutAsync(int id, Equipment item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                DateTime? originalSeniorityEffectiveDate = _context.GetEquipmentSeniorityEffectiveDate(id);

                bool exists = _context.Equipments.Any(a => a.Id == id);

                // check for duplicates (serial number)
                if (exists &&
                    id == item.Id &&
                    !string.IsNullOrEmpty(item.SerialNumber))
                {
                    bool duplicatesExist = _context.Equipments.Any(x => x.SerialNumber == item.SerialNumber &&
                                                                        x.Id != item.Id &&
                                                                        x.ArchiveCode == "N");
                    if (duplicatesExist)
                    {
                        // duplicate equipment error
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-08", _configuration)));
                    }
                }

                if (exists && id == item.Id)
                {
                    _context.Equipments.Update(item);

                    // save the changes
                    _context.SaveChanges();

                    // if seniority has changed, update blocks
                    if ((originalSeniorityEffectiveDate == null && item.SeniorityEffectiveDate != null) ||
                        (originalSeniorityEffectiveDate != null && item.SeniorityEffectiveDate != null
                            && originalSeniorityEffectiveDate < item.SeniorityEffectiveDate))
                    {
                        EquipmentRecalcSeniority(item.LocalArea.Id, item.DistrictEquipmentType.Id, _context, _configuration);
                    }                                        

                    // retrieve updated equipment record to return to ui
                    Equipment result = _context.Equipments.AsNoTracking()
                        .Include(x => x.LocalArea.ServiceArea.District.Region)
                        .Include(x => x.DistrictEquipmentType)
                            .ThenInclude(d => d.EquipmentType)
                        .Include(x => x.DumpTruck)
                        .Include(x => x.Owner)
                        .Include(x => x.EquipmentAttachments)
                        .Include(x => x.Notes)
                        .Include(x => x.Attachments)
                        .Include(x => x.History)
                        .First(a => a.Id == id);

                    result.IsHired = IsHired(id);

                    return new ObjectResult(new HetsResponse(result));
                }

                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update equipment status
        /// </summary>
        /// <param name="id">id of Equipment to update</param>
        /// <param name="item"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdStatusPutAsync(int id, EquipmentStatus item)
        {
            bool recalcSeniority = false;

            if (item != null)
            {
                bool exists = _context.Equipments.Any(a => a.Id == id);

                if (exists)
                {
                    Equipment equipment = _context.Equipments
                        .Include(x => x.LocalArea.ServiceArea.District.Region)
                        .Include(x => x.DistrictEquipmentType)
                            .ThenInclude(d => d.EquipmentType)
                        .Include(x => x.DumpTruck)
                        .Include(x => x.Owner)
                        .Include(x => x.EquipmentAttachments)
                        .First(a => a.Id == id);                                      

                    // used for seniority recalc
                    int localAreaId = equipment.LocalArea.Id;
                    int districtEquipmentTypeId = equipment.DistrictEquipmentType.Id;
                    string oldStatus = equipment.Status;

                    // update equipment status
                    equipment.Status = item.Status;
                    equipment.StatusComment = item.StatusComment;

                    if (equipment.Status.Equals("Archived", StringComparison.CurrentCultureIgnoreCase))
                    {
                        equipment.ArchiveCode = "Y";
                        equipment.ArchiveDate = DateTime.UtcNow;
                        equipment.ArchiveReason = "Equipment Archived";                        

                        // recalc seniority (move out of the block and adjust)
                        recalcSeniority = true;                        
                    }
                    else
                    {
                        equipment.ArchiveCode = "N";
                        equipment.ArchiveDate = null;
                        equipment.ArchiveReason = null;                        

                        // make sure the seniority is set when shifting to "Active" state
                        // (if this was a new record with no block/seniority yet)
                        if (equipment.BlockNumber == null &&
                            equipment.Seniority == null &&
                            equipment.Status.Equals("Approved", StringComparison.CurrentCultureIgnoreCase))
                        {
                            // set the approved date (only the first time the record is approved in the system)
                            equipment.ApprovedDate = DateTime.UtcNow;

                            // recalc seniority (move into a block)
                            recalcSeniority = true;
                        }
                        else if ((oldStatus.Equals("Approved", StringComparison.CurrentCultureIgnoreCase) &&
                                  !equipment.Status.Equals("Approved", StringComparison.CurrentCultureIgnoreCase)) ||
                                 (!oldStatus.Equals("Approved", StringComparison.CurrentCultureIgnoreCase) &&
                                  equipment.Status.Equals("Approved", StringComparison.CurrentCultureIgnoreCase)))
                        {
                            // recalc seniority (move into or out of a block)
                            recalcSeniority = true;
                        }
                    }   

                    // save the changes
                    _context.SaveChanges();

                    // recalc seniority (if required)
                    if (recalcSeniority)
                    {
                        EquipmentRecalcSeniority(localAreaId, districtEquipmentTypeId, _context, _configuration);
                    }
                    
                    equipment.IsHired = IsHired(id);

                    return new ObjectResult(new HetsResponse(equipment));
                }

                // record not found
                return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Create equipment record
        /// </summary>
        /// <param name="item"></param>
        /// <response code="201">Equipment created</response>
        public virtual IActionResult EquipmentPostAsync(Equipment item)
        {
            if (item != null)
            {
                AdjustRecord(item);

                bool exists = _context.Equipments.Any(a => a.Id == item.Id);

                // check for duplicates (serial number)
                if (exists &&
                    !string.IsNullOrEmpty(item.SerialNumber))
                {
                    bool duplicatesExist = _context.Equipments.Any(x => x.SerialNumber == item.SerialNumber &&
                                                                        x.Id != item.Id &&
                                                                        x.ArchiveCode == "N");
                    if (duplicatesExist)
                    {
                        // duplicate equipment error
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-08", _configuration)));
                    }
                }

                if (exists)
                {
                    _context.Equipments.Update(item);
                    _context.SaveChanges();
                }
                else
                {
                    // certain fields are set on new record - set defaults (including status = "Inactive"
                    SetNewRecordFields(item);

                    _context.Equipments.Add(item);
                    _context.SaveChanges();

                    // add the equipment to the Owner's equipment list
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

                    // save changes
                    _context.SaveChanges();                    
                }
                
                item.IsHired = IsHired(item.Id);

                return new ObjectResult(new HetsResponse(item));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }
        
        /// <summary>
        /// Searches Equipment
        /// </summary>
        /// <remarks>Used for the equipment search page.</remarks>
        /// <param name="localareas">Local Areas (array of id numbers)</param>
        /// <param name="types">Equipment Types (array of id numbers)</param>
        /// <param name="equipmentAttachment">Equipment Attachments </param>
        /// <param name="owner"></param>
        /// <param name="status">Status</param>
        /// <param name="hired">Hired</param>
        /// <param name="notverifiedsincedate">Not Verified Since Date</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentSearchGetAsync(string localareas, string types, string equipmentAttachment, 
            int? owner, string status, bool? hired, DateTime? notverifiedsincedate)
        {
            int?[] localareasArray = ParseIntArray(localareas);
            int?[] typesArray = ParseIntArray(types);

            // **********************************************************************
            // get initial resultset - results must be limited to user's dsitrict
            // **********************************************************************
            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();

            IQueryable<Equipment> data = _context.Equipments.AsNoTracking()
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId))
                .Include(x => x.LocalArea)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(y => y.EquipmentType)
                .Include(x => x.Owner)
                .Include(x => x.EquipmentAttachments)                
                .Select(x => x);

            // **********************************************************************
            // filter results based on search critera
            // **********************************************************************
            if (localareasArray != null && localareasArray.Length > 0)
            {
                data = data.Where(x => localareasArray.Contains(x.LocalArea.Id));
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
                data = data.Where(x => String.Equals(x.Status, status, StringComparison.CurrentCultureIgnoreCase));
            }

            // is the equipment is hired (search criteria)
            if (hired == true)
            {
                IQueryable<int?> hiredEquipmentQuery = _context.RentalAgreements
                                    .Where(agreement => agreement.Status == "Active")
                                    .Select(agreement => agreement.EquipmentId)
                                    .Distinct();

                data = data.Where(e => hiredEquipmentQuery.Contains(e.Id));
            }

            if (typesArray != null && typesArray.Length > 0)
            {
                data = data.Where(x => typesArray.Contains(x.DistrictEquipmentType.Id));
            }

            if (notverifiedsincedate != null)
            {
                data = data.Where(x => x.LastVerifiedDate >= notverifiedsincedate);
            }
            
            // **********************************************************************
            // convert Equipment Model to View Model
            // **********************************************************************
            List<EquipmentViewModel> result = new List<EquipmentViewModel>();

            foreach (Equipment item in data)
            {
                EquipmentViewModel newItem = item.ToViewModel();

                newItem.NumberOfBlocks = GetNumberOfBlocks(item) + 1;
                result.Add(newItem);
            }            

            // return to the client            
            return new ObjectResult(new HetsResponse(result));
        }

        /// <summary>
        /// Get the number of blocks for this type of equipment 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int GetNumberOfBlocks(Equipment item)
        {
            int numberOfBlocks = -1;

            try
            {            
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);

                numberOfBlocks = item.DistrictEquipmentType.EquipmentType.IsDumpTruck ?
                    scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();
            }
            catch
            {
                // do nothing
            }

            return numberOfBlocks;
        }

        # region Clone Equipment Agreements

        /// <summary>
        /// Get renatal agreements associated with an equipment id
        /// </summary>
        /// <param name="id">id of Equipment to fetch agreements for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdGetAgreementsAsync(int id)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists)
            {
                List<RentalAgreement> result = _context.RentalAgreements.AsNoTracking()
                    .Include(x => x.Equipment)
                        .ThenInclude(d => d.DistrictEquipmentType)
                    .Include(e => e.Equipment)
                        .ThenInclude(a => a.EquipmentAttachments)
                    .Include(e => e.Project)
                    .Where(x => x.EquipmentId == id)
                    .ToList();

                // remove all of the additional agreements being returned
                foreach (RentalAgreement agreement in result)
                {
                    agreement.Project.RentalAgreements = null;
                }

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update a rental agreement by cloning a previous equipment's rental agreement
        /// </summary>
        /// <param name="id">Equipment id</param>
        /// <param name="item"></param>
        /// <response code="200">Rental Agreement updated</response>
        public virtual IActionResult EquipmentRentalAgreementClonePostAsync(int id, EquipmentRentalAgreementClone item)
        {
            if (item != null && id == item.EquipmentId)
            {
                bool exists = _context.Equipments.Any(a => a.Id == id);

                if (!exists)
                {
                    // record not found - equipment
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                List<RentalAgreement> agreements = _context.RentalAgreements
                    .Include(x => x.Equipment)
                        .ThenInclude(d => d.DistrictEquipmentType)
                    .Include(e => e.Equipment)
                        .ThenInclude(a => a.EquipmentAttachments)
                    .Include(x =>x .RentalAgreementRates)
                    .Include(x => x.RentalAgreementConditions)
                    .Include(x => x.TimeRecords)
                    .Where(x => x.EquipmentId == id)
                    .ToList();                

                // check that the rental agreements exist
                exists = agreements.Any(a => a.Id == item.RentalAgreementId);

                if (!exists)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                }

                // check that the rental agreement to clone exist
                exists = agreements.Any(a => a.Id == item.AgreementToCloneId);

                if (!exists)
                {
                    // (RENTAL AGREEMENT) record not found
                    return new ObjectResult(new HetsResponse("HETS-11", ErrorViewModel.GetDescription("HETS-11", _configuration)));
                }

                int agreementToCloneIndex = agreements.FindIndex(a => a.Id == item.AgreementToCloneId);
                int newRentalagreementIndex = agreements.FindIndex(a => a.Id == item.RentalAgreementId);

                // ******************************************************************
                // Business Rules in the backend:
                // *Can't clone into an Agreement if it isn't Active
                // *Can't clone into an Agreement if it has existing time records
                // ******************************************************************
                if (!agreements[newRentalagreementIndex].Status
                    .Equals("Active", StringComparison.InvariantCultureIgnoreCase))
                {
                    // (RENTAL AGREEMENT) is not active
                    return new ObjectResult(new HetsResponse("HETS-12", ErrorViewModel.GetDescription("HETS-12", _configuration)));
                }

                if (agreements[newRentalagreementIndex].TimeRecords != null &&
                    agreements[newRentalagreementIndex].TimeRecords.Count > 0)
                {
                    // (RENTAL AGREEMENT) has tme records
                    return new ObjectResult(new HetsResponse("HETS-13", ErrorViewModel.GetDescription("HETS-13", _configuration)));
                }

                // ******************************************************************
                // clone agreement
                // ******************************************************************
                // update agreement attributes                
                agreements[newRentalagreementIndex].EquipmentRate =
                    agreements[agreementToCloneIndex].EquipmentRate;

                agreements[newRentalagreementIndex].Note =
                    agreements[agreementToCloneIndex].Note;

                agreements[newRentalagreementIndex].RateComment =
                    agreements[agreementToCloneIndex].RateComment;

                agreements[newRentalagreementIndex].RatePeriod =
                    agreements[agreementToCloneIndex].RatePeriod;

                // setup new agreement number
                string agreementNumber = RentalAgreementService.GetRentalAgreementNumber(agreements[newRentalagreementIndex], _context);
                agreements[newRentalagreementIndex].Number = agreementNumber;

                // update rates
                agreements[newRentalagreementIndex].RentalAgreementRates = null;

                foreach (RentalAgreementRate rate in agreements[agreementToCloneIndex].RentalAgreementRates)
                {
                    RentalAgreementRate temp = new RentalAgreementRate
                    {
                        Comment = rate.Comment,
                        ComponentName = rate.ComponentName,
                        Rate = rate.Rate,
                        RatePeriod = rate.RatePeriod,
                        IsIncludedInTotal = rate.IsIncludedInTotal,
                        IsAttachment = rate.IsAttachment,
                        PercentOfEquipmentRate = rate.PercentOfEquipmentRate
                    };

                    if (agreements[newRentalagreementIndex].RentalAgreementRates == null)
                    {
                        agreements[newRentalagreementIndex].RentalAgreementRates =
                            new List<RentalAgreementRate>();
                    }

                    agreements[newRentalagreementIndex].RentalAgreementRates.Add(temp);
                }

                // update conditions
                agreements[newRentalagreementIndex].RentalAgreementConditions = null;

                foreach (RentalAgreementCondition condition in agreements[agreementToCloneIndex].RentalAgreementConditions)
                {
                    RentalAgreementCondition temp = new RentalAgreementCondition
                    {
                        Comment = condition.Comment,
                        ConditionName = condition.ConditionName
                    };

                    if (agreements[newRentalagreementIndex].RentalAgreementConditions == null)
                    {
                        agreements[newRentalagreementIndex].RentalAgreementConditions =
                            new List<RentalAgreementCondition>();
                    }

                    agreements[newRentalagreementIndex].RentalAgreementConditions.Add(temp);
                }

                // save the changes
                _context.SaveChanges();

                // ******************************************************************
                // return updated rental agreement to update the screen
                // ******************************************************************
                RentalAgreement result = _context.RentalAgreements.AsNoTracking()
                    .Include(x => x.Equipment)
                        .ThenInclude(y => y.Owner)
                    .Include(x => x.Equipment)
                        .ThenInclude(y => y.DistrictEquipmentType)
                            .ThenInclude(d => d.EquipmentType)
                    .Include(x => x.Equipment)
                        .ThenInclude(y => y.EquipmentAttachments)
                    .Include(x => x.Equipment)
                        .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                    .Include(x => x.Project)
                        .ThenInclude(p => p.District.Region)
                    .Include(x => x.RentalAgreementConditions)
                    .Include(x => x.RentalAgreementRates)
                    .Include(x => x.TimeRecords)
                    .First(a => a.Id == item.RentalAgreementId);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Duplicate Equiment

        /// <summary>
        /// Get all duplicate equipment records
        /// </summary>
        /// <param name="id">id of Equipment</param>
        /// <param name="serialNumber">Serial Number of Equipment</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdEquipmentduplicatesGetAsync(int id, string serialNumber)
        {
            bool exists = _context.Equipments.Any(x => x.Id == id);

            // id = 0 -> need to allow for new records too
            if (exists || id == 0)
            {
                List<Equipment> result = _context.Equipments.AsNoTracking()
                    .Include(x => x.LocalArea.ServiceArea.District)
                    .Include(x => x.Owner)
                    .Where(x => x.SerialNumber == serialNumber &&
                                x.Id != id &&
                                x.ArchiveCode == "N").ToList();

                List<DuplicateEquipmentViewModel> duplicates = new List<DuplicateEquipmentViewModel>();
                int idCount = -1;

                foreach (Equipment equipment in result)
                {
                    idCount++;

                    DuplicateEquipmentViewModel duplicate = new DuplicateEquipmentViewModel
                    {
                        Id = idCount,
                        SerialNumber = serialNumber,                        
                        DuplicateEquipment = equipment,
                        DistrictName = ""
                    };

                    if (equipment.LocalArea.ServiceArea.District != null &&
                        !string.IsNullOrEmpty(equipment.LocalArea.ServiceArea.District.Name))
                    {
                        duplicate.DistrictName = equipment.LocalArea.ServiceArea.District.Name;
                    }

                    duplicates.Add(duplicate);
                }

                // return to the client            
                return new ObjectResult(new HetsResponse(duplicates));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Recalculate Seniority

        /// <summary>
        /// Recalculates seniority for a specific local area and equipment type
        /// </summary>
        public static void EquipmentRecalcSeniority(int localAreaId, int districtEquipmentTypeId, DbAppContext context, IConfiguration configuration)
        {
            // check if the local area and equipment are valid
            bool exists = context.LocalAreas.Any(a => a.Id == localAreaId);

            if (!exists)
            {
                throw new ArgumentException("Local Area is invalid");
            }

            exists = context.DistrictEquipmentTypes.Any(a => a.Id == districtEquipmentTypeId);

            if (!exists)
            {
                throw new ArgumentException("District Equipment Type is invalid");
            }

            // get the local area
            LocalArea localArea = context.LocalAreas
                .First(x => x.Id == localAreaId);

            // get the equipment
            DistrictEquipmentType districtEquipmentType = context.DistrictEquipmentTypes
                .Include(x => x.EquipmentType)
                .First(x => x.Id == districtEquipmentTypeId);

            // recalc the seniority list
            context.CalculateSeniorityList(localArea.Id, districtEquipmentType.Id, districtEquipmentType.EquipmentType.Id, configuration);             
        }

        /// <summary>
        /// Recalculates seniority for an entire region
        /// </summary>
        /// <remarks>Used to calculate seniority for all database records</remarks>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentRecalcSeniorityGetAsync(int region)
        {
            // check if the region is valid
            bool exists = _context.Regions.Any(a => a.Id == region);

            // region not found
            if (!exists)
            {
                return new ObjectResult(new HetsResponse("HETS-03", ErrorViewModel.GetDescription("HETS-03", _configuration)));
            }

            // get all local areas for this region
            List<LocalArea> localAreas = _context.Equipments
                .Include(x => x.LocalArea)
                .Where(x => x.LocalArea.ServiceArea.District.Region.Id == region)
                .Select(x => x.LocalArea)
                .Distinct()
                .ToList();

            // get all district equipment types for this region
            List<DistrictEquipmentType> equipmentTypes = _context.Equipments                
                .Include(x => x.DistrictEquipmentType)
                .Where(x => x.LocalArea.ServiceArea.District.Region.Id == region)              
                .Select(x => x.DistrictEquipmentType)
                .Distinct()            
                .ToList();

            foreach (DistrictEquipmentType equipment in equipmentTypes)
            {
                _context.Entry(equipment).Reference(x => x.EquipmentType).Load();
            }            

            foreach (LocalArea localArea in localAreas)
            {
                foreach (DistrictEquipmentType districtEquipmentType in equipmentTypes)
                {
                    _context.CalculateSeniorityList(localArea.Id, districtEquipmentType.Id, districtEquipmentType.EquipmentType.Id, _configuration);
                }
            }

            return new ObjectResult("Done Recalc");
        }

        #endregion

        #region Functions to setup/fix the Equipment Record (cleanup record submitted by UI for update/insert)

        /// <summary>
        /// Cleanup new equipment record for insert
        /// </summary>
        /// <param name="item"></param>
        private void AdjustRecord(Equipment item)
        {
            if (item != null)
            {
                // adjust the record to allow it to be updated / inserted
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
        /// Generate an equipment code
        /// </summary>
        /// <param name="ownerEquipmentCodePrefix"></param>
        /// <param name="equipmentNumber"></param>
        /// <returns></returns>
        private string GenerateEquipmentCode(string ownerEquipmentCodePrefix, int equipmentNumber)
        {
            string result = ownerEquipmentCodePrefix + "-" + equipmentNumber.ToString("D4");
            return result;
        }

        /// <summary>
        /// Set the Equipment fields for a new record for fields that are not provided by the front end
        /// </summary>
        /// <param name="item"></param>
        private void SetNewRecordFields(Equipment item)
        {
            item.ReceivedDate = DateTime.UtcNow;
            item.LastVerifiedDate = DateTime.UtcNow;            
            
            item.Seniority = 0.0F;
            item.YearsOfService = 0.0F;
            item.ServiceHoursLastYear = 0.0F;
            item.ServiceHoursTwoYearsAgo = 0.0F;
            item.ServiceHoursThreeYearsAgo = 0.0F;
            item.ArchiveCode = "N";
            item.IsSeniorityOverridden = false;

            // new equipment MUST always start as unaproved - it isn't assigned to any block yet
            item.Status = "Unapproved";

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
                        string candidate = GenerateEquipmentCode(item.Owner.OwnerCode, equipmentNumber);
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
                item.EquipmentCode = GenerateEquipmentCode(item.Owner.OwnerCode, equipmentNumber);
            }
        }

        #endregion

        #region Equipment Audit

        /// <summary>
        /// Delete seniority audit records
        /// </summary>
        /// <param name="equipmentId"></param>
        private void RemoveSeniorityAudits(int equipmentId)
        {
            List<SeniorityAudit> seniorityAudits = _context.SeniorityAudits
                .Include(x => x.Equipment)
                .Where(x => x.Equipment.Id == equipmentId)
                .ToList();

            if (seniorityAudits.Count > 0)
            {
                foreach (SeniorityAudit seniorityAudit in seniorityAudits)
                {
                    _context.SeniorityAudits.Remove(seniorityAudit);
                }
            }

            _context.SaveChanges();
        }

        #endregion

        #region Equipment History

        /// <summary>
        /// Get history associated with an equipment record
        /// </summary>
        /// <remarks>Returns History for a particular Equipment</remarks>
        /// <param name="id">id of SchoolBus to fetch History for</param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdHistoryGetAsync(int id, int? offset, int? limit)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists)
            {
                Equipment equipment = _context.Equipments
                    .Include(x => x.History)
                    .First(a => a.Id == id);

                List<History> data = equipment.History.OrderByDescending(y => y.AppLastUpdateTimestamp).ToList();

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
        /// Create history record associated with equipment
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
            result.LastUpdateTimestamp = item.AppLastUpdateTimestamp;
            result.LastUpdateUserid = item.AppLastUpdateUserid;
            result.AffectedEntityId = id;

            return new ObjectResult(new HetsResponse(result));
        }

        #endregion

        #region Equipment Attachments

        /// <summary>
        /// Get "equipment attachments" associated with an equipment record
        /// </summary>
        /// <param name="id">id of Equipment to fetch EquipmentAttachments for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdEquipmentattachmentsGetAsync(int id)
        {
            bool exists = _context.Equipments.Any(x => x.Id == id);

            if (exists)
            {
                IQueryable<EquipmentAttachment> result = _context.EquipmentAttachments
                    .Include(x => x.Equipment)
                    .Where(x => x.Equipment.Id == id);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion

        #region Attachments

        /// <summary>
        /// Get attachments associated with an equipment record
        /// </summary>
        /// <remarks>Returns attachments for a particular Equipment</remarks>
        /// <param name="id">id of Equipment to fetch attachments for</param>
        /// <response code="200">OK</response>
        /// <response code="404">Equipment not found</response>
        public virtual IActionResult EquipmentIdAttachmentsGetAsync(int id)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists)
            {
                Equipment equipment = _context.Equipments
                    .Include(x => x.Attachments)
                    .First(a => a.Id == id);

                List<AttachmentViewModel> result = MappingExtensions.GetAttachmentListAsViewModel(equipment.Attachments);

                return new ObjectResult(new HetsResponse(result));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }


        #endregion

        #region Equipment Note Records

        /// <summary>
        /// Get note records associated with equipment
        /// </summary>
        /// <param name="id">id of Equipment to fetch Notes for</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdNotesGetAsync(int id)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists)
            {
                Equipment equipment = _context.Equipments.AsNoTracking()
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);

                List<Note> notes = new List<Note>();

                foreach (Note note in equipment.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create a note associated with a equipment
        /// </summary>
        /// <remarks>Update a Equipment&#39;s Notes</remarks>
        /// <param name="id">id of Equipment to update Notes for</param>
        /// <param name="item">Equipment Note</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdNotesPostAsync(int id, Note item)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists && item != null)
            {
                Equipment equipment = _context.Equipments
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);

                // ******************************************************************
                // add or update note
                // ******************************************************************
                if (item.Id > 0)
                {
                    int noteIndex = equipment.Notes.FindIndex(a => a.Id == item.Id);

                    if (noteIndex < 0)
                    {
                        // record not found
                        return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                    }

                    equipment.Notes[noteIndex].Text = item.Text;
                    equipment.Notes[noteIndex].IsNoLongerRelevant = item.IsNoLongerRelevant;
                }
                else  // add note
                {
                    equipment.Notes.Add(item);
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated time records
                // *************************************************************              
                List<Note> notes = new List<Note>();

                foreach (Note note in equipment.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        /// <summary>
        /// Update or create an array of notes associated with a equipment
        /// </summary>
        /// <remarks>Update a Equipment&#39;s Notes</remarks>
        /// <param name="id">id of Equipment to update Notes for</param>
        /// <param name="items">Array of Equipment Notes</param>
        /// <response code="200">OK</response>
        public virtual IActionResult EquipmentIdNotesBulkPostAsync(int id, Note[] items)
        {
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists && items != null)
            {
                Equipment equipment = _context.Equipments
                    .Include(x => x.Notes)
                    .First(x => x.Id == id);

                // process each note
                foreach (Note item in items)
                {
                    // ******************************************************************
                    // add or update note
                    // ******************************************************************
                    if (item.Id > 0)
                    {
                        int noteIndex = equipment.Notes.FindIndex(a => a.Id == item.Id);

                        if (noteIndex < 0)
                        {
                            // record not found
                            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
                        }

                        equipment.Notes[noteIndex].Text = item.Text;
                        equipment.Notes[noteIndex].IsNoLongerRelevant = item.IsNoLongerRelevant;
                    }
                    else  // add note
                    {
                        equipment.Notes.Add(item);
                    }

                    _context.SaveChanges();
                }

                _context.SaveChanges();

                // *************************************************************
                // return updated notes                
                // *************************************************************
                List<Note> notes = new List<Note>();

                foreach (Note note in equipment.Notes)
                {
                    if (note.IsNoLongerRelevant == false)
                    {
                        notes.Add(note);
                    }
                }

                return new ObjectResult(new HetsResponse(notes));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        #endregion
    }
}
