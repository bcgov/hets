using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire.Console;
using Hangfire.Server;
using Hangfire.Console.Progress;
using Microsoft.EntityFrameworkCore;
using HetsData.Model;

namespace HetsData.Helpers
{
    /// <summary>
    /// Annual Rollover List Helper
    /// </summary>
    public static class AnnualRolloverHelper
    {
        #region Get a District Status record

        /// <summary>
        /// Get a District Status record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HetDistrictStatus GetRecord(int id, DbAppContext context)
        {
            HetDistrictStatus status = context.HetDistrictStatus.AsNoTracking()
                .Include(a => a.District)
                .FirstOrDefault(a => a.DistrictId == id);

            // if there isn't a status - we'll add one now
            if (status == null)
            {
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

                status = new HetDistrictStatus
                {
                    DistrictId = id,
                    CurrentFiscalYear = fiscalStart.Year,
                    NextFiscalYear = fiscalStart.Year + 1,
                    DisplayRolloverMessage = false
                };

                context.HetDistrictStatus.Add(status);
                context.SaveChanges();

                // get updated record
                status = context.HetDistrictStatus.AsNoTracking()
                    .Include(a => a.District)
                    .First(a => a.DistrictId == id);
            }

            return status;
        }

        #endregion

        #region Annual Rollover Process        

