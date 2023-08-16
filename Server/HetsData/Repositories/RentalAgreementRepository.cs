using AutoMapper;
using HetsData.Dtos;
using HetsData.Helpers;
using HetsData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HetsCommon;

namespace HetsData.Repositories
{
    public interface IRentalAgreementRepository
    {
        RentalAgreementDto GetRecord(int id);
        TimeRecordLite GetTimeRecords(int id, int? districtId);
        List<RentalAgreementRateDto> GetRentalRates(int id);
        List<RentalAgreementConditionDto> GetConditions(int id);
        RentalAgreementDocViewModel GetRentalAgreementReportModel(RentalAgreementDto agreement, string agreementCity);
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
            var entity = _dbContext.HetRentalAgreements.AsNoTracking()
                .Include(x => x.RentalAgreementStatusType)
                .Include(x => x.RatePeriodType)
                .Include(x => x.District)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.Owner)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(d => d.EquipmentType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.HetEquipmentAttachments)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.EquipmentStatusType)
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.LocalArea.ServiceArea.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(p => p.District.Region)
                .Include(x => x.Project)
                    .ThenInclude(p => p.ProjectStatusType)
                .Include(x => x.HetRentalAgreementConditions)
                .Include(x => x.HetRentalAgreementRates)
                    .ThenInclude(x => x.RatePeriodType)
                .Include(x => x.HetTimeRecords)
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
            HetDistrictStatus status = _dbContext.HetDistrictStatuses.AsNoTracking()
                .First(x => x.DistrictId == districtId);

            int? fiscalYear = status.CurrentFiscalYear;

            // get agreement and time records
            HetRentalAgreement agreement = _dbContext.HetRentalAgreements.AsNoTracking()
                .Include(x => x.Equipment)
                    .ThenInclude(y => y.DistrictEquipmentType)
                        .ThenInclude(z => z.EquipmentType)
                .Include(x => x.Project)
                .Include(x => x.HetTimeRecords)
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
                DateTime fiscalYearStart = DateUtils.ConvertPacificToUtcTime(
                    new DateTime((int)fiscalYear, 4, 1, 0, 0, 0));

                timeRecord.TimeRecords = new List<TimeRecordDto>();
                timeRecord.TimeRecords.AddRange(
                    _mapper.Map<List<TimeRecordDto>>(
                        agreement.HetTimeRecords.Where(x => 
                            x.WorkedDate >= fiscalYearStart)));
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
            HetRentalAgreement agreement = _dbContext.HetRentalAgreements.AsNoTracking()
                .Include(x => x.HetRentalAgreementRates)
                    .ThenInclude(x => x.RatePeriodType)
                .First(x => x.RentalAgreementId == id);

            List<HetRentalAgreementRate> rates = new List<HetRentalAgreementRate>();

            rates.AddRange(agreement.HetRentalAgreementRates);

            return _mapper.Map<List<RentalAgreementRateDto>>(rates);
        }

        public List<RentalAgreementConditionDto> GetConditions(int id)
        {
            HetRentalAgreement agreement = _dbContext.HetRentalAgreements.AsNoTracking()
                .Include(x => x.HetRentalAgreementConditions)
                .First(x => x.RentalAgreementId == id);

            return _mapper.Map<List<RentalAgreementConditionDto>>(agreement.HetRentalAgreementConditions);
        }

        /// <summary>
        /// Printed rental agreement view agreement
        /// </summary>
        /// <param name="agreement"></param>
        /// <param name="agreementCity"></param>
        /// <returns></returns>
        public RentalAgreementDocViewModel GetRentalAgreementReportModel(RentalAgreementDto agreement, string agreementCity)
        {
            RentalAgreementDocViewModel docModel = new RentalAgreementDocViewModel();

            if (agreement != null)
            {
                docModel.AgreementCity = agreementCity;
                docModel.Equipment = agreement.Equipment;
                docModel.EquipmentRate = agreement.EquipmentRate;
                docModel.EstimateHours = agreement.EstimateHours;
                docModel.EstimateStartWork = ConvertDate(agreement.EstimateStartWork);
                docModel.Number = agreement.Number;
                docModel.Project = agreement.Project;
                docModel.RateComment = agreement.RateComment;
                docModel.RatePeriod = agreement.RatePeriodType.Description;
                docModel.AgreementCity = agreement.AgreementCity;
                docModel.DatedOn = (agreement.DatedOn ?? DateTime.UtcNow).ToString("MM/dd/yyyy");
                docModel.DoingBusinessAs = agreement.Equipment.Owner.DoingBusinessAs;
                docModel.EmailAddress = agreement.Equipment.Owner.PrimaryContact.EmailAddress;

                // format owner address
                string tempAddress = agreement.Equipment.Owner.Address2;

                if (string.IsNullOrEmpty(tempAddress) && !string.IsNullOrEmpty(agreement.Equipment.Owner.City))
                    tempAddress = $"{agreement.Equipment.Owner.City}";

                if (!string.IsNullOrEmpty(tempAddress) && !string.IsNullOrEmpty(agreement.Equipment.Owner.City) && agreement.Equipment.Owner.City.Trim() != tempAddress.Trim())
                    tempAddress = $"{tempAddress}, {agreement.Equipment.Owner.City}";

                if (string.IsNullOrEmpty(tempAddress) && !string.IsNullOrEmpty(agreement.Equipment.Owner.Province))
                    tempAddress = $"{agreement.Equipment.Owner.Province}";

                if (!string.IsNullOrEmpty(tempAddress) && !string.IsNullOrEmpty(agreement.Equipment.Owner.Province))
                    tempAddress = $"{tempAddress}, {agreement.Equipment.Owner.Province}";

                if (string.IsNullOrEmpty(tempAddress) && !string.IsNullOrEmpty(agreement.Equipment.Owner.PostalCode))
                    tempAddress = $"{agreement.Equipment.Owner.PostalCode}";

                if (!string.IsNullOrEmpty(tempAddress) && !string.IsNullOrEmpty(agreement.Equipment.Owner.PostalCode))
                    tempAddress = $"{tempAddress}  {agreement.Equipment.Owner.PostalCode}";

                agreement.Equipment.Owner.Address2 = tempAddress;

                // format the note
                if (!string.IsNullOrEmpty(agreement.Note))
                {
                    string temp = Regex.Replace(agreement.Note, @"\n", "<BR>");
                    string[] tempArray = temp.Split("<BR>");

                    docModel.Note = new List<NoteLine>();

                    foreach (string row in tempArray)
                    {
                        NoteLine line = new NoteLine { Line = row };
                        docModel.Note.Add(line);
                    }
                }

                // ensure they are ordered the way they were added
                docModel.RentalAgreementConditions = agreement.RentalAgreementConditions
                    .OrderBy(x => x.RentalAgreementConditionId)
                    .ToList();

                docModel.RentalAgreementRates = agreement.RentalAgreementRates.Where(x => x.Active).ToList();
                docModel.Status = agreement.RentalAgreementStatusType.Description;
                docModel.ConditionsPresent = agreement.RentalAgreementConditions.Count > 0;

                foreach (var condition in docModel.RentalAgreementConditions)
                {
                    if (!string.IsNullOrEmpty(condition.Comment))
                    {
                        condition.ConditionName = condition.Comment;
                    }
                }

                docModel = CalculateTotals(docModel);

                // classification (Rental Agreement)
                docModel.Classification = $"23010-30/{agreement.Number}";
            }

            return docModel;
        }

        private string ConvertDate(DateTime? dateObject)
        {
            string result = "";

            if (dateObject != null)
            {
                // since the PDF template is raw HTML and won't convert a date object, we must adjust the time zone here
                TimeZoneInfo tzi;

                try
                {
                    // try the iana time zone first.
                    tzi = TimeZoneInfo.FindSystemTimeZoneById("America / Vancouver");
                }
                catch
                {
                    tzi = null;
                }

                if (tzi == null)
                {
                    try
                    {
                        tzi = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    }
                    catch
                    {
                        tzi = null;
                    }
                }

                DateTime dt;

                if (tzi != null)
                {
                    dt = TimeZoneInfo.ConvertTime((DateTime)dateObject, tzi);

                }
                else
                {
                    dt = (DateTime)dateObject;

                }

                result = dt.ToString("yyyy-MMM-dd").ToUpper();
            }

            return result;
        }

        /// <summary>
        /// Uses the rates data to calculate the totals and setup the required data for printing
        /// </summary>
        public static RentalAgreementDocViewModel CalculateTotals(RentalAgreementDocViewModel agreement)
        {
            // **********************************************
            // setup the rates lists ->
            // 1. overtime records
            // 2. records in the total and
            // 3. records not included
            // **********************************************
            agreement.RentalAgreementRatesOvertime = agreement.RentalAgreementRates
                .FindAll(x => x.Overtime)
                .OrderBy(x => x.RentalAgreementRateId).ToList();

            agreement.RentalAgreementRatesWithTotal = agreement.RentalAgreementRates
                .FindAll(x => x.IsIncludedInTotal && !x.Overtime)
                .OrderBy(x => x.RentalAgreementRateId).ToList();

            agreement.RentalAgreementRatesWithoutTotal = agreement.RentalAgreementRates
                .FindAll(x => !x.IsIncludedInTotal && !x.Overtime)
                .OrderBy(x => x.RentalAgreementRateId).ToList();

            // **********************************************
            // calculate the total
            // **********************************************
            float temp = 0.0F;

            foreach (var rentalRate in agreement.RentalAgreementRatesWithTotal)
            {
                if (rentalRate.Rate != null) temp = temp + (float)rentalRate.Rate;

                // format the rate / percent at the same time
                rentalRate.RateString = FormatRateString(rentalRate, agreement, rentalRate.RatePeriodType.RatePeriodTypeCode);
            }

            // add the base amount to the total too
            if (agreement.EquipmentRate != null)
            {
                temp = temp + (float)agreement.EquipmentRate;
            }

            agreement.AgreementTotal = temp;

            // format the base rate
            agreement.BaseRateString = $"$ {agreement.EquipmentRate:0.00} / {FormatRatePeriod(agreement.RatePeriod)}";

            // format the total
            agreement.AgreementTotalString =
                $"$ {agreement.AgreementTotal:0.00} / {FormatRatePeriod(agreement.RatePeriod)}";

            // **********************************************
            // format the rate / percent values
            // **********************************************
            foreach (var rentalRate in agreement.RentalAgreementRatesOvertime)
            {
                rentalRate.RateString = FormatOvertimeRateString(rentalRate);
            }

            foreach (var rentalRate in agreement.RentalAgreementRatesWithoutTotal)
            {
                rentalRate.RateString = FormatRateString(rentalRate, agreement, rentalRate.RatePeriodType.RatePeriodTypeCode);
            }

            return agreement;
        }

        private static string FormatRatePeriod(string period)
        {
            if (!string.IsNullOrEmpty(period))
            {
                switch (period.ToLower())
                {
                    case "daily":
                    case "dy":
                        return "Day";

                    case "hourly":
                    case "hr":
                        return "Hr";

                    case "weekly":
                    case "wk":
                        return "Wk";

                    case "monthly":
                    case "mo":
                        return "Mth";

                    case "negotiated":
                    case "neg":
                        return "Neg";
                }
            }
            else
            {
                period = "";
            }

            return period;
        }

        private static string FormatRateString(RentalAgreementRateDto rentalRate, RentalAgreementDocViewModel agreement, string ratePeriodType)
        {
            string temp = "";

            // format the rate
            if (rentalRate.Rate != null && rentalRate.Set)
            {
                temp = $"$ {rentalRate.Rate:0.00} / Set";
            }
            else if (rentalRate.Rate != null && !string.IsNullOrEmpty(ratePeriodType))
            {
                temp = $"$ {rentalRate.Rate:0.00} / {FormatRatePeriod(ratePeriodType)}";
            }
            else if (rentalRate.Rate != null)
            {
                temp = $"$ {rentalRate.Rate:0.00} / {FormatRatePeriod(agreement.RatePeriod)}";
            }

            return temp;
        }

        // HETS-1017 - Updates to Rental Agreement -> Overtime is always in Hours (/Hr)
        private static string FormatOvertimeRateString(RentalAgreementRateDto rentalRate)
        {
            string temp = "";

            // format the rate
            if (rentalRate.Rate != null)
            {
                temp = $"$ {rentalRate.Rate:0.00} / Hr";
            }

            return temp;
        }

    }
}
