using System;
using System.Globalization;

namespace HetsCommon
{
    public static class DateUtils
    {
        public const string VancouverTimeZone = "America/Vancouver";
        public const string PacificTimeZone = "Pacific Standard Time";

        public static (bool parsed, DateTime? parsedDate) ParseDate(object val)
        {
            if (val == null)
                return (true, null);

            if (val.GetType() == typeof(DateTime))
            {
                return (true, (DateTime)val);
            }

            var formats = new string[] { 
                "yyyyMMdd", 
                "yyyy-MM-dd", 
                "yyyy/MM/dd", 
                "yyyy.MM.dd", 
                "yyyyMd", 
                "yyyy-M-d", 
                "yyyy/M/dd", 
                "yyyy.M.d" 
            };

            var dateStr = val.ToString();

            if (string.IsNullOrWhiteSpace(dateStr))
                return (true, null);

            return (
                DateTime.TryParseExact(
                    dateStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate), 
                parsedDate
            );
        }

        public static string CovertToString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Returns Pacific time if VancouverTimeZone or PacificTimeZone is defined in the system
        /// Otherwise return UTC time.
        /// </summary>
        /// <param name="utcDate"></param>
        /// <returns></returns>
        public static DateTime ConvertUtcToPacificTime(DateTime utcDate)
        {
            return ConvertTimeFromUtc(utcDate, VancouverTimeZone)
                ?? ConvertTimeFromUtc(utcDate, PacificTimeZone)
                ?? utcDate;
        }

        private static DateTime? ConvertTimeFromUtc(DateTime date, string timeZoneId)
        {
            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTimeFromUtc(date, timezone);
            }
            catch (TimeZoneNotFoundException)
            {
                return null;
            }
        }

        public static DateTime ConvertPacificToUtcTime(DateTime pstDate)
        {
            return ConvertTimeToUtc(pstDate, VancouverTimeZone) 
                ?? ConvertTimeToUtc(pstDate, PacificTimeZone) 
                ?? AsUTC(pstDate);
        }

        public static DateTime AsUTC(DateTime dt)
        {
            return new DateTime(
                dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Microsecond, DateTimeKind.Utc);
        }

        private static DateTime? ConvertTimeToUtc(DateTime date, string timeZoneId)
        {
            try
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTimeToUtc(date, timezone);
            }
            catch (TimeZoneNotFoundException)
            {
                return null;
            }
        }

        public static (DateTime utcDateFrom, DateTime utcDateTo) GetUtcDateRange(DateTime pstDateFrom, DateTime pstDateTo)
        {
            pstDateFrom = new DateTime(pstDateFrom.Year, pstDateFrom.Month, pstDateFrom.Day, 0, 0, 0, DateTimeKind.Unspecified);
            pstDateTo = new DateTime(pstDateTo.Year, pstDateTo.Month, pstDateTo.Day, 0, 0, 0, DateTimeKind.Unspecified)
                .AddDays(1)
                .AddSeconds(-1);

            var utcDateFrom = ConvertPacificToUtcTime(pstDateFrom);
            var utcDateTo = ConvertPacificToUtcTime(pstDateTo);

            return (utcDateFrom, utcDateTo);
        }
    }
}
