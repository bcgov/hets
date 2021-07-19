using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HetsData.Entities;
using HetsData.Dtos;

namespace HetsData.Helpers
{
    #region Rental Agreement Models

    public class RentalAgreementSummaryLite
    {
        public int Id { get; set; }
        public DateTime? DatedOn { get; set; }
    }

    public class RentalAgreementDocViewModel
    {
        public string Classification { get; set; }
        public int Id { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }
        public EquipmentDto Equipment { get; set; }
        public ProjectDto Project { get; set; }
        public List<RentalAgreementRateDto> RentalAgreementRates { get; set; }
        public List<RentalAgreementRateDto> RentalAgreementRatesWithTotal { get; set; }
        public List<RentalAgreementRateDto> RentalAgreementRatesOvertime { get; set; }
        public float? AgreementTotal { get; set; }
        public string AgreementTotalString { get; set; }
        public string BaseRateString { get; set; }
        public List<RentalAgreementRateDto> RentalAgreementRatesWithoutTotal { get; set; }
        public List<RentalAgreementConditionDto> RentalAgreementConditions { get; set; }
        public List<NoteLine> Note { get; set; }
        public string EstimateStartWork { get; set; }
        public string DatedOn { get; set; }
        public string AgreementCity { get; set; }
        public int? EstimateHours { get; set; }
        public float? EquipmentRate { get; set; }
        public string RatePeriod { get; set; }
        public string RateComment { get; set; }
        public bool ConditionsPresent { get; set; }
        public string DoingBusinessAs { get; set; }
        public string EmailAddress { get; set; }
    }

    public class NoteLine
    {
        public string Line { get; set; }
    }

    #endregion

    public static class RentalAgreementHelper
    {
        #region Convert full agreement record to RentalAgreementSummary version

        /// <summary>
        /// Convert to RentalAgreementSummary Model
        /// </summary>
        /// <param name="agreement"></param>
        public static RentalAgreementSummaryLite ToSummaryLiteModel(HetRentalAgreement agreement)
        {
            RentalAgreementSummaryLite agreementSummary = new RentalAgreementSummaryLite();

            if (agreement != null)
            {
                agreementSummary.Id = agreement.RentalAgreementId;
                agreementSummary.DatedOn = agreement.DatedOn;
            }

            return agreementSummary;
        }

        #endregion

        #region Create Rental Agreement Number

        /// <summary>
        /// Get rental agreement
        /// </summary>
        /// <param name="equipment"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetRentalAgreementNumber(int? localAreaId, DbAppContext context)
        {
            string result = "";

            // validate item.
            if (localAreaId != null)
            {
                // get the district
                HetLocalArea localArea = context.HetLocalAreas.AsNoTracking()
                    .Include(x => x.ServiceArea)
                        .ThenInclude(y => y.District)
                    .First(x => x.LocalAreaId == localAreaId);

                int? districtId = localArea.ServiceArea.DistrictId;
                int ministryDistrictId = localArea.ServiceArea.District.MinistryDistrictId;
                if (districtId == null) return result;

                // get fiscal year
                HetDistrictStatus status = context.HetDistrictStatuses.AsNoTracking()
                .First(x => x.DistrictId == districtId);

                int? fiscalYear = status.CurrentFiscalYear;
                if (fiscalYear == null) return result;

                // fiscal year in the status table stores the "start" of the year
                DateTime fiscalYearStart = new DateTime((int)fiscalYear, 4, 1);
                fiscalYear = fiscalYear + 1;

                // count the number of rental agreements in the system in this district
                int currentCount = context.HetRentalAgreements
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
                HetRentalAgreement agreement = context.HetRentalAgreements.AsNoTracking()
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
    }
}
