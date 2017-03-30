using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace HETSAPI.Models
{
    public static class SeniorityListExtensions
    {

        static public DateTime? GetEquipmentSeniorityEffectiveDate(this DbAppContext context, int equipmentId)
        {
            DateTime? result = null;
            Equipment equipment = context.Equipments.FirstOrDefault(x => x.Id == equipmentId);
            if (equipment != null)
            {
                result = equipment.SeniorityEffectiveDate;
            }
            context.Entry(equipment).State = EntityState.Detached;
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="equipmentType"></param>
        static public void CalculateSeniorityList(this DbAppContext context, int localAreaId, int equipmentType)
        {
            // Validate data
            if (context != null && context.LocalAreas.Any(x => x.Id == localAreaId) && context.EquipmentTypes.Any(x => x.Id == equipmentType))
            {
                // get the associated equipment type

                EquipmentType equipmentTypeRecord = context.EquipmentTypes.FirstOrDefault(x => x.Id == equipmentType);
                if (equipmentTypeRecord != null)
                {
                    int blocks = DistrictEquipmentType.OTHER_BLOCKS;
                    blocks = (int)equipmentTypeRecord.NumberOfBlocks;

                    // get the list of equipment in this seniority list.

                    // first pass will update the seniority score.

                    var data = context.Equipments
                         .Where(x => x.Status == Equipment.STATUS_APPROVED && x.LocalArea.Id == localAreaId && x.DistrictEquipmentType.Id == equipmentType)
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
        /// update blocks for the seniority list of a given piece of equipment
        /// </summary>
        /// <param name="context"></param>
        /// <param name="equipment"></param>
        static public void UpdateBlocksFromEquipment(this DbAppContext context, Equipment equipment)
        {
            if (equipment != null && equipment.LocalArea != null && equipment.DistrictEquipmentType != null && equipment.DistrictEquipmentType.EquipmentType != null)
            {
                AssignBlocks(context, equipment.LocalArea.Id, equipment.DistrictEquipmentType.EquipmentType);
            }
        }

        /// <summary>
        /// Hangfire job to do the Annual Rollover tasks.
        /// </summary>
        /// <param name="connectionstring"></param>
        static public void AnnualRolloverJob(PerformContext context, string connectionstring)
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

            int totalAreas = localareas.Count();
            int currentArea = 0;
            foreach (var localarea in localareas.WithProgress(progress))
            {
                currentArea++;

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
                            .Where(x => x.Status == Equipment.STATUS_APPROVED && x.LocalArea.Id == localarea.Id && x.DistrictEquipmentType.EquipmentType.Id == equipmentType.Id)
                            .Select(x => x)
                            .ToList();

                        foreach (Equipment equipment in data)
                        {
                            // rollover the year.
                            equipment.ServiceHoursThreeYearsAgo = equipment.ServiceHoursTwoYearsAgo;
                            equipment.ServiceHoursTwoYearsAgo = equipment.ServiceHoursLastYear;
                            equipment.ServiceHoursLastYear = equipment.GetYTDServiceHours(dbContext, startingYear);
                            equipment.CalculateYearsOfService(DateTime.UtcNow.Year);
                            // blank out the override reason.
                            equipment.SeniorityOverrideReason = "";
                            // update the seniority score.
                            equipment.CalculateSeniority();
                            etContext.Equipments.Update(equipment);
                            etContext.SaveChanges();
                            etContext.Entry(equipment).State = EntityState.Detached;
                        }
                    }
                    // now update the rotation list.
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
        static public void AssignBlocks(DbAppContext context, int localAreaId, EquipmentType equipmentType)
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
        /// Assign blocks for an equipment list.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="equipmentType"></param>
        static public void AssignBlocksDumpTruck(DbAppContext context, int localAreaId, int equipmentType)
        {

            // second pass will set the block.
            int primaryCount = 0;
            int secondaryCount = 0;
            int openCount = 0;

            var data = context.Equipments
                 .Include(x => x.Owner)
                 .Include(x => x.DistrictEquipmentType.EquipmentType)
                 .Where(x => x.Status == Equipment.STATUS_APPROVED && x.LocalArea.Id == localAreaId && x.DistrictEquipmentType.Id == equipmentType)
                 .OrderByDescending(x => x.Seniority)
                 .Select(x => x)
                 .ToList();

            List<int> primaryBlockOwners = new List<int>();
            List<int> secondaryBlockOwners = new List<int>();

            foreach (Equipment equipment in data)
            {
                // The primary block has a restriction such that each owner can only appear in the primary block once.                
                bool primaryFound = false;
                if (equipment.Owner != null && primaryBlockOwners.Contains(equipment.Owner.Id))
                {
                    primaryFound = true;
                }
                if (primaryFound || primaryCount >= 10) // has to go in secondary block.
                {
                    // scan the secondary block.
                    bool secondaryFound = false;
                    if (equipment.Owner != null && secondaryBlockOwners.Contains(equipment.Owner.Id))
                    {
                        secondaryFound = true;
                    }
                    if (secondaryFound || secondaryCount >= 10) // has to go in the Open block.
                    {
                        equipment.BlockNumber = DistrictEquipmentType.OPEN_BLOCK_DUMP_TRUCK;
                        openCount++;
                        equipment.NumberInBlock = openCount;
                    }
                    else // add to the secondary block.
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
                else // can go in primary block.
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
        /// Assign blocks for an equipment list.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="equipmentType"></param>
        static public void AssignBlocksNonDumpTruck(this DbAppContext context, int localAreaId, int equipmentType)
        {
            int primaryCount = 0;
            int openCount = 0;

            var data = context.Equipments
                 .Include(x => x.Owner)
                 .Include(x => x.DistrictEquipmentType.EquipmentType)
                 .Where(x => x.Status == Equipment.STATUS_APPROVED && x.LocalArea.Id == localAreaId && x.DistrictEquipmentType.Id == equipmentType)
                 .OrderByDescending(x => x.Seniority)
                 .Select(x => x)
                 .ToList();

            List<int> primaryBlockOwners = new List<int>();

            foreach (Equipment equipment in data)
            {
                // The primary block has a restriction such that each owner can only appear in the primary block once.
                bool primaryFound = false;
                if (equipment.Owner != null && primaryBlockOwners.Contains(equipment.Owner.Id))
                {
                    primaryFound = true;
                }
                if (primaryFound || primaryCount >= 10) // has to go in open block.
                {
                    equipment.BlockNumber = DistrictEquipmentType.OPEN_BLOCK_NON_DUMP_TRUCK;
                    openCount++;
                    equipment.NumberInBlock = openCount;
                }
                else // can go in primary block.
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
    }
}
