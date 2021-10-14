using Hangfire;
using HetsApi.Helpers;
using HetsData.Helpers;
using HetsData.Entities;
using HetsData.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace HetsData.Hangfire
{
    public interface IAnnualRollover
    {
        public DistrictStatusDto GetRecord(int id);
        public void AnnualRolloverJob(int districtId, string seniorityScoringRules);
        public RolloverProgressDto KickoffProgress(int districtId);
    }

    public class AnnualRollover : IAnnualRollover
    {
        private DbAppContext _dbContextMain;
        private DbAppMonitorContext _dbContextMonitor;
        private string _jobId;
        private IMapper _mapper;
        private ILogger<AnnualRollover> _logger;

        public AnnualRollover(DbAppContext dbContextMain, DbAppMonitorContext dbContextMonitor, ILogger<AnnualRollover> logger, IMapper mapper)
        {
            _dbContextMain = dbContextMain;
            _dbContextMonitor = dbContextMonitor;
            _jobId = Guid.NewGuid().ToString();
            _mapper = mapper;
            _logger = logger;
        }

        #region Get a District Status record

        /// <summary>
        /// Get a District Status record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_context"></param>
        /// <returns></returns>
        public DistrictStatusDto GetRecord(int id)
        {
            HetDistrictStatus status = _dbContextMain.HetDistrictStatuses
                .Include(a => a.District)
                .FirstOrDefault(a => a.DistrictId == id);

            if (status == null)
            {
                var rolloverYear = FiscalHelper.GetCurrentFiscalStartYear();

                return new DistrictStatusDto
                {
                    DistrictId = id,
                    CurrentFiscalYear = rolloverYear - 1,
                    NextFiscalYear = rolloverYear,
                    DisplayRolloverMessage = false
                };
            }
            else
            {
                return _mapper.Map<DistrictStatusDto>(status);
            }
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
                HetDistrict district = _dbContextMain.HetDistricts
                    .FirstOrDefault(x => x.DistrictId == districtId);

                if (district == null)
                {
                    WriteLog("District not found");
                    return;
                }

                WriteLog("Starting - District #" + district.DistrictNumber);

                // get status record - and ensure we're active
                HetDistrictStatus status = GetStatus(districtId);

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
                List<HetDistrictEquipmentType> equipmentTypes = _dbContextMain.HetDistrictEquipmentTypes
                    .Include(x => x.EquipmentType)
                    .Where(x => x.DistrictId == districtId).ToList();

                // get all local areas
                List<HetLocalArea> localAreas = _dbContextMain.HetLocalAreas
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

                        List<HetEquipment> data = _dbContextMain.HetEquipments
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

                        SeniorityListHelper.AssignBlocks(localAreaId, equipmentTypeId, blockSize, totalBlocks, _dbContextMain);

                        // increment counters and update status
                        equipmentCompleteCount++;
                        WriteLog($"Equipment Type ({equipmentCompleteCount}/{equipmentTypes.Count})");
                    }

                    // increment counters and update status
                    localAreaCompleteCount++;
                    UpdateProgress(districtId, localAreaCompleteCount, localAreas.Count);
                    WriteLog($"Local Area ({localAreaCompleteCount}/{localAreas.Count})");
                }

                // done!
                UpdateStatusComplete(status, localAreaCompleteCount, equipmentCompleteCount);
                _dbContextMain.SaveChanges(); //commit;
                UpdateProgress(districtId, 0, 0, true);

                WriteLog("Rollover Save Completed");

                // **********************************************************
                // regenerate Owner Secret Keys for this district
                // **********************************************************
                WriteLog("Generate New Secret Keys - District #" + districtId);

                // get records
                List<HetOwner> owners = _dbContextMain.HetOwners
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
                    HetOwner ownerRecord = _dbContextMain.HetOwners.First(x => x.OwnerId == owner.OwnerId);
                    ownerRecord.SharedKey = key;

                    WriteLog($"Owner ({i}/{ownerCount})");
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

        private void UpdateProgress(int districtId, int localAreaCompleteCount, int localAreaCount, bool finished = false)
        {
            decimal percentage = localAreaCount == 0 ? 80 : Convert.ToDecimal(localAreaCompleteCount) / Convert.ToDecimal(localAreaCount) * 80;

            if (finished) percentage = 100;

            try
            {
                var progress = _dbContextMonitor.HetRolloverProgresses.FirstOrDefault(x => x.DistrictId == districtId);
                if (progress == null)
                {
                    progress = new HetRolloverProgress { DistrictId = districtId, ProgressPercentage = (int?)percentage };
                    _dbContextMonitor.Add(progress);
                }
                else
                {
                    progress.ProgressPercentage = (int?)percentage;
                }

                _dbContextMonitor.SaveChanges();                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public RolloverProgressDto KickoffProgress(int districtId)
        {
            try
            {
                var progress = _dbContextMonitor.HetRolloverProgresses.FirstOrDefault(x => x.DistrictId == districtId);

                if (progress == null)
                {
                    progress = new HetRolloverProgress { DistrictId = districtId, ProgressPercentage = (int?)0 };
                    _dbContextMonitor.HetRolloverProgresses.Add(progress);
                }
                else
                {
                    progress.ProgressPercentage = (int?)0;
                }

                _dbContextMonitor.SaveChanges();

                return new RolloverProgressDto { DistrictId = districtId, ProgressPercentage = 0 };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private HetDistrictStatus GetStatus(int id)
        {
            HetDistrictStatus status = _dbContextMain
                .HetDistrictStatuses
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

                _dbContextMain.HetDistrictStatuses.Add(status);
            }

            return status;
        }

        private void WriteLog(string message)
        {
            _logger.LogInformation($"Annual Rollover[{_jobId}] {message}");
        }

        #endregion

    }
}
