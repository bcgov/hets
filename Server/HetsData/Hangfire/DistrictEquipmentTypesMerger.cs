using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HetsData.Hangfire
{
    public class DistrictEquipmentTypesMerger
    {
        private DbAppContext _dbContext;
        private string _jobId;

        public DistrictEquipmentTypesMerger(DbAppContext dbContext)
        {
            _dbContext = dbContext;
            _jobId = Guid.NewGuid().ToString();
        }

        public void MergeDistrictEquipmentTypes(string seniorityScoringRules, Action<string> logInfoAction, Action<string, Exception> logErrorAction)
        {
            // get equipment status
            int? equipmentStatusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _dbContext);
            if (equipmentStatusId == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            // **************************************************
            // Phase 1: Identify Master District Equipment Types
            // **************************************************
            WriteLog("Phase 1: Identify Master District Equipment Types", logInfoAction);

            // get records
            List<MergeRecord> masterList = _dbContext.HetDistrictEquipmentTypes
                .Where(x => x.ServiceAreaId != null &&
                            x.Deleted == false)
                .Select(x => new MergeRecord
                {
                    DistrictEquipmentTypeId = x.DistrictEquipmentTypeId,
                    DistrictEquipmentName = x.DistrictEquipmentName,
                    EquipmentPrefix = GetPrefix(x.DistrictEquipmentName),
                    DistrictId = x.DistrictId,
                    EquipmentTypeId = x.EquipmentTypeId
                })
                .Distinct()
                .ToList();

            // sort the list accordingly
            masterList = masterList
                .OrderBy(x => x.DistrictId)
                .ThenBy(x => x.EquipmentTypeId)
                .ThenBy(x => x.EquipmentPrefix).ToList();

            int increment = 0;
            int? currentDistrict = -1;
            int? masterDistrictEquipmentTypeId = -1;
            int? currentEquipmentType = -1;
            string currentPrefix = "";

            foreach (MergeRecord detRecord in masterList)
            {
                bool newMerge;

                if (detRecord.DistrictId != currentDistrict ||
                    detRecord.EquipmentTypeId != currentEquipmentType ||
                    detRecord.EquipmentPrefix != currentPrefix)
                {
                    newMerge = true;
                    currentDistrict = detRecord.DistrictId;
                    currentEquipmentType = detRecord.EquipmentTypeId;
                    currentPrefix = detRecord.EquipmentPrefix;

                    masterDistrictEquipmentTypeId = detRecord.DistrictEquipmentTypeId;
                    detRecord.Master = true;
                    detRecord.MasterDistrictEquipmentTypeId = masterDistrictEquipmentTypeId;
                }
                else
                {
                    newMerge = false;
                    detRecord.Master = false;
                    detRecord.MasterDistrictEquipmentTypeId = masterDistrictEquipmentTypeId;
                }

                // kickoff the merge for this district, equipment type and prefix
                if (newMerge)
                {
                    int district = currentDistrict ?? -1;
                    int type = currentEquipmentType ?? -1;
                    string prefix = currentPrefix;
                    string masterName = "";

                    List<MergeRecord> types = masterList
                        .Where(x => x.DistrictId == district &&
                                    x.EquipmentTypeId == type &&
                                    x.EquipmentPrefix == prefix).ToList();

                    // create master name and update master record
                    foreach (MergeRecord equipmentType in types)
                    {
                        string temp = equipmentType.DistrictEquipmentName.Replace(currentPrefix, "").Trim();
                        int start = temp.IndexOf("-", StringComparison.Ordinal);
                        if (start > -1) start++;
                        int length = temp.Length - start < 0 ? 0 : temp.Length - start;
                        temp = temp.Substring(start, length).Trim();

                        masterName = masterName.Length > 0 ?
                            $"{masterName} | {temp}" :
                            temp;
                    }

                    masterName = $"{currentPrefix} - {masterName}";
                    types.ElementAt(0).DistrictEquipmentName = masterName;
                }

                // update status bar
                increment++;
            }

            // **************************************************
            // Phase 2: Update Master District Equipment Types
            // **************************************************
            WriteLog("Phase 2: Update Master District Equipment Types", logInfoAction);

            List<MergeRecord> masterRecords = masterList.Where(x => x.Master).ToList();

            increment = 0;

            foreach (MergeRecord detRecord in masterRecords)
            {
                // get det record & update name
                HetDistrictEquipmentType det = _dbContext.HetDistrictEquipmentTypes
                    .First(x => x.DistrictEquipmentTypeId == detRecord.DistrictEquipmentTypeId);

                det.DistrictEquipmentName = detRecord.DistrictEquipmentName;
                det.ServiceAreaId = null;

                // save changes to district equipment types and associated equipment records
                _dbContext.SaveChangesForImport();

                // update status bar
                increment++;
            }

            // **************************************************
            // Phase 3: Update Non-Master District Equipment Types
            // **************************************************
            WriteLog("Phase 3: Update Non-Master District Equipment Types", logInfoAction);

            List<MergeRecord> mergeRecords = masterList.Where(x => !x.Master).ToList();

            increment = 0;

            foreach (MergeRecord detRecord in mergeRecords)
            {
                int originalDistrictEquipmentTypeId = detRecord.DistrictEquipmentTypeId;
                int? newDistrictEquipmentTypeId = detRecord.MasterDistrictEquipmentTypeId;

                // get equipment & update
                IEnumerable<HetEquipment> equipmentRecords = _dbContext.HetEquipments
                    .Where(x => x.DistrictEquipmentTypeId == originalDistrictEquipmentTypeId);

                foreach (HetEquipment equipment in equipmentRecords)
                {
                    equipment.DistrictEquipmentTypeId = newDistrictEquipmentTypeId;
                }

                // save changes to associated equipment records
                _dbContext.SaveChangesForImport();

                // get det record
                HetDistrictEquipmentType det = _dbContext.HetDistrictEquipmentTypes
                    .First(x => x.DistrictEquipmentTypeId == originalDistrictEquipmentTypeId);

                // delete old det record
                HetRentalRequest request = _dbContext.HetRentalRequests
                    .FirstOrDefault(x => x.DistrictEquipmentTypeId == originalDistrictEquipmentTypeId);

                if (request != null)
                {
                    det.Deleted = true;
                }
                else
                {
                    _dbContext.HetDistrictEquipmentTypes.Remove(det);
                }

                // save changes to district equipment types and associated equipment records
                _dbContext.SaveChangesForImport();

                // update status bar
                increment++;
            }


            // **************************************************
            // Phase 4: Update seniority and block assignments
            // **************************************************
            WriteLog("Phase 4: Update seniority and block assignments", logInfoAction);

            increment = 0;

            foreach (MergeRecord detRecord in masterRecords)
            {
                // update the seniority and block assignments for the master record
                List<HetLocalArea> localAreas = _dbContext.HetEquipments
                    .Include(x => x.LocalArea)
                    .Where(x => x.EquipmentStatusTypeId == equipmentStatusId &&
                                x.DistrictEquipmentTypeId == detRecord.DistrictEquipmentTypeId)
                    .Select(x => x.LocalArea)
                    .Distinct()
                    .ToList();

                foreach (HetLocalArea localArea in localAreas)
                {
                    EquipmentHelper.RecalculateSeniority(localArea.LocalAreaId,
                        detRecord.DistrictEquipmentTypeId, _dbContext, seniorityScoringRules, logErrorAction);
                }

                // save changes to equipment records
                _dbContext.SaveChangesForImport();

                // update status bar
                increment++;
            }

            // done!

            // **************************************************
            // Phase 5: Cleanup "empty" District Equipment Types
            // **************************************************
            WriteLog("Phase 5: Cleanup empty District Equipment Types", logInfoAction);

            // get records
            List<HetDistrictEquipmentType> districtEquipmentTypes = _dbContext.HetDistrictEquipmentTypes.AsNoTracking()
                .Include(x => x.HetEquipments)
                .Where(x => x.Deleted == false)
                .Distinct()
                .ToList();

            increment = 0;

            foreach (HetDistrictEquipmentType districtEquipmentType in districtEquipmentTypes)
            {
                int districtEquipmentTypeId = districtEquipmentType.DistrictEquipmentTypeId;

                // does this det have any equipment records?
                if (districtEquipmentType.HetEquipments.Count < 1)
                {
                    // get det record
                    HetDistrictEquipmentType det = _dbContext.HetDistrictEquipmentTypes
                        .First(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId);

                    // delete old det record
                    HetRentalRequest request = _dbContext.HetRentalRequests.AsNoTracking()
                        .FirstOrDefault(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId);

                    if (request != null)
                    {
                        det.Deleted = true;
                    }
                    else
                    {
                        _dbContext.HetDistrictEquipmentTypes.Remove(det);
                    }

                    // save changes to district equipment types and associated equipment records
                    _dbContext.SaveChangesForImport();
                }

                // update status bar
                increment++;
            }

            // done!
        }

        private static string GetPrefix(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Invalid District Equipment Name");

            int start = name.IndexOf("-", StringComparison.Ordinal);

            if (start <= 1) return name;

            return name.Substring(0, start).Trim();
        }

        private void WriteLog(string message, Action<string> logInfoAction)
        {
            logInfoAction($"Seniority Calculator[{_jobId}] {message}");
        }
    }
}
