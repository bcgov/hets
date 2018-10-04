using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsData.Model;

namespace HetsData.Helpers
{
    #region Equipment Models

    public class EquipmentStatus
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

    public class DuplicateEquipmentModel
    {
        public int Id { get; set; }
        public string DistrictName { get; set; }
        public string SerialNumber { get; set; }
        public HetEquipment DuplicateEquipment { get; set; }
    }

    public class EquipmentLite
    {
        public int Id { get; set; }

        public string EquipmentType { get; set; }

        public string OwnerName { get; set; }

        public int? OwnerId { get; set; }

        public bool IsHired { get; set; }

        public string SeniorityString { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public string Size { get; set; }

        public string EquipmentCode { get; set; }

        public int AttachmentCount { get; set; }

        public DateTime? LastVerifiedDate { get; set; }

        public int SenioritySortOrder { get; set; }
    }

    #endregion

    public static class EquipmentHelper
    {
        #region Get an Equipment record (plus associated records)

        /// <summary>
        /// Get an Equipment record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static HetEquipment GetRecord(int id, DbAppContext context, IConfiguration configuration)
        {
            // retrieve updated equipment record to return to ui
            HetEquipment equipment = context.HetEquipment.AsNoTracking()
                .Include(x => x.EquipmentStatusType)
                .Include(x => x.LocalArea)
                    .ThenInclude(y => y.ServiceArea)
                        .ThenInclude(z => z.District)
                            .ThenInclude(a => a.Region)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Owner)
                .Include(x => x.HetEquipmentAttachment)
                .Include(x => x.HetNote)
                .Include(x => x.HetDigitalFile)
                .Include(x => x.HetHistory)
                .FirstOrDefault(a => a.EquipmentId == id);

            if (equipment != null)
            {
                equipment.IsHired = IsHired(id, context);
                equipment.NumberOfBlocks = GetNumberOfBlocks(equipment, configuration);
                equipment.HoursYtd = GetYtdServiceHours(id, context);
                equipment.Status = equipment.EquipmentStatusType.EquipmentStatusTypeCode;

                if (equipment.Seniority != null && equipment.BlockNumber != null)
                {
                    equipment.SeniorityString = FormatSeniorityString((float)equipment.Seniority, (int)equipment.BlockNumber, equipment.NumberOfBlocks);
                }                
            }
            
            return equipment;
        }

        #endregion

        #region Convert full equipment record to a "Lite" version

