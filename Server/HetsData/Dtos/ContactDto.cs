using Newtonsoft.Json;

namespace HetsData.Dtos
{
    public class ContactDto
    {
        [JsonProperty("Id")]
        public int ContactId { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string Role { get; set; }
        public string Notes { get; set; }
        public string EmailAddress { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string FaxPhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public int? OwnerId { get; set; }
        public int? ProjectId { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
