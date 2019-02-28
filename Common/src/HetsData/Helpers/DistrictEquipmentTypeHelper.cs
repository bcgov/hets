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
    }

    #endregion

    public static class DistrictEquipmentTypeHelper
    {
        public static void MergeDistrictEquipmentTypes(PerformContext context, string seniorityScoringRules, 
            string connectionString)
        {
            context.WriteLine("Merge District Equipment Types Starting");

            IProgressBar progress = context.WriteProgressBar();
            progress.SetValue(0);

            // open a connection to the database
            DbAppContext dbContext = new DbAppContext(connectionString);

            // get equipment status
            int? equipmentStatusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", dbContext);
            if (equipmentStatusId == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            // get records
            List<MergeRecord> masterList = dbContext.HetDistrictEquipmentType.AsNoTracking()
                .Where(x => x.ServiceAreaId != null &&
                            x.Deleted == false)
                .OrderBy(x => x.DistrictEquipmentTypeId)
                .Select(x => new MergeRecord
                {
                    DistrictEquipmentTypeId = x.DistrictEquipmentTypeId,
                    DistrictEquipmentName = x.DistrictEquipmentName,
                    EquipmentPrefix = GetPrefix(x.DistrictEquipmentName),
                    DistrictId = x.DistrictId,
                    EquipmentTypeId = x.EquipmentTypeId
                })
                .ToList();

            int increment = 0;
            int? currentDistrict = -1;
            int? currentEquipmentType = -1;
            string currentPrefix = "";
            bool newMerge = true;

            foreach (MergeRecord detRecord in masterList)
            {
                if (detRecord.DistrictId != currentDistrict ||
                    detRecord.EquipmentTypeId != currentEquipmentType ||
                    detRecord.EquipmentPrefix != currentPrefix)
                {
                    newMerge = true;
                    currentDistrict = detRecord.DistrictId;
                    currentEquipmentType = detRecord.EquipmentTypeId;
                    currentPrefix = detRecord.EquipmentPrefix;
                }

                // kickoff the merge for this district, equipment type and prefix
                if (newMerge)
                {
                    int district = currentDistrict ?? -1;
                    int type = currentEquipmentType ?? -1;
                    string prefix = currentPrefix;
                    string masterName = "";

                    IEnumerable<HetDistrictEquipmentType> types = dbContext.HetDistrictEquipmentType
                        .Where(x => x.DistrictId == district &&
                                    x.EquipmentTypeId == type &&
                                    x.Deleted == false &&
                                    GetPrefix(x.DistrictEquipmentName) == prefix)
                        .OrderBy(x => x.DistrictEquipmentTypeId);

                    // create master name and update master record
                    foreach (HetDistrictEquipmentType equipmentType in types)
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
                    types.ElementAt(0).ServiceAreaId = null;

                    // switch equipment records over to the master record
                    foreach (HetDistrictEquipmentType equipmentType in types)
                    {
                        if (equipmentType.DistrictEquipmentTypeId != types.ElementAt(0).DistrictEquipmentTypeId)
                        {
                            IEnumerable<HetEquipment> equipments = dbContext.HetEquipment
                                .Where(x => x.DistrictEquipmentTypeId == equipmentType.DistrictEquipmentTypeId);

                            foreach (HetEquipment equipment in equipments)
                            {
                                equipment.DistrictEquipmentTypeId = types.ElementAt(0).DistrictEquipmentTypeId;
                            }

                            // delete record
                            HetRentalRequest request = dbContext.HetRentalRequest.AsNoTracking()
                                .FirstOrDefault(x => x.DistrictEquipmentTypeId == equipmentType.DistrictEquipmentTypeId);

                            HetLocalAreaRotationList rotationList = dbContext.HetLocalAreaRotationList.AsNoTracking()
                                .FirstOrDefault(x => x.DistrictEquipmentTypeId == equipmentType.DistrictEquipmentTypeId);

                            if (request != null || rotationList != null)
                            {
                                equipmentType.Deleted = true;
                            }
                            else
                            {
                                dbContext.HetDistrictEquipmentType.Remove(equipmentType);
                            }
                        }
                    }

                    // save changes to district equipment types and associated equipment records
                    dbContext.SaveChanges();

                    // finally - update the seniority and block assignments for the master record
                    List<HetLocalArea> localAreas = dbContext.HetEquipment.AsNoTracking()
                        .Include(x => x.LocalArea)
                        .Where(x => x.EquipmentStatusTypeId == equipmentStatusId)
                        .Select(x => x.LocalArea)
                        .Distinct()
                        .ToList();

                    foreach (HetLocalArea localArea in localAreas)
                    {
                        EquipmentHelper.RecalculateSeniority(localArea.LocalAreaId, 
                            types.ElementAt(0).DistrictEquipmentTypeId, dbContext, seniorityScoringRules);
                    }

                    // save changes to equipment records
                    dbContext.SaveChanges();
                }

                newMerge = false;

                // update status bar
                increment++;
                progress.SetValue(increment);
            }

            // done!
            progress.SetValue(100);
            context.WriteLine("Merge District Equipment Types Complete");
        }

        private static string GetPrefix(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Invalid District Equipment Name");

            int start = name.IndexOf("-", StringComparison.Ordinal);

            if (start <= 1) return name;

            return name.Substring(0, start).Trim();
        }
    }
}
