using Hangfire.Server;
using Hangfire.Console;
using Hangfire.Console.Progress;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HETSAPI.Models
{    
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


    /// <summary>
    /// Seniority List Database Model Extension
    /// </summary>
    public static class SeniorityListExtensions
    {
        /// <summary>
        /// Get the Effective Date of the Equipment Seniority
        /// </summary>
        /// <param name="context"></param>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public static DateTime? GetEquipmentSeniorityEffectiveDate(this DbAppContext context, int equipmentId)
        {
            DateTime? result = null;
            Equipment equipment = context.Equipments.FirstOrDefault(x => x.Id == equipmentId);

            if (equipment != null)
            {
                result = equipment.SeniorityEffectiveDate;
            }

            context.Entry(equipment ?? throw new InvalidOperationException()).State = EntityState.Detached;
            return result;
        }        
        
        /// <summary>
        /// Hangfire job to do the Annual Rollover tasks
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        /// <param name="configuration"></param>
        public static void AnnualRolloverJob(PerformContext context, string connectionstring, IConfiguration configuration)
        {
            try
            {            
                // open a connection to the database
                DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
                options.UseNpgsql(connectionstring);
                DbAppContext dbContext = new DbAppContext(null, options.Options);

                // get processing rules
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration);

                // update progress bar
                IProgressBar progress = context.WriteProgressBar();
                context.WriteLine("Starting Annual Rollover Job");

                progress.SetValue(0);

                // get all equipment types
                List<EquipmentType> equipmentTypes = dbContext.EquipmentTypes.ToList();

                // The annual rollover will process all local areas in turn
                List<LocalArea> localAreas = dbContext.LocalAreas.ToList();                

                foreach (LocalArea localArea in localAreas.WithProgress(progress))
                {                
                    if (localArea.Name != null)
                    {
                        context.WriteLine("Local Area: " + localArea.Name);
                    }
                    else
                    {
                        context.WriteLine("Local Area ID: " + localArea.Id);
                    }

                    foreach (EquipmentType equipmentType in equipmentTypes)
                    {
                        // it this a dumptruck? 
                        bool isDumpTruck = equipmentType.IsDumpTruck;

                        // get rules for scoring and seniority block
                        int seniorityScoring = isDumpTruck ? scoringRules.GetEquipmentScore("DumpTruck") : scoringRules.GetEquipmentScore();
                        int blockSize = isDumpTruck ? scoringRules.GetBlockSize("DumpTruck") : scoringRules.GetBlockSize();
                        int totalBlocks = isDumpTruck ? scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();

                        using (DbAppContext etContext = new DbAppContext(null, options.Options))
                        {
                            List<Equipment> data = etContext.Equipments
                                .Include(x => x.LocalArea)
                                .Include(x => x.DistrictEquipmentType.EquipmentType)
                                .Where(x => x.Status == Equipment.StatusApproved && 
                                            x.LocalArea.Id == localArea.Id && 
                                            x.DistrictEquipmentType.EquipmentType.Id == equipmentType.Id)
                                .Select(x => x)
                                .ToList();

                            foreach (Equipment equipment in data)
                            {
                                // rollover the year
                                equipment.ServiceHoursThreeYearsAgo = equipment.ServiceHoursTwoYearsAgo;
                                equipment.ServiceHoursTwoYearsAgo = equipment.ServiceHoursLastYear;
                                equipment.ServiceHoursLastYear = equipment.GetYtdServiceHours(dbContext);
                                equipment.CalculateYearsOfService(DateTime.UtcNow);
                            
                                // blank out the override reason
                                equipment.SeniorityOverrideReason = "";
                            
                                // update the seniority score
                                equipment.CalculateSeniority(seniorityScoring);

                                etContext.Equipments.Update(equipment);
                                etContext.SaveChanges();
                                etContext.Entry(equipment).State = EntityState.Detached;
                            }
                        }

                        // now update the rotation list
                        using (DbAppContext abContext = new DbAppContext(null, options.Options))
                        {
                            int localAreaId = localArea.Id;
                            int equipmentTypeId = equipmentType.Id;

                            AssignBlocks(abContext, localAreaId, equipmentTypeId, blockSize, totalBlocks);
                        }
                    }                
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #region Manage the Seniority List for a Specific Location

        /// <summary>
        /// Calculate the Seniority List
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="districtEquipmentTypeId"></param>
        /// <param name="equipmentTypeId"></param>
        /// <param name="configuration"></param>
        public static void CalculateSeniorityList(this DbAppContext context, int localAreaId, int districtEquipmentTypeId, int equipmentTypeId, IConfiguration configuration)
        {
            try
            {          
                // validate data
                if (context != null &&
                    context.LocalAreas.Any(x => x.Id == localAreaId) &&
                    context.DistrictEquipmentTypes.Any(x => x.Id == districtEquipmentTypeId))
                {
                    // get processing rules
                    SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration);

                    // get the associated equipment type
                    EquipmentType equipmentTypeRecord = context.EquipmentTypes.FirstOrDefault(x => x.Id == equipmentTypeId);

                    if (equipmentTypeRecord != null)
                    {
                        // get rules                  
                        int seniorityScoring = equipmentTypeRecord.IsDumpTruck ? scoringRules.GetEquipmentScore("DumpTruck") : scoringRules.GetEquipmentScore();
                        int blockSize = equipmentTypeRecord.IsDumpTruck ? scoringRules.GetBlockSize("DumpTruck") : scoringRules.GetBlockSize();
                        int totalBlocks = equipmentTypeRecord.IsDumpTruck ? scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();

                        // get all equipment records
                        IQueryable<Equipment> data = context.Equipments
                             .Where(x => x.LocalAreaId == localAreaId &&
                                         x.DistrictEquipmentTypeId == districtEquipmentTypeId)
                             .Select(x => x);

                        // update the seniority score
                        foreach (Equipment equipment in data)
                        {
                            if (equipment.Status != Equipment.StatusApproved)
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
                        
                            context.Equipments.Update(equipment);
                        }

                        context.SaveChanges();

                        // put equipment into the correct blocks
                        AssignBlocks(context, localAreaId, districtEquipmentTypeId, blockSize, totalBlocks);
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
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="districtEquipmentTypeId"></param>
        /// <param name="blockSize"></param>
        /// <param name="totalBlocks"></param>
        /// <param name="saveChanges"></param>
        public static void AssignBlocks(DbAppContext context, int localAreaId, int districtEquipmentTypeId, int blockSize, int totalBlocks, bool saveChanges = true)
        {
            try
            {            
                // get all equipment records
                List<Equipment> data = context.Equipments
                    .Include(x => x.Owner)
                    .Where(x => x.Status == Equipment.StatusApproved &&
                                x.LocalArea.Id == localAreaId &&
                                x.DistrictEquipmentTypeId == districtEquipmentTypeId)
                    .OrderByDescending(x => x.Seniority).ThenBy(x => x.ReceivedDate)
                    .Select(x => x)
                    .ToList();

                // total blocks only counts the "main" blocks - we need to add 1 more for the remaining records
                totalBlocks = totalBlocks + 1;

                // instantiate lists to hold equipment by block
                List<int>[] blocks = new List<int>[totalBlocks];         

                foreach (Equipment equipment in data)
                {
                    // iterate the blocks and add the record
                    for (int i = 0; i < totalBlocks; i++)
                    {
                        if (AddedToBlock(context, i, totalBlocks, blockSize, blocks, equipment, saveChanges))
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

        private static bool AddedToBlock(DbAppContext context, int currentBlock, int totalBlocks, 
            int blockSize, List<int>[] blocks, Equipment equipment, bool saveChanges = true)
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
                if (currentBlock < (totalBlocks - 1) && blocks[currentBlock].Contains(equipment.Owner.Id))
                {
                    return false; // not adding this record to the block
                }

                // add record to the block                        
                blocks[currentBlock].Add(equipment.Owner.Id);

                // update the equipment record
                equipment.BlockNumber = currentBlock + 1;
                equipment.NumberInBlock = blocks[currentBlock].Count;
            
                context.Equipments.Update(equipment);

                if (saveChanges)
                {
                    context.SaveChanges();
                }

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
    }

    #region Seniority Scoring Rules

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
                            string ruleSectionNamde = ruleSection.Key;

                            IEnumerable<IConfigurationSection> rules = ruleSection.GetChildren();

                            foreach (IConfigurationSection rule in rules)
                            {
                                string name = rule.Key;
                                int value = Convert.ToInt32(rule.Value);

                                switch (ruleSectionNamde)
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
