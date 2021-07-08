namespace HetsData.Dtos
{
    public class ProvincialRateTypeDto
    {
        public int Id { get; set; }
        public string RateType { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public float? Rate { get; set; }
        public bool Overtime { get; set; }
        public bool IsIncludedInTotal { get; set; }
        public bool IsPercentRate { get; set; }
        public bool IsRateEditable { get; set; }
        public bool IsInTotalEditable { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
