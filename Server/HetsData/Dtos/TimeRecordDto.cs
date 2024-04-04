using Newtonsoft.Json;
using System;

namespace HetsData.Dtos
{
    public class TimeRecordDto
    {
        [JsonProperty("Id")]
        public int TimeRecordId { get; set; }

        private DateTime? _enteredDate;
        public DateTime? EnteredDate {
            get => _enteredDate is DateTime dt ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
            set => _enteredDate = (value.HasValue && value.Value is DateTime dt) ? 
                DateTime.SpecifyKind(dt, DateTimeKind.Utc) : null;
        }
        
        private DateTime _workedDate = new(0001, 01, 01, 00, 00, 00, DateTimeKind.Utc);
        public DateTime WorkedDate {
            get => DateTime.SpecifyKind(_workedDate, DateTimeKind.Utc);
            set => _workedDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }
        
        public int TimePeriodTypeId { get; set; }
        public float? Hours { get; set; }
        public int? RentalAgreementRateId { get; set; }
        public int? RentalAgreementId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public string TimePeriod { get; set; }
    }
}
