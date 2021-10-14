using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class UserDistrictDto
    {
        [JsonProperty("Id")]
        public int UserDistrictId { get; set; }
        public bool IsPrimary { get; set; }
        public int? UserId { get; set; }
        public int? DistrictId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public DistrictDto District { get; set; }
        public UserDto User { get; set; }
    }
}
