using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HETSAPI.Models
{
    public static class SeniorityListExtensions
    {

        static public float GetEquipmentSeniority (this DbAppContext context, int equipmentId)
        {
            float result = -1.0f;
            Equipment equipment = context.Equipments.FirstOrDefault(x => x.Id == equipmentId);
            if (equipment != null && equipment.Seniority != null)
            {
                result = (float) equipment.Seniority;
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
                         .Where(x => x.Status == Equipment.STATUS_ACTIVE && x.LocalArea.Id == localAreaId && x.DistrictEquipmentType.Id == equipmentType)
                         .Select(x => x);

                    foreach (Equipment equipment in data)
                    {
                        // update the seniority score.
                        equipment.CalculateSeniority();
                        context.Equipments.Update(equipment);
                    }
                    context.SaveChanges();

                    AssignBlocks(context, localAreaId, equipmentTypeRecord);

                    context.SaveChanges();
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
        /// Assign blocks for the given local area and equipment type
        /// </summary>
        /// <param name="context"></param>
        /// <param name="localAreaId"></param>
        /// <param name="equipmentType"></param>
        static public void AssignBlocks (DbAppContext context, int localAreaId, EquipmentType equipmentType)
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
                 .Where(x => x.Status == Equipment.STATUS_ACTIVE && x.LocalArea.Id == localAreaId && x.DistrictEquipmentType.Id == equipmentType)
                 .OrderByDescending(x => x.Seniority)
                 .Select(x => x);

            List<Equipment> primaryBlock = new List<Equipment>();
            List<Equipment> secondaryBlock = new List<Equipment>();

            foreach (Equipment equipment in data)
            {
                // The primary block has a restriction such that each owner can only appear in the primary block once.                
                bool primaryFound = false;
                foreach (Equipment item in primaryBlock)
                {
                    if (item.Owner.Id == equipment.Owner.Id)
                    {
                        primaryFound = true;
                    }
                }
                if (primaryFound || primaryCount >= 10) // has to go in secondary block.
                {
                    // scan the secondary block.
                    bool secondaryFound = false;
                    foreach (Equipment item in secondaryBlock)
                    {
                        if (item.Owner.Id == equipment.Owner.Id)
                        {
                            secondaryFound = true;
                        }
                    }
                    if (secondaryFound || secondaryCount >= 10) // has to go in the Open block.
                    {
                        equipment.BlockNumber = DistrictEquipmentType.OPEN_BLOCK_DUMP_TRUCK;
                        openCount++;
                    }
                    else
                    {
                        secondaryBlock.Add(equipment);
                        equipment.BlockNumber = DistrictEquipmentType.SECONDARY_BLOCK;
                        openCount++;
                    }
                }
                else // can go in primary block.
                {
                    primaryBlock.Add(equipment);
                    equipment.BlockNumber = DistrictEquipmentType.PRIMARY_BLOCK;
                    primaryCount++;
                }                
                context.Equipments.Update(equipment);
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
            
            var data = context.Equipments
                 .Where(x => x.Status == Equipment.STATUS_ACTIVE && x.LocalArea.Id == localAreaId && x.DistrictEquipmentType.Id == equipmentType)
                 .OrderByDescending(x => x.Seniority)
                 .Select(x => x);

            List<Equipment> primaryBlock = new List<Equipment>();

            foreach (Equipment equipment in data)
            {
                // The primary block has a restriction such that each owner can only appear in the primary block once.
                bool primaryFound = false;
                foreach (Equipment item in primaryBlock)
                {
                    if (item.Owner.Id == equipment.Owner.Id)
                    {
                        primaryFound = true;
                    }
                }
                if (primaryFound || primaryCount >= 10) // has to go in open block.
                {
                    equipment.BlockNumber = DistrictEquipmentType.OPEN_BLOCK_NON_DUMP_TRUCK;
                }
                else // can go in primary block.
                {
                    primaryBlock.Add(equipment);
                    equipment.BlockNumber = DistrictEquipmentType.PRIMARY_BLOCK;
                    primaryCount++;
                }
                context.Equipments.Update(equipment);
            }            
        }
    }
}
