using Hangfire.Server;
using Hangfire.Console;
using Hangfire.Console.Progress;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace HETSAPI.Models
{
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
        /// Calculate the Seniority List
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="districtEquipmentTypeId"></param>
        /// <param name="equipmentTypeId"></param>
        /// <param name="configuration"></param>
        public static void CalculateSeniorityList(this DbAppContext context, int localAreaId, int districtEquipmentTypeId, int equipmentTypeId, IConfiguration configuration)
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
                         .Where(x => x.Status == Equipment.StatusApproved && 
                                     x.LocalAreaId == localAreaId && 
                                     x.DistrictEquipmentTypeId == districtEquipmentTypeId)
                         .Select(x => x);

                    // update the seniority score
                    foreach (Equipment equipment in data)
                    {                                                
                        equipment.CalculateSeniority(seniorityScoring);
                        context.Equipments.Update(equipment);
                    }

                    context.SaveChanges();

                    AssignBlocks(context, localAreaId, districtEquipmentTypeId, blockSize, totalBlocks);
                }
            }
        }

        /// <summary>
        /// Update blocks for the seniority list of a given piece of equipment
        /// </summary>
        /// <param name="context"></param>
        /// <param name="equipment"></param>
        /// <param name="configuration"></param>
        public static void UpdateBlocksFromEquipment(this DbAppContext context, Equipment equipment, IConfiguration configuration)
        {
            if (equipment != null && 
                equipment.LocalArea != null && 
                equipment.DistrictEquipmentType != null && 
                equipment.DistrictEquipmentType.EquipmentType != null)
            {
                int localAreaId = equipment.LocalArea.Id;
                int equipmentTypeId = equipment.DistrictEquipmentType.EquipmentType.Id;
                bool isDumpTruck = equipment.DistrictEquipmentType.EquipmentType.IsDumpTruck;

                // get processing rules
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration);

                // get rules                                  
                int blockSize = isDumpTruck ? scoringRules.GetBlockSize("DumpTruck") : scoringRules.GetBlockSize();
                int totalBlocks = isDumpTruck ? scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();

                // update blocks
                AssignBlocks(context, localAreaId, equipmentTypeId, blockSize, totalBlocks);
            }
        }

        /// <summary>
        /// Hangfire job to do the Annual Rollover tasks.
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
                DbAppContext dbContext = new DbAppContext(null, options.Options, configuration);

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

                // since this action is meant to run at the end of March, YTD calcs will always start from the year prior.
                int startingYear = DateTime.UtcNow.Year - 1;

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

                        using (DbAppContext etContext = new DbAppContext(null, options.Options, configuration))
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
                                equipment.ServiceHoursLastYear = equipment.GetYTDServiceHours(dbContext, startingYear);
                                equipment.CalculateYearsOfService(DateTime.UtcNow.Year);
                            
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
                        using (DbAppContext abContext = new DbAppContext(null, options.Options, configuration))
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

        /// <summary>
        /// Assign blocks for the given local area and equipment type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="districtEquipmentTypeId"></param>
        /// <param name="blockSize"></param>
        /// <param name="totalBlocks"></param>
        public static void AssignBlocks(DbAppContext context, int localAreaId, int districtEquipmentTypeId, int blockSize, int totalBlocks)
        {
            // get all equipment records
            List<Equipment> data = context.Equipments
                .Include(x => x.Owner)
                .Where(x => x.Status == Equipment.StatusApproved &&
                            x.LocalArea.Id == localAreaId &&
                            x.DistrictEquipmentTypeId == districtEquipmentTypeId)
                .OrderByDescending(x => x.Seniority)
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
                    if (AddedToBlock(context, i, totalBlocks, blockSize, blocks, equipment))
                    {                        
                        break; // move to next record
                    }
                }
            }            
        }

        private static bool AddedToBlock(DbAppContext context, int currentBlock, int totalBlocks, int blockSize, List<int>[] blocks, Equipment equipment)
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
            if (currentBlock < totalBlocks - 1 && blocks[currentBlock].Count >= blockSize)
            {
                return false; // not adding this record to the block
            }            

            // check if this record's Owner already exists in the block   
            if (currentBlock < totalBlocks - 1 && blocks[currentBlock].Contains(equipment.Owner.Id))
            {
                // add record to the next block         
                if (blocks[currentBlock + 1] == null)
                {
                    blocks[currentBlock + 1] = new List<int>();
                }

                blocks[currentBlock + 1].Add(equipment.Owner.Id);

                // update the equipment record
                equipment.BlockNumber = currentBlock + 2;
                equipment.NumberInBlock = blocks[currentBlock + 1].Count;

                context.Equipments.Update(equipment);
                context.SaveChanges();

                return true;
            }

            // add record to the block                        
            blocks[currentBlock].Add(equipment.Owner.Id);

            // update the equipment record
            equipment.BlockNumber = currentBlock + 1;
            equipment.NumberInBlock = blocks[currentBlock].Count;
            
            context.Equipments.Update(equipment);
            context.SaveChanges();

            // record added to the block
            return true;
        }        

        /// <summary>
        /// Returns the Equipment that is next on a Local Rotation List
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="localAreaId">Local Area</param>
        /// <param name="equipmentTypeId">Type of equipment</param>
        /// <param name="currentEquipmentId">ID of the equipment that will be replaced</param>
        /// <param name="blockNumber">Block number within the list</param>
        /// <returns></returns>
        public static Equipment GetNextEquipmentOnLocalRotationList(this DbAppContext context, int localAreaId, int equipmentTypeId, int currentEquipmentId, int blockNumber)
        {
            Equipment result = null;

            // first get the complete list
            List<Equipment> fullList = context.Equipments
                .Include(x => x.LocalArea)
                .Include(x => x.DistrictEquipmentType.EquipmentType)
                .Where(x => x.LocalArea.Id == localAreaId && 
                            x.DistrictEquipmentType.EquipmentType != null && 
                            x.DistrictEquipmentType.EquipmentType.Id == equipmentTypeId && 
                            x.BlockNumber == blockNumber)
                .OrderByDescending(x => x.Seniority)
                .ToList();  
                      
            if (fullList.Count > 0)
            {
                // find the current equipment.  if none then we start at the top
                int foundPosition = -1;

                for (int i = 0; i < fullList.Count; i++)
                {
                    if (fullList[i].Id == currentEquipmentId)
                    {
                        foundPosition = i;
                    }
                }

                int nextPosition = 0;

                if (foundPosition != -1)
                {
                    nextPosition = foundPosition + 1;

                    if (nextPosition >= fullList.Count)
                    {
                        nextPosition = 0;
                    }
                }

                result = fullList[nextPosition];
            } 
            
            return result;
        }

        /// <summary>
        /// Update the local area rotation list
        /// </summary>
        /// <param name="context"></param>
        /// <param name="rentalRequestRotationListId"></param>
        public static void UpdateLocalAreaRotationList (this DbAppContext context, int rentalRequestRotationListId)
        {
            // start by getting the context.
            RentalRequestRotationList rentalRequestRotationList = context.RentalRequestRotationLists
                .Include (x => x.Equipment.LocalArea)
                .Include(x => x.Equipment.DistrictEquipmentType.EquipmentType)
                .FirstOrDefault(x => x.Id == rentalRequestRotationListId);

            if (rentalRequestRotationList != null && 
                rentalRequestRotationList.Equipment != null && 
                rentalRequestRotationList.Equipment.LocalArea != null && 
                rentalRequestRotationList.Equipment.DistrictEquipmentType != null)
            {
                LocalAreaRotationList localAreaRotationList = context.LocalAreaRotationLists
                    .Include (x => x.AskNextBlock1)
                    .Include(x => x.AskNextBlock2)
                    .Include(x => x.AskNextBlockOpen)
                    .FirstOrDefault(x => x.LocalArea.Id == rentalRequestRotationList.Equipment.LocalArea.Id && 
                                         x.DistrictEquipmentType.Id == rentalRequestRotationList.Equipment.DistrictEquipmentType.Id);

                if (localAreaRotationList == null ||
                    rentalRequestRotationList.Equipment.BlockNumber == null) return;                

                switch (rentalRequestRotationList.Equipment.BlockNumber)
                {
                    // primary block
                    case 1:
                        if (localAreaRotationList.AskNextBlock1.Id == rentalRequestRotationList.Equipment.Id)
                        {
                            localAreaRotationList.AskNextBlock1 = context.GetNextEquipmentOnLocalRotationList(rentalRequestRotationList.Equipment.LocalArea.Id, rentalRequestRotationList.Equipment.DistrictEquipmentType.EquipmentType.Id, rentalRequestRotationList.Equipment.Id, 1);
                        }
                        break;

                    // secondary block for dump trucks, or open block for other types
                    case 2:
                        if (rentalRequestRotationList.Equipment.DistrictEquipmentType.EquipmentType.IsDumpTruck)
                        {
                            if (localAreaRotationList.AskNextBlock2.Id == rentalRequestRotationList.Equipment.Id)
                            {
                                localAreaRotationList.AskNextBlock1 = context.GetNextEquipmentOnLocalRotationList(rentalRequestRotationList.Equipment.LocalArea.Id, rentalRequestRotationList.Equipment.DistrictEquipmentType.EquipmentType.Id, rentalRequestRotationList.Equipment.Id , 2);
                            }
                        }
                        else
                        {
                            if (localAreaRotationList.AskNextBlockOpen.Id == rentalRequestRotationList.Equipment.Id)
                            {
                                localAreaRotationList.AskNextBlockOpen = context.GetNextEquipmentOnLocalRotationList(rentalRequestRotationList.Equipment.LocalArea.Id, rentalRequestRotationList.Equipment.DistrictEquipmentType.EquipmentType.Id, rentalRequestRotationList.Equipment.Id, 2);
                            }
                        }
                                
                        break;

                    // open block for dump trucks
                    case 3:
                        if (localAreaRotationList.AskNextBlockOpen.Id == rentalRequestRotationList.Equipment.Id)
                        {
                            localAreaRotationList.AskNextBlockOpen = context.GetNextEquipmentOnLocalRotationList(rentalRequestRotationList.Equipment.LocalArea.Id, rentalRequestRotationList.Equipment.DistrictEquipmentType.EquipmentType.Id, rentalRequestRotationList.Equipment.Id, 3);
                        }
                        break;
                }

                // update the local area rotation record
                context.LocalAreaRotationLists.Update(localAreaRotationList);
                context.SaveChanges();                
            }
        }
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
