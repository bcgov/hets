using AutoMapper;
using HetsData.Dtos;
using HetsData.Helpers;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HetsData.Repositories
{
    public interface IRentalAgreementRepository
    {
        RentalAgreementDto GetRecord(int id);
        TimeRecordLite GetTimeRecords(int id, int? districtId);
        List<RentalAgreementRateDto> GetRentalRates(int id);
        List<RentalAgreementConditionDto> GetConditions(int id);
    }

    public class RentalAgreementRepository : IRentalAgreementRepository
    {
        private IMapper _mapper;
        private DbAppContext _dbContext;
        private IConfiguration _configuration;

        public RentalAgreementRepository(DbAppContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Get a Rental Agreement record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RentalAgreementDto GetRecord(int id)
        {
            var entity = _dbContext.HetRentalAgreement.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.RatePeriodType)
                .Include(x => x.District)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.HetEquipmentAttachment)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.EquipmentStatusType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(p => p.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(p => p.ProjectStatusType)
                .Include(x => x.HetRentalAgreementCondition)
                .Include(x => x.HetRentalAgreementRate)
                    .ThenInclude(x => x.RatePeriodType)
                .Include(x => x.HetTimeRecord)
                .FirstOrDefault(a => a.RentalAgreementId == id);

            if (entity == null)
                return null;

            var dto = _mapper.Map<RentalAgreementDto>(entity);

            dto.Status = entity.RentalAgreementStatusType.RentalAgreementStatusTypeCode;
            dto.RatePeriod = entity.RatePeriodType.RatePeriodTypeCode;
            dto.OvertimeRates = dto.RentalAgreementRates.Where(x => x.Overtime).ToList();
            dto.RentalAgreementRates = dto.RentalAgreementRates.Where(x => !x.Overtime).ToList();

            return dto;
        }

        public TimeRecordLite GetTimeRecords(int id, int? districtId)
        {
            // get fiscal year
            HetDistrictStatus status = _dbContext.HetDistrictStatus.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;

            // get agreement and time records
            HetRentalAgreement agreement = _dbContext.HetRentalAgreement.AsNoTracking()
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(z => z.EquipmentType)
                .Include(x => x.Project)
                .Include(x => x.HetTimeRecord)
                .First(x => x.RentalAgreementId == id);

            // get the max hours for this equipment type
            float? hoursYtd = 0.0F;
            int maxHours = 0;
            string equipmentCode = "";

            if (agreement.Equipment?.EquipmentId != null &&
                agreement.Equipment.DistrictEquipmentType?.EquipmentType != null)
            {
                maxHours = Convert.ToInt32(agreement.Equipment.DistrictEquipmentType.EquipmentType.IsDumpTruck ?
                    _configuration.GetSection("MaximumHours:DumpTruck").Value :
                    _configuration.GetSection("MaximumHours:Default").Value);

                int equipmentId = agreement.Equipment.EquipmentId;

                hoursYtd = EquipmentHelper.GetYtdServiceHours(equipmentId, _dbContext);

                equipmentCode = agreement.Equipment.EquipmentCode;
            }

            // get the project info
            string projectName = "";
            string projectNumber = "";

            if (agreement.Project != null)
            {
                projectName = agreement.Project.Name;
                projectNumber = agreement.Project.ProvincialProjectNumber;
            }

            // fiscal year in the status table stores the "start" of the year
            TimeRecordLite timeRecord = new TimeRecordLite();

            if (fiscalYear != null)
            {
                DateTime fiscalYearStart = new DateTime((int)fiscalYear, 4, 1);

                timeRecord.TimeRecords = new List<TimeRecordDto>();
                timeRecord.TimeRecords
                    .AddRange(_mapper.Map<List<TimeRecordDto>>(agreement.HetTimeRecord.Where(x => x.WorkedDate >= fiscalYearStart)));
            }

            timeRecord.EquipmentCode = equipmentCode;
            timeRecord.ProjectName = projectName;
            timeRecord.ProvincialProjectNumber = projectNumber;
            timeRecord.HoursYtd = hoursYtd;
            timeRecord.MaximumHours = maxHours;

            return timeRecord;
        }

        public List<RentalAgreementRateDto> GetRentalRates(int id)
        {
            HetRentalAgreement agreement = _dbContext.HetRentalAgreement.AsNoTracking()
                .Include(x => x.HetRentalAgreementRate)
                    .ThenInclude(x => x.RatePeriodType)
                .First(x => x.RentalAgreementId == id);

            List<HetRentalAgreementRate> rates = new List<HetRentalAgreementRate>();

            rates.AddRange(agreement.HetRentalAgreementRate);

            return _mapper.Map<List<RentalAgreementRateDto>>(rates);
        }

        public List<RentalAgreementConditionDto> GetConditions(int id)
        {
            HetRentalAgreement agreement = _dbContext.HetRentalAgreement.AsNoTracking()
                .Include(x => x.HetRentalAgreementCondition)
                .First(x => x.RentalAgreementId == id);

            return _mapper.Map<List<RentalAgreementConditionDto>>(agreement.HetRentalAgreementCondition);
        }
    }
}
