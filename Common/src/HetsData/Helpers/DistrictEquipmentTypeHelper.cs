using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire.Console;
using Hangfire.Console.Progress;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using HetsData.Model;

namespace HetsData.Helpers
{
    #region Merge Record Model

    public class MergeRecord
    {
        public int DistrictEquipmentTypeId { get; set; }
        public string DistrictEquipmentName { get; set; }
        public string EquipmentPrefix { get; set; }
        public int? DistrictId { get; set; }
        public int? EquipmentTypeId { get; set; }
        public bool Master { get; set; }
        public int? MasterDistrictEquipmentTypeId { get; set; }
    }

    #endregion

    public static class DistrictEquipmentTypeHelper
    {
        public static void MergeDistrictEquipmentTypes(PerformContext context, string seniorityScoringRules,
            string connectionString)
        {
            // open a connection to the database
            DbAppContext dbContext = new DbAppContext(connectionString);

            // get equipment status
            int? equipmentStatusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", dbContext);
            if (equipmentStatusId == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            // **************************************************
            // Phase 1: Identify Master District Equipment Types
            // **************************************************
            context.WriteLine("Phase 1: Identify Master District Equipment Types");
            IProgressBar progress = context.WriteProgressBar();
            progress.SetValue(0);

            // get records
            List<MergeRecord> masterList = dbContext.HetDistrictEquipmentType.AsNoTracking()
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
                progress.SetValue(Convert.ToInt32((decimal)(masterList.Count - (masterList.Count - increment)) / masterList.Count * 100));
            }

            // done!
            progress.SetValue(100);

            // **************************************************
            // Phase 2: Update Master District Equipment Types
            // **************************************************
            context.WriteLine("Phase 2: Update Master District Equipment Types");
            progress = context.WriteProgressBar();
            progress.SetValue(0);

            List<MergeRecord> masterRecords = masterList.Where(x => x.Master).ToList();

            increment = 0;

            foreach (MergeRecord detRecord in masterRecords)
            {
                // get det record & update name
                HetDistrictEquipmentType det = dbContext.HetDistrictEquipmentType
                    .First(x => x.DistrictEquipmentTypeId == detRecord.DistrictEquipmentTypeId);

                det.DistrictEquipmentName = detRecord.DistrictEquipmentName;
                det.ServiceAreaId = null;

                // save changes to district equipment types and associated equipment records
                dbContext.SaveChangesForImport();

                // update status bar
                increment++;
                progress.SetValue(Convert.ToInt32((decimal)(masterRecords.Count - (masterRecords.Count - increment)) / masterRecords.Count * 100));
            }

            // done!
            progress.SetValue(100);

            // **************************************************
            // Phase 3: Update Non-Master District Equipment Types
            // **************************************************
            context.WriteLine("Phase 3: Update Non-Master District Equipment Types");
            progress = context.WriteProgressBar();
            progress.SetValue(0);

            List<MergeRecord> mergeRecords = masterList.Where(x => !x.Master).ToList();

            increment = 0;

            foreach (MergeRecord detRecord in mergeRecords)
            {
                int originalDistrictEquipmentTypeId = detRecord.DistrictEquipmentTypeId;
                int? newDistrictEquipmentTypeId = detRecord.MasterDistrictEquipmentTypeId;

                // get equipment & update
                IEnumerable<HetEquipment> equipmentRecords = dbContext.HetEquipment
                    .Where(x => x.DistrictEquipmentTypeId == originalDistrictEquipmentTypeId);

                foreach (HetEquipment equipment in equipmentRecords)
                {
                    equipment.DistrictEquipmentTypeId = newDistrictEquipmentTypeId;
                }

                // save changes to associated equipment records
                dbContext.SaveChangesForImport();

                // get det record
                HetDistrictEquipmentType det = dbContext.HetDistrictEquipmentType
                    .First(x => x.DistrictEquipmentTypeId == originalDistrictEquipmentTypeId);

                // delete old det record
                HetRentalRequest request = dbContext.HetRentalRequest.AsNoTracking()
                    .FirstOrDefault(x => x.DistrictEquipmentTypeId == originalDistrictEquipmentTypeId);

                HetLocalAreaRotationList rotationList = dbContext.HetLocalAreaRotationList.AsNoTracking()
                    .FirstOrDefault(x => x.DistrictEquipmentTypeId == originalDistrictEquipmentTypeId);

                if (request != null || rotationList != null)
                {
                    det.Deleted = true;
                }
                else
                {
                    dbContext.HetDistrictEquipmentType.Remove(det);
                }

                // save changes to district equipment types and associated equipment records
                dbContext.SaveChangesForImport();

                // update status bar
                increment++;
                progress.SetValue(Convert.ToInt32((decimal)(mergeRecords.Count - (mergeRecords.Count - increment)) / mergeRecords.Count * 100));
            }

            // done!
            progress.SetValue(100);

            // **************************************************
            // Phase 4: Update seniority and block assignments
            // **************************************************
            context.WriteLine("Phase 4: Update seniority and block assignments");
            progress = context.WriteProgressBar();
            progress.SetValue(0);

            increment = 0;

            foreach (MergeRecord detRecord in masterRecords)
            {
                // update the seniority and block assignments for the master record
                List<HetLocalArea> localAreas = dbContext.HetEquipment.AsNoTracking()
                    .Include(x => x.LocalArea)
                    .Where(x => x.EquipmentStatusTypeId == equipmentStatusId &&
                                x.DistrictEquipmentTypeId == detRecord.DistrictEquipmentTypeId)
                    .Select(x => x.LocalArea)
                    .Distinct()
                    .ToList();

                foreach (HetLocalArea localArea in localAreas)
                {
                    EquipmentHelper.RecalculateSeniority(localArea.LocalAreaId,
                        detRecord.DistrictEquipmentTypeId, dbContext, seniorityScoringRules);
                }

                // save changes to equipment records
                dbContext.SaveChangesForImport();

                // update status bar
                increment++;
                progress.SetValue(Convert.ToInt32((decimal)(masterRecords.Count - (masterRecords.Count - increment)) / masterRecords.Count * 100));
            }

            // done!
            progress.SetValue(100);

            // **************************************************
            // Phase 5: Cleanup "empty" District Equipment Types
            // **************************************************
            context.WriteLine("Phase 5: Cleanup empty District Equipment Types");
            progress = context.WriteProgressBar();
            progress.SetValue(0);

            // get records
            List<HetDistrictEquipmentType> districtEquipmentTypes = dbContext.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.HetEquipment)
                .Where(x => x.Deleted == false)
                .Distinct()
                .ToList();

