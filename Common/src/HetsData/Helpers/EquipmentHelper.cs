using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsData.Model;

namespace HetsData.Helpers
{
    public static class EquipmentHelper
    {
        #region Check if the Equipment is currently hired (on an Active Agreement)

        /// <summary>
        /// Check if the Equipment is currently hired (on an Active Agreement)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        public static bool IsHired(int id, DbAppContext context)
        {
            // add an "IsHired" flag to indicate if this equipment is currently in use
            IQueryable<HetRentalAgreement> agreements = context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Equipment)
                .Where(x => x.Status == "Active");

            return agreements.Any(x => x.Equipment.EquipmentId == id);
        }

        #endregion

        #region Get the number of blocks for the equipment type

        /// <summary>
        /// Get the number of blocks for the equipment type
        /// </summary>
        /// <param name="item"></param>
        /// <param name="configuration"></param>
        public static int GetNumberOfBlocks(HetEquipment item, IConfiguration configuration)
        {
            int numberOfBlocks = -1;

            try
            {
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration);

                numberOfBlocks = item.DistrictEquipmentType.EquipmentType.IsDumpTruck ?
                    scoringRules.GetTotalBlocks("DumpTruck") + 1 : scoringRules.GetTotalBlocks() + 1;
            }
            catch
            {
                // do nothing
            }

