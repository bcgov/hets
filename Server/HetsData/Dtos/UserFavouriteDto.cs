using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class UserFavouriteDto
    {
        [JsonProperty("Id")]
        public int UserFavouriteId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool? IsDefault { get; set; }
        public int? UserId { get; set; }
        public int? DistrictId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