        /// <summary>
        /// Annual Rollover
        /// </summary>
        /// <param name="context"></param>
        /// <param name="districtId"></param>        
        /// <param name="seniorityScoringRules"></param>
        /// <param name="connectionString"></param>
        public static void AnnualRolloverJob(PerformContext context, int districtId, string seniorityScoringRules, string connectionString)
        {
            try
            {                
                // open a connection to the database
                DbAppContext dbContext = new DbAppContext(connectionString);

                // get processing rules
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(seniorityScoringRules);

                // update progress bar
                IProgressBar progress = context.WriteProgressBar();
                context.WriteLine("Starting Annual Rollover Job - District #" + districtId);

                progress.SetValue(0);

                // validate district id
                HetDistrict district = dbContext.HetDistrict.AsNoTracking()
                    .FirstOrDefault(x => x.DistrictId == districtId);

                if (district == null)
                {
                    context.WriteLine("District not found");
                    progress.SetValue(100);
                    return;
                }

                // get status record - and ensure we're active
                HetDistrictStatus status = GetRecord(districtId, dbContext);

                if (status == null)
                {
                    context.WriteLine("District Status not found");
                    progress.SetValue(100);
                    return;
                }
                
                // get equipment status
                int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", dbContext);
                if (statusId == null)
                {
                    context.WriteLine("Equipment Status not found");
                    progress.SetValue(100);
                    return;                    
                }

                // get all district equipment types
                List<HetDistrictEquipmentType> equipmentTypes = dbContext.HetDistrictEquipmentType.AsNoTracking()
                    .Include(x => x.EquipmentType)
                    .Where(x => x.DistrictId == districtId).ToList();

                // get all local areas
                List<HetLocalArea> localAreas = dbContext.HetLocalArea.AsNoTracking()
                    .Where(a => a.ServiceArea.DistrictId == districtId).ToList();

                // update status - job is kicked off
                int localAreaCompleteCount = 0;
                int equipmentCompleteCount = 0;

                UpdateStatusKickoff(dbContext, status, localAreaCompleteCount, equipmentCompleteCount);

                // process all local areas and equipment types
                foreach (HetLocalArea localArea in localAreas.WithProgress(progress))
                {
                    if (localArea.Name != null)
                    {
                        context.WriteLine("Local Area: " + localArea.Name);
                    }
                    else
                    {
                        context.WriteLine("Local Area ID: " + localArea.LocalAreaId);
                    }

                    // reset equipment counter
                    equipmentCompleteCount = 0;

                    foreach (HetDistrictEquipmentType equipmentType in equipmentTypes)
                    {
                        // it this a dump truck? 
                        bool isDumpTruck = equipmentType.EquipmentType.IsDumpTruck;

                        // get rules for scoring and seniority block
                        int seniorityScoring = isDumpTruck ? scoringRules.GetEquipmentScore("DumpTruck") : scoringRules.GetEquipmentScore();
                        int blockSize = isDumpTruck ? scoringRules.GetBlockSize("DumpTruck") : scoringRules.GetBlockSize();
                        int totalBlocks = isDumpTruck ? scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();

                        using (DbAppContext etContext = new DbAppContext(connectionString))
                        {
                            List<HetEquipment> data = etContext.HetEquipment
                                .Include(x => x.LocalArea)
                                .Include(x => x.DistrictEquipmentType.EquipmentType)
                                .Where(x => x.EquipmentStatusTypeId == statusId &&
                                            x.LocalAreaId == localArea.LocalAreaId &&
                                            x.DistrictEquipmentTypeId == equipmentType.DistrictEquipmentTypeId)
                                .ToList();

                            foreach (HetEquipment equipment in data)
                            {
                                // rollover the year
                                equipment.ServiceHoursThreeYearsAgo = equipment.ServiceHoursTwoYearsAgo;
                                equipment.ServiceHoursTwoYearsAgo = equipment.ServiceHoursLastYear;
                                equipment.ServiceHoursLastYear = EquipmentHelper.GetYtdServiceHours(equipment.EquipmentId, dbContext);
                                equipment.CalculateYearsOfService(DateTime.UtcNow);

                                // blank out the override reason
                                equipment.SeniorityOverrideReason = "";

                                // update the seniority score
                                equipment.CalculateSeniority(seniorityScoring);

                                etContext.HetEquipment.Update(equipment);
                                etContext.SaveChanges();
                            }
                        }

                        // now update the rotation list
                        using (DbAppContext abContext = new DbAppContext(connectionString))
                        {
                            int localAreaId = localArea.LocalAreaId;
                            int equipmentTypeId = equipmentType.DistrictEquipmentTypeId;

                            SeniorityListHelper.AssignBlocks(localAreaId, equipmentTypeId, blockSize, totalBlocks, abContext);
                        }

                        // increment counters and update status
                        equipmentCompleteCount++;
                        UpdateStatus(dbContext, status, localAreaCompleteCount, equipmentCompleteCount);
                    }

                    // increment counters and update status
                    localAreaCompleteCount++;
                    UpdateStatus(dbContext, status, localAreaCompleteCount, equipmentCompleteCount);
                    if (status.ProgressPercentage != null) progress.SetValue((int)status.ProgressPercentage);
                }

                // done!
                UpdateStatusComplete(dbContext, status, localAreaCompleteCount, equipmentCompleteCount);
                progress.SetValue(100);

                // **********************************************************
                // regenerate Owner Secret Keys for this district
                // **********************************************************    
                context.WriteLine("");
                context.WriteLine("Generate New Secret Keys - District #" + districtId);
                progress = context.WriteProgressBar();

                progress.SetValue(0);
                // get records
                List<HetOwner> owners = dbContext.HetOwner.AsNoTracking()
                    .Where(x => x.BusinessId == null &&
                                x.DistrictId == districtId)
                    .ToList();

                int i = 0;
                int ownerCount = owners.Count;

                foreach (HetOwner owner in owners)
                {
                    i++;
                    string key = SecretKeyHelper.RandomString(8);

                    string temp = owner.OwnerCode;

                    if (string.IsNullOrEmpty(temp))
                    {
                        temp = SecretKeyHelper.RandomString(4);
                    }

                    key = temp + "-" + DateTime.UtcNow.Year + "-" + key;

                    // get owner and update
                    HetOwner ownerRecord = dbContext.HetOwner.First(x => x.OwnerId == owner.OwnerId);
                    ownerRecord.SharedKey = key;

                    if (i % 500 == 0)
                    {
                        dbContext.SaveChangesForImport();
                    }

                    decimal tempProgress = Convert.ToDecimal(i) / Convert.ToDecimal(ownerCount);
                    tempProgress = tempProgress * 100;
                    int percentComplete = Convert.ToInt32(tempProgress);

                    if (percentComplete < 1) percentComplete = 1;
                    if (percentComplete > 99) percentComplete = 100;
                    
                    progress.SetValue(percentComplete);
                }

                // save remaining updates - done!
                dbContext.SaveChangesForImport();
                progress.SetValue(100);
                context.WriteLine("Generate New Secret Keys - Done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void UpdateStatus(DbAppContext dbContext, HetDistrictStatus status,
            int localAreaCompleteCount, int equipmentCompleteCount)
        {
            try
            {
                int localAreaCount = status.LocalAreaCount ?? 0;
                int equipmentCount = status.DistrictEquipmentTypeCount ?? 0;
                int percentComplete;

                if (localAreaCount == 0 &&
                    equipmentCount == 0)
                {
                    percentComplete = 100;
                }
                else
                {
                    // (current / maximum) * 100 -> just using the local area
                    decimal temp = Convert.ToDecimal(localAreaCompleteCount) / Convert.ToDecimal(localAreaCount);
                    temp = temp * 100;
                    percentComplete = Convert.ToInt32(temp);

                    if (percentComplete < 1) percentComplete = 1;
                    if (percentComplete > 99) percentComplete = 100;
                }

                status.LocalAreaCompleteCount = localAreaCompleteCount;
                status.DistrictEquipmentTypeCompleteCount = equipmentCompleteCount;
                status.ProgressPercentage = percentComplete;
                status.DisplayRolloverMessage = true;

                dbContext.HetDistrictStatus.Update(status);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void UpdateStatusKickoff(DbAppContext dbContext, HetDistrictStatus status,
            int localAreaCompleteCount, int equipmentCompleteCount)
        {
            try
            {
                status.LocalAreaCompleteCount = localAreaCompleteCount;
                status.DistrictEquipmentTypeCompleteCount = equipmentCompleteCount;
                status.ProgressPercentage = 1;
                status.RolloverStartDate = DateTime.UtcNow;
                status.DisplayRolloverMessage = true;

                dbContext.HetDistrictStatus.Update(status);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void UpdateStatusComplete(DbAppContext dbContext, HetDistrictStatus status,
            int localAreaCompleteCount, int equipmentCompleteCount)
        {
            try
            {
                // determine the current fiscal year
                DateTime fiscalStart;

                if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
                {
                    fiscalStart = new DateTime(DateTime.UtcNow.AddYears(-1).Year, 4, 1);
                }
                else
                {
                    fiscalStart = new DateTime(DateTime.UtcNow.Year, 4, 1);
                }

                status.LocalAreaCompleteCount = localAreaCompleteCount;
                status.DistrictEquipmentTypeCompleteCount = equipmentCompleteCount;
                status.ProgressPercentage = 100;
                status.RolloverEndDate = DateTime.UtcNow;
                status.CurrentFiscalYear = fiscalStart.Year;
                status.NextFiscalYear = fiscalStart.Year + 1;
                status.DisplayRolloverMessage = true;

                dbContext.HetDistrictStatus.Update(status);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion
    }
}
