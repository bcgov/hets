using Hangfire;
using HetsApi.Helpers;
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HetsData.Hangfire
{
    public interface IAnnualRollover
    {
        public HetDistrictStatus GetRecord(int id);
        public void AnnualRolloverJob(int districtId, string seniorityScoringRules);
    }

    public class AnnualRollover : IAnnualRollover
    {
        private DbAppContext _dbContextMain;
        private DbAppContext _dbContextSub;
        private string _jobId;

        public AnnualRollover(DbAppContext dbContextMain, DbAppContext dbContextSub)
        {
            _dbContextMain = dbContextMain;
            _dbContextSub = dbContextSub;
            _jobId = Guid.NewGuid().ToString();
        }

        #region Get a District Status record

        /// <summary>
        /// Get a District Status record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_context"></param>
        /// <returns></returns>
        public HetDistrictStatus GetRecord(int id)
        {
            HetDistrictStatus status = _dbContextMain.HetDistrictStatus
                .Include(a => a.District)
                .FirstOrDefault(a => a.DistrictId == id);

            // if there isn't a status - we'll add one now
            if (status == null)
            {
                var rolloverYear = FiscalHelper.GetCurrentFiscalStartYear();

                status = new HetDistrictStatus
                {
                    DistrictId = id,
                    CurrentFiscalYear = rolloverYear - 1,
                    NextFiscalYear = rolloverYear,
                    DisplayRolloverMessage = false
                };

                _dbContextMain.HetDistrictStatus.Add(status);
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
        [SkipSameJob]
        [AutomaticRetry(Attempts = 0)]
        public void AnnualRolloverJob(int districtId, string seniorityScoringRules)
        {
            try
            {
                var rolloverYear = FiscalHelper.GetCurrentFiscalStartYear();

                // get processing rules
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(seniorityScoringRules);

                // validate district id
                HetDistrict district = _dbContextMain.HetDistrict
                    .FirstOrDefault(x => x.DistrictId == districtId);

                if (district == null)
                {
                    WriteLog("District not found");
                    return;
                }

                WriteLog("Starting - District #" + district.DistrictNumber);

                // get status record - and ensure we're active
                HetDistrictStatus status = GetRecord(districtId);

                if (status == null)
                {
                    WriteLog("District Status not found");
                    return;
                }

                if (status.CurrentFiscalYear == rolloverYear)
                {
                    // return - cannot rollover again
                    WriteLog($"Annual Rollover for the fiscal year ({rolloverYear}/{rolloverYear+1}) of the district #{district.DistrictNumber} cannot be run again.");
                    return;
                }

                // get equipment status
                int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _dbContextMain);
                if (statusId == null)
                {
                    WriteLog("Equipment Status not found");
                    return;
                }

                //// determine the "Rollover Date" (required for testing)
                //DateTime rolloverDate = new DateTime(rolloverYear, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
                //status.CurrentFiscalYear = rolloverYear;
                //status.NextFiscalYear = rolloverYear + 1;

                // get all district equipment types
                List<HetDistrictEquipmentType> equipmentTypes = _dbContextMain.HetDistrictEquipmentType
                    .Include(x => x.EquipmentType)
                    .Where(x => x.DistrictId == districtId).ToList();

                // get all local areas
                List<HetLocalArea> localAreas = _dbContextMain.HetLocalArea
                    .Where(a => a.ServiceArea.DistrictId == districtId).ToList();

                // update status - job is kicked off
                int localAreaCompleteCount = 0;
                int equipmentCompleteCount = 0;

                // process all local areas and equipment types
                foreach (HetLocalArea localArea in localAreas)
                {
                    if (localArea.Name != null)
                    {
                        WriteLog("Local Area: " + localArea.Name);
                    }
                    else
                    {
                        WriteLog("Local Area ID: " + localArea.LocalAreaId);
                    }

                    // reset equipment counter
                    equipmentCompleteCount = 0;

                    foreach (HetDistrictEquipmentType equipmentType in equipmentTypes)
                    {
                        WriteLog($"Equipment Type: {equipmentType.EquipmentTypeId}");
                        // it this a dump truck?
                        bool isDumpTruck = equipmentType.EquipmentType.IsDumpTruck;

                        // get rules for scoring and seniority block
                        int seniorityScoring = isDumpTruck ? scoringRules.GetEquipmentScore("DumpTruck") : scoringRules.GetEquipmentScore();
                        int blockSize = isDumpTruck ? scoringRules.GetBlockSize("DumpTruck") : scoringRules.GetBlockSize();
                        int totalBlocks = isDumpTruck ? scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();

                        List<HetEquipment> data = _dbContextMain.HetEquipment
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
                            equipment.ServiceHoursLastYear = EquipmentHelper.GetYtdServiceHours(equipment.EquipmentId, _dbContextMain);
                            equipment.CalculateYearsOfService(DateTime.UtcNow);

                            // blank out the override reason
                            equipment.SeniorityOverrideReason = "";

                            // update the seniority score
                            equipment.CalculateSeniority(seniorityScoring);
                        }

                        // now update the rotation list
                        int localAreaId = localArea.LocalAreaId;
                        int equipmentTypeId = equipmentType.DistrictEquipmentTypeId;

                        SeniorityListHelper.AssignBlocks(localAreaId, equipmentTypeId, blockSize, totalBlocks, _dbContextMain, false);

                        // increment counters and update status
                        equipmentCompleteCount++;
                        WriteLog($"Equipment Type ({equipmentCompleteCount}/{equipmentTypes.Count}");
                    }

                    // increment counters and update status
                    localAreaCompleteCount++;
                    WriteLog($"Local Area ({localAreaCompleteCount}/{localAreas.Count}");
                }

                // done!
                UpdateStatusComplete(status, localAreaCompleteCount, equipmentCompleteCount);
                _dbContextMain.SaveChanges(); //commit;
                WriteLog("Save Completed");

                // **********************************************************
                // regenerate Owner Secret Keys for this district
                // **********************************************************
                WriteLog("Generate New Secret Keys - District #" + districtId);

                // get records
                List<HetOwner> owners = _dbContextMain.HetOwner
                    .Where(x => x.BusinessId == null &&
                                x.LocalArea.ServiceArea.DistrictId == districtId)
                    .ToList();

                int i = 0;
                int ownerCount = owners.Count;

                foreach (HetOwner owner in owners)
                {
                    i++;
                    string key = SecretKeyHelper.RandomString(8, owner.OwnerId);

                    string temp = owner.OwnerCode;

                    if (string.IsNullOrEmpty(temp))
                    {
                        temp = SecretKeyHelper.RandomString(4, owner.OwnerId);
                    }

                    key = temp + "-" + (rolloverYear + 1) + "-" + key;

                    // get owner and update
                    HetOwner ownerRecord = _dbContextMain.HetOwner.First(x => x.OwnerId == owner.OwnerId);
                    ownerRecord.SharedKey = key;

                    WriteLog($"Owner {i}/{ownerCount}");
                }

                // save remaining updates - done!
                _dbContextMain.SaveChangesForImport();
                WriteLog("Generate New Secret Keys - Done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void UpdateStatusComplete(HetDistrictStatus status, int localAreaCompleteCount, int equipmentCompleteCount)
        {
            var rolloverYear = FiscalHelper.GetCurrentFiscalStartYear();

            status.LocalAreaCompleteCount = localAreaCompleteCount;
            status.DistrictEquipmentTypeCompleteCount = equipmentCompleteCount;
            status.ProgressPercentage = 100;
            status.RolloverEndDate = DateTime.UtcNow;
            status.CurrentFiscalYear = rolloverYear;
            status.NextFiscalYear = rolloverYear + 1;
            status.DisplayRolloverMessage = true;
        }

        private void WriteLog(string message)
        {
            Console.WriteLine($"Annual Rollover[{_jobId}] {message}");
        }

        #endregion

    }
}
