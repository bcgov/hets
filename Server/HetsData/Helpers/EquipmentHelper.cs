using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsData.Entities;
using HetsApi.Helpers;
using HetsData.Dtos;

namespace HetsData.Helpers
{
    #region Equipment Models

    public class EquipmentLiteList
    {
        public int Id { get; set; }
        public string EquipmentCode { get; set; }
        public int? OwnerId { get; set; }
        public int? LocalAreaId { get; set; }
        public IEnumerable<int?> ProjectIds { get; set; }
        public int DistrictEquipmentTypeId { get; set; }
    }

    public class EquipmentAgreementSummary
    {
        public int Id { get; set; }
        public string EquipmentCode { get; set; }
        public List<int> AgreementIds { get; set; }
        public List<int?> ProjectIds { get; set; }
        public int DistrictEquipmentTypeId { get; set; }
    }

    public class EquipmentStatusDto
    {
        public string Status { get; set; }
        public string StatusComment { get; set; }
    }

    public class EquipmentRentalAgreementClone
    {
        public int EquipmentId { get; set; }
        public int AgreementToCloneId { get; set; }
        public int RentalAgreementId { get; set; }
    }

    public class DuplicateEquipmentDto
    {
        public int Id { get; set; }
        public string DistrictName { get; set; }
        public string SerialNumber { get; set; }
        public EquipmentDto DuplicateEquipment { get; set; }
    }

