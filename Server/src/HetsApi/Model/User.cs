using System.Collections.Generic;
using HetsData.Model;
using Newtonsoft.Json;

namespace HetsApi.Model
{
    public class User
    {
        public int Id { get; set; }
        public string SmUserId { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string DisplayName { get; set; }
        public string UserGuid { get; set; }
        public string SmAuthorizationDirectory { get; set; }
        public int? BusinessId { get; set; }
        public string BusinessGuid { get; set; }

        /// <summary>
        /// Business user flag
        /// </summary>
        public bool BusinessUser { get; set; }

        public HetDistrict District { get; set; }

        [JsonProperty("UserDistricts")]
        public ICollection<HetUserDistrict> HetUserDistrict { get; set; }

        [JsonProperty("UserRoles")]
        public ICollection<HetUserRole> HetUserRole { get; set; }
    }
}