            increment = 0;

            foreach (HetDistrictEquipmentType districtEquipmentType in districtEquipmentTypes)
            {
                int districtEquipmentTypeId = districtEquipmentType.DistrictEquipmentTypeId;

                // does this det have any equipment records?
                if (districtEquipmentType.HetEquipment.Count < 1)
                {
                    // get det record
                    HetDistrictEquipmentType det = dbContext.HetDistrictEquipmentType
                        .First(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId);

                    // delete old det record
                    HetRentalRequest request = dbContext.HetRentalRequest.AsNoTracking()
                        .FirstOrDefault(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId);

                    HetLocalAreaRotationList rotationList = dbContext.HetLocalAreaRotationList.AsNoTracking()
                        .FirstOrDefault(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId);

                    if (request != null || rotationList != null)
                    {
                        det.Deleted = true;
                    }
                    else
                    {
                        dbContext.HetDistrictEquipmentType.Remove(det);
                    }

                    // save changes to district equipment types and associated equipment records
                    dbContext.SaveChangesForImport();
                }

                // update status bar
                increment++;
                progress.SetValue(Convert.ToInt32((decimal)(districtEquipmentTypes.Count - (districtEquipmentTypes.Count - increment)) / districtEquipmentTypes.Count * 100));
            }

            // done!
            progress.SetValue(100);
        }

        private static string GetPrefix(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Invalid District Equipment Name");

            int start = name.IndexOf("-", StringComparison.Ordinal);

            if (start <= 1) return name;

            return name.Substring(0, start).Trim();
        }
    }

    public class DistrictEquipmentTypeHire
    {
        public int DistrictEquipmentTypeId { get; set; }
        public string DistrictEquipmentName { get; set; }
        public List<int?> ProjectIds { get; set; }
    }
}
