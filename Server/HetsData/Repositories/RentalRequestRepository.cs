using AutoMapper;
using HetsData.Dtos;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace HetsData.Repositories
{
    public interface IRentalRequestRepository
    {
        RentalRequestLite ToLiteModel(HetRentalRequest request);
        RentalRequestDto GetRecord(int id);
        RentalRequestDto GetRecordWithRotationList(int id, SeniorityScoringRules scoringRules);
    }

    public class RentalRequestRepository : IRentalRequestRepository
    {
        private IMapper _mapper;
        private DbAppContext _dbContext;
        private IConfiguration _configuration;

        public RentalRequestRepository(DbAppContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Convert to Rental Request Lite Model
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RentalRequestLite ToLiteModel(HetRentalRequest request)
        {
            RentalRequestLite requestLite = new RentalRequestLite();

            if (request != null)
            {
                requestLite.YesCount = CalculateYesCount(request);

                if (request.DistrictEquipmentType != null)
                {
                    requestLite.EquipmentTypeName = request.DistrictEquipmentType.EquipmentType.Name;
                    requestLite.DistrictEquipmentName = request.DistrictEquipmentType.DistrictEquipmentName;
                }

                requestLite.Id = request.RentalRequestId;
                requestLite.LocalArea = _mapper.Map<LocalAreaDto>(request.LocalArea);

                if (request.Project != null)
                {
                    requestLite.PrimaryContact = _mapper.Map<ContactDto>(request.Project.PrimaryContact);
                    requestLite.ProjectName = request.Project.Name;
                    requestLite.ProjectId = request.Project.ProjectId;
                }
                else
                {
                    requestLite.ProjectName = "Request - View Only";
                }

                requestLite.Status = request.RentalRequestStatusType.Description;
                requestLite.EquipmentCount = request.EquipmentCount;
                requestLite.ExpectedEndDate = request.ExpectedEndDate;
                requestLite.ExpectedStartDate = request.ExpectedStartDate;
            }

            return requestLite;
        }

        /// <summary>
        /// Get rental request record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_dbContext"></param>
        public RentalRequestDto GetRecord(int id)
        {
            HetRentalRequest request = _dbContext.HetRentalRequests.AsNoTracking()
                .Include(x => x.RentalRequestStatusType)
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(c => c.PrimaryContact)
                .Include(x => x.Project)
                    .ThenInclude(c => c.ProjectStatusType)
                .Include(x => x.HetRentalRequestAttachments)
                .Include(x => x.DistrictEquipmentType)
                .FirstOrDefault(a => a.RentalRequestId == id);

            request.Status = request.RentalRequestStatusType.RentalRequestStatusTypeCode;

            return _mapper.Map<RentalRequestDto>(request);
        }

        /// <summary>
        /// Get rental request record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="scoringRules"></param>
        /// <param name="_dbContext"></param>
        public RentalRequestDto GetRecordWithRotationList(int id, SeniorityScoringRules scoringRules)
        {
            #region replace
            //load up the rental request with the equipment decoupled
            HetRentalRequest request = _dbContext.HetRentalRequests.AsNoTracking()
                .Include(x => x.HetRentalRequestSeniorityLists)
                    .ThenInclude(x => x.Owner)
                        .ThenInclude(x => x.PrimaryContact)
                .Include(x => x.HetRentalRequestSeniorityLists)
                    .ThenInclude(x => x.LocalArea)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(y => y.EquipmentType)
                .Include(x => x.FirstOnRotationList)
                .Include(x => x.HetRentalRequestAttachments)
                .Include(x => x.HetRentalRequestRotationLists)
                .FirstOrDefault(a => a.RentalRequestId == id);

            //pull out the date that request was last updated
            var requestDate = request.AppLastUpdateTimestamp;

            foreach (var rrrl in request.HetRentalRequestRotationLists)
            {
                //get the equipment id from the rental request rotation list item (mouthful!)
                var equipmentId = rrrl.EquipmentId;

                var equipment = request.HetRentalRequestSeniorityLists
                    .Where(x => x.EquipmentId == equipmentId)
                    .FirstOrDefault();

                //lets make sure we actually have an equipment object
                if (equipment != null)
                {
                    //assign the equipment data into the rotation hire list
                    rrrl.Equipment = _mapper.Map<HetEquipment>(equipment);

                    //some queries to pull the rest of the data related to equipment.. not historical (is that a potential issue?)
                    rrrl.Equipment.HetEquipmentAttachments = _dbContext.HetEquipmentAttachments.AsNoTracking()
                        .Where(x => x.EquipmentId == equipmentId).ToList();
                }
            }
            #endregion

            if (request != null)
            {
                // re-sort list using: LocalArea / District Equipment Type and SenioritySortOrder (desc)
                request.HetRentalRequestRotationLists = request.HetRentalRequestRotationLists
                    .OrderBy(e => e.RotationListSortOrder)
                    .ToList();

                // calculate the Yes Count based on the RentalRequestList
                request.YesCount = CalculateYesCount(request);

                var numberOfBlocks = request.DistrictEquipmentType.EquipmentType.IsDumpTruck
                                    ? scoringRules.GetTotalBlocks("DumpTruck") + 1
                                    : scoringRules.GetTotalBlocks() + 1;

                // calculate YTD hours for the equipment records
                if (request.HetRentalRequestRotationLists != null)
                {
                    foreach (HetRentalRequestRotationList rotationList in request.HetRentalRequestRotationLists)
                    {
                        if (rotationList.Equipment != null)
                        {
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

                            rotationList.Equipment.SeniorityString = EquipmentHelper.FormatSeniorityString(seniority, blockNumber, numberOfBlocks);
                        }
                    }
                }
            }

            return _mapper.Map<RentalRequestDto>(request);
        }

        /// <summary>
        /// Check how many Yes' we currently have from Owners
        /// </summary>
        /// <returns></returns>
        private static int CalculateYesCount(HetRentalRequest rentalRequest)
        {
            int temp = 0;

            if (rentalRequest.HetRentalRequestRotationLists != null)
            {
                foreach (HetRentalRequestRotationList equipment in rentalRequest.HetRentalRequestRotationLists)
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
    }
}
