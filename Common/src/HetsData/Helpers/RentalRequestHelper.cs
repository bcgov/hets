using System;
using System.Collections.Generic;
using System.Linq;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HetsData.Helpers
{
    #region Rental Request Models

    public class RentalRequestLite
    {
        public int Id { get; set; }
        public HetLocalArea LocalArea { get; set; }
        public int? EquipmentCount { get; set; }
        public string EquipmentTypeName { get; set; }
        public string DistrictEquipmentName { get; set; }
        public string ProjectName { get; set; }
        public HetContact PrimaryContact { get; set; }
        public string Status { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? ExpectedStartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
    }    

    #endregion

    public static class RentalRequestHelper
    {
        #region Get Rental Request Record

        /// <summary>
        /// Get rental request record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        public static HetRentalRequest GetRecord(int id, DbAppContext context)
        {
            HetRentalRequest request = context.HetRentalRequest.AsNoTracking()
                .Include(x => x.RentalRequestStatusType)
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(c => c.PrimaryContact)
                .Include(x => x.Project)
                    .ThenInclude(c => c.ProjectStatusType)
                .Include(x => x.HetRentalRequestAttachment)
                .Include(x => x.DistrictEquipmentType)
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(y => y.Equipment)
                        .ThenInclude(z => z.EquipmentStatusType)
                .FirstOrDefault(a => a.RentalRequestId == id);

            if (request != null)
            {
                request.Status = request.RentalRequestStatusType.RentalRequestStatusTypeCode;

                // calculate the Yes Count based on the RentalRequestList
                request.YesCount = CalculateYesCount(request);

                // calculate YTD hours for the equipment records
                if (request.HetRentalRequestRotationList != null)
                {
                    foreach (HetRentalRequestRotationList rotationList in request.HetRentalRequestRotationList)
                    {
                        if (rotationList.Equipment != null)
                        {
                            rotationList.Equipment.HoursYtd = EquipmentHelper.GetYtdServiceHours(rotationList.Equipment.EquipmentId, context);
                        }
                    }
                }
            }

            return request;
        }

        /// <summary>
        /// Get rental request record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scoringRules"></param>
        /// <param name="context"></param>
        public static HetRentalRequest GetRecordWithRotationList(int id, SeniorityScoringRules scoringRules, DbAppContext context)
        {
            HetRentalRequest request = context.HetRentalRequest.AsNoTracking()
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(y => y.EquipmentType)
                .Include(x => x.FirstOnRotationList)
                .Include(x => x.HetRentalRequestAttachment)
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(y => y.Equipment)
                        .ThenInclude(r => r.HetEquipmentAttachment)
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(y => y.Equipment)
                        .ThenInclude(r => r.LocalArea)
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(y => y.Equipment)
                        .ThenInclude(r => r.DistrictEquipmentType)
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(y => y.Equipment)
                        .ThenInclude(e => e.Owner)
                            .ThenInclude(c => c.PrimaryContact)
                .FirstOrDefault(a => a.RentalRequestId == id);            

            if (request != null)
            {
                // re-sort list using: LocalArea / District Equipment Type and SenioritySortOrder (desc)
                request.HetRentalRequestRotationList = request.HetRentalRequestRotationList
                    .OrderBy(e => e.RotationListSortOrder)
                    .ToList();

                // calculate the Yes Count based on the RentalRequestList
                request.YesCount = CalculateYesCount(request);

                // calculate YTD hours for the equipment records
                if (request.HetRentalRequestRotationList != null)
                {
                    foreach (HetRentalRequestRotationList rotationList in request.HetRentalRequestRotationList)
                    {
                        if (rotationList.Equipment != null)
                        {
                            int numberOfBlocks = 0;

                            // get number of blocks for this equipment type
                            if (rotationList.Equipment.DistrictEquipmentType != null)
                            {
                                numberOfBlocks = rotationList.Equipment.DistrictEquipmentType.EquipmentType.IsDumpTruck
                                    ? scoringRules.GetTotalBlocks("DumpTruck") + 1
                                    : scoringRules.GetTotalBlocks() + 1;
                            }

                            // get equipment seniority
                            float seniority = 0F;
                            if (rotationList.Equipment.Seniority != null)
                            {
                                seniority = (float)rotationList.Equipment.Seniority;
                            }

                            // get equipment block number
                            int blockNumber = 0;
                            if (rotationList.Equipment.BlockNumber != null)
                            {
                                blockNumber = (int)rotationList.Equipment.BlockNumber;
                            }
                            
                            rotationList.Equipment.HoursYtd = EquipmentHelper.GetYtdServiceHours(rotationList.Equipment.EquipmentId, context);
                            rotationList.Equipment.SeniorityString = EquipmentHelper.FormatSeniorityString(seniority, blockNumber, numberOfBlocks);
                        }
                    }
                }
            }

            return request;
        }

        #endregion

        #region Convert full equipment record to a "Lite" version

        /// <summary>
        /// Convert to Rental Request Lite Model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static RentalRequestLite ToLiteModel(HetRentalRequest request)
        {
            RentalRequestLite requestLite = new RentalRequestLite();

            if (request != null)
            {                
                if (request.DistrictEquipmentType != null)
                {
                    requestLite.EquipmentTypeName = request.DistrictEquipmentType.EquipmentType.Name;
                    requestLite.DistrictEquipmentName = request.DistrictEquipmentType.DistrictEquipmentName;
                }

                requestLite.Id = request.RentalRequestId;
                requestLite.LocalArea = request.LocalArea;

                if (request.Project != null)
                {
                    requestLite.PrimaryContact = request.Project.PrimaryContact;
                    requestLite.ProjectName = request.Project.Name;
                    requestLite.ProjectId = request.Project.ProjectId;
                }

                requestLite.Status = request.RentalRequestStatusType.Description;
                requestLite.EquipmentCount = request.EquipmentCount;
                requestLite.ExpectedEndDate = request.ExpectedEndDate;
                requestLite.ExpectedStartDate = request.ExpectedStartDate;                
            }

            return requestLite;
        }

        #endregion

        #region Calculate the Number of "Yes" responses to a Rental Request

        /// <summary>
        /// Check how many Yes' we currently have from Owners
        /// </summary>
        /// <returns></returns>
        public static int CalculateYesCount(HetRentalRequest rentalRequest)
        {
            int temp = 0;

            if (rentalRequest.HetRentalRequestRotationList != null)
            {
                foreach (HetRentalRequestRotationList equipment in rentalRequest.HetRentalRequestRotationList)
                {
                    if (equipment.OfferResponse != null &&
                        equipment.OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                    {
                        temp++;
                    }

                    if (equipment.IsForceHire != null &&
                        equipment.IsForceHire == true)
                    {
                        temp++;
                    }
                }
            }

            // set the current Yes / Forced Hire Count
            return temp;
        }

        #endregion

        #region Get the number of blocks for the request / equipment type

        /// <summary>
        /// Get the number of blocks for this type of equipment 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static int GetNumberOfBlocks(HetRentalRequest item, DbAppContext context, IConfiguration configuration)
        {
            int numberOfBlocks = -1;

            try
            {
                SeniorityScoringRules scoringRules = new SeniorityScoringRules(configuration);

                // get record
                HetDistrictEquipmentType equipment = context.HetDistrictEquipmentType.AsNoTracking()
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
        public static List<HetRentalRequestRotationList> CreateRotationList(HetRentalRequest request, DbAppContext context, IConfiguration configuration)
        {            
            List<HetRentalRequestRotationList> rotationLists = new List<HetRentalRequestRotationList>();

            // validate input parameters
            if (request?.LocalAreaId == null || request.DistrictEquipmentTypeId == null) return rotationLists;

            int currentSortOrder = 1;

            // get the number of blocks for this piece of equipment
            int numberOfBlocks = GetNumberOfBlocks(request, context, configuration);
            numberOfBlocks = numberOfBlocks + 1;

            int? statusIdRentalAgreement = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", context);
            if (statusIdRentalAgreement == null) throw new ArgumentException("Status Id cannot be null");

            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", context);
            if (statusId == null) throw new ArgumentException("Status Id cannot be null");

            // get the equipment based on the current seniority list for the area
            // (and sort the results based on block then 
            //      numberInBlock -> ensures consistency with the UI)
            for (int currentBlock = 1; currentBlock <= numberOfBlocks; currentBlock++)
            {
                // start by getting the current set of equipment for the given equipment type
                List<HetEquipment> blockEquipment = context.HetEquipment.AsNoTracking()
                    .Where(x => x.DistrictEquipmentTypeId == request.DistrictEquipmentTypeId &&
                                x.BlockNumber == currentBlock &&
                                x.LocalAreaId == request.LocalAreaId &&
                                x.EquipmentStatusTypeId == statusId)
                    .OrderBy(x => x.NumberInBlock)
                    .ToList();

                int listSize = blockEquipment.Count;

                // check if any items have "Active" contracts - and drop them from the list
                for (int i = listSize - 1; i >= 0; i--)
                {
                    bool agreementExists = context.HetRentalAgreement
                        .Any(x => x.EquipmentId == blockEquipment[i].EquipmentId &&
                                  x.RentalAgreementStatusTypeId == statusIdRentalAgreement);

                    if (agreementExists)
                    {
                        blockEquipment.RemoveAt(i);
                    }
                }

                // update list size for sorting
                listSize = blockEquipment.Count;

                // sets the rotation list sort order
                int currentPosition = 0;

                for (int i = 0; i < listSize; i++)
                {
                    HetRentalRequestRotationList rentalRequestRotationList = new HetRentalRequestRotationList
                    {
                        EquipmentId = blockEquipment[i].EquipmentId,
                        SeniorityFloat = blockEquipment[i].Seniority,
                        BlockNumber = blockEquipment[i].BlockNumber,
                        AppCreateTimestamp = DateTime.UtcNow,
                        RotationListSortOrder = currentSortOrder
                    };

                    rotationLists.Add(rentalRequestRotationList);

                    currentPosition++;
                    currentSortOrder++;

                    if (currentPosition >= listSize)
                    {
                        currentPosition = 0;
                    }
                }
            }

            // add the new rotation lists
            request.HetRentalRequestRotationList = rotationLists;

            // update the local area rotation list - find record #1
            SetupNewRotationList(request, numberOfBlocks, context);

            return rotationLists;
        }

        #endregion

        #region Setup Local Area Rotation Lists

        /// <summary>
        /// New Rotation List
        /// * Find Record Number 1
        /// * Then update the Local Area Rotation List
        /// 
        /// Business rules
        /// * is this the first request of the new fiscal year -> Yes: start from #1
        /// * get the "next equipment to be asked" from "LOCAL_AREA_ROTATION_LIST"
        ///   -> if this is Block 1 -> temporarily becomes #1 on the list
        ///   -> if not block 1 -> #1 i block 1 temporarily becomes #1 on the list
        /// * check all records before temporary #1
        ///   -> if a record was Force Hired -> make them #1
        /// </summary>
        /// <param name="rentalRequest"></param>
        /// <param name="numberOfBlocks"></param>
        /// <param name="context"></param>
        public static void SetupNewRotationList(HetRentalRequest rentalRequest, int numberOfBlocks, DbAppContext context)
        {
            // nothing to do!
            if (rentalRequest.HetRentalRequestRotationList.Count <= 0) return;            

            // sort our new rotation list
            rentalRequest.HetRentalRequestRotationList = rentalRequest.HetRentalRequestRotationList
                .OrderBy(x => x.RotationListSortOrder)
                .ToList();

            // check if we have a localAreaRotationList.askNextBlock"N" for the given local area
            bool localAreaRotationListExists = context.HetLocalAreaRotationList
                .Any(a => a.LocalArea.LocalAreaId == rentalRequest.LocalAreaId);

            HetLocalAreaRotationList newAreaRotationList;

            if (localAreaRotationListExists)
            {
                newAreaRotationList = context.HetLocalAreaRotationList
                    .First(a => a.LocalArea.LocalAreaId == rentalRequest.LocalAreaId);
            }
            else
            {
                // setup our new "local area rotation list"
                newAreaRotationList = new HetLocalAreaRotationList
                {
                    LocalAreaId = rentalRequest.LocalAreaId,
                    DistrictEquipmentTypeId = rentalRequest.DistrictEquipmentTypeId
                };
            }

            // determine current fiscal year - check for existing rotation lists this year
            DateTime fiscalStart;

            if (DateTime.UtcNow.Month == 1 || DateTime.UtcNow.Month == 2 || DateTime.UtcNow.Month == 3)
            {
                fiscalStart = new DateTime(DateTime.UtcNow.AddYears(-1).Year, 4, 1);
            }
            else
            {
                fiscalStart = new DateTime(DateTime.UtcNow.Year, 4, 1);
            }

            // get the last rotation list created this fiscal year
            bool previousRequestExists = context.HetRentalRequest
                .Any(x => x.DistrictEquipmentType.DistrictEquipmentTypeId == rentalRequest.DistrictEquipmentTypeId &&
                          x.LocalArea.LocalAreaId == rentalRequest.LocalAreaId &&
                          x.AppCreateTimestamp >= fiscalStart);

            // if we don't have a request -> pick block 1 / record 1 & we're done
            if (!previousRequestExists)
            {
                rentalRequest.FirstOnRotationListId = rentalRequest.HetRentalRequestRotationList.ElementAt(0).EquipmentId;

                int block = numberOfBlocks;

                if (rentalRequest.HetRentalRequestRotationList.ElementAt(0).BlockNumber != null)
                {
                    int? equipmentBlockNumber = rentalRequest.HetRentalRequestRotationList.ElementAt(0).BlockNumber;
                    if (equipmentBlockNumber != null) block = (int) equipmentBlockNumber;
                }

                if (block == 1)
                {
                    newAreaRotationList.AskNextBlock1Id = rentalRequest.HetRentalRequestRotationList.ElementAt(0).EquipmentId;
                    newAreaRotationList.AskNextBlock1Seniority = rentalRequest.HetRentalRequestRotationList.ElementAt(0).SeniorityFloat;
                    newAreaRotationList.AskNextBlock2Id = null;
                    newAreaRotationList.AskNextBlock2Seniority = null;
                    newAreaRotationList.AskNextBlockOpenId = null;
                }
                else if (block == 2)
                {
                    newAreaRotationList.AskNextBlock1Id = null;
                    newAreaRotationList.AskNextBlock1Seniority = null;
                    newAreaRotationList.AskNextBlock2Id = rentalRequest.HetRentalRequestRotationList.ElementAt(0).EquipmentId;
                    newAreaRotationList.AskNextBlock2Seniority = rentalRequest.HetRentalRequestRotationList.ElementAt(0).SeniorityFloat;
                    newAreaRotationList.AskNextBlockOpenId = null;
                }
                else
                {
                    newAreaRotationList.AskNextBlock1Id = null;
                    newAreaRotationList.AskNextBlock1Seniority = null;
                    newAreaRotationList.AskNextBlock2Id = null;
                    newAreaRotationList.AskNextBlock2Seniority = null;
                    newAreaRotationList.AskNextBlockOpenId = rentalRequest.HetRentalRequestRotationList.ElementAt(0).EquipmentId;
                }

                if (localAreaRotationListExists)
                {
                    context.HetLocalAreaRotationList.Update(newAreaRotationList);
                }
                else
                {
                    context.HetLocalAreaRotationList.Add(newAreaRotationList);
                }

                return; //done!
            }

            // use the previous rotation list to determine where we were
            // ** if we find a record that was force hired - this is where we continue
            // ** otherwise - we pick the record after the last hire
            // ** if all records in that block were ASKED - then we need to start the new list
            //    at the same point as the old list (2018-03-01)
            HetRentalRequest previousRentalRequest = context.HetRentalRequest.AsNoTracking()
                .Include(x => x.HetRentalRequestRotationList)
                    .ThenInclude(e => e.Equipment)
                .Where(x => x.DistrictEquipmentType.DistrictEquipmentTypeId == rentalRequest.DistrictEquipmentTypeId &&
                            x.LocalArea.LocalAreaId == rentalRequest.LocalAreaId &&
                            x.AppCreateTimestamp >= fiscalStart)
                .OrderByDescending(x => x.RentalRequestId)
                .First();

            // sort the previous rotation list
            previousRentalRequest.HetRentalRequestRotationList = previousRentalRequest.HetRentalRequestRotationList
                .OrderBy(x => x.RotationListSortOrder)
                .ToList();

            int[] nextRecordToAskIndex = new int[numberOfBlocks];
            int[] nextRecordToAskId = new int[numberOfBlocks];
            int[] nextRecordToAskBlock = new int[numberOfBlocks];
            int[] blockSize = new int[numberOfBlocks];
            float[] nextRecordToAskSeniority = new float[numberOfBlocks];
            int startBlock = -1;

            for (int b = 0; b < numberOfBlocks; b++)
            {
                nextRecordToAskIndex[b] = 0;
                nextRecordToAskId[b] = 0;
                nextRecordToAskBlock[b] = 0;
                blockSize[b] = previousRentalRequest.HetRentalRequestRotationList.Count(x => x.Equipment.BlockNumber == (b + 1));

                int blockRecordCount = 0;
                int blockRecordOneIndex = -1;

                for (int i = 0; i < previousRentalRequest.HetRentalRequestRotationList.Count; i++)
                {
                    // make sure we're working in the right block
                    if (previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).Equipment.BlockNumber != (b + 1))
                    {
                        continue; // move to next record
                    }

                    blockRecordCount++;

                    if (blockRecordOneIndex == -1)
                    {
                        blockRecordOneIndex = i;
                    }

                    // if this record hired previously - then skip
                    // if this record was asked but said no - then skip
                    if (previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).OfferResponse != null &&
                        (previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase) ||
                         previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).OfferResponse.Equals("No", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        if (blockRecordCount >= blockSize[b] && blockSize[b] > 1)
                        {
                            // we've reached the end of our block - and nothing has been found
                            // (i.e. all records in that block were ASKED)
                            // Therefore: start the new list at the same point as the old list
                            nextRecordToAskId[b] = previousRentalRequest.HetRentalRequestRotationList.ElementAt(blockRecordOneIndex).Equipment.EquipmentId;
                            nextRecordToAskBlock[b] = b;

                            // find this record in the new list...
                            for (int j = 0; j < rentalRequest.HetRentalRequestRotationList.Count; j++)
                            {
                                if (previousRentalRequest.HetRentalRequestRotationList.ElementAt(blockRecordOneIndex).Equipment.EquipmentId == rentalRequest.HetRentalRequestRotationList.ElementAt(j).Equipment.EquipmentId)
                                {
                                    nextRecordToAskIndex[b] = j;
                                    break;
                                }
                            }
                        }

                        continue; // move to next record
                    }

                    // if this record isn't on our NEW list - then we need to continue to the next record
                    bool found = false;

                    for (int j = 0; j < rentalRequest.HetRentalRequestRotationList.Count; j++)
                    {
                        if (previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).Equipment.EquipmentId == rentalRequest.HetRentalRequestRotationList.ElementAt(j).EquipmentId)
                        {
                            nextRecordToAskIndex[b] = j;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        continue; // move to next record
                    }

                    // we've found our next record - exit and update the lists
                    nextRecordToAskId[b] = previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).Equipment.EquipmentId;

                    if (previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).Equipment.Seniority == null)
                    {
                        nextRecordToAskSeniority[b] = 0.0F;
                    }
                    else
                    {
                        float? equipmentSeniority = previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).Equipment.Seniority;
                        if (equipmentSeniority != null) nextRecordToAskSeniority[b] = (float)equipmentSeniority;
                    }

                    nextRecordToAskBlock[b] = b;

                    break; //move to the next block
                }
            }

            // determine our start block
            for (int i = 0; i < blockSize.Length; i++)
            {
                if (blockSize[i] > 0)
                {
                    startBlock = i;
                    break; // found our start block - we're done!
                }
            }

            // nothing found - defaulting to the first in the new list!
            if (nextRecordToAskId[startBlock] == 0)
            {
                int? equipmentId = rentalRequest.HetRentalRequestRotationList.ElementAt(0).EquipmentId;
                if (equipmentId != null) nextRecordToAskId[startBlock] = (int)equipmentId;

                if (rentalRequest.HetRentalRequestRotationList.ElementAt(0).SeniorityFloat != null)
                {
                    float? equipmentSeniority = rentalRequest.HetRentalRequestRotationList.ElementAt(0).SeniorityFloat;
                    if (equipmentSeniority != null) nextRecordToAskSeniority[startBlock] = (float)equipmentSeniority;
                }

                if (rentalRequest.HetRentalRequestRotationList.ElementAt(0).BlockNumber != null)
                {
                    int? equipmentBlockNumber = rentalRequest.HetRentalRequestRotationList.ElementAt(0).BlockNumber;
                    if (equipmentBlockNumber != null) nextRecordToAskBlock[startBlock] = (int)equipmentBlockNumber;
                }
            }

            // update the rotation list
            rentalRequest.FirstOnRotationListId = nextRecordToAskId[startBlock];

            if (nextRecordToAskBlock[startBlock] == 1)
            {
                newAreaRotationList.AskNextBlock1Id = nextRecordToAskId[startBlock];
                newAreaRotationList.AskNextBlock1Seniority = nextRecordToAskSeniority[startBlock];
                newAreaRotationList.AskNextBlock2Id = null;
                newAreaRotationList.AskNextBlock2Seniority = null;
                newAreaRotationList.AskNextBlockOpenId = null;
            }
            else if (nextRecordToAskBlock[startBlock] == 2)
            {
                newAreaRotationList.AskNextBlock1Id = null;
                newAreaRotationList.AskNextBlock1Seniority = null;
                newAreaRotationList.AskNextBlock2Id = nextRecordToAskId[startBlock];
                newAreaRotationList.AskNextBlock2Seniority = nextRecordToAskSeniority[startBlock];
                newAreaRotationList.AskNextBlockOpenId = null;
            }
            else
            {
                newAreaRotationList.AskNextBlock1Id = null;
                newAreaRotationList.AskNextBlock1Seniority = null;
                newAreaRotationList.AskNextBlock2Id = null;
                newAreaRotationList.AskNextBlock2Seniority = null;
                newAreaRotationList.AskNextBlockOpenId = nextRecordToAskId[startBlock];
            }

            // reset the rotation list sort order
            // ** starting @ nextRecordToAskIndex
            int masterSortOrder = 0;

            // process the start block first
            for (int i = nextRecordToAskIndex[startBlock]; i < rentalRequest.HetRentalRequestRotationList.Count; i++)
            {
                if (rentalRequest.HetRentalRequestRotationList.ElementAt(i).BlockNumber != startBlock + 1)
                {
                    break; // done with the start block / start index to end of block
                }

                masterSortOrder++;
                rentalRequest.HetRentalRequestRotationList.ElementAt(i).RotationListSortOrder = masterSortOrder;
            }

            // finish the "first set" of records in the start block (before the index)            
            for (int i = 0; i < nextRecordToAskIndex[startBlock]; i++)
            {
                if (rentalRequest.HetRentalRequestRotationList.ElementAt(i).BlockNumber != startBlock + 1)
                {
                    continue; // move to the next record
                }

                masterSortOrder++;
                rentalRequest.HetRentalRequestRotationList.ElementAt(i).RotationListSortOrder = masterSortOrder;
            }

            // process blocks after the start block
            for (int b = startBlock + 1; b < numberOfBlocks; b++)
            {
                for (int i = nextRecordToAskIndex[b]; i < rentalRequest.HetRentalRequestRotationList.Count; i++)
                {
                    if (rentalRequest.HetRentalRequestRotationList.ElementAt(i).BlockNumber != b + 1)
                    {
                        continue; // move to the next record
                    }

                    masterSortOrder++;
                    rentalRequest.HetRentalRequestRotationList.ElementAt(i).RotationListSortOrder = masterSortOrder;
                }

                // finish the "first set" of records in the block (before the index)            
                for (int i = 0; i < nextRecordToAskIndex[b]; i++)
                {
                    if (rentalRequest.HetRentalRequestRotationList.ElementAt(i).BlockNumber != b + 1)
                    {
                        continue; // move to the next record
                    }

                    masterSortOrder++;
                    rentalRequest.HetRentalRequestRotationList.ElementAt(i).RotationListSortOrder = masterSortOrder;
                }
            }

            // add the new list
            if (!localAreaRotationListExists)
            {
                context.HetLocalAreaRotationList.Add(newAreaRotationList);
            }
        }

        #endregion

        #region Update Local Area Rotation List

        /// <summary>
        /// Update the Local Area Rotation List        
        /// </summary>
        /// <param name="request"></param>
        /// <param name="numberOfBlocks"></param>
        /// <param name="context"></param>
        public static void UpdateRotationList(HetRentalRequest request, int numberOfBlocks, DbAppContext context)
        {
            // first get the localAreaRotationList.askNextBlock"N" for the given local area
            bool exists = context.HetLocalAreaRotationList.Any(a => a.LocalArea.LocalAreaId == request.LocalArea.LocalAreaId);

            HetLocalAreaRotationList localAreaRotationList;

            if (exists)
            {
                localAreaRotationList = context.HetLocalAreaRotationList
                    .Include(x => x.LocalArea)
                    .Include(x => x.AskNextBlock1)
                    .Include(x => x.AskNextBlock2)
                    .Include(x => x.AskNextBlockOpen)
                    .FirstOrDefault(x => x.LocalArea.LocalAreaId == request.LocalArea.LocalAreaId &&
                                         x.DistrictEquipmentTypeId == request.DistrictEquipmentTypeId);
            }
            else
            {
                localAreaRotationList = new HetLocalAreaRotationList
                {
                    LocalAreaId = request.LocalAreaId,
                    DistrictEquipmentTypeId = request.DistrictEquipmentTypeId
                };
            }

            // determine what the next id is
            int? nextId = null;

            if (localAreaRotationList != null && localAreaRotationList.LocalAreaRotationListId > 0)
            {
                if (localAreaRotationList.AskNextBlock1Id != null)
                {
                    nextId = localAreaRotationList.AskNextBlock1Id;
                }
                else if (localAreaRotationList.AskNextBlock2Id != null)
                {
                    nextId = localAreaRotationList.AskNextBlock2Id;
                }
                else
                {
                    nextId = localAreaRotationList.AskNextBlockOpenId;
                }
            }

            // populate:
            // 1. the "next on the list" table for the Local Area  (HET_LOCAL_AREA_ROTATION_LIST)
            // 2. the first on the list id for the Rental Request  (HET_RENTAL_REQUEST.FIRST_ON_ROTATION_LIST_ID)
            if (request.HetRentalRequestRotationList.Count > 0)
            {
                request.HetRentalRequestRotationList = request.HetRentalRequestRotationList
                    .OrderBy(x => x.RotationListSortOrder).ToList();

                request.FirstOnRotationListId = request.HetRentalRequestRotationList.ElementAt(0).Equipment.EquipmentId;

                // find our next record
                int nextRecordToAskIndex = 0;
                bool foundCurrentRecord = false;

                if (nextId != null)
                {
                    for (int i = 0; i < request.HetRentalRequestRotationList.Count; i++)
                    {
                        bool forcedHire;
                        bool asked;

                        if (foundCurrentRecord &&
                            request.HetRentalRequestRotationList.ElementAt(i).IsForceHire != null &&
                            request.HetRentalRequestRotationList.ElementAt(i).IsForceHire == false)
                        {
                            forcedHire = false;
                        }
                        else if (foundCurrentRecord && request.HetRentalRequestRotationList.ElementAt(i).IsForceHire == null)
                        {
                            forcedHire = false;
                        }
                        else
                        {
                            forcedHire = true;
                        }

                        if (foundCurrentRecord &&
                            request.HetRentalRequestRotationList.ElementAt(i).OfferResponse != null &&
                            !request.HetRentalRequestRotationList.ElementAt(i).OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase) &&
                            !request.HetRentalRequestRotationList.ElementAt(i).OfferResponse.Equals("No", StringComparison.InvariantCultureIgnoreCase))
                        {
                            asked = false;
                        }
                        else if (foundCurrentRecord && request.HetRentalRequestRotationList.ElementAt(i).OfferResponse == null)
                        {
                            asked = false;
                        }
                        else
                        {
                            asked = true;
                        }

                        if (foundCurrentRecord && !forcedHire && !asked)
                        {
                            // we've found our next record - exit and update the lists
                            nextRecordToAskIndex = i;
                            break;
                        }

                        if (!foundCurrentRecord &&
                            request.HetRentalRequestRotationList.ElementAt(i).EquipmentId == nextId)
                        {
                            foundCurrentRecord = true;
                        }
                    }
                }

                if (request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.BlockNumber == 1 &&
                    request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.BlockNumber <= numberOfBlocks &&
                    localAreaRotationList != null)
                {
                    localAreaRotationList.AskNextBlock1Id = request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.EquipmentId;
                    localAreaRotationList.AskNextBlock1Seniority = request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.Seniority;
                    localAreaRotationList.AskNextBlock2Id = null;
                    localAreaRotationList.AskNextBlock2Seniority = null;
                    localAreaRotationList.AskNextBlockOpen = null;
                    localAreaRotationList.AskNextBlockOpenId = null;
                }
                else if (request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.BlockNumber == 2 &&
                         request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.BlockNumber <= numberOfBlocks &&
                         localAreaRotationList != null)
                {
                    localAreaRotationList.AskNextBlock2Id = request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.EquipmentId;
                    localAreaRotationList.AskNextBlock2Seniority = request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.Seniority;
                    localAreaRotationList.AskNextBlock1Id = null;
                    localAreaRotationList.AskNextBlock1Seniority = null;
                    localAreaRotationList.AskNextBlockOpen = null;
                    localAreaRotationList.AskNextBlockOpenId = null;
                }
                else if (localAreaRotationList != null)
                {
                    localAreaRotationList.AskNextBlockOpenId = request.HetRentalRequestRotationList.ElementAt(nextRecordToAskIndex).Equipment.EquipmentId;
                    localAreaRotationList.AskNextBlock1Id = null;
                    localAreaRotationList.AskNextBlock1Seniority = null;
                    localAreaRotationList.AskNextBlock2Id = null;
                    localAreaRotationList.AskNextBlock2Seniority = null;
                }

                // save the list - Done!           
                if (!exists)
                {
                    context.HetLocalAreaRotationList.Add(localAreaRotationList);
                }
            }
        }

        #endregion

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
                List<HetRentalRequest> requests = context.HetRentalRequest
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
            HetRentalRequest request = context.HetRentalRequest.AsNoTracking()
                .Include(x => x.HetHistory)
                .First(a => a.RentalRequestId == id);

            List<HetHistory> data = request.HetHistory
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
                    temp.AffectedEntityId = data[i].RentalRequestId;
                }

                result.Add(temp);
            }

            return result;
        }

        #endregion  
    }
}