        /// <summary>
        /// Convert to Equipment Lite Model
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="scoringRules"></param>
        /// <returns></returns>
        public static EquipmentLite ToLiteModel(HetEquipment equipment, SeniorityScoringRules scoringRules)
        {
            EquipmentLite equipmentLite = new EquipmentLite();

            if (equipment != null)
            {
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

                equipmentLite.SeniorityString = FormatSeniorityString(seniority, blockNumber, numberOfBlocks);

                equipmentLite.IsHired = CheckIsHired(equipment.HetRentalAgreement.ToList());

                equipmentLite.Make = equipment.Make;
                equipmentLite.Model = equipment.Model;
                equipmentLite.Size = equipment.Size;
                equipmentLite.EquipmentCode = equipment.EquipmentCode;
                equipmentLite.AttachmentCount = CalculateAttachmentCount(equipment.HetEquipmentAttachment.ToList());
                equipmentLite.LastVerifiedDate = equipment.LastVerifiedDate;
                equipmentLite.SenioritySortOrder = CalculateSenioritySortOrder(blockNumber, numberInBlock);
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

            if (blockNumber < numberOfBlocks)
            {
                return string.Format("{0} - {1:0.###}", blockNumber, seniority);
            }

            return string.Format("Open - {0:0.###}", seniority);
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
        /// Function to determine if this piece of equipment is hired
        /// </summary>
        public static bool CheckIsHired(List<HetRentalAgreement> rentalAgreements)
        {
            if (rentalAgreements.Count == 0) return false;

            int? count = rentalAgreements.Count(x => x.RentalAgreementStatusType.RentalAgreementStatusTypeCode
                .Equals(HetRentalAgreement.StatusActive, StringComparison.InvariantCultureIgnoreCase));

            return count > 0;
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
            IQueryable<HetRentalAgreement> agreements = context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Equipment)
                .Where(x => x.RentalAgreementStatusType.RentalAgreementStatusTypeCode.Equals("Active", StringComparison.InvariantCultureIgnoreCase));

            return agreements.Any(x => x.Equipment.EquipmentId == id);
        }

        #endregion

        #region Get the number of blocks for the equipment type

        /// <summary>
        /// Get the number of blocks for the equipment type
        /// </summary>
        /// <param name="item"></param>
        /// <param name="configuration"></param>
        public static int GetNumberOfBlocks(HetEquipment item, IConfiguration configuration)
        {
            int numberOfBlocks = -1;

            try
            {
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration);

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
        public static float GetYtdServiceHours(int id, DbAppContext context)
        {
            float result = 0.0F;

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

            // *******************************************************************************
            // get all the time data for the current fiscal year
            // *******************************************************************************
            float? summation = context.HetTimeRecord.AsNoTracking()
                .Include(x => x.RentalAgreement.Equipment)
                .Where(x => x.RentalAgreement.Equipment.EquipmentId == id &&
                            x.WorkedDate >= fiscalStart)
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
            DbAppContext context, IConfiguration configuration)
        {
            // check if the local area exists
            bool exists = context.HetLocalArea.Any(a => a.LocalAreaId == localAreaId);

            if (!exists) throw new ArgumentException("Local Area is invalid");

            // check if the equipment type exists
            exists = context.HetDistrictEquipmentType
                .Any(a => a.DistrictEquipmentTypeId == districtEquipmentTypeId);

            if (!exists) throw new ArgumentException("District Equipment Type is invalid");
            
            // get the local area
            HetLocalArea localArea = context.HetLocalArea.AsNoTracking()
                .First(a => a.LocalAreaId == localAreaId);

            // get the equipment type
            HetDistrictEquipmentType districtEquipmentType = context.HetDistrictEquipmentType.AsNoTracking()
                .Include(x => x.EquipmentType)
                .First(x => x.DistrictEquipmentTypeId == districtEquipmentTypeId);

            // recalculate the seniority list
            SeniorityListHelper.CalculateSeniorityList(localArea.LocalAreaId, 
                districtEquipmentType.DistrictEquipmentTypeId, 
                districtEquipmentType.EquipmentType.EquipmentTypeId, 
                context,
                configuration);
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
            string result = ownerEquipmentCodePrefix + "-" + equipmentNumber.ToString("D4");
            return result;
        }

        #endregion

        #region Setup new Equipment Record

        /// <summary>
        /// Set the Equipment fields for a new record for fields that are not provided by the front end
        /// </summary>
        /// <param name="item"></param>
        /// <param name="context"></param>
        public static HetEquipment SetNewRecordFields(HetEquipment item, DbAppContext context)
        {
            item.ReceivedDate = DateTime.UtcNow;
            item.LastVerifiedDate = DateTime.UtcNow;

            // per JIRA HETS-536
            item.ApprovedDate = DateTime.UtcNow;

            item.Seniority = 0.0F;
            item.YearsOfService = 0.0F;
            item.ServiceHoursLastYear = 0.0F;
            item.ServiceHoursTwoYearsAgo = 0.0F;
            item.ServiceHoursThreeYearsAgo = 0.0F;
            item.ArchiveCode = "N";
            item.IsSeniorityOverridden = false;

            int tmpAreaId = item.LocalArea.LocalAreaId;
            item.LocalAreaId = tmpAreaId;
            item.LocalArea = null;
            
            int tmpEqipId = item.DistrictEquipmentType.DistrictEquipmentTypeId;
            item.DistrictEquipmentTypeId = tmpEqipId;
            item.DistrictEquipmentType = null;

            // new equipment MUST always start as unapproved - it isn't assigned to any block yet
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusUnapproved, "equipmentStatus", context);

            if (statusId == null)
            {
                throw new DataException("Status Id cannot be null");
            }

            item.EquipmentStatusTypeId = (int)statusId;

            // generate a new equipment code
            if (item.Owner != null)
            {
                // get equipment owner
                HetOwner owner = context.HetOwner.AsNoTracking()
                    .Include(x => x.HetEquipment)
                    .FirstOrDefault(x => x.OwnerId == item.Owner.OwnerId);

                if (owner != null)
                {
                    int equipmentNumber = 1;

                    if (owner.HetEquipment != null)
                    {
                        bool looking = true;
                        equipmentNumber = owner.HetEquipment.Count + 1;

                        // generate a unique equipment number
                        while (looking)
                        {
                            string candidate = GenerateEquipmentCode(owner.OwnerCode, equipmentNumber);

                            if ((owner.HetEquipment).Any(x => x.EquipmentCode == candidate))
                            {
                                equipmentNumber++;
                            }
                            else
                            {
                                looking = false;
                            }
                        }
                    }

                    // set the equipment code
                    item.EquipmentCode = GenerateEquipmentCode(owner.OwnerCode, equipmentNumber);                    
                }

                // cleanup owner reference
                int tmpOwnerId = item.Owner.OwnerId;
                item.OwnerId = tmpOwnerId;
                item.Owner = null;
            }            

            return item;
        }

        #endregion

        #region Get Equipment History

        public static List<History> GetHistoryRecords(int id, int? offset, int? limit, DbAppContext context)
        {
            HetEquipment equipment = context.HetEquipment.AsNoTracking()
                .Include(x => x.HetHistory)
                .First(a => a.EquipmentId == id);

            List<HetHistory> data = equipment.HetHistory
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
