using System;
using System.Globalization;

namespace HetsCommon
{
    public static class DateUtils
    {
        public const string VancouverTimeZone = "America/Vancouver";
        public const string PacificTimeZone = "Pacific Standard Time";

        // Supporting Permanent Pacific Time (UTC-7) after March 9, 2026, as per the new regulation.
        // System zone stays for historical data
        // Does not allow stale OS time-zone data to reintroduce the former November fallback
        private static readonly DateTime PermanentPacificTimeEffectiveUtc =
            new(2026, 3, 9, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime PermanentPacificTimeEffectiveLocal =
            new(2026, 3, 9, 0, 0, 0, DateTimeKind.Unspecified);
        private static readonly TimeSpan PermanentPacificOffset = TimeSpan.FromHours(-7);

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
        /// Formats a business date without applying a time zone conversion.
        /// </summary>
        public static string FormatDateOnly(DateTime? date)
        {
            return date?.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture).ToUpperInvariant() ?? "";
        }

        /// <summary>
        /// Returns Pacific time if VancouverTimeZone or PacificTimeZone is defined in the system
        /// Otherwise return UTC time.
        /// </summary>
        /// <param name="utcDate"></param>
        /// <returns></returns>
        public static DateTime ConvertUtcToPacificTime(DateTime utcDate)
        {
            if (utcDate >= PermanentPacificTimeEffectiveUtc)
            {
                return DateTime.SpecifyKind(utcDate + PermanentPacificOffset, DateTimeKind.Unspecified);
            }

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
            if (pstDate >= PermanentPacificTimeEffectiveLocal)
            {
                return DateTime.SpecifyKind(pstDate - PermanentPacificOffset, DateTimeKind.Utc);
            }

            return ConvertTimeToUtc(pstDate, VancouverTimeZone) 
                ?? ConvertTimeToUtc(pstDate, PacificTimeZone) 
                ?? AsUTC(pstDate);
        }

        public static DateTime GetPacificNow()
        {
            return ConvertUtcToPacificTime(DateTime.UtcNow);
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
