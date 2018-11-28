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
        public static HetRentalRequest CreateRotationList(HetRentalRequest request, DbAppContext context, IConfiguration configuration)
        {            
            request.HetRentalRequestRotationList = new List<HetRentalRequestRotationList>();

            // validate input parameters
            if (request.LocalAreaId == null || request.DistrictEquipmentTypeId == null) return request;

            int currentSortOrder = 1;

            // get the number of blocks for this piece of equipment
            int numberOfBlocks = GetNumberOfBlocks(request, context, configuration);
            numberOfBlocks = numberOfBlocks + 1;            

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
                
                // sets the rotation list sort order
                int currentPosition = 0;

                for (int i = 0; i < listSize; i++)
                {
                    HetRentalRequestRotationList rentalRequestRotationList = new HetRentalRequestRotationList
                    {
                        Equipment = blockEquipment[i],
                        EquipmentId = blockEquipment[i].EquipmentId,
                        SeniorityFloat = blockEquipment[i].Seniority,
                        BlockNumber = blockEquipment[i].BlockNumber,
                        AppCreateTimestamp = DateTime.UtcNow,
                        RotationListSortOrder = currentSortOrder
                    };

                    request.HetRentalRequestRotationList.Add(rentalRequestRotationList);

                    currentPosition++;
                    currentSortOrder++;

                    if (currentPosition >= listSize)
                    {
                        currentPosition = 0;
                    }
                }
            }

            // update the local area rotation list - find record #1
            request = SetupNewRotationList(request, numberOfBlocks, context);       
            
            // remove equipment records
            foreach (HetRentalRequestRotationList rotationList in request.HetRentalRequestRotationList)
            {
                rotationList.Equipment = null;
            }

            // Done!
            return request;
        }

        #endregion

        #region Setup Local Area Rotation Lists

        private static HetRentalRequest DropHiredEquipment(HetRentalRequest rentalRequest, DbAppContext context)
        {
            // check if any items have "Active" contracts - and drop them from the list
            // * ensure we ignore "blank" rental agreements (i.e. rental request is null)

            int? statusIdRentalAgreement = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", context);
            if (statusIdRentalAgreement == null) throw new ArgumentException("Status Id cannot be null");

            int listSize = rentalRequest.HetRentalRequestRotationList.Count;

            for (int i = listSize - 1; i >= 0; i--)
            {
                bool agreementExists = context.HetRentalAgreement
                    .Any(x => x.EquipmentId == rentalRequest.HetRentalRequestRotationList.ElementAt(i).EquipmentId &&
                              x.RentalRequestId != null &&
                              x.RentalAgreementStatusTypeId == statusIdRentalAgreement);

                if (agreementExists)
                {
                    rentalRequest.HetRentalRequestRotationList.Remove(rentalRequest.HetRentalRequestRotationList.ElementAt(i));
                }
            }

            return rentalRequest;
        }

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
        public static HetRentalRequest SetupNewRotationList(HetRentalRequest rentalRequest, int numberOfBlocks, DbAppContext context)
        {
            // nothing to do!
            if (rentalRequest.HetRentalRequestRotationList.Count <= 0) return rentalRequest;            

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

            // *****************************************************************
            // if we don't have a request
            // ** remove hired equipment
            // ** pick block 1 / record 1 & we're done
            // *****************************************************************
            if (!previousRequestExists)
            {
                // remove hired equipment from the list
                rentalRequest = DropHiredEquipment(rentalRequest, context);

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

                // save rotation list
                if (localAreaRotationListExists)
                {
                    context.HetLocalAreaRotationList.Update(newAreaRotationList);
                }
                else
                {
                    context.HetLocalAreaRotationList.Add(newAreaRotationList);
                }

                return rentalRequest; //done!
            }

            // *****************************************************************
            // use the previous rotation list to determine where we were
            // ** we pick the record after the last "asked" (Yes/No)
            // ** if all records in that block were "asked" - then we need to
            //    move on to the next block
            // *****************************************************************
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

            // *****************************************************************
            // 1. Find the last record that was asked in each block
            // *****************************************************************
            int[] lastRecordAskedIndex = new int[numberOfBlocks];
            int[] lastRecordAskedId = new int[numberOfBlocks];
            int[] lastRecordAskedBlock = new int[numberOfBlocks];                               

            for (int b = 0; b < numberOfBlocks; b++)
            {
                lastRecordAskedIndex[b] = -1;
                lastRecordAskedId[b] = -1;
                lastRecordAskedBlock[b] = -1;                

                int blockRecordOneIndex = -1;

                for (int i = 0; i < previousRentalRequest.HetRentalRequestRotationList.Count; i++)
                {
                    // make sure we're working in the right block
                    if (previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).Equipment.BlockNumber != (b + 1)) continue; // move to next record
                    
                    if (blockRecordOneIndex == -1) blockRecordOneIndex = i;
                    
                    // if this record was asked and said yes or no - then record (but keep looking)
                    if (previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).OfferResponse != null &&
                        (previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).OfferResponse.Equals("Yes", StringComparison.InvariantCultureIgnoreCase) ||
                         previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).OfferResponse.Equals("No", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        lastRecordAskedIndex[b] = i;
                        lastRecordAskedId[b] = previousRentalRequest.HetRentalRequestRotationList.ElementAt(i).Equipment.EquipmentId;
                        lastRecordAskedBlock[b] = b;

                        // find this record in the new list...
                        for (int j = 0; j < rentalRequest.HetRentalRequestRotationList.Count; j++)
                        {
                            if (lastRecordAskedId[b] == rentalRequest.HetRentalRequestRotationList.ElementAt(j).EquipmentId)
                            {
                                lastRecordAskedIndex[b] = j;
                                break;
                            }
                        }                                                
                    }
                }                
            }

            // *****************************************************************
            // 2. Remove all hired equipment from the new list
            // *****************************************************************
            rentalRequest = DropHiredEquipment(rentalRequest, context);

            // *****************************************************************
            // 3. Locate the next record in the list
            // *****************************************************************
            int[] nextRecordToAskIndex = new int[numberOfBlocks];
            int[] nextRecordToAskId = new int[numberOfBlocks];
            int[] nextRecordToAskBlock = new int[numberOfBlocks];
            float[] nextRecordToAskSeniority = new float[numberOfBlocks];
            int startBlock = -1;

            for (int b = 0; b < numberOfBlocks; b++)
            {
                nextRecordToAskIndex[b] = -1;
                nextRecordToAskId[b] = -1;
                nextRecordToAskBlock[b] = -1;

                if (lastRecordAskedIndex[b] > -1)
                {
                    for (int j = 0; j < rentalRequest.HetRentalRequestRotationList.Count; j++)
                    {
                        if (lastRecordAskedId[b] == rentalRequest.HetRentalRequestRotationList.ElementAt(j).Equipment.EquipmentId)
                        {
                            if (rentalRequest.HetRentalRequestRotationList.Count > (j + 1) &&
                                rentalRequest.HetRentalRequestRotationList.ElementAt(j + 1) != null)
                            {
                                nextRecordToAskIndex[b] = j + 1;
                                nextRecordToAskId[b] = rentalRequest.HetRentalRequestRotationList.ElementAt(j + 1).Equipment.EquipmentId;

                                if (rentalRequest.HetRentalRequestRotationList.ElementAt(j + 1).Equipment.Seniority == null)
                                {
                                    nextRecordToAskSeniority[b] = 0.0F;
                                }
                                else
                                {
                                    float? equipmentSeniority = rentalRequest.HetRentalRequestRotationList.ElementAt(j + 1).Equipment.Seniority;
                                    if (equipmentSeniority != null) nextRecordToAskSeniority[b] = (float) equipmentSeniority;
                                }

                                if (startBlock == -1) startBlock = b;
                            }

                            break;
                        }                        
                    }                    
                }
                else if (rentalRequest.HetRentalRequestRotationList.ElementAt(0) != null)
                {
                    nextRecordToAskIndex[b] = 0;
                    nextRecordToAskId[b] = rentalRequest.HetRentalRequestRotationList.ElementAt(0).Equipment.EquipmentId;

                    if (rentalRequest.HetRentalRequestRotationList.ElementAt(0).Equipment.Seniority == null)
                    {
                        nextRecordToAskSeniority[b] = 0.0F;
                    }
                    else
                    {
                        float? equipmentSeniority = rentalRequest.HetRentalRequestRotationList.ElementAt(0).Equipment.Seniority;
                        if (equipmentSeniority != null) nextRecordToAskSeniority[b] = (float)equipmentSeniority;
                    }

                    if (startBlock == -1) startBlock = b;
                }
            }

            // *****************************************************************
            // 3(a). Default the index to the first record if nothing is selected
            // *****************************************************************
            for (int b = 0; b < numberOfBlocks; b++)
            {
                if (nextRecordToAskIndex[b] < 0)
                {
                    if (rentalRequest.HetRentalRequestRotationList.ElementAt(0) != null)
                    {
                        nextRecordToAskIndex[b] = 0;
                        nextRecordToAskId[b] = rentalRequest.HetRentalRequestRotationList.ElementAt(0).Equipment.EquipmentId;

                        if (rentalRequest.HetRentalRequestRotationList.ElementAt(0).Equipment.Seniority == null)
                        {
                            nextRecordToAskSeniority[b] = 0.0F;
                        }
                        else
                        {
                            float? equipmentSeniority = rentalRequest.HetRentalRequestRotationList.ElementAt(0).Equipment.Seniority;
                            if (equipmentSeniority != null) nextRecordToAskSeniority[b] = (float)equipmentSeniority;
                        }

                        if (startBlock == -1) startBlock = b;
                    }
                }
            }

            // *****************************************************************
            // 4. Update the local area rotation list
            // *****************************************************************
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

            // *****************************************************************
            // 4. Reset the rotation list sort order
            //    ** starting @ nextRecordToAskIndex
            // *****************************************************************
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
            if (!localAreaRotationListExists) context.HetLocalAreaRotationList.Add(newAreaRotationList);

            return rentalRequest;
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