    public class EquipmentLiteDto
    {
        public int Id { get; set; }
        public string EquipmentType { get; set; }
        public string LocalArea { get; set; }
        public string Status { get; set; }
        public string OwnerName { get; set; }
        public int? OwnerId { get; set; }
        public bool IsHired { get; set; }
        public string SeniorityString { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string Year { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentPrefix { get; set; }
        public int EquipmentNumber { get; set; }
        public int AttachmentCount { get; set; }
        public DateTime? LastVerifiedDate { get; set; }
        public int SenioritySortOrder { get; set; }
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
    }

    public class EquipmentExtraLite
    {
        public int Id { get; set; }
        public string EquipmentCode { get; set; }
    }

    #endregion

    # region Equipment Code Model

    public class EquipmentCodeModel
    {
        public int EquipmentId { get; set; }
        public string EquipmentCode { get; set; }
        public int EquipmentNumber { get; set; }
    }

    #endregion

    public static class EquipmentHelper
    {
        #region Returns true if the equipment is on an active rotation list

        /// <summary>
        /// Returns true if the equipment is on an active rotation list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool RentalRequestStatus(int id, DbAppContext context)
        {
            // get rental request status type
            int? statusIdInProgress = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", context);
            if (statusIdInProgress == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            return context.HetRentalRequestRotationLists.AsNoTracking()
                .Include(x => x.RentalRequest)
                .Any(x => x.EquipmentId == id &&
                          x.RentalRequest.RentalRequestStatusTypeId == statusIdInProgress);
        }

        #endregion

        #region Convert full equipment record to a "Lite" version

        /// <summary>
        /// Convert to Equipment Lite Model
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="scoringRules"></param>
        /// <param name="agreementStatusId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static EquipmentLiteDto ToLiteModel(HetEquipment equipment, SeniorityScoringRules scoringRules,
            int agreementStatusId, DbAppContext context)
        {
            EquipmentLiteDto equipmentLite = new EquipmentLiteDto();

            if (equipment != null)
            {
                // HETS - 709 [BVT - Adjust the search for Equipment search screen]
                /*
                int numberOfBlocks = 0;

                // get number of blocks for this equipment type
                if (equipment.DistrictEquipmentType != null)
                {
                    numberOfBlocks = equipment.DistrictEquipmentType.EquipmentType.IsDumpTruck
                        ? scoringRules.GetTotalBlocks("DumpTruck") + 1
                        : scoringRules.GetTotalBlocks() + 1;
                }

                // get equipment seniority
                float seniority = 0F;
                if (equipment.Seniority != null)
                {
                    seniority = (float)equipment.Seniority;
                }

                // get equipment block number
                int blockNumber = 0;
                if (equipment.BlockNumber != null)
                {
                    blockNumber = (int)equipment.BlockNumber;
                }

                // get equipment block number
                int numberInBlock = 0;
                if (equipment.NumberInBlock != null)
                {
                    numberInBlock = (int)equipment.NumberInBlock;
                }
                */

                // map data to light model
                equipmentLite.Id = equipment.EquipmentId;

                if (equipment.DistrictEquipmentType != null)
                {
                    equipmentLite.EquipmentType = equipment.DistrictEquipmentType.DistrictEquipmentName;
                }

                if (equipment.Owner != null)
                {
                    equipmentLite.OwnerName = equipment.Owner.OrganizationName;
                    equipmentLite.OwnerId = equipment.OwnerId;
                }

                // HETS - 709 [BVT - Adjust the search for Equipment search screen]
                //equipmentLite.SeniorityString = FormatSeniorityString(seniority, blockNumber, numberOfBlocks);
                equipmentLite.SeniorityString = "0";

                // HETS - 709 [BVT - Adjust the search for Equipment search screen]
                //equipmentLite.IsHired = CheckIsHired(equipment.HetRentalAgreement.ToList());
                equipmentLite.IsHired = false;

                // HETS - 709 [BVT - Adjust the search for Equipment search screen]
                //equipmentLite.SenioritySortOrder = CalculateSenioritySortOrder(blockNumber, numberInBlock);
                equipmentLite.SenioritySortOrder = 0;

                equipmentLite.Make = equipment.Make;
                equipmentLite.Model = equipment.Model;
                equipmentLite.Size = equipment.Size;
                equipmentLite.Year = equipment.Year;
                equipmentLite.EquipmentCode = equipment.EquipmentCode;
                equipmentLite.EquipmentPrefix = Regex.Match(equipment.EquipmentCode, @"^[^\d-]+").Value;

                string temp = "";

                if (!string.IsNullOrEmpty(equipmentLite.EquipmentPrefix))
                {
                    temp = equipment.EquipmentCode.Replace(equipmentLite.EquipmentPrefix, "");
                }

                temp = temp.Replace("-", "");

                equipmentLite.EquipmentNumber = !string.IsNullOrEmpty(temp) ?
                    int.Parse(Regex.Match(temp, @"\d+").Value) :
                    0;

                equipmentLite.AttachmentCount = CalculateAttachmentCount(equipment.HetEquipmentAttachments.ToList());
                equipmentLite.LastVerifiedDate = equipment.LastVerifiedDate;
                equipmentLite.Status = equipment.EquipmentStatusType.EquipmentStatusTypeCode;
                equipmentLite.LocalArea = equipment.LocalArea.Name;

                // get project
                HetRentalAgreement agreement = context.HetRentalAgreements
                    .AsNoTracking()
                    .Include(x => x.Project)
                    .Include(x => x.Equipment)
                    .FirstOrDefault(x => x.RentalAgreementStatusTypeId == agreementStatusId &&
                                         x.EquipmentId == equipmentLite.Id);

                if (agreement?.Project != null)
                {
                    equipmentLite.ProjectId = agreement.Project.ProjectId;
                    equipmentLite.ProjectName = agreement.Project.Name;
                }
            }

            return equipmentLite;
        }

        /// <summary>
        /// Create/format Seniority String
        /// </summary>
        public static string FormatSeniorityString(float seniority = 0F, int blockNumber = 0, int numberOfBlocks = 3)
        {
            // E.g. For equipment with 3 blocks
            // 1 - 133.277
            // 2 - 323.333
            // Open - 21.333
            // The last block is always open
            // HETS-968 - Rotation list -Wrong Block number for Open block
            if (blockNumber == numberOfBlocks) blockNumber = 3;

            return $"{blockNumber} - {seniority:0.###}";
        }

        /// <summary>
        /// Function to create a sortable value for the seniority column
        /// Calculate "seniority sort order" & round the seniority value (3 decimal places)
        /// </summary>
        public static int CalculateSenioritySortOrder(int blockNumber = 0, int numberInBlock = 0)
        {
            return (blockNumber * 100) + numberInBlock;
        }

        /// <summary>
        /// Calculate attachment count
        /// </summary>
        public static int CalculateAttachmentCount(List<HetEquipmentAttachment> attachments)
        {
            return attachments?.Count ?? 0;
        }

        #endregion

        #region Check if the Equipment is currently hired (on an Active Agreement)

        /// <summary>
        /// Check if the Equipment is currently hired (on an Active Agreement)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        public static bool IsHired(int id, DbAppContext context)
        {
            // add an "IsHired" flag to indicate if this equipment is currently in use
            IQueryable<HetRentalAgreement> agreements = context.HetRentalAgreements.AsNoTracking()
                .Where(x => x.RentalAgreementStatusType.RentalAgreementStatusTypeCode.Equals(HetRentalAgreement.StatusActive));

            return agreements.Any(x => x.EquipmentId == id);
        }

        #endregion

        #region Get the number of blocks for the equipment type

        /// <summary>
        /// Get the number of blocks for the equipment type
        /// </summary>
        /// <param name="item"></param>
        /// <param name="configuration"></param>
        public static int GetNumberOfBlocks(HetEquipment item, IConfiguration configuration, Action<string, Exception> logErrorAction)
        {
            int numberOfBlocks = -1;

            try
            {
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration, logErrorAction);

                numberOfBlocks = item.DistrictEquipmentType.EquipmentType.IsDumpTruck ?
                    scoringRules.GetTotalBlocks("DumpTruck") + 1 : scoringRules.GetTotalBlocks() + 1;
            }
            catch
            {
                // do nothing
            }

            return numberOfBlocks;
        }

        #endregion

        #region Get the YTD service hours for a piece of equipment

        /// <summary>
        /// Get the YTD service hours for a piece of equipment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <param name="rolloverDate"></param>
        public static float GetYtdServiceHours(int id, DbAppContext context)
        {
            float result = 0.0F;

            // *******************************************************************************
            // determine current fiscal year - check for existing rotation lists this year
            // * the rollover uses the dates from the status table (rolloverDate)
            // *******************************************************************************
            HetEquipment equipment = context.HetEquipments.AsNoTracking()
                .Include(x => x.LocalArea.ServiceArea.District)
                .First(x => x.EquipmentId == id);

            HetDistrictStatus district = context.HetDistrictStatuses.AsNoTracking()
                .First(x => x.DistrictId == equipment.LocalArea.ServiceArea.DistrictId);

            var fiscalYear = DateTime.Today.Year;

            if (district?.NextFiscalYear == null)
            {
                fiscalYear = FiscalHelper.GetCurrentFiscalStartYear() + 1;
            }
            else
            {
                fiscalYear = (int)district.NextFiscalYear; // status table uses the start of the year
            }

            DateTime fiscalEnd = new DateTime(fiscalYear, 3, 31);
            DateTime fiscalStart = new DateTime(fiscalYear - 1, 4, 1);

            // *******************************************************************************
            // get all the time data for the current fiscal year
            // *******************************************************************************
            float? summation = context.HetTimeRecords.AsNoTracking()
                .Include(x => x.RentalAgreement.Equipment)
                .Where(x => x.RentalAgreement.EquipmentId == id &&
                            x.WorkedDate >= fiscalStart &&
                            x.WorkedDate <= fiscalEnd)
                .Sum(x => x.Hours);

            if (summation != null)
            {
                result = (float)summation;
            }

            return result;
        }

        #endregion

        #region Recalculate Equipment's Seniority

        /// <summary>
        /// Recalculates seniority for a specific local area and equipment type
        /// </summary>
        public static void RecalculateSeniority(int? localAreaId, int? districtEquipmentTypeId,
            DbAppContext context, IConfiguration configuration, Action<string, Exception> logErrorAction, HetEquipment changedEquipment = null)
        {
            IConfigurationSection scoringRules = configuration.GetSection("SeniorityScoringRules");
            string seniorityScoringRules = GetConfigJson(scoringRules);

            RecalculateSeniority(localAreaId, districtEquipmentTypeId, context, seniorityScoringRules, logErrorAction, changedEquipment);
        }

        /// <summary>
        /// Recalculates seniority for a specific local area and equipment type
        /// </summary>
        public static void RecalculateSeniority(int? localAreaId, int? districtEquipmentTypeId,
            DbAppContext context, string seniorityScoringRules, Action<string, Exception> logErrorAction, HetEquipment changedEquipment = null)
        {
            // check if the local area exists
            bool exists = context.HetLocalAreas.Any(a => a.LocalAreaId == localAreaId);

            if (!exists) throw new ArgumentException("Local Area is invalid");

            // check if the equipment type exists
            exists = context.HetDistrictEquipmentTypes
                .Any(a => a.DistrictEquipmentTypeId == districtEquipmentTypeId);

            if (!exists) throw new ArgumentException("District Equipment Type is invalid");

            // get the local area
            HetLocalArea localArea = context.HetLocalAreas
                .First(a => a.LocalAreaId == localAreaId);

            // get the equipment type
            HetDistrictEquipmentType districtEquipmentType = context.HetDistrictEquipmentTypes
                .Include(x => x.EquipmentType)
                .First(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId);

            // recalculate the seniority list
            SeniorityListHelper.CalculateSeniorityList(localArea.LocalAreaId,
                districtEquipmentType.DistrictEquipmentTypeId,
                context,
                seniorityScoringRules, 
                logErrorAction, 
                changedEquipment);
        }

        #endregion

        #region Get Scoring Rules

        private static string GetConfigJson(IConfigurationSection scoringRules)
        {
            string jsonString = RecurseConfigJson(scoringRules);

            if (jsonString.EndsWith("},"))
            {
                jsonString = jsonString.Substring(0, jsonString.Length - 1);
            }

            return jsonString;
        }

        private static string RecurseConfigJson(IConfigurationSection scoringRules)
        {
            StringBuilder temp = new StringBuilder();

            temp.Append("{");

            // check for children
            foreach (IConfigurationSection section in scoringRules.GetChildren())
            {
                temp.Append(@"""" + section.Key + @"""" + ":");

                if (section.Value == null)
                {
                    temp.Append(RecurseConfigJson(section));
                }
                else
                {
                    temp.Append(@"""" + section.Value + @"""" + ",");
                }
            }

            string jsonString = temp.ToString();

            if (jsonString.EndsWith(","))
            {
                jsonString = jsonString.Substring(0, jsonString.Length - 1);
            }

            jsonString = jsonString + "},";
            return jsonString;
        }

        #endregion

        #region Create Equipment Code

        /// <summary>
        /// Generate an equipment code
        /// </summary>
        /// <param name="ownerEquipmentCodePrefix"></param>
        /// <param name="equipmentNumber"></param>
        public static string GenerateEquipmentCode(string ownerEquipmentCodePrefix, int equipmentNumber)
        {
            string result = ownerEquipmentCodePrefix + equipmentNumber.ToString("D3");
            return result;
        }

        #endregion

        #region Get new equipment code

        public static string GetEquipmentCode(int ownerId, DbAppContext context)
        {
            // get equipment owner
            HetOwner owner = context.HetOwners.AsNoTracking()
                .FirstOrDefault(x => x.OwnerId == ownerId);

            if (owner != null)
            {
                string ownerCode = owner.OwnerCode;
                int equipmentNumber = 1;

                // get the last "added" equipment record
                // 1. convert code to numeric (extract the numeric portion)
                List<EquipmentCodeModel> equipmentList = (
                    from equip in context.HetEquipments
                    where equip.OwnerId == owner.OwnerId &&
                          equip.EquipmentCode.StartsWith(owner.OwnerCode)
                    select new EquipmentCodeModel
                    {
                        EquipmentId = equip.EquipmentId,
                        EquipmentCode = equip.EquipmentCode
                    })
                    .AsNoTracking()
                    .ToList();

                if (equipmentList.Any())
                {

                    foreach (EquipmentCodeModel equipment in equipmentList)
                    {
                        // TH-51215 Dupilicate Equipment IDs
                        // The previous code causes incorrect sorting when owner code containers numeric characters
                        string equipmentCode = equipment.EquipmentCode.Replace(ownerCode, "");

                        equipment.EquipmentNumber = int.Parse(Regex.Match(equipmentCode, @"\d+").Value);
                    }

                    // 2. sort by the numeric and get last equipment
                    equipmentList = equipmentList.OrderByDescending(x => x.EquipmentNumber).ToList();

                    // 3. get last equipment
                    HetEquipment lastEquipment = context.HetEquipments.AsNoTracking()
                        .FirstOrDefault(x => x.EquipmentId == equipmentList[0].EquipmentId);

                    if (lastEquipment != null)
                    {
                        bool looking = true;

                        // parse last equipment records id
                        if (lastEquipment.EquipmentCode.StartsWith(ownerCode))
                        {
                            string temp = lastEquipment.EquipmentCode.Replace(ownerCode, "");
                            bool isNumeric = int.TryParse(temp, out int lastEquipmentNumber);
                            if (isNumeric) equipmentNumber = lastEquipmentNumber + 1;
                        }
                        else
                        {
                            char[] testChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                            int index = lastEquipment.EquipmentCode.IndexOfAny(testChars);

                            if (index >= 0 && lastEquipment.EquipmentCode.Length > index)
                            {
                                string temp = lastEquipment.EquipmentCode.Substring(index,
                                    lastEquipment.EquipmentCode.Length - index);
                                bool isNumeric = int.TryParse(temp, out int lastEquipmentNumber);
                                if (isNumeric) equipmentNumber = lastEquipmentNumber + 1;

                                ownerCode = lastEquipment.EquipmentCode.Substring(0, index);
                            }
                        }

                        // generate a unique equipment number
                        while (looking)
                        {
                            string candidate = GenerateEquipmentCode(ownerCode, equipmentNumber);

                            if ((owner.HetEquipments).Any(x => x.EquipmentCode == candidate))
                            {
                                equipmentNumber++;
                            }
                            else
                            {
                                looking = false;
                            }
                        }
                    }
                }

                // return the new equipment code
                return GenerateEquipmentCode(ownerCode, equipmentNumber);
            }

            return null;
        }

        #endregion

        #region Get Equipment History

        public static List<History> GetHistoryRecords(int id, int? offset, int? limit, DbAppContext context)
        {
            HetEquipment equipment = context.HetEquipments.AsNoTracking()
                .Include(x => x.HetHistories)
                .First(a => a.EquipmentId == id);

            List<HetHistory> data = equipment.HetHistories
                .OrderByDescending(y => y.AppLastUpdateTimestamp)
                .ToList();

            if (offset == null)
            {
                offset = 0;
            }

            if (limit == null)
            {
                limit = data.Count - offset;
            }

            List<History> result = new List<History>();

            for (int i = (int)offset; i < data.Count && i < offset + limit; i++)
            {
                History temp = new History();

                if (data[i] != null)
                {
                    temp.HistoryText = data[i].HistoryText;
                    temp.Id = data[i].HistoryId;
                    temp.LastUpdateTimestamp = data[i].AppLastUpdateTimestamp;
                    temp.LastUpdateUserid = data[i].AppLastUpdateUserid;
                    temp.AffectedEntityId = data[i].EquipmentId;
                }

                result.Add(temp);
            }

            return result;
        }

        #endregion
    }
}
