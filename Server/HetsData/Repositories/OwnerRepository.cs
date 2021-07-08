using AutoMapper;
using HetsData.Dtos;
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HetsData.Repositories
{
    public interface IOwnerRepository
    {
        OwnerDto GetRecord(int id);
        bool RentalRequestStatus(int id);
        List<History> GetHistoryRecords(int id, int? offset, int? limit);
    }

    public class OwnerRepository : IOwnerRepository
    {
        private IMapper _mapper;
        private DbAppContext _dbContext;
        private IConfiguration _configuration;

        public OwnerRepository(DbAppContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        #region Get an Owner record (plus associated records)

        /// <summary>
        /// Get an Owner record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OwnerDto GetRecord(int id)
        {
            // get equipment status types
            int? statusIdArchived = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _dbContext);
            if (statusIdArchived == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            // get owner record
            HetOwner owner = _dbContext.HetOwner.AsNoTracking()
                .Include(x => x.OwnerStatusType)
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(z => z.EquipmentType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.EquipmentStatusType)
                .Include(x => x.HetEquipment)
                    .ThenInclude(y => y.HetEquipmentAttachment)
                .Include(x => x.HetContact)
                .Include(x => x.PrimaryContact)
                .FirstOrDefault(a => a.OwnerId == id);

            if (owner != null)
            {
                // remove any archived equipment
                owner.HetEquipment = owner.HetEquipment.Where(e => e.EquipmentStatusTypeId != statusIdArchived).ToList();

                // populate the "Status" description
                owner.Status = owner.OwnerStatusType.OwnerStatusTypeCode;

                foreach (HetEquipment equipment in owner.HetEquipment)
                {
                    equipment.IsHired = EquipmentHelper.IsHired(id, _dbContext);
                    equipment.NumberOfBlocks = EquipmentHelper.GetNumberOfBlocks(equipment, _configuration);
                    equipment.HoursYtd = EquipmentHelper.GetYtdServiceHours(id, _dbContext);
                    equipment.Status = equipment.EquipmentStatusType.EquipmentStatusTypeCode;
                    equipment.EquipmentNumber = int.Parse(Regex.Match(equipment.EquipmentCode, @"\d+").Value);
                }

                // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
                owner.ActiveRentalRequest = RentalRequestStatus(id);
            }

            return _mapper.Map<OwnerDto>(owner);
        }

        #endregion

        /// <summary>
        /// Returns true if any equipment associated with this owner is on an active rotation list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RentalRequestStatus(int id)
        {
            // get equipment status types
            int? statusIdActive = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _dbContext);
            if (statusIdActive == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            // get rental request status type
            int? statusIdInProgress = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", _dbContext);
            if (statusIdInProgress == null)
            {
                throw new ArgumentException("Status Code not found");
            }

            return _dbContext.HetRentalRequestRotationList.AsNoTracking()
                .Include(x => x.RentalRequest)
                .Include(x => x.Equipment)
                .Any(x => x.Equipment.OwnerId == id &&
                          x.Equipment.EquipmentStatusTypeId == statusIdActive &&
                          x.RentalRequest.RentalRequestStatusTypeId == statusIdInProgress);

        }


        public List<History> GetHistoryRecords(int id, int? offset, int? limit)
        {
            HetOwner owner = _dbContext.HetOwner.AsNoTracking()
                .Include(x => x.HetHistory)
                .First(a => a.OwnerId == id);

            List<HetHistory> data = owner.HetHistory
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
                    temp.AffectedEntityId = data[i].OwnerId;
                }

                result.Add(temp);
            }

            return result;
        }

    }
}
