using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HetsData.Entities;

namespace HetsData.Helpers
{
    #region Seniority List Models

    public class RuleType
    {
        public int Default { get; set; }
        public int DumpTruck { get; set; }
    }

    public class ScoringRules
    {
        public RuleType EquipmentScore { get; set; }
        public RuleType BlockSize { get; set; }
        public RuleType TotalBlocks { get; set; }
    }

    public class SeniorityViewModel
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public int? OwnerId { get; set; }
        public string Block { get; set; }
        public string Seniority { get; set; }
        public string IsHired { get; set; }
        public string YearMakeModelSize { get; set; }
        public string EquipmentCode { get; set; }
        public string LastCalled { get; set; }
        public string YearsRegistered { get; set; }
        public string YtdHours { get; set; }
        public string HoursYearMinus1 { get; set; }
        public string HoursYearMinus2 { get; set; }
        public string HoursYearMinus3 { get; set; }
        public int SenioritySortOrder { get; set; }
    }

    public class SeniorityListRecord
    {
        public string DistrictName { get; set; }
        public string LocalAreaName { get; set; }
        public string DistrictEquipmentTypeName { get; set; }
        public string YearMinus1 { get; set; }
        public string YearMinus2 { get; set; }
        public string YearMinus3 { get; set; }
        public List<SeniorityViewModel> SeniorityList { get; set; }
    }

    public class SeniorityListReportViewModel
    {
        public string Classification { get; set; }
        public string GeneratedOn { get; set; }
        public string SeniorityListType { get; set; }

        public List<SeniorityListRecord> SeniorityListRecords { get; set; }

        public SeniorityListReportViewModel()
        {
            SeniorityListRecords = new List<SeniorityListRecord>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    #endregion

    /// <summary>
    /// Seniority List Helper
    /// </summary>
    public static class SeniorityListHelper
    {
        #region Manage the Seniority List for a Specific Location

        /// <summary>
        /// Calculate the Seniority List
        /// </summary>
        /// <param name="localAreaId"></param>
        /// <param name="districtEquipmentTypeId"></param>
        /// <param name="context"></param>
        /// <param name="seniorityScoringRules"></param>
        public static void CalculateSeniorityList(int localAreaId, int districtEquipmentTypeId,
            DbAppContext context, string seniorityScoringRules, HetEquipment changedEquipment = null)
        {
            try
            {
                // validate data
                if (context != null &&
                    context.HetLocalAreas.Any(x => x.LocalAreaId == localAreaId) &&
                    context.HetDistrictEquipmentTypes.Any(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId))
                {
                    // get processing rules
                    SeniorityScoringRules scoringRules = new SeniorityScoringRules(seniorityScoringRules);

                    // get the associated equipment type
                    HetDistrictEquipmentType districtEquipmentTypeRecord = context.HetDistrictEquipmentTypes
                        .First(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId);

                    HetEquipmentType equipmentTypeRecord = context.HetEquipmentTypes
                        .FirstOrDefault(x => x.EquipmentTypeId == districtEquipmentTypeRecord.EquipmentTypeId);

                    if (equipmentTypeRecord != null)
                    {
                        // get rules
                        int seniorityScoring = equipmentTypeRecord.IsDumpTruck
                            ? scoringRules.GetEquipmentScore("DumpTruck")
                            : scoringRules.GetEquipmentScore();
                        int blockSize = equipmentTypeRecord.IsDumpTruck
                            ? scoringRules.GetBlockSize("DumpTruck")
                            : scoringRules.GetBlockSize();
                        int totalBlocks = equipmentTypeRecord.IsDumpTruck
                            ? scoringRules.GetTotalBlocks("DumpTruck")
                            : scoringRules.GetTotalBlocks();

                        // get all equipment records
                        IQueryable<HetEquipment> data = context.HetEquipments
                            .Where(x => x.LocalAreaId == localAreaId &&
                                        x.DistrictEquipmentTypeId == districtEquipmentTypeId);

                        // get status id
                        int? eqStatusId =
                            StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", context);
                        if (eqStatusId == null)
                        {
                            throw new ArgumentException("Status Code not found");
                        }

                        // update the seniority score
                        foreach (HetEquipment equipment in data)
                        {
                            if (equipment.EquipmentStatusTypeId != eqStatusId)
                            {
                                equipment.SeniorityEffectiveDate = DateTime.UtcNow;
                                equipment.BlockNumber = null;
                                equipment.Seniority = null;
                                equipment.NumberInBlock = null;
                            }
                            else
                            {
                                equipment.CalculateSeniority(seniorityScoring);
                                equipment.SeniorityEffectiveDate = DateTime.UtcNow;
                            }
                        }

                        // put equipment into the correct blocks
                        AssignBlocks(localAreaId, districtEquipmentTypeId, blockSize, totalBlocks, context, changedEquipment);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: CalculateSeniorityList");
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Assign blocks for the given local area and equipment type
        /// </summary>
        /// <param name="localAreaId"></param>
        /// <param name="districtEquipmentTypeId"></param>
        /// <param name="blockSize"></param>
        /// <param name="totalBlocks"></param>
        /// <param name="context"></param>
        /// <param name="changedEquipment">
        ///  Pass it when an equipment has changed locally (in memory) while EF is tracking the change.
        ///  For example, when district equipment type of an equipment has been changed locally and hasn't bee saved to DB,
        ///  The first query result (dataFromDb) won't have the locally changed equipment, so the equipment should be added to the result.
        ///  If local changes are not about the scope of seniority calculation i.e. equipment status type, local area and district equipment type,
        ///  you don't need to pass changedEquipment.
        ///  This fix is not ideal but given the time constraint, it works. The fix is done for HETS-1341
        /// </param>
        /// <param name="saveChanges"></param>
        public static void AssignBlocks(int localAreaId, int districtEquipmentTypeId, int blockSize, int totalBlocks, DbAppContext context, HetEquipment changedEquipment = null)
        {
            try
            {
                // get all equipment records
                var dataFromDb = context.HetEquipments
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentStatusType)
                    .Where(x => x.EquipmentStatusType.EquipmentStatusTypeCode == HetEquipment.StatusApproved &&
                                x.LocalAreaId == localAreaId &&
                                x.DistrictEquipmentTypeId == districtEquipmentTypeId)
                    .ToList();

                if (changedEquipment != null && !dataFromDb.Any(x => x.EquipmentId == changedEquipment.EquipmentId))
                {
                    var dataToAdd = context.HetEquipments
                    .Include(x => x.Owner)
                    .Include(x => x.EquipmentStatusType)
                    .FirstOrDefault(x => x.EquipmentId == changedEquipment.EquipmentId);

                    if (dataToAdd != null)
                        dataFromDb.Add(dataToAdd); 
                }

                //reselect with data that include local changes
                var dataToProcess = dataFromDb
                    .Where(x => x.EquipmentStatusType.EquipmentStatusTypeCode == HetEquipment.StatusApproved &&
                                x.LocalAreaId == localAreaId &&
                                x.DistrictEquipmentTypeId == districtEquipmentTypeId)
                    .OrderByDescending(x => x.Seniority)
                    .ThenBy(x => x.ReceivedDate)
                    .ThenBy(x => x.EquipmentCode)
                    .ToList();

                // total blocks only counts the "main" blocks - we need to add 1 more for the remaining records
                totalBlocks = totalBlocks + 1;

                // instantiate lists to hold equipment by block
                List<int>[] blocks = new List<int>[totalBlocks];

                foreach (HetEquipment equipment in dataToProcess)
                {
                    // iterate the blocks and add the record
                    for (int i = 0; i < totalBlocks; i++)
                    {
                        if (AddedToBlock(i, totalBlocks, blockSize, blocks, equipment))
                        {
                            break; // move to next record
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: AssignBlocks");
                Console.WriteLine(e);
                throw;
            }
        }

        private static bool AddedToBlock(int currentBlock, int totalBlocks, int blockSize, List<int>[] blocks, HetEquipment equipment)
        {
            try
            {
                // check if this record's Owner is null
                if (equipment.Owner == null)
                {
                    return false; // not adding this record to the block
                }

                if (blocks[currentBlock] == null)
                {
                    blocks[currentBlock] = new List<int>();
                }

                // check if the current block is full
                if (currentBlock < (totalBlocks - 1) && blocks[currentBlock].Count >= blockSize)
                {
                    return false; // not adding this record to the block
                }

                // check if this record's Owner already exists in the block
                if (currentBlock < (totalBlocks - 1) && blocks[currentBlock].Contains(equipment.Owner.OwnerId))
                {
                    return false; // not adding this record to the block
                }

                // (HETS-877) check if this is a maintenance contractor - can only be in the last block
                if (currentBlock < (totalBlocks - 1) &&
                    equipment.Owner.IsMaintenanceContractor != null &&
                    equipment.Owner.IsMaintenanceContractor == true)
                {
                    return false; // not adding this record to the block
                }

                // add record to the block
                blocks[currentBlock].Add(equipment.Owner.OwnerId);

                // update the equipment record
                equipment.BlockNumber = currentBlock + 1;
                equipment.NumberInBlock = blocks[currentBlock].Count;

                // record added to the block
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: AddedToBlock");
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Convert full equipment model record to a "Lite" seniority version

        /// <summary>
        /// Seniority List view model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="scoringRules"></param>
        /// <param name="rotationList"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SeniorityViewModel ToSeniorityViewModel(HetEquipment model, SeniorityScoringRules scoringRules,
            int lastCalledEquipmentId, DbAppContext context)
        {
            SeniorityViewModel seniorityViewModel = new SeniorityViewModel();

            if (model == null) return seniorityViewModel;

            int numberOfBlocks = 0;

            // get number of blocks for this equipment type
            if (model.DistrictEquipmentType != null)
            {
                numberOfBlocks = model.DistrictEquipmentType.EquipmentType.IsDumpTruck
                    ? scoringRules.GetTotalBlocks("DumpTruck") + 1
                    : scoringRules.GetTotalBlocks() + 1;
            }

            // get equipment block number
            int blockNumber = 0;
            if (model.BlockNumber != null)
            {
                blockNumber = (int)model.BlockNumber;
            }

            // get equipment block number
            int numberInBlock = 0;
            if (model.NumberInBlock != null)
            {
                numberInBlock = (int)model.NumberInBlock;
            }

            // *************************************************************
            // check if this record/owner was called last
            // *************************************************************
            bool callLast = lastCalledEquipmentId == model.EquipmentId;
            seniorityViewModel.LastCalled = callLast ? "Y" : " ";

            // *************************************************************
            // map data to view model
            // *************************************************************
            seniorityViewModel.Id = model.EquipmentId;

            if (model.Owner != null)
            {
                seniorityViewModel.OwnerName = model.Owner.OrganizationName;
                seniorityViewModel.OwnerId = model.OwnerId;
            }

            // replacing Open with 3 (HETS-968 Rotation list -Wrong Block number for Open block)
            seniorityViewModel.Block = blockNumber == numberOfBlocks ? "3" : blockNumber.ToString();

            // format the seniority value
            seniorityViewModel.Seniority = $"{model.Seniority:0.###}";

            // format year / make / model / size
            seniorityViewModel.YearMakeModelSize = $"{model.Year}/{model.Make}/{model.Model}/{model.Size}";

            seniorityViewModel.EquipmentCode = model.EquipmentCode;

            seniorityViewModel.YearsRegistered = model.YearsOfService.ToString();

            // Determine if this equipment is currently hired
            seniorityViewModel.IsHired = EquipmentHelper.IsHired(model.EquipmentId, context) ? "Y" : "N";

            // calculate and format the ytd hours
            float tempHours = EquipmentHelper.GetYtdServiceHours(model.EquipmentId, context);
            seniorityViewModel.YtdHours = $"{tempHours:0.###}";

            // format the hours
            seniorityViewModel.HoursYearMinus1 = $"{model.ServiceHoursLastYear:0.###}";
            seniorityViewModel.HoursYearMinus2 = $"{model.ServiceHoursTwoYearsAgo:0.###}";
            seniorityViewModel.HoursYearMinus3 = $"{model.ServiceHoursThreeYearsAgo:0.###}";

            // add the correct sorting order (numeric)
            seniorityViewModel.SenioritySortOrder = EquipmentHelper.CalculateSenioritySortOrder(blockNumber, numberInBlock);

            return seniorityViewModel;
        }

        #endregion

        public static SeniorityViewModel ToSeniorityViewModel(HetRentalRequestSeniorityList equipment, int numberOfBlocks)
        {
            var seniorityViewModel = new SeniorityViewModel();

            if (equipment == null) return seniorityViewModel;

            var blockNumber = (int)equipment.BlockNumber;
            var numberInBlock = (int)equipment.NumberInBlock;

            // *************************************************************
            // map data to view model
            // *************************************************************
            seniorityViewModel.Id = equipment.EquipmentId;
            seniorityViewModel.OwnerName = equipment.Owner?.OrganizationName;
            seniorityViewModel.OwnerId = equipment.OwnerId;
            seniorityViewModel.Seniority = $"{equipment.Seniority:0.###}";
            seniorityViewModel.YearMakeModelSize = $"{equipment.Year}/{equipment.Make}/{equipment.Model}/{equipment.Size}";
            seniorityViewModel.EquipmentCode = equipment.EquipmentCode;
            seniorityViewModel.YearsRegistered = equipment.YearsOfService.ToString();
            seniorityViewModel.IsHired = equipment.WorkingNow ? "Y" : "N";
            seniorityViewModel.LastCalled = equipment.LastCalled ? "Y" : " ";
            seniorityViewModel.YtdHours = $"{equipment.YtdHours:0.###}";

            // replacing Open with 3 (HETS-968 Rotation list -Wrong Block number for Open block)
            seniorityViewModel.Block = blockNumber == numberOfBlocks ? "3" : blockNumber.ToString();

            // format the hours
            seniorityViewModel.HoursYearMinus1 = $"{equipment.ServiceHoursLastYear:0.###}";
            seniorityViewModel.HoursYearMinus2 = $"{equipment.ServiceHoursTwoYearsAgo:0.###}";
            seniorityViewModel.HoursYearMinus3 = $"{equipment.ServiceHoursThreeYearsAgo:0.###}";

            // add the correct sorting order (numeric)
            seniorityViewModel.SenioritySortOrder = EquipmentHelper.CalculateSenioritySortOrder(blockNumber, numberInBlock);

            return seniorityViewModel;
        }
    }

    #region Seniority Scoring Rules Class

    /// <summary>
    /// Object to Manage Scoring Rules
    /// </summary>
    public class SeniorityScoringRules
    {
        private readonly string DefaultConstant = "Default";

        private readonly Dictionary<string, int> _equipmentScore = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _blockSize = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _totalBlocks = new Dictionary<string, int>();

        /// <summary>
        /// Scoring Rules Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public SeniorityScoringRules(IConfiguration configuration)
        {
            try
            {
                IEnumerable<IConfigurationSection> root = configuration.GetChildren();

                foreach (IConfigurationSection section in root)
                {
                    if (string.Equals(section.Key.ToLower(), "SeniorityScoringRules", StringComparison.OrdinalIgnoreCase))
                    {
                        // get children
                        IEnumerable<IConfigurationSection> ruleSections = section.GetChildren();

                        foreach (IConfigurationSection ruleSection in ruleSections)
                        {
                            string ruleSectionName = ruleSection.Key;

                            IEnumerable<IConfigurationSection> rules = ruleSection.GetChildren();

                            foreach (IConfigurationSection rule in rules)
                            {
                                string name = rule.Key;
                                int value = Convert.ToInt32(rule.Value);

                                switch (ruleSectionName)
                                {
                                    case "EquipmentScore":
                                        _equipmentScore.Add(name, value);
                                        break;

                                    case "BlockSize":
                                        _blockSize.Add(name, value);
                                        break;

                                    case "TotalBlocks":
                                        _totalBlocks.Add(name, value);
                                        break;

                                    default:
                                        throw new ArgumentException("Invalid seniority scoring rules");
                                }
                            }
                        }

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public SeniorityScoringRules(string seniorityScoringRules)
        {
            try
            {
                // convert string to json object
                ScoringRules rules = JsonConvert.DeserializeObject<ScoringRules>(seniorityScoringRules);

                // Equipment Score
                _equipmentScore.Add("Default", rules.EquipmentScore.Default);
                _equipmentScore.Add("DumpTruck", rules.EquipmentScore.DumpTruck);

                // Block Size
                _blockSize.Add("Default", rules.BlockSize.Default);
                _blockSize.Add("DumpTruck", rules.BlockSize.DumpTruck);

                // Total Blocks"
                _totalBlocks.Add("Default", rules.TotalBlocks.Default);
                _totalBlocks.Add("DumpTruck", rules.TotalBlocks.DumpTruck);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public int GetEquipmentScore(string type = null)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = DefaultConstant;
            }

            return _equipmentScore[type];
        }

        public int GetBlockSize(string type = null)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = DefaultConstant;
            }

            return _blockSize[type];
        }

        public int GetTotalBlocks(string type = null)
        {
            if (string.IsNullOrEmpty(type))
            {
                type = DefaultConstant;
            }

            return _totalBlocks[type];
        }
    }

    #endregion
}
