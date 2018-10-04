using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public string Note { get; set; }
        public string EstimateStartWork { get; set; }
        public string DatedOn { get; set; }
        public int? EstimateHours { get; set; }
        public float? EquipmentRate { get; set; }
        public string RatePeriod { get; set; }
        public string RateComment { get; set; }
        public bool ConditionsPresent { get; set; }
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
            }

            return agreement;
        }

        #endregion
        
        #region Adjust Agreement Pdf

        /// <summary>
        /// Look for "Other" and replace with Comment text
        /// </summary>
        public static RentalAgreementPdfViewModel FixOther(RentalAgreementPdfViewModel agreement)
        {
            foreach (HetRentalAgreementRate rentalRate in agreement.RentalAgreementRatesWithTotal)
            {
                if (rentalRate.ComponentName.Equals("Other", StringComparison.InvariantCultureIgnoreCase))
                {
                    rentalRate.ComponentName = rentalRate.Comment;
                    rentalRate.Comment = "";
                }
            }

            foreach (HetRentalAgreementRate rentalRate in agreement.RentalAgreementRatesWithoutTotal)
            {
                if (rentalRate.ComponentName.Equals("Other", StringComparison.InvariantCultureIgnoreCase))
                {
                    rentalRate.ComponentName = rentalRate.Comment;
                    rentalRate.Comment = "";
                }
            }

            foreach (HetRentalAgreementCondition condition in agreement.RentalAgreementConditions)
            {
                if (condition.ConditionName.Equals("Other", StringComparison.InvariantCultureIgnoreCase))
                {
                    condition.ConditionName = condition.Comment;
                    condition.Comment = "";
                }
            }

            return agreement;
        }

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
                .FindAll(x => x.ComponentName.StartsWith("Overtime", true, CultureInfo.InvariantCulture))
                .OrderBy(x => x.RentalAgreementRateId).ToList();

            agreement.RentalAgreementRatesWithTotal = agreement.RentalAgreementRates
                .FindAll(x => x.IsIncludedInTotal &&
                              !x.ComponentName.StartsWith("Overtime", true, CultureInfo.InvariantCulture))
                .OrderBy(x => x.RentalAgreementRateId).ToList();

            agreement.RentalAgreementRatesWithoutTotal = agreement.RentalAgreementRates
                .FindAll(x => !x.IsIncludedInTotal &&
                              !x.ComponentName.StartsWith("Overtime", true, CultureInfo.InvariantCulture))
                .OrderBy(x => x.RentalAgreementRateId).ToList();

            // **********************************************
            // calculate the total
            // **********************************************
            float temp = 0.0F;

            foreach (HetRentalAgreementRate rentalRate in agreement.RentalAgreementRatesWithTotal)
            {
                if (rentalRate.PercentOfEquipmentRate != null &&
                    agreement.EquipmentRate != null &&
                    rentalRate.PercentOfEquipmentRate > 0)
                {
                    rentalRate.Rate = (float)rentalRate.PercentOfEquipmentRate * ((float)agreement.EquipmentRate / 100);
                    temp = temp + (float)rentalRate.Rate;
                }
                else if (rentalRate.Rate != null)
                {
                    temp = temp + (float)rentalRate.Rate;
                }

                // format the rate / percent at the same time
                rentalRate.RateString = FormatRateString(rentalRate);
            }

            // add the base amount to the total too
            if (agreement.EquipmentRate != null)
            {
                temp = temp + (float)agreement.EquipmentRate;
            }

            agreement.AgreementTotal = temp;

            // format the base rate
            agreement.BaseRateString = string.Format("$ {0:0.00} / {1}", agreement.EquipmentRate, FormatRatePeriod(agreement.RatePeriod));

            // format the total
            agreement.AgreementTotalString = string.Format("$ {0:0.00} / {1}", agreement.AgreementTotal, FormatRatePeriod(agreement.RatePeriod));

            // **********************************************
            // format the rate / percent values
            // **********************************************
            foreach (HetRentalAgreementRate rentalRate in agreement.RentalAgreementRatesOvertime)
            {
                if (rentalRate.PercentOfEquipmentRate != null &&
                    agreement.EquipmentRate != null &&
                    rentalRate.PercentOfEquipmentRate > 0)
                {
                    rentalRate.Rate = (float)rentalRate.PercentOfEquipmentRate * ((float)agreement.EquipmentRate / 100);
                }

                rentalRate.RateString = FormatRateString(rentalRate);
            }

            foreach (HetRentalAgreementRate rentalRate in agreement.RentalAgreementRatesWithoutTotal)
            {
                if (rentalRate.PercentOfEquipmentRate != null &&
                    agreement.EquipmentRate != null &&
                    rentalRate.PercentOfEquipmentRate > 0)
                {
                    rentalRate.Rate = (float)rentalRate.PercentOfEquipmentRate * ((float)agreement.EquipmentRate / 100);
                }

                rentalRate.RateString = FormatRateString(rentalRate);
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
                        return "Dy";

                    case "hourly":
                    case "hr":
                        return "Hr";

                    case "weekly":
                    case "wk":
                        return "Wk";

                    case "monthly":
                    case "mo":
                        return "Mo";
                }
            }
            else
            {
                period = "";
            }

            return period;
        }

        private static string FormatRateString(HetRentalAgreementRate rentalRate)
        {
            string temp = "";

            // format the rate
            if (rentalRate.Rate != null)
            {
                temp = string.Format("$ {0:0.00} / {1}", rentalRate.Rate, FormatRatePeriod(rentalRate.RatePeriodType.Description));
            }

            // format the percent
            if (rentalRate.PercentOfEquipmentRate != null &&
                rentalRate.PercentOfEquipmentRate > 0)
            {
                temp = string.Format("({0:0.00}%) ", rentalRate.PercentOfEquipmentRate) + temp;
            }

            return temp;
        }

        #endregion

        #region Convert Rental Agreement to Pdf Model

        /// <summary>
        /// Printed rental agreement view agreement
        /// </summary>
        /// <param name="agreement"></param>
        /// <returns></returns>
        public static RentalAgreementPdfViewModel ToPdfModel(this HetRentalAgreement agreement)
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
                pdfModel.Note = agreement.Note;
                pdfModel.Number = agreement.Number;
                pdfModel.Project = agreement.Project;
                pdfModel.RateComment = agreement.RateComment;
                pdfModel.RatePeriod = agreement.RatePeriodType.Description;
                pdfModel.RentalAgreementConditions = agreement.HetRentalAgreementCondition.ToList();
                pdfModel.RentalAgreementRates = agreement.HetRentalAgreementRate.ToList();
                pdfModel.Status = agreement.RentalAgreementStatusType.Description;
                pdfModel.ConditionsPresent = agreement.HetRentalAgreementCondition.Count > 0;

                pdfModel = CalculateTotals(pdfModel);
                pdfModel = FixOther(pdfModel);
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

                result = dt.ToString("yyyy-MM-dd");
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
                DateTime currentTime = DateTime.UtcNow;

                int fiscalYear = currentTime.Year;

                // fiscal year always ends in March.
                if (currentTime.Month > 3)
                {
                    fiscalYear++;
                }

                int localAreaNumber = equipment.LocalArea.LocalAreaNumber;
                int localAreaId = equipment.LocalArea.LocalAreaId;

                DateTime fiscalYearStart = new DateTime(fiscalYear - 1, 1, 1);

                // count the number of rental agreements in the system
                int currentCount = context.HetRentalAgreement
                    .Include(x => x.Equipment.LocalArea)
                    .Count(x => x.Equipment.LocalArea.LocalAreaId == localAreaId && x.AppCreateTimestamp >= fiscalYearStart);

                currentCount++;

                // format of the Rental Agreement number is YYYY-#-####
                result = fiscalYear + "-" + localAreaNumber + "-" + currentCount.ToString("D4");
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
