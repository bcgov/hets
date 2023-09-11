using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using HetsCommon;
using HetsData.Dtos;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HetsData.Helpers
{
    #region Rental Request Models

    public class RentalRequestLite
    {
        public int Id { get; set; }
        public LocalAreaDto LocalArea { get; set; }
        public int? EquipmentCount { get; set; }
        public string EquipmentTypeName { get; set; }
        public string DistrictEquipmentName { get; set; }
        public string ProjectName { get; set; }
        public ContactDto PrimaryContact { get; set; }
        public string Status { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public int YesCount { get; set; }
    }

    public class RentalRequestHires
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int EquipmentId { get; set; }
        public string LocalAreaName { get; set; }
        public int ServiceAreaId { get; set; }
        public string OwnerCode { get; set; }
        public string CompanyName { get; set; }
        public string EquipmentCode { get; set; }
        public string EquipmentPrefix { get; set; }
        public int EquipmentNumber { get; set; }
        public string EquipmentMake { get; set; }
        public string EquipmentModel { get; set; }
        public string EquipmentSize { get; set; }
        public string EquipmentYear { get; set; }
        public int ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public DateTime? NoteDate { get; set; }
        public string NoteType { get; set; }
        public string Reason { get; set; }
        public string OfferResponseNote { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    #endregion

    public static class RentalRequestHelper
    {

        #region Convert full equipment record to a "Hires" version

        /// <summary>
        /// Convert to Rental Request Hires (Lite) Model
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static RentalRequestHires ToHiresModel(HetRentalRequestRotationList request, HetUser user)
        {
            RentalRequestHires requestLite = new();

            if (request != null)
            {
                requestLite.Id = request.RentalRequestRotationListId;
                requestLite.OwnerId = request.Equipment.OwnerId ?? 0;
                requestLite.EquipmentId = request.EquipmentId ?? 0;
                requestLite.LocalAreaName = request.RentalRequest.LocalArea.Name;
                requestLite.ServiceAreaId = request.RentalRequest.LocalArea.ServiceArea.ServiceAreaId;

                // owner data
                requestLite.OwnerCode = request.Equipment.Owner.OwnerCode;
                requestLite.CompanyName = request.Equipment.Owner.OrganizationName;

                // equipment data
                requestLite.EquipmentCode = request.Equipment.EquipmentCode;
                requestLite.EquipmentPrefix = Regex.Match(request.Equipment.EquipmentCode, @"^[^\d-]+").Value;
                requestLite.EquipmentNumber = int.Parse(Regex.Match(request.Equipment.EquipmentCode, @"\d+").Value);
                requestLite.EquipmentMake = request.Equipment.Make;
                requestLite.EquipmentModel = request.Equipment.Model;
                requestLite.EquipmentSize = request.Equipment.Size;
                requestLite.EquipmentYear = request.Equipment.Year;

                // project data
                requestLite.ProjectId = request.RentalRequest.Project.ProjectId;
                requestLite.ProjectNumber = request.RentalRequest.Project.ProvincialProjectNumber;
                requestLite.NoteDate = request.OfferResponseDatetime;

                // Note Type -
                // * Not hired (for recording the response NO for hiring.
                // * Force Hire -For force hiring an equipment
                requestLite.NoteType = "Not Hired"; // default
                requestLite.Reason = request.OfferRefusalReason;
                requestLite.OfferResponseNote = request.OfferResponseNote;

                if (request.IsForceHire != null && request.IsForceHire == true)
                {
                    requestLite.NoteType = "Force Hire";
                    requestLite.Reason = request.Note;
                }

                requestLite.UserId = request.AppCreateUserid;

                if (user != null)
                {
                    requestLite.UserName = user.GivenName ?? "";

                    if (requestLite.UserName.Length > 0)
                    {
                        requestLite.UserName = requestLite.UserName + " ";
                    }

                    requestLite.UserName = requestLite.UserName + user.Surname;
                }
            }

            return requestLite;
        }

        #endregion

        #region Calculate the Number of "Yes" responses to a Rental Request



        #endregion

        #region Get the number of blocks for the request / equipment type

        /// <summary>
        /// Get the number of blocks for this type of equipment
        /// </summary>
        /// <param name="item"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static int GetNumberOfBlocks(HetRentalRequest item, DbAppContext context, IConfiguration configuration, Action<string, Exception> logErrorAction)
        {
            int numberOfBlocks = -1;

            try
            {
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration, logErrorAction);

                // get record
                HetDistrictEquipmentType equipment = context.HetDistrictEquipmentTypes.AsNoTracking()
                    .Include(x => x.EquipmentType)
                    .FirstOrDefault(x => x.DistrictEquipmentTypeId == item.DistrictEquipmentTypeId);

                if (equipment == null) return 0;

                numberOfBlocks = equipment.EquipmentType.IsDumpTruck ?
                    scoringRules.GetTotalBlocks("DumpTruck") : scoringRules.GetTotalBlocks();
            }
            catch
            {
                // do nothing
            }

            return numberOfBlocks;
        }

        #endregion

        #region Create new Rental Request Rotation List

        /// <summary>
        /// Create Rental Request Rotation List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        public static HetRentalRequest CreateRotationList(
            HetRentalRequest request, DbAppContext context, IConfiguration configuration, IMapper mapper, Action<string, Exception> logErrorAction)
        {
            var hetRentalRequestRotationList = new List<HetRentalRequestRotationList>();

            request.HetRentalRequestRotationLists = hetRentalRequestRotationList;

            // validate input parameters
            if (request.LocalAreaId == null || request.DistrictEquipmentTypeId == null) return request;

            int currentSortOrder = 1;

            // get the number of blocks for this piece of equipment
            int numberOfBlocks = GetNumberOfBlocks(request, context, configuration, logErrorAction);
            numberOfBlocks++;

            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", context) 
                ?? throw new ArgumentException("Status Id cannot be null");

            // get the equipment based on the current seniority list for the area
            // (and sort the results based on block then
            //      numberInBlock -> ensures consistency with the UI)
            for (int currentBlock = 1; currentBlock <= numberOfBlocks; currentBlock++)
            {
                // start by getting the current set of equipment for the given equipment type
                List<HetEquipment> blockEquipment = context.HetEquipments.AsNoTracking()
                    .Where(x => x.DistrictEquipmentTypeId == request.DistrictEquipmentTypeId &&
                                x.BlockNumber == currentBlock &&
                                x.LocalAreaId == request.LocalAreaId &&
                                x.EquipmentStatusTypeId == statusId)
                    .OrderBy(x => x.NumberInBlock)
                    .ToList();

                int listSize = blockEquipment.Count;

                for (int i = 0; i < listSize; i++)
                {
                    HetRentalRequestRotationList rentalRequestRotationList = new()
                    {
                        Equipment = blockEquipment[i],
                        EquipmentId = blockEquipment[i].EquipmentId,
                        SeniorityFloat = blockEquipment[i].Seniority,
                        BlockNumber = blockEquipment[i].BlockNumber,
                        AppCreateTimestamp = DateTime.UtcNow,
                        RotationListSortOrder = currentSortOrder
                    };

                    hetRentalRequestRotationList.Add(rentalRequestRotationList);

                    //Rental Request Seniority List (Snapshot)
                    var seniorityListDto = mapper.Map<RentalRequestSeniorityListDto>(blockEquipment[i]);
                    seniorityListDto.YtdHours = EquipmentHelper.GetYtdServiceHours(seniorityListDto.EquipmentId, context);
                    request.HetRentalRequestSeniorityLists.Add(mapper.Map<HetRentalRequestSeniorityList>(seniorityListDto));

                    currentSortOrder++;
                }
            }

            // update the local area rotation list - find record #1
            request = SetupNewRotationList(request, numberOfBlocks, context);

            // remove equipment records
            foreach (HetRentalRequestRotationList rotationList in request.HetRentalRequestRotationLists)
            {
                rotationList.Equipment = null;
            }

            // Done!
            return request;
        }

        #endregion

        private static void DropHiredEquipment(List<HetRentalRequestRotationList> hetRentalRequestRotationList, DbAppContext context)
        {
            // check if any items have "Active" contracts - and drop them from the list
            // * ensure we ignore "blank" rental agreements (i.e. rental request is null)

            int? statusIdRentalAgreement = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", context);
            if (statusIdRentalAgreement == null) throw new ArgumentException("Status Id cannot be null");

            int listSize = hetRentalRequestRotationList.Count;

            for (int i = listSize - 1; i >= 0; i--)
            {
                bool agreementExists = context.HetRentalAgreements.AsNoTracking()
                    .Any(x => x.EquipmentId == hetRentalRequestRotationList[i].EquipmentId &&
                              x.RentalRequestId != null &&
                              x.RentalAgreementStatusTypeId == statusIdRentalAgreement);

                if (agreementExists)
                {
                    hetRentalRequestRotationList.Remove(hetRentalRequestRotationList[i]);
                }
            }
        }

        private static HetEquipment LastAskedByBlockInRotationList(
            int blockNumber, 
            int? districtEquipmentTypeId,
            int? localAreaId, 
            DateTime fiscalStart, 
            DbAppContext context, 
            List<HetRentalRequestRotationList> hetRentalRequestRotationList)
        {
            if (districtEquipmentTypeId == null || localAreaId == null) return null;

            // if this is not block 1 - check that we have "asked" anyone in the previous list
            var rotationListquery = context.HetRentalRequestRotationLists.AsNoTracking()
                .Include(x => x.RentalRequest)
                .Include(x => x.Equipment)
                .Where(x => x.RentalRequest.DistrictEquipmentTypeId == districtEquipmentTypeId &&
                            x.RentalRequest.LocalAreaId == localAreaId &&
                            x.RentalRequest.AppCreateTimestamp >= fiscalStart &&
                            x.BlockNumber == blockNumber && //use historical block number of the equipment
                            x.WasAsked == true &&
                            x.IsForceHire != true)
                .OrderByDescending(x => x.RentalRequestId)
                .ThenByDescending(x => x.RotationListSortOrder);

            foreach(var equipment in rotationListquery)
            {
                if (hetRentalRequestRotationList.Any(x => x.BlockNumber == blockNumber && x.EquipmentId == equipment.EquipmentId))
                    return equipment.Equipment;
            }

            return null;
        }

        private static HetEquipment LastAskedByBlockInSeniorityList(
            int blockNumber, 
            int? districtEquipmentTypeId,
            int? localAreaId, 
            DateTime fiscalStart, 
            DbAppContext context, 
            List<HetRentalRequestSeniorityList> hetRentalRequestSeniorityList)
        {
            if (districtEquipmentTypeId == null || localAreaId == null) return null;

            // if this is not block 1 - check that we have "asked" anyone in the previous list
            var rotationListquery = context.HetRentalRequestRotationLists.AsNoTracking()
                .Include(x => x.RentalRequest)
                .Include(x => x.Equipment)
                .Where(x => x.RentalRequest.DistrictEquipmentTypeId == districtEquipmentTypeId &&
                            x.RentalRequest.LocalAreaId == localAreaId &&
                            x.RentalRequest.AppCreateTimestamp >= fiscalStart &&
                            x.BlockNumber == blockNumber && //use historical block number of the equipment
                            x.WasAsked == true &&
                            x.IsForceHire != true)
                .OrderByDescending(x => x.RentalRequestId)
                .ThenByDescending(x => x.RotationListSortOrder);

            foreach (var equipment in rotationListquery)
            {
                if (hetRentalRequestSeniorityList.Any(x => x.BlockNumber == blockNumber && x.EquipmentId == equipment.EquipmentId))
                    return equipment.Equipment;
            }

            return null;
        }

        /// <summary>
        /// New Rotation List
        /// * Find Record Number #1
        ///
        /// Business rules
        /// * start from seniority list
        /// * remove hired equipments from the seniority list
        /// * if this is the first request of the new fiscal year -> Yes: start from #1
        /// * get the starting equipment (next equipment to be asked) for each block
        /// * sort the equipment for each block (considering rotation of the equipments in the block)
        ///    - first, starting equipment (next equipment to be asked) to the last equipment in the block
        ///    - then, starting the first equipment in the list to the one just before the next equipment to be asked
        /// </summary>
        /// <param name="rentalRequest"></param>
        /// <param name="numberOfBlocks"></param>
        /// <param name="context"></param>
        public static HetRentalRequest SetupNewRotationList(
            HetRentalRequest rentalRequest, int numberOfBlocks, DbAppContext context)
        {
            // remove hired equipment from the list
            DropHiredEquipment((List<HetRentalRequestRotationList>)rentalRequest.HetRentalRequestRotationLists, context);

            SetWorkingNow(rentalRequest);

            // nothing to do!
            if (rentalRequest.HetRentalRequestRotationLists.Count <= 0) return rentalRequest;

            // sort our new rotation list
            var hetRentalRequestRotationList = rentalRequest.HetRentalRequestRotationLists
                .OrderBy(x => x.RotationListSortOrder)
                .ToList();

            rentalRequest.HetRentalRequestRotationLists = hetRentalRequestRotationList;

            int? disEquipmentTypeId = rentalRequest.DistrictEquipmentTypeId;
            int? localAreaId = rentalRequest.LocalAreaId;

            // determine current fiscal year - check for existing rotation lists this year
            // HETS-1195: Adjust seniority list and rotation list for lists hired between Apr1 and roll over
            // ** Need to use the "rollover date" to ensure we don't include records created
            //    after April 1 (but before rollover)
            HetLocalArea localArea = context.HetLocalAreas.AsNoTracking()
                .Include(x => x.ServiceArea.District)
                .First(x => x.LocalAreaId == localAreaId);

            HetDistrictStatus districtStatus = context.HetDistrictStatuses.AsNoTracking()
                .First(x => x.DistrictId == localArea.ServiceArea.DistrictId);

            int fiscalYear = Convert.ToInt32(districtStatus.NextFiscalYear); // status table uses the start of the year
            rentalRequest.FiscalYear = fiscalYear;

            DateTime fiscalStart = districtStatus.RolloverEndDate ??
                DateUtils.ConvertPacificToUtcTime(
                    new DateTime(fiscalYear - 1, 4, 1, 0, 0, 0, DateTimeKind.Unspecified));

            // *****************************************************************
            // if we don't have a request for the current fiscal,
            // ** pick the first one in the list and we are done.
            // *****************************************************************
            if (!PreviousRequestExists(context, disEquipmentTypeId, localAreaId, fiscalStart))
            {
                var firstOnList = hetRentalRequestRotationList[0];
                rentalRequest.FirstOnRotationListId = firstOnList.EquipmentId;

                return rentalRequest; 
            }

            // *****************************************************************
            // use the previous rotation list to determine where we were
            // ** find the equipment after the last "asked in each block
            // ** locate the first equipment and its block number on list in the list
            // *****************************************************************
            int startBlockIndex = -1; //the block index of the first equipment in the new rotation list
            int startBlockNumber = -1;

            (HetEquipment equipment, int position)[] startEquipInBlock = new (HetEquipment, int)[numberOfBlocks];

            // find the equipment after the last asked in each block
            for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
            {
                var blockNumber = blockIndex + 1;
                startEquipInBlock[blockIndex].position = -1;

                // get the last asked equipment id for this "block". This method ensures that the returned equipment exists in our list.
                var lastEquipmentInRotationList = LastAskedByBlockInRotationList(
                    blockNumber, 
                    rentalRequest.DistrictEquipmentTypeId, 
                    rentalRequest.LocalAreaId,
                    fiscalStart, 
                    context, 
                    hetRentalRequestRotationList);

                var lastEquipmentInSeniorityList = LastAskedByBlockInSeniorityList(
                    blockNumber, 
                    rentalRequest.DistrictEquipmentTypeId, 
                    rentalRequest.LocalAreaId,
                    fiscalStart, 
                    context, 
                    rentalRequest.HetRentalRequestSeniorityLists.ToList());

                SetSeniorityListLastCalled(rentalRequest, lastEquipmentInSeniorityList);

                SetNextStartEquipWithSameBlock(
                    hetRentalRequestRotationList, lastEquipmentInRotationList, startEquipInBlock, blockNumber, blockIndex);
            }

            // find the starting equipment and its block number on the list
            for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
            {
                if (startEquipInBlock[blockIndex].equipment != null)
                {
                    startBlockNumber = (int)startEquipInBlock[blockIndex].equipment.BlockNumber;
                    startBlockIndex = startBlockNumber - 1;
                    rentalRequest.FirstOnRotationListId = startEquipInBlock[blockIndex].equipment.EquipmentId;
                    break;
                }
            }

            // *****************************************************************
            // Reset the rotation list sort order
            // *****************************************************************
            ResetRotationListSortOrder(
                startEquipInBlock, startBlockIndex, hetRentalRequestRotationList, startBlockNumber, numberOfBlocks);

            return rentalRequest;
        }

        private static void SetWorkingNow(HetRentalRequest rentalRequest)
        {
            // set working now - if an equipment is dropped, it's working now.
            foreach (var equipment in rentalRequest.HetRentalRequestSeniorityLists)
            {
                if (!rentalRequest.HetRentalRequestRotationLists.Any(x => x.EquipmentId == equipment.EquipmentId))
                {
                    equipment.WorkingNow = true;
                }
            }
        }

        private static bool PreviousRequestExists(
            DbAppContext context, int? disEquipmentTypeId, int? localAreaId, DateTime fiscalStart)
        {
            // get the last rotation list created this fiscal year
            return context.HetRentalRequests
                .Any(x =>
                    x.DistrictEquipmentType.DistrictEquipmentTypeId == disEquipmentTypeId
                    && x.LocalArea.LocalAreaId == localAreaId
                    && x.AppCreateTimestamp >= fiscalStart);
        }

        private static void SetSeniorityListLastCalled(
            HetRentalRequest rentalRequest, HetEquipment lastEquipmentInSeniorityList)
        {
            if (lastEquipmentInSeniorityList != null)
            {
                rentalRequest.HetRentalRequestSeniorityLists
                    .First(x => x.EquipmentId == lastEquipmentInSeniorityList.EquipmentId)
                    .LastCalled = true;
            }
        }

        private static void SetStartEquipInBlock(
            (HetEquipment equipment, int position)[] startEquipInBlock, int blockIndex, HetEquipment equipment, int position)
        {
            startEquipInBlock[blockIndex].equipment = equipment;
            startEquipInBlock[blockIndex].position = position;
        } 

        private static void SetNextStartEquipWithSameBlock(
            List<HetRentalRequestRotationList> hetRentalRequestRotationList, 
            HetEquipment lastEquipmentInRotationList,
            (HetEquipment equipment, int position)[] startEquipInBlock,
            int blockNumber, 
            int blockIndex)
        {
            int startIndex =
                (lastEquipmentInRotationList == null && hetRentalRequestRotationList.Count > 0) 
                    ? 0 // nothing found for this block - start at 0
                    : 1 + hetRentalRequestRotationList
                        .FindIndex(x => x.EquipmentId == lastEquipmentInRotationList.EquipmentId); // we know the equipment exists in the list

            // find the next record which has the same block
            for (int i = startIndex; i < hetRentalRequestRotationList.Count; i++)
            {
                if (hetRentalRequestRotationList[i].BlockNumber != blockNumber) continue;

                SetStartEquipInBlock(
                    startEquipInBlock, blockIndex, hetRentalRequestRotationList[i].Equipment, i);
                break;
            }

            // if we haven't found a start equip yet, choose the first one in the block.
            var foundIndex = hetRentalRequestRotationList.FindIndex(x => x.BlockNumber == blockNumber);
            if (startEquipInBlock[blockIndex].equipment == null && foundIndex >= 0)
            {
                SetStartEquipInBlock(
                    startEquipInBlock, blockIndex, hetRentalRequestRotationList[foundIndex].Equipment, foundIndex);
            }
        }

        private static void ResetRotationListSortOrder(
            (HetEquipment equipment, int position)[] startEquipInBlock,
            int startBlockIndex,
            List<HetRentalRequestRotationList> hetRentalRequestRotationList,
            int startBlockNumber,
            int numberOfBlocks)
        {
            int masterSortOrder = 0;

            #region starting block
            for (int i = startEquipInBlock[startBlockIndex].position; i < hetRentalRequestRotationList.Count; i++)
            {
                if (hetRentalRequestRotationList[i].BlockNumber != startBlockNumber)
                    break;

                masterSortOrder++;
                hetRentalRequestRotationList[i].RotationListSortOrder = masterSortOrder;
            }

            // finish the "first set" of records in the block (before the starting position)
            for (int i = 0; i < startEquipInBlock[startBlockIndex].position; i++)
            {
                if (hetRentalRequestRotationList[i].BlockNumber != startBlockNumber)
                    continue;

                masterSortOrder++;
                hetRentalRequestRotationList[i].RotationListSortOrder = masterSortOrder;
            }
            #endregion

            ResetRemainingBlocksSortOrder(
                startBlockIndex, numberOfBlocks, masterSortOrder, startEquipInBlock, hetRentalRequestRotationList);
        }

        private static void ResetRemainingBlocksSortOrder(
            int startBlockIndex, 
            int numberOfBlocks, 
            int masterSortOrder, 
            (HetEquipment equipment, int position)[] startEquipInBlock,
            List<HetRentalRequestRotationList> hetRentalRequestRotationList)
        {
            #region remaining blocks if any
            for (int blockIndex = startBlockIndex + 1; blockIndex < numberOfBlocks; blockIndex++)
            {
                var blockNumber = blockIndex + 1;
                for (int i = startEquipInBlock[blockIndex].position; i < hetRentalRequestRotationList.Count; i++)
                {
                    if (i < 0 || hetRentalRequestRotationList[i].BlockNumber != blockNumber)
                        break;

                    masterSortOrder++;
                    hetRentalRequestRotationList[i].RotationListSortOrder = masterSortOrder;
                }

                // finish the "first set" of records in the block (before the starting position)
                for (int i = 0; i < startEquipInBlock[blockIndex].position; i++)
                {
                    if (hetRentalRequestRotationList[i].BlockNumber != blockNumber)
                        continue;

                    masterSortOrder++;
                    hetRentalRequestRotationList[i].RotationListSortOrder = masterSortOrder;
                }
            }
            #endregion
        }

        /// <summary>
        /// Update the Rotation List
        /// </summary>
        /// <param name="request"></param>
        /// <param name="numberOfBlocks"></param>
        /// <param name="context"></param>
        public static void UpdateRotationList(HetRentalRequest request)
        {
            if (request.HetRentalRequestRotationLists.Count > 0)
            {
                request.HetRentalRequestRotationLists = request.HetRentalRequestRotationLists
                    .OrderBy(x => x.RotationListSortOrder).ToList();

                request.FirstOnRotationListId = request.HetRentalRequestRotationLists.ElementAt(0).Equipment.EquipmentId;
            }
        }

        #region Set Status of new Rental Request

        /// <summary>
        /// Set the Status of the Rental Request Rotation List
        /// </summary>
        /// <param name="rentalRequest"></param>
        /// <param name="context"></param>
        public static string RentalRequestStatus(HetRentalRequest rentalRequest, DbAppContext context)
        {
            string tempStatus = "New";

            // validate input parameters
            if (rentalRequest?.LocalAreaId != null && rentalRequest.DistrictEquipmentTypeId != null)
            {
                int? statusIdInProgress = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", context);
                if (statusIdInProgress == null) return null;

                // check if there is an existing "In Progress" Rental Request
                List<HetRentalRequest> requests = context.HetRentalRequests
                    .Where(x => x.DistrictEquipmentType.DistrictEquipmentTypeId == rentalRequest.DistrictEquipmentTypeId &&
                                x.LocalArea.LocalAreaId == rentalRequest.LocalAreaId &&
                                x.RentalRequestStatusTypeId == statusIdInProgress)
                    .ToList();

                tempStatus = requests.Count == 0 ? "In Progress" : "New";
            }

            return tempStatus;
        }

        #endregion

        #region Get Rental Request History

        public static List<History> GetHistoryRecords(int id, int? offset, int? limit, DbAppContext context)
        {
            HetRentalRequest request = context.HetRentalRequests.AsNoTracking()
                .Include(x => x.HetHistories)
                .First(a => a.RentalRequestId == id);

            List<HetHistory> data = request.HetHistories
                .OrderByDescending(y => y.AppLastUpdateTimestamp)
                .ToList();

            offset ??= 0;

            limit ??= data.Count - offset;

            List<History> result = new();

            for (int i = (int)offset; i < data.Count && i < offset + limit; i++)
            {
                History temp = new();

                if (data[i] != null)
                {
                    temp.HistoryText = data[i].HistoryText;
                    temp.Id = data[i].HistoryId;
                    temp.LastUpdateTimestamp = data[i].AppLastUpdateTimestamp;
                    temp.LastUpdateUserid = data[i].AppLastUpdateUserid;
                    temp.AffectedEntityId = data[i].RentalRequestId;
                }

                result.Add(temp);
            }

            return result;
        }

        #endregion
    }
}
