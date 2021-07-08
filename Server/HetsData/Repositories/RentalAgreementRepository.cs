using AutoMapper;
using HetsData.Dtos;
using HetsData.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HetsData.Repositories
{
    public interface IRentalAgreementRepository
    {
        RentalAgreementDto GetRecord(int id);
    }

    public class RentalAgreementRepository : IRentalAgreementRepository
    {
        private IMapper _mapper;
        private DbAppContext _dbContext;

        public RentalAgreementRepository(DbAppContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
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
    }
}
