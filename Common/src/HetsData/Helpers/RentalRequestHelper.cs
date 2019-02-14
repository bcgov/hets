using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

                                //HETS-968 - Rotation list -Wrong Block number for Open block
                                if (blockNumber == numberOfBlocks)
                                {
                                    blockNumber = 3;
                                    rotationList.Equipment.BlockNumber = blockNumber;
                                }                                
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

        #region Convert full equipment record to a "Hires" version

        /// <summary>
        /// Convert to Rental Request Hires (Lite) Model
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static RentalRequestHires ToHiresModel(HetRentalRequestRotationList request, HetUser user)
        {
            RentalRequestHires requestLite = new RentalRequestHires();

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
                    requestLite.Reason = request.OfferResponseNote;
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
                bool agreementExists = context.HetRentalAgreement.AsNoTracking()
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

        private static bool IsEquipmentHired(int? equipmentId, DbAppContext context)
        {
            if (equipmentId == null) return false;

            // check if this item has an "Active" contract
            int? statusIdRentalAgreement = StatusHelper.GetStatusId(HetRentalAgreement.StatusActive, "rentalAgreementStatus", context);
            if (statusIdRentalAgreement == null) throw new ArgumentException("Status Id cannot be null");

            bool agreementExists = context.HetRentalAgreement.AsNoTracking()
                .Any(x => x.EquipmentId == equipmentId &&
                          x.RentalRequestId != null &&
                          x.RentalAgreementStatusTypeId == statusIdRentalAgreement);                

            return agreementExists;
        }

        private static HetEquipment LastAskedByBlock(int block, int? districtEquipmentTypeId, 
            int? localAreaId, DateTime fiscalStart, DbAppContext context)
        {
            if (districtEquipmentTypeId == null || localAreaId == null) return null;

            block++;

            // if this is not block 1 - check that we have "asked" anyone in the previous list
            HetRentalRequestRotationList rotationList = context.HetRentalRequestRotationList.AsNoTracking()
                .Include(x => x.RentalRequest)
                .Include(x => x.Equipment)
                .Where(x => x.RentalRequest.DistrictEquipmentTypeId == districtEquipmentTypeId &&
                            x.RentalRequest.LocalAreaId == localAreaId &&                            
                            x.RentalRequest.AppCreateTimestamp >= fiscalStart &&
                            x.Equipment.BlockNumber == block &&
                            x.WasAsked == true &&
                            x.IsForceHire != true)
                .OrderByDescending(x => x.RentalRequestId)
                .ThenByDescending(x => x.RotationListSortOrder)
                .FirstOrDefault();

            // return the equipment record that was last asked for this block
            return rotationList?.Equipment;
        }

        private static HetLocalAreaRotationList UpdateNextRecordToAsk(int block, int? equipmentId, float? seniority, HetLocalAreaRotationList rotationList)
        {
            if (block == 1)
            {
                rotationList.AskNextBlock1Id = equipmentId;
                rotationList.AskNextBlock1Seniority = seniority;
                rotationList.AskNextBlock2Id = null;
                rotationList.AskNextBlock2Seniority = null;
                rotationList.AskNextBlockOpenId = null;
            }
            else if (block == 2)
            {
                rotationList.AskNextBlock1Id = null;
                rotationList.AskNextBlock1Seniority = null;
                rotationList.AskNextBlock2Id = equipmentId;
                rotationList.AskNextBlock2Seniority = seniority;
                rotationList.AskNextBlockOpenId = null;
            }
            else
            {
                rotationList.AskNextBlock1Id = null;
                rotationList.AskNextBlock1Seniority = null;
                rotationList.AskNextBlock2Id = null;
                rotationList.AskNextBlock2Seniority = null;
                rotationList.AskNextBlockOpenId = equipmentId;
            }

            return rotationList;
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

                // update local area rotation list
                newAreaRotationList = UpdateNextRecordToAsk(block,
                    rentalRequest.HetRentalRequestRotationList.ElementAt(0).EquipmentId,
                    rentalRequest.HetRentalRequestRotationList.ElementAt(0).SeniorityFloat,
                    newAreaRotationList);

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
            // ** also need to check if the equipment is currently hired
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

                // get the last asked equipment id for this "block"
                HetEquipment lastEquipment = LastAskedByBlock(b, rentalRequest.DistrictEquipmentTypeId, rentalRequest.LocalAreaId,
                    fiscalStart, context);

                // nothing found for this block - start at 0
                if (lastEquipment == null && rentalRequest.HetRentalRequestRotationList.Count > 0)
                {
                    for (int j = 0; j < rentalRequest.HetRentalRequestRotationList.Count; j++)
                    {
                        if (rentalRequest.HetRentalRequestRotationList.ElementAt(j).BlockNumber != (b + 1)) continue; // move to next record

                        if (!IsEquipmentHired(rentalRequest.HetRentalRequestRotationList.ElementAt(j).EquipmentId, context))
                        {
                            int temp = rentalRequest.HetRentalRequestRotationList.ElementAt(j).EquipmentId ?? -1;

                            if (temp > -1)
                            {
                                nextRecordToAskId[b] = temp;
                                nextRecordToAskBlock[b] = b;

                                break;
                            }                            
                        }
                    }
                }

                // find the next record - and ensure it's not currently hired
                bool foundLast = false;

                for (int j = 0; j < rentalRequest.HetRentalRequestRotationList.Count; j++)
                {
                    // make sure we're working in the right block
                    if (rentalRequest.HetRentalRequestRotationList.ElementAt(j).Equipment.BlockNumber != (b + 1)) continue; // move to next record

                    if (foundLast)
                    {
                        if (!IsEquipmentHired(rentalRequest.HetRentalRequestRotationList.ElementAt(j).EquipmentId, context))
                        {
                            int temp = rentalRequest.HetRentalRequestRotationList.ElementAt(j).EquipmentId ?? -1;

                            if (temp > -1)
                            {
                                nextRecordToAskId[b] = temp;
                                nextRecordToAskBlock[b] = b;
                                break;
                            }
                        }
                    }

                    foundLast = rentalRequest.HetRentalRequestRotationList.ElementAt(j).EquipmentId == lastEquipment?.EquipmentId;
                }                
            }            
            
            // *****************************************************************
            // 2. Remove all hired equipment from the new list
            // *****************************************************************
            rentalRequest = DropHiredEquipment(rentalRequest, context);

            // locate our "start" records in the updated list
            for (int b = 0; b < numberOfBlocks; b++)
            {
                if (nextRecordToAskId[b] > 0)
                {
                    for (int j = 0; j < rentalRequest.HetRentalRequestRotationList.Count; j++)
                    {
                        // make sure we're working in the right block
                        if (rentalRequest.HetRentalRequestRotationList.ElementAt(j).Equipment.BlockNumber != (b + 1)) continue; // move to next record

                        if (rentalRequest.HetRentalRequestRotationList.ElementAt(j).EquipmentId == nextRecordToAskId[b])
                        {
                            nextRecordToAskIndex[b] = j;
                            if (startBlock == -1) startBlock = b;
                            break;
                        }
                    }
                }
            }

            // check we have equipment still after removing everything
            if (rentalRequest.HetRentalRequestRotationList.Count <= 0)
            {
                throw new ArgumentException("HETS-35"); // no more records available
            }

            // *****************************************************************
            // 3. Default the index to the first record if nothing is selected
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
                        break; // done
                    }
                }
            }            

            // *****************************************************************
            // 4. Update the local area rotation list
            // *****************************************************************
            rentalRequest.FirstOnRotationListId = nextRecordToAskId[startBlock];

            // update local area rotation list
            newAreaRotationList = UpdateNextRecordToAsk(nextRecordToAskBlock[startBlock],
                nextRecordToAskId[startBlock],
                nextRecordToAskSeniority[startBlock],
                newAreaRotationList);
            
            // *****************************************************************
            // 5. Reset the rotation list sort order
            //    ** starting @ nextRecordToAskIndex
            // *****************************************************************
            int masterSortOrder = 0;

            // process the start block first
            for (int i = nextRecordToAskIndex[startBlock]; i < rentalRequest.HetRentalRequestRotationList.Count; i++)
            {
                if (rentalRequest.HetRentalRequestRotationList.ElementAt(i).BlockNumber != startBlock)
                {
                    break; // done with the start block / start index to end of block
                }

                masterSortOrder++;
                rentalRequest.HetRentalRequestRotationList.ElementAt(i).RotationListSortOrder = masterSortOrder;
            }

            // finish the "first set" of records in the start block (before the index)            
            for (int i = 0; i < nextRecordToAskIndex[startBlock]; i++)
            {
                if (rentalRequest.HetRentalRequestRotationList.ElementAt(i).BlockNumber != startBlock)
                {
                    continue; // move to the next record
                }

                masterSortOrder++;
                rentalRequest.HetRentalRequestRotationList.ElementAt(i).RotationListSortOrder = masterSortOrder;
            }

            // process other blocks
            for (int b = 0; b < numberOfBlocks; b++)
            {
                if (b + 1 == startBlock) continue; // already processed

                for (int i = nextRecordToAskIndex[b]; i < rentalRequest.HetRentalRequestRotationList.Count; i++)
                {
                    if (i < 0 || rentalRequest.HetRentalRequestRotationList.ElementAt(i).BlockNumber != b + 1)
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