            return numberOfBlocks;
        }

        #endregion

        #region Get the YTD service hours for a piece of equipment

        /// <summary>
        /// Get the YTD service hours for a piece of equipment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        public static float GetYtdServiceHours(int id, DbAppContext context)
        {
            float result = 0.0F;

            // *******************************************************************************
            // determine current fiscal year - check for existing rotation lists this year
            // *******************************************************************************
            DateTime fiscalStart;

            if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
            {
                fiscalStart = new DateTime(DateTime.UtcNow.AddYears(-1).Year, 4, 1);
            }
            else
            {
                fiscalStart = new DateTime(DateTime.UtcNow.Year, 4, 1);
            }

            // *******************************************************************************
            // get all the time data for the current fiscal year
            // *******************************************************************************
            float? summation = context.HetTimeRecord.AsNoTracking()
                .Include(x => x.RentalAgreement.Equipment)
                .Where(x => x.RentalAgreement.Equipment.EquipmentId == id &&
                            x.WorkedDate >= fiscalStart)
                .Sum(x => x.Hours);

            if (summation != null)
            {
                result = (float)summation;
            }

            return result;
        }

        #endregion


        /*
        #region Clone Equipment Agreements

        /// <summary>
        /// Get rental agreements associated with an equipment id
        /// </summary>
        /// <param name="id">id of Equipment to fetch agreements for</param>
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
                    .Include(x => x.RentalAgreementRates)
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
            IQueryable<LocalArea> localAreas = _context.Equipments
                .Include(x => x.LocalArea)
                .Where(x => x.LocalArea.ServiceArea.District.Region.Id == region)
                .Select(x => x.LocalArea)
                .Distinct();

            foreach (LocalArea localArea in localAreas)
            {
                // get all district equipment types for this region
                IQueryable<DistrictEquipmentType> equipmentTypes = _context.Equipments
                    .Include(x => x.DistrictEquipmentType)
                    .Where(x => x.LocalArea.Id == localArea.Id)
                    .Select(x => x.DistrictEquipmentType)
                    .Distinct();

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

            // per JIRA HETS-536
            item.ApprovedDate = DateTime.UtcNow;

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
                return new ObjectResult(new HetsResponse(GetHistoryRecords(id, offset, limit)));
            }

            // record not found
            return new ObjectResult(new HetsResponse("HETS-01", ErrorViewModel.GetDescription("HETS-01", _configuration)));
        }

        private List<HistoryViewModel> GetHistoryRecords(int id, int? offset, int? limit)
        {
            Equipment equipment = _context.Equipments.AsNoTracking()
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
                result.Add(data[i].ToViewModel());
            }

            return result;
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
            bool exists = _context.Equipments.Any(a => a.Id == id);

            if (exists)
            {
                Equipment equipment = _context.Equipments.AsNoTracking()
                    .First(a => a.Id == id);

                History history = new History
                {
                    Id = 0,
                    HistoryText = item.HistoryText,
                    CreatedDate = item.CreatedDate,
                    EquipmentId = equipment.Id
                };

                _context.Historys.Add(history);
                _context.SaveChanges();
            }

            return new ObjectResult(new HetsResponse(GetHistoryRecords(id, null, null)));
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

        #region Equipment Seniority List

        /// <summary>
        /// Get a pdf version of the seniority list
        /// </summary>
        /// <remarks>Returns a PDF version of the seniority list</remarks>
        /// <param name="localareas">Local Areas (array of id numbers)</param>
        /// <param name="types">Equipment Types (array of id numbers)</param>
        /// <response code="200">OK</response>
        public IActionResult EquipmentSeniorityListPdfGetAsync(string localareas, string types)
        {
            _logger.LogInformation("Equipment Seniority List Pdf");

            int?[] localareasArray = ParseIntArray(localareas);
            int?[] typesArray = ParseIntArray(types);

            int? districtId = _context.GetDistrictIdByUserId(GetCurrentUserId()).Single();

            IQueryable<Equipment> data = _context.Equipments.AsNoTracking()
                .Include(x => x.LocalArea)
                    .ThenInclude(y => y.ServiceArea)
                        .ThenInclude(z => z.District)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(y => y.EquipmentType)
                .Include(x => x.Owner)
                .Include(x => x.RentalAgreements)
                .Where(x => x.LocalArea.ServiceArea.DistrictId.Equals(districtId) &&
                            x.Status == Equipment.StatusApproved)
                .OrderBy(x => x.LocalArea).ThenBy(x => x.DistrictEquipmentType);

            if (localareasArray != null && localareasArray.Length > 0)
            {
                data = data.Where(x => localareasArray.Contains(x.LocalArea.Id));
            }

            if (typesArray != null && typesArray.Length > 0)
            {
                data = data.Where(x => typesArray.Contains(x.DistrictEquipmentType.Id));
            }

            // **********************************************************************
            // determine the year header values
            // * start by getting the current fiscal year
            // **********************************************************************
            // determine end of current fscal year
            DateTime fiscalEnd;

            if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
            {
                fiscalEnd = new DateTime(DateTime.UtcNow.Year, 3, 31);
            }
            else
            {
                fiscalEnd = new DateTime(DateTime.UtcNow.AddYears(1).Year, 3, 31);
            }

            string yearMinus1 = string.Format("{0}/{1}", fiscalEnd.AddYears(-1).Year.ToString(),
                fiscalEnd.Year.ToString().Substring(2, 2));

            string yearMinus2 = string.Format("{0}/{1}", fiscalEnd.AddYears(-2).Year.ToString(),
                fiscalEnd.AddYears(-1).Year.ToString().Substring(2, 2));

            string yearMinus3 = string.Format("{0}/{1}", fiscalEnd.AddYears(-3).Year.ToString(),
                fiscalEnd.AddYears(-2).Year.ToString().Substring(2, 2));

            // **********************************************************************
            // convert Equipment Model to Pdf View Model
            // **********************************************************************
            SeniorityListPdfViewModel seniorityList = new SeniorityListPdfViewModel();
            SeniorityScoringRules scoringRules = new SeniorityScoringRules(_configuration);
            SeniorityListRecord listRecord = new SeniorityListRecord();

            // get last called data
            LocalAreaRotationList rotation = null;

            foreach (Equipment item in data)
            {
                if (listRecord.LocalAreaName != item.LocalArea.Name ||
                    listRecord.DistrictEquipmentTypeName != item.DistrictEquipmentType.DistrictEquipmentName)
                {
                    rotation = _context.LocalAreaRotationLists.AsNoTracking()
                        .Include(x => x.LocalArea)
                        .Include(x => x.DistrictEquipmentType)
                        .FirstOrDefault(x => x.LocalArea.Id == item.LocalArea.Id &&
                                             x.DistrictEquipmentType.Id == item.DistrictEquipmentType.Id);

                    if (!string.IsNullOrEmpty(listRecord.LocalAreaName))
                    {
                        if (seniorityList.SeniorityListRecords == null)
                        {
                            seniorityList.SeniorityListRecords = new List<SeniorityListRecord>();
                        }

                        seniorityList.SeniorityListRecords.Add(listRecord);
                    }

                    listRecord = new SeniorityListRecord
                    {
                        LocalAreaName = item.LocalArea.Name,
                        DistrictEquipmentTypeName = item.DistrictEquipmentType.DistrictEquipmentName,
                        YearMinus1 = yearMinus1,
                        YearMinus2 = yearMinus2,
                        YearMinus3 = yearMinus3,
                        SeniorityList = new List<SeniorityViewModel>()
                    };

                    if (item.LocalArea.ServiceArea != null &&
                        item.LocalArea.ServiceArea.District != null)
                    {
                        listRecord.DistrictName = item.LocalArea.ServiceArea.District.Name;
                    }
                }

                listRecord.SeniorityList.Add(item.ToSeniorityViewModel(scoringRules, rotation, _context));
            }

            // add last record
            if (!string.IsNullOrEmpty(listRecord.LocalAreaName))
            {
                if (seniorityList.SeniorityListRecords == null)
                {
                    seniorityList.SeniorityListRecords = new List<SeniorityListRecord>();
                }

                seniorityList.SeniorityListRecords.Add(listRecord);
            }

            // sort seniority lists
            foreach (SeniorityListRecord list in seniorityList.SeniorityListRecords)
            {
                list.SeniorityList = list.SeniorityList.OrderByDescending(x => x.SenioritySortOrder).ToList();
            }

            // fix the ask next (if no rotation list existed
            foreach (SeniorityListRecord list in seniorityList.SeniorityListRecords)
            {
                bool askNextExists = list.SeniorityList.Exists(x => x.LastCalled == "Y");

                if (!askNextExists && list.SeniorityList.Count > 0)
                {
                    list.SeniorityList[0].LastCalled = "Y";
                }
            }

            // **********************************************************************
            // create the payload and call the pdf service
            // **********************************************************************
            string payload = JsonConvert.SerializeObject(seniorityList, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            });

            _logger.LogInformation("Equipment Seniority List Pdf - Payload Length: {0}", payload.Length);

            // pass the request on to the Pdf Micro Service
            string pdfHost = _configuration["PDF_SERVICE_NAME"];
            string pdfUrl = _configuration.GetSection("Constants:SeniorityListPdfUrl").Value;
            string targetUrl = pdfHost + pdfUrl;

            // generate pdf document name [unique portion only]
            string fileName = "HETS_SeniorityList";

            targetUrl = targetUrl + "/" + fileName;

            _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Service Url: {0}", targetUrl);

            // call the microservice
            try
            {
                HttpClient client = new HttpClient();
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

                _logger.LogInformation("Equipment Seniority List Pdf - Calling HETS Pdf Service");
                HttpResponseMessage response = client.PostAsync(targetUrl, stringContent).Result;

                // success
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Service Response: OK");

                    var pdfResponseBytes = GetPdf(response);

                    // convert to string and log
                    string pdfResponse = Encoding.Default.GetString(pdfResponseBytes);

                    fileName = fileName + ".pdf";

                    _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Filename: {0}", fileName);
                    _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Size: {0}", pdfResponse.Length);

                    // return content
                    FileContentResult pdfResult = new FileContentResult(pdfResponseBytes, "application/pdf")
                    {
                        FileDownloadName = fileName
                    };

                    return pdfResult;
                }

                _logger.LogInformation("Equipment Seniority List Pdf - HETS Pdf Service Response: {0}", response.StatusCode);

                // problem occured
                return new ObjectResult(new HetsResponse("HETS-05", ErrorViewModel.GetDescription("HETS-05", _configuration)));
            }
            catch (Exception ex)
            {
                Debug.Write("Error generating pdf: " + ex.Message);
                return new ObjectResult(new HetsResponse("HETS-05", ErrorViewModel.GetDescription("HETS-05", _configuration)));
            }
        }

        private static byte[] GetPdf(HttpResponseMessage response)
        {
            try
            {
                var pdfResponseBytes = response.Content.ReadAsByteArrayAsync();
                pdfResponseBytes.Wait();

                return pdfResponseBytes.Result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

    */

        
    }
}
