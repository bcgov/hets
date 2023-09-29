using AutoMapper;
using HetsData.Dtos;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HetsCommon;

namespace HetsData.Repositories
{
    public interface IOwnerRepository
    {
        OwnerDto GetRecord(int id);
        bool RentalRequestStatus(int id);
        List<History> GetHistoryRecords(int id, int? offset, int? limit);
        OwnerVerificationReportModel GetOwnerVerificationLetterData(
            int?[] localAreas, int?[] owners, int? equipmentStatusId, int? ownerStatusId, int? districtId);
    }

    public class OwnerRepository : IOwnerRepository
    {
        private IMapper _mapper;
        private DbAppContext _dbContext;
        private IConfiguration _configuration;
        private readonly ILogger<OwnerRepository> _logger;

        public OwnerRepository(DbAppContext dbContext, IMapper mapper, IConfiguration configuration, ILogger<OwnerRepository> logger)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
            _logger = logger;
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
            int? statusIdArchived = StatusHelper.GetStatusId(HetEquipment.StatusArchived, "equipmentStatus", _dbContext) 
                ?? throw new ArgumentException("Status Code not found");

            // get owner record
            HetOwner owner = _dbContext.HetOwners.AsNoTracking()
                .Include(x => x.OwnerStatusType)
                .Include(x => x.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipments)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.HetEquipments)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(z => z.EquipmentType)
                .Include(x => x.HetEquipments)
                    .ThenInclude(y => y.EquipmentStatusType)
                .Include(x => x.HetEquipments)
                    .ThenInclude(y => y.HetEquipmentAttachments)
                .Include(x => x.HetContacts)
                .Include(x => x.PrimaryContact)
                .FirstOrDefault(a => a.OwnerId == id);

            if (owner != null)
            {
                // remove any archived equipment
                owner.HetEquipments = owner.HetEquipments.Where(e => e.EquipmentStatusTypeId != statusIdArchived).ToList();

                // populate the "Status" description
                owner.Status = owner.OwnerStatusType.OwnerStatusTypeCode;

                foreach (HetEquipment equipment in owner.HetEquipments)
                {
                    equipment.IsHired = EquipmentHelper.IsHired(id, _dbContext);
                    equipment.NumberOfBlocks = EquipmentHelper.GetNumberOfBlocks(equipment, _configuration, (errMessage, ex) => {
                        _logger.LogError(errMessage);
                        _logger.LogError(ex.ToString());
                    });
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
            int? statusIdActive = StatusHelper.GetStatusId(HetEquipment.StatusApproved, "equipmentStatus", _dbContext) 
                ?? throw new ArgumentException("Status Code not found");

            // get rental request status type
            int? statusIdInProgress = StatusHelper.GetStatusId(HetRentalRequest.StatusInProgress, "rentalRequestStatus", _dbContext) 
                ?? throw new ArgumentException("Status Code not found");

            return _dbContext.HetRentalRequestRotationLists.AsNoTracking()
                .Include(x => x.RentalRequest)
                .Include(x => x.Equipment)
                .Any(x => x.Equipment.OwnerId == id &&
                          x.Equipment.EquipmentStatusTypeId == statusIdActive &&
                          x.RentalRequest.RentalRequestStatusTypeId == statusIdInProgress);
        }


        public List<History> GetHistoryRecords(int id, int? offset, int? limit)
        {
            HetOwner owner = _dbContext.HetOwners.AsNoTracking()
                .Include(x => x.HetHistories)
                .First(a => a.OwnerId == id);

            List<HetHistory> data = owner.HetHistories
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
                    temp.LastUpdateTimestamp = DateUtils.AsUTC(data[i].AppLastUpdateTimestamp);
                    temp.LastUpdateUserid = data[i].AppLastUpdateUserid;
                    temp.AffectedEntityId = data[i].OwnerId;
                }

                result.Add(temp);
            }

