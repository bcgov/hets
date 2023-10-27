using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HetsData.Entities
{
    /// <summary>
    /// Rental Request Database Model Extension
    /// </summary>
    public partial class HetProject
    {
        /// <summary>
        /// Status Codes
        /// </summary>
        public const string StatusActive = "Active";
        public const string StatusComplete = "Completed";

        [NotMapped]
        public bool CanEditStatus { get; set; }

        [NotMapped]
        public string Status { get; set; }

        [NotMapped]
        private DateTime _fiscalYearStartDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);

        [NotMapped]
        public DateTime FiscalYearStartDate {
            get => DateTime.SpecifyKind(_fiscalYearStartDate, DateTimeKind.Utc);
            set => _fiscalYearStartDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
    }
}
