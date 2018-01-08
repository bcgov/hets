using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="equipmentType"></param>
        public static void CalculateSeniorityList(this DbAppContext context, int localAreaId, int equipmentType)
        {
            // Validate data
            if (context != null && context.LocalAreas.Any(x => x.Id == localAreaId) && context.EquipmentTypes.Any(x => x.Id == equipmentType))
            {
                // get the associated equipment type
                EquipmentType equipmentTypeRecord = context.EquipmentTypes.FirstOrDefault(x => x.Id == equipmentType);

                if (equipmentTypeRecord != null)
                {                    
                    // first pass will update the seniority score.
                    IQueryable<Equipment> data = context.Equipments
                         .Where(x => x.Status == Equipment.STATUS_APPROVED && 
                                     x.LocalArea.Id == localAreaId && 
                                     x.DistrictEquipmentType.Id == equipmentType)
                         .Select(x => x);

                    foreach (Equipment equipment in data)
                    {
                        // update the seniority score.
                        equipment.CalculateSeniority();
                        context.Equipments.Update(equipment);
                    }

                    context.SaveChanges();

                    AssignBlocks(context, localAreaId, equipmentTypeRecord);
                }
            }
        }

        /// <summary>
        /// Update blocks for the seniority list of a given piece of equipment
        /// </summary>
        /// <param name="context"></param>
        /// <param name="equipment"></param>
        public static void UpdateBlocksFromEquipment(this DbAppContext context, Equipment equipment)
        {
            if (equipment != null && equipment.LocalArea != null && equipment.DistrictEquipmentType != null && equipment.DistrictEquipmentType.EquipmentType != null)
            {
                AssignBlocks(context, equipment.LocalArea.Id, equipment.DistrictEquipmentType.EquipmentType);
            }
        }

        /// <summary>
        /// Hangfire job to do the Annual Rollover tasks.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="connectionstring"></param>
        public static void AnnualRolloverJob(PerformContext context, string connectionstring)
        {
            // open a connection to the database.
            DbContextOptionsBuilder<DbAppContext> options = new DbContextOptionsBuilder<DbAppContext>();
            options.UseNpgsql(connectionstring);

            DbAppContext dbContext = new DbAppContext(null, options.Options);

            var progress = context.WriteProgressBar();
            context.WriteLine("Starting Annual Rollover Job");

            progress.SetValue(0);

            var equipmentTypes = dbContext.EquipmentTypes.ToList();

            // The annual rollover will process all local areas in turn.
            var localareas = dbContext.LocalAreas.ToList();

            // since this action is meant to run at the end of March, YTD calcs will always start from the year prior.
            int startingYear = DateTime.UtcNow.Year - 1;

            foreach (var localarea in localareas.WithProgress(progress))
            {                
                if (localarea.Name != null)
                {
                    context.WriteLine("Local Area: " + localarea.Name);
                }
                else
                {
                    context.WriteLine("Local Area ID: " + localarea.Id);
                }

                foreach (EquipmentType equipmentType in equipmentTypes)
                {
                    using (DbAppContext etContext = new DbAppContext(null, options.Options))
                    {
                        var data = etContext.Equipments
                            .Include(x => x.LocalArea)
                            .Include(x => x.DistrictEquipmentType.EquipmentType)
                            .Where(x => x.Status == Equipment.STATUS_APPROVED && 
                                        x.LocalArea.Id == localarea.Id && 
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
                            equipment.CalculateSeniority();
                            etContext.Equipments.Update(equipment);
                            etContext.SaveChanges();
                            etContext.Entry(equipment).State = EntityState.Detached;
                        }
                    }

                    // now update the rotation list
                    using (DbAppContext abContext = new DbAppContext(null, options.Options))
                    {
                        AssignBlocks(abContext, localarea.Id, equipmentType);
                    }
                }
            }
        }

        /// <summary>
        /// Assign blocks for the given local area and equipment type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="equipmentType"></param>
        public static void AssignBlocks(DbAppContext context, int localAreaId, EquipmentType equipmentType)
        {
            if (equipmentType != null)
            {
                if (equipmentType.IsDumpTruck)
                {
                    AssignBlocksDumpTruck(context, localAreaId, equipmentType.Id);
                }
                else
                {
                    AssignBlocksNonDumpTruck(context, localAreaId, equipmentType.Id);
                }
            }
        }

        /// <summary>
        /// Assign blocks for an equipment list
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="equipmentType"></param>
        public static void AssignBlocksDumpTruck(DbAppContext context, int localAreaId, int equipmentType)
        {
            // second pass will set the block
            int primaryCount = 0;
            int secondaryCount = 0;
            int openCount = 0;

            var data = context.Equipments
                 .Include(x => x.Owner)
                 .Include(x => x.DistrictEquipmentType.EquipmentType)
                 .Where(x => x.Status == Equipment.STATUS_APPROVED && 
                             x.LocalArea.Id == localAreaId && 
                             x.DistrictEquipmentType.Id == equipmentType)
                 .OrderByDescending(x => x.Seniority)
                 .Select(x => x)
                 .ToList();

            List<int> primaryBlockOwners = new List<int>();
            List<int> secondaryBlockOwners = new List<int>();

            foreach (Equipment equipment in data)
            {
                // The primary block has a restriction such that each owner can only appear in the primary block once            
                bool primaryFound = equipment.Owner != null && primaryBlockOwners.Contains(equipment.Owner.Id);

                if (primaryFound || primaryCount >= 10) // has to go in secondary block
                {
                    // scan the secondary block.
                    bool secondaryFound = equipment.Owner != null && secondaryBlockOwners.Contains(equipment.Owner.Id);

                    if (secondaryFound || secondaryCount >= 10) // has to go in the Open block
                    {
                        equipment.BlockNumber = DistrictEquipmentType.OPEN_BLOCK_DUMP_TRUCK;
                        openCount++;
                        equipment.NumberInBlock = openCount;
                    }
                    else // add to the secondary block
                    {
                        if (equipment.Owner != null)
                        {
                            secondaryBlockOwners.Add(equipment.Owner.Id);
                        }

                        equipment.BlockNumber = DistrictEquipmentType.SECONDARY_BLOCK;
                        secondaryCount++;
                        equipment.NumberInBlock = secondaryCount;
                    }
                }
                else // can go in primary block
                {
                    if (equipment.Owner != null)
                    {
                        primaryBlockOwners.Add(equipment.Owner.Id);
                    }

                    equipment.BlockNumber = DistrictEquipmentType.PRIMARY_BLOCK;
                    primaryCount++;
                    equipment.NumberInBlock = primaryCount;
                }

                context.Equipments.Update(equipment);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Assign blocks for an equipment list
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="equipmentType"></param>
        public static void AssignBlocksNonDumpTruck(this DbAppContext context, int localAreaId, int equipmentType)
        {
            int primaryCount = 0;
            int openCount = 0;

            var data = context.Equipments
                 .Include(x => x.Owner)
                 .Include(x => x.DistrictEquipmentType.EquipmentType)
                 .Where(x => x.Status == Equipment.STATUS_APPROVED && 
                             x.LocalArea.Id == localAreaId && 
                             x.DistrictEquipmentType.Id == equipmentType)
                 .OrderByDescending(x => x.Seniority)
                 .Select(x => x)
                 .ToList();

            List<int> primaryBlockOwners = new List<int>();

            foreach (Equipment equipment in data)
            {
                // The primary block has a restriction such that each owner can only appear in the primary block once
                bool primaryFound = equipment.Owner != null && primaryBlockOwners.Contains(equipment.Owner.Id);

                if (primaryFound || primaryCount >= 10) // has to go in open block
                {
                    equipment.BlockNumber = DistrictEquipmentType.OPEN_BLOCK_NON_DUMP_TRUCK;
                    openCount++;
                    equipment.NumberInBlock = openCount;
                }
                else // can go in primary block
                {
                    if (equipment.Owner != null)
                    {
                        primaryBlockOwners.Add(equipment.Owner.Id);
                    }

                    equipment.BlockNumber = DistrictEquipmentType.PRIMARY_BLOCK;
                    primaryCount++;
                    equipment.NumberInBlock = primaryCount;
                }

                context.Equipments.Update(equipment);
                context.SaveChanges();
                context.Entry(equipment).State = EntityState.Detached;
            }
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
            var fullList = context.Equipments
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
                // find the current equipment.  if none then we start at the top.
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
}