            return result;
        }


        public OwnerVerificationReportModel GetOwnerVerificationLetterData(
            int?[] localAreas, int?[] owners,
            int? equipmentStatusId, int? ownerStatusId, int? districtId)
        {
            try
            {
                OwnerVerificationReportModel model = new OwnerVerificationReportModel();

                // get owner records
                IQueryable<HetOwner> ownerRecords = _dbContext.HetOwners.AsNoTracking()
                    .Include(x => x.PrimaryContact)
                    .Include(x => x.Business)
                    .Include(x => x.HetEquipments)
                        .ThenInclude(a => a.HetEquipmentAttachments)
                    .Include(x => x.HetEquipments)
                        .ThenInclude(l => l.LocalArea)
                    .Include(x => x.HetEquipments)
                        .ThenInclude(y => y.DistrictEquipmentType)
                    .Include(x => x.LocalArea)
                        .ThenInclude(s => s.ServiceArea)
                            .ThenInclude(d => d.District)
                    .Where(x => x.OwnerStatusTypeId == ownerStatusId &&
                                x.LocalArea.ServiceArea.DistrictId == districtId)
                    .OrderBy(x => x.LocalArea.Name).ThenBy(x => x.OrganizationName);

                if (owners?.Length > 0)
                {
                    ownerRecords = ownerRecords.Where(x => owners.Contains(x.OwnerId));
                }

                if (localAreas?.Length > 0)
                {
                    ownerRecords = ownerRecords.Where(x => localAreas.Contains(x.LocalAreaId));
                }

                // convert to list
                List<HetOwner> ownerList = ownerRecords.ToList();

                if (ownerList.Any())
                {
                    // get address and contact info
                    string address = ownerList[0].LocalArea.ServiceArea.Address;

                    if (!string.IsNullOrEmpty(address))
                    {
                        address = address.Replace("  ", " ");
                        address = address.Replace("amp;", "and");
                        address = address.Replace("|", " | ");
                    }
                    else
                    {
                        address = "";
                    }

                    string contact = $"Phone: {ownerList[0].LocalArea.ServiceArea.Phone} | Fax: {ownerList[0].LocalArea.ServiceArea.Fax}";

                    // generate pdf document name [unique portion only]
                    string fileName = "OwnerVerification";

                    // setup model document generation
                    model = new OwnerVerificationReportModel
                    {
                        ReportDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        Title = fileName,
                        DistrictId = ownerList[0].LocalArea.ServiceArea.District.DistrictId,
                        MinistryDistrictId = ownerList[0].LocalArea.ServiceArea.District.MinistryDistrictId,
                        DistrictName = ownerList[0].LocalArea.ServiceArea.District.Name,
                        DistrictAddress = address,
                        DistrictContact = contact,
                        LocalAreaName = ownerList[0].LocalArea.Name,
                        Owners = new List<OwnerDto>()
                    };

                    // add owner records - must verify district ids too
                    foreach (HetOwner owner in ownerRecords)
                    {
                        if (owner.LocalArea.ServiceArea.District == null ||
                            owner.LocalArea.ServiceArea.District.DistrictId != model.DistrictId)
                        {
                            // missing district - data error [HETS-16]
                            throw new ArgumentException("Missing district id");
                        }

                        owner.ReportDate = model.ReportDate;
                        owner.Title = model.Title;
                        owner.DistrictId = model.DistrictId;
                        owner.MinistryDistrictId = model.MinistryDistrictId;
                        owner.DistrictName = model.DistrictName;
                        owner.DistrictAddress = model.DistrictAddress;
                        owner.DistrictContact = model.DistrictContact;
                        owner.LocalAreaName = model.LocalAreaName;

                        // classification
                        owner.Classification = $"23010-23/{model.MinistryDistrictId}/{owner.OwnerCode}";

                        if (!string.IsNullOrEmpty(owner.SharedKey))
                        {
                            owner.SharedKeyHeader = "Secret Key: ";
                        }

                        // strip out inactive and archived equipment
                        owner.HetEquipments = owner.HetEquipments.Where(x => x.EquipmentStatusTypeId == equipmentStatusId).ToList();

                        // setup address line 2
                        string temp = owner.Address2;

                        if (string.IsNullOrEmpty(temp) && !string.IsNullOrEmpty(owner.City))
                            temp = $"{owner.City}";

                        if (!string.IsNullOrEmpty(temp) && !string.IsNullOrEmpty(owner.City) && owner.City.Trim() != temp.Trim())
                            temp = $"{temp}, {owner.City}";

                        if (string.IsNullOrEmpty(temp) && !string.IsNullOrEmpty(owner.Province))
                            temp = $"{owner.Province}";

                        if (!string.IsNullOrEmpty(temp) && !string.IsNullOrEmpty(owner.Province))
                            temp = $"{temp}, {owner.Province}";

                        if (string.IsNullOrEmpty(temp) && !string.IsNullOrEmpty(owner.PostalCode))
                            temp = $"{owner.PostalCode}";

                        if (!string.IsNullOrEmpty(temp) && !string.IsNullOrEmpty(owner.PostalCode))
                            temp = $"{temp}  {owner.PostalCode}";

                        owner.Address2 = temp;

                        model.Owners.Add(_mapper.Map<OwnerDto>(owner));
                    }
                }

                return model;
            }
            catch (Exception e)
            {
                _logger.LogError("GetOwnerVerificationLetterData exception: {e}", e);
                throw;
            }
        }
    }
}
