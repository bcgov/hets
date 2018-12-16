using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsData.Model;

namespace HetsData.Helpers
{
    #region Rental Agreement Models

    public class RentalAgreementPdfViewModel
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public HetEquipment Equipment { get; set; }
        public HetProject Project { get; set; }
        public List<HetRentalAgreementRate> RentalAgreementRates { get; set; }
        public List<HetRentalAgreementRate> RentalAgreementRatesWithTotal { get; set; }
        public List<HetRentalAgreementRate> RentalAgreementRatesOvertime { get; set; }
        public float? AgreementTotal { get; set; }
        public string AgreementTotalString { get; set; }
        public string BaseRateString { get; set; }
        public List<HetRentalAgreementRate> RentalAgreementRatesWithoutTotal { get; set; }
        public List<HetRentalAgreementCondition> RentalAgreementConditions { get; set; }
        public List<NoteLine> Note { get; set; }
        public string EstimateStartWork { get; set; }
        public string DatedOn { get; set; }
        public string AgreementCity { get; set; }
        public int? EstimateHours { get; set; }
        public float? EquipmentRate { get; set; }
        public string RatePeriod { get; set; }
        public string RateComment { get; set; }
        public bool ConditionsPresent { get; set; }
    }

    public class NoteLine
    {
        public string Line { get; set; }
    }

    public class TimeRecordLite
    {
        public string EquipmentCode { get; set; }
        public string ProjectName { get; set; }
        public string ProvincialProjectNumber { get; set; }
        public float? HoursYtd { get; set; }
        public int MaximumHours { get; set; }
        public List<HetTimeRecord> TimeRecords { get; set; }
    }

    #endregion

    public static class RentalAgreementHelper
    {
        #region Get a Rental Agreement record (plus associated records)

        /// <summary>
        /// Get a Rental Agreement record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HetRentalAgreement GetRecord(int id, DbAppContext context)
        {
            HetRentalAgreement agreement = context.HetRentalAgreement.AsNoTracking()
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
                .Include(x => x.HetTimeRecord)
                .FirstOrDefault(a => a.RentalAgreementId == id);

            if (agreement != null)
            {
                agreement.Status = agreement.RentalAgreementStatusType.RentalAgreementStatusTypeCode;
                agreement.RatePeriod = agreement.RatePeriodType.RatePeriodTypeCode;

                agreement.HetRentalAgreementOvertimeRate = agreement.HetRentalAgreementRate.Where(x => x.Overtime).ToList();
                agreement.HetRentalAgreementRate = agreement.HetRentalAgreementRate.Where(x => !x.Overtime).ToList();
            }

            return agreement;
        }

        #endregion
        
        #region Adjust Agreement Pdf

        /// <summary>
        /// Uses the rates data to calculate the totals and setup the required data for printing
        /// </summary>
        public static RentalAgreementPdfViewModel CalculateTotals(RentalAgreementPdfViewModel agreement)
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

            foreach (HetRentalAgreementRate rentalRate in agreement.RentalAgreementRatesWithTotal)
            {
                if (rentalRate.Rate != null)  temp = temp + (float)rentalRate.Rate;
                
                // format the rate / percent at the same time
                rentalRate.RateString = FormatRateString(rentalRate, agreement);
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
            foreach (HetRentalAgreementRate rentalRate in agreement.RentalAgreementRatesOvertime)
            {
                rentalRate.RateString = FormatRateString(rentalRate, agreement);
            }

            foreach (HetRentalAgreementRate rentalRate in agreement.RentalAgreementRatesWithoutTotal)
            {
                rentalRate.RateString = FormatRateString(rentalRate, agreement);
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

        private static string FormatRateString(HetRentalAgreementRate rentalRate, RentalAgreementPdfViewModel agreement)
        {
            string temp = "";

            // format the rate
            if (rentalRate.Rate != null)
            {
                temp = $"$ {rentalRate.Rate:0.00} / {FormatRatePeriod(agreement.RatePeriod)}";
            }

            return temp;
        }

        #endregion

        #region Convert Rental Agreement to Pdf Model

        /// <summary>
        /// Printed rental agreement view agreement
        /// </summary>
        /// <param name="agreement"></param>
        /// <param name="agreementCity"></param>
        /// <returns></returns>
        public static RentalAgreementPdfViewModel ToPdfModel(this HetRentalAgreement agreement, string agreementCity)
        {
            RentalAgreementPdfViewModel pdfModel = new RentalAgreementPdfViewModel();

            if (agreement != null)
            {
                pdfModel.DatedOn = ConvertDate(agreement.DatedOn);                
                pdfModel.Equipment = agreement.Equipment;
                pdfModel.EquipmentRate = agreement.EquipmentRate;
                pdfModel.EstimateHours = agreement.EstimateHours;
                pdfModel.EstimateStartWork = ConvertDate(agreement.EstimateStartWork);
                pdfModel.Id = agreement.RentalAgreementId;                
                pdfModel.Number = agreement.Number;
                pdfModel.Project = agreement.Project;
                pdfModel.RateComment = agreement.RateComment;
                pdfModel.RatePeriod = agreement.RatePeriodType.Description;

                // set the agreement city
                if (string.IsNullOrEmpty(agreementCity))
                {
                    agreementCity = " ";
                }

                pdfModel.AgreementCity = agreementCity;

                // format the note
                if (!string.IsNullOrEmpty(agreement.Note))
                {                    
                    string temp = Regex.Replace(agreement.Note, @"\n", "<BR>");
                    string[] tempArray = temp.Split("<BR>");

                    pdfModel.Note = new List<NoteLine>();

                    foreach (string row in tempArray)
                    {
                        NoteLine line = new NoteLine { Line = row };
                        pdfModel.Note.Add(line);
                    }
                }
                
                // ensure they are ordered the way they were added
                pdfModel.RentalAgreementConditions = agreement.HetRentalAgreementCondition
                    .OrderBy(x => x.RentalAgreementConditionId)
                    .ToList();

                pdfModel.RentalAgreementRates = agreement.HetRentalAgreementRate.ToList();
                pdfModel.Status = agreement.RentalAgreementStatusType.Description;
                pdfModel.ConditionsPresent = agreement.HetRentalAgreementCondition.Count > 0;

                foreach (HetRentalAgreementCondition condition in pdfModel.RentalAgreementConditions)
                {
                    if (!string.IsNullOrEmpty(condition.Comment))
                    {
                        condition.ConditionName = condition.Comment;
                    }
                }

                pdfModel = CalculateTotals(pdfModel);
            }

            return pdfModel;
        }

        private static string ConvertDate(DateTime? dateObject)
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

        #endregion

        #region Create Rental Agreement Number

        /// <summary>
        /// Get rental agreement
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRentalAgreementNumber(HetEquipment equipment, DbAppContext context)
        {
            string result = "";

            // validate item.
            if (equipment?.LocalArea != null)
            {
                int localAreaId = equipment.LocalArea.LocalAreaId;

                // get the district
                HetLocalArea localArea = context.HetLocalArea.AsNoTracking()
                    .Include(x => x.ServiceArea)
                        .ThenInclude(y => y.District)
                    .First(x => x.LocalAreaId == localAreaId);

                int? districtId = localArea.ServiceArea.DistrictId;
                int ministryDistrictId = localArea.ServiceArea.District.MinistryDistrictId;
                if (districtId == null) return result;

                // get fiscal year
                HetDistrictStatus status = context.HetDistrictStatus.AsNoTracking()
                .First(x => x.DistrictId == districtId);

                int? fiscalYear = status.CurrentFiscalYear;
                if (fiscalYear == null) return result;

                // fiscal year in the status table stores the "start" of the year
                DateTime fiscalYearStart = new DateTime((int)fiscalYear, 4, 1);
                fiscalYear = fiscalYear + 1;                

                // count the number of rental agreements in the system in this district
                int currentCount = context.HetRentalAgreement
                    .Count(x => x.DistrictId == districtId && 
                                x.AppCreateTimestamp >= fiscalYearStart);

                currentCount++;

                // format of the Rental Agreement number is:
                // * FY-DD-####
                //   FY = last 2 digits of the year
                //   DD - District(2 digits - 1 to 11)
                result = fiscalYear.ToString().Substring(2, 2) + "-" +
                         ministryDistrictId + "-" + 
                         currentCount.ToString("D4");
            }

            return result;
        }

        /// <summary>
        /// Get rental agreement
        /// </summary>
        /// <param name="district"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRentalAgreementNumber(HetDistrict district, DbAppContext context)
        {
            int currentCount = 0;
            string result = "";

            // validate item
            if (district?.DistrictId != null)
            {
                DateTime currentTime = DateTime.UtcNow;

                int fiscalYear = currentTime.Year;

                // fiscal year always ends in March
                if (currentTime.Month > 3)
                {
                    fiscalYear++;
                }

                int districtNumber = district.MinistryDistrictId;
                int districtId = district.DistrictId;

                DateTime fiscalYearStart = new DateTime(fiscalYear - 1, 1, 1);

                // count the number of rental agreements in the system (for this district)
                HetRentalAgreement agreement = context.HetRentalAgreement.AsNoTracking()
                    .Include(x => x.Project.District)
                    .OrderBy(x => x.RentalAgreementId)
                    .LastOrDefault(x => x.DistrictId == districtId && 
                                        x.AppCreateTimestamp >= fiscalYearStart &&
                                        x.Number.Contains("-D"));

                if (agreement != null)
                {
                    string temp = agreement.Number;
                    temp = temp.Substring(temp.LastIndexOf('-') + 1);
                    int.TryParse(temp, out currentCount);
                }

                currentCount++;

                // format of the Rental Agreement number is YYYY-#-#### (new for blank agreements)
                result = fiscalYear + "-D" + districtNumber + "-" + currentCount.ToString("D4");
            }

            return result;
        }

        #endregion

        #region Get Agreement Time Records

        public static TimeRecordLite GetTimeRecords(int id, DbAppContext context, IConfiguration configuration)
        {
            HetRentalAgreement agreement = context.HetRentalAgreement.AsNoTracking()
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
                    configuration.GetSection("MaximumHours:DumpTruck").Value :
                    configuration.GetSection("MaximumHours:Default").Value);

                int equipmentId = agreement.Equipment.EquipmentId;

                hoursYtd = EquipmentHelper.GetYtdServiceHours(equipmentId, context);

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

            TimeRecordLite timeRecord = new TimeRecordLite
            {
                TimeRecords = new List<HetTimeRecord>()
            };

            timeRecord.TimeRecords.AddRange(agreement.HetTimeRecord);
            timeRecord.EquipmentCode = equipmentCode;
            timeRecord.ProjectName = projectName;
            timeRecord.ProvincialProjectNumber = projectNumber;
            timeRecord.HoursYtd = hoursYtd;
            timeRecord.MaximumHours = maxHours;

            return timeRecord;
        }

        #endregion

        #region Get Agreement Rental Rates

        public static List<HetRentalAgreementRate> GetRentalRates(int id, DbAppContext context, IConfiguration configuration)
        {
            HetRentalAgreement agreement = context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.HetRentalAgreementRate)
                .First(x => x.RentalAgreementId == id);

            List<HetRentalAgreementRate> rates = new List<HetRentalAgreementRate>();

            rates.AddRange(agreement.HetRentalAgreementRate);

            return rates;
        }

        #endregion

        #region Get Agreement Conditions

        public static List<HetRentalAgreementCondition> GetConditions(int id, DbAppContext context, IConfiguration configuration)
        {
            HetRentalAgreement agreement = context.HetRentalAgreement.AsNoTracking()
                .Include(x => x.HetRentalAgreementCondition)
                .First(x => x.RentalAgreementId == id);

            List<HetRentalAgreementCondition> conditions = new List<HetRentalAgreementCondition>();

            conditions.AddRange(agreement.HetRentalAgreementCondition);

            return conditions;
        }

        #endregion
    }
}
