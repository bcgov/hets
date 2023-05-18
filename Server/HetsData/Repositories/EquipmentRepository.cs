using AutoMapper;
using HetsData.Dtos;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HetsData.Repositories
{
    public interface IEquipmentRepository
    {
        public EquipmentDto GetEquipment(int equipmentId);
        HetEquipment CreateNewEquipment(EquipmentDto item);
    }

    public class EquipmentRepository : IEquipmentRepository
    {
        private IMapper _mapper;
        private DbAppContext _dbContext;
        private IConfiguration _configuration;
        private readonly ILogger<EquipmentRepository> _logger;

        public EquipmentRepository(DbAppContext dbContext, IMapper mapper, IConfiguration configuration, ILogger<EquipmentRepository> logger)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Get an Equipment record
        /// </summary>
        /// <param name="equipmentId"></param>
        /// <returns></returns>
        public EquipmentDto GetEquipment(int equipmentId)
        {
            // retrieve updated equipment record to return to ui
            HetEquipment equipment = _dbContext.HetEquipments.AsNoTracking()
                .Include(x => x.EquipmentStatusType)
                .Include(x => x.LocalArea)
                    .ThenInclude(y => y.ServiceArea)
                        .ThenInclude(z => z.District)
                            .ThenInclude(a => a.Region)
                .Include(x => x.DistrictEquipmentType)
                    .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Owner)
                    .ThenInclude(x => x.OwnerStatusType)
                .Include(x => x.HetEquipmentAttachments)
                .Include(x => x.HetNotes)
                .Include(x => x.HetDigitalFiles)
                .Include(x => x.HetHistories)
                .FirstOrDefault(a => a.EquipmentId == equipmentId);

            if (equipment != null)
            {
                equipment.IsHired = EquipmentHelper.IsHired(equipmentId, _dbContext);
                equipment.NumberOfBlocks = EquipmentHelper.GetNumberOfBlocks(equipment, _configuration, (errMessage, ex) => {
                    _logger.LogError(errMessage);
                    _logger.LogError(ex.ToString());
                });
                equipment.HoursYtd = EquipmentHelper.GetYtdServiceHours(equipmentId, _dbContext);
                equipment.Status = equipment.EquipmentStatusType.EquipmentStatusTypeCode;

                if (equipment.Seniority != null && equipment.BlockNumber != null)
                {
                    equipment.SeniorityString = EquipmentHelper.FormatSeniorityString((float)equipment.Seniority, (int)equipment.BlockNumber, equipment.NumberOfBlocks);
                }

                if (equipment.Owner != null)
                {
                    // populate the "Status" description
                    equipment.Owner.Status = equipment.Owner.OwnerStatusType.OwnerStatusTypeCode;
                }

                // set fiscal year headers
                if (equipment.LocalArea?.ServiceArea?.District != null)
                {
                    int districtId = equipment.LocalArea.ServiceArea.District.DistrictId;

                    HetDistrictStatus district = _dbContext.HetDistrictStatuses.AsNoTracking()
                        .FirstOrDefault(x => x.DistrictId == districtId);

                    if (district?.NextFiscalYear != null)
                    {
                        int fiscalYear = (int)district.NextFiscalYear; // status table uses the start of the tear

                        equipment.YearMinus1 = $"{fiscalYear - 2}/{fiscalYear - 1}";
                        equipment.YearMinus2 = $"{fiscalYear - 3}/{fiscalYear - 2}";
                        equipment.YearMinus3 = $"{fiscalYear - 4}/{fiscalYear - 3}";
                    }
                }

                // HETS-1115 - Do not allow changing seniority affecting entities if an active request exists
                equipment.ActiveRentalRequest = EquipmentHelper.RentalRequestStatus(equipmentId, _dbContext);
            }

            return _mapper.Map<EquipmentDto>(equipment);
        }

        /// <summary>
        /// Set the Equipment fields for a new record for fields that are not provided by the front end
        /// </summary>
        /// <param name="entity"></param>
        public HetEquipment CreateNewEquipment(EquipmentDto item)
        {
            var entity = _mapper.Map<HetEquipment>(item);

            entity.ReceivedDate = DateTime.UtcNow;
            entity.LastVerifiedDate = DateTime.UtcNow;

            // per JIRA HETS-536
            entity.ApprovedDate = DateTime.UtcNow;

            entity.Seniority = 0.0F;
            entity.YearsOfService = 0.0F;
            entity.ServiceHoursLastYear = 0.0F;
            entity.ServiceHoursTwoYearsAgo = 0.0F;
            entity.ServiceHoursThreeYearsAgo = 0.0F;
            entity.ArchiveCode = "N";
            entity.IsSeniorityOverridden = false;

            int tmpAreaId = item.LocalArea.LocalAreaId;
            entity.LocalAreaId = tmpAreaId;
            entity.LocalArea = null;

            int tmpEquipId = item.DistrictEquipmentType.DistrictEquipmentTypeId;
            entity.DistrictEquipmentTypeId = tmpEquipId;
            entity.DistrictEquipmentType = null;

            // [Original: new equipment MUST always start as unapproved - it isn't assigned to any block yet]
            // HETS-834 - BVT - New Equipment Added default to APPROVED
            // * Set to Approved
            // * Update all equipment blocks, etc.
            int? statusId = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _dbContext);

            if (statusId == null)
            {
                throw new DataException("Status Id cannot be null");
            }

            entity.EquipmentStatusTypeId = (int)statusId;

            // generate a new equipment code
            if (entity.Owner != null)
            {
                // set the equipment code
                entity.EquipmentCode = EquipmentHelper.GetEquipmentCode(item.Owner.OwnerId, _dbContext);

                // cleanup owner reference
                int tmpOwnerId = item.Owner.OwnerId;
                entity.OwnerId = tmpOwnerId;
                entity.Owner = null;
            }

            return entity;
        }
    }
}
