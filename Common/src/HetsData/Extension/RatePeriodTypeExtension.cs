using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Model
{
    /// <summary>
    /// Rate Period Type Database Model Extension
    /// </summary>
    public sealed partial class HetRatePeriodType
    {
        /// <summary>
        /// Rate Period Types
        /// </summary>
        public const string PeriodWeekly = "Weekly";
        public const string PeriodHourly = "Hr";
        public const string PeriodDaily = "Daily";
        public const string PeriodMonthly = "Monthly";
    }
}
