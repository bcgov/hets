using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class PermissionDto
    {
        [JsonProperty("Id")]
        public int PermissionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
