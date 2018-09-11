using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HetsData.Model
{
    public partial class HetRatePeriodType
    {
        public HetRatePeriodType()
        {
            HetRentalAgreement = new HashSet<HetRentalAgreement>();
            HetRentalAgreementRate = new HashSet<HetRentalAgreementRate>();
        }

        [JsonProperty("Id")]
        public int RatePeriodTypeId { get; set; }

        public string RatePeriodTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        [JsonIgnore]public string AppCreateUserDirectory { get; set; }
        [JsonIgnore]public string AppCreateUserGuid { get; set; }
        [JsonIgnore]public string AppCreateUserid { get; set; }
        [JsonIgnore]public DateTime AppCreateTimestamp { get; set; }
        [JsonIgnore]public string AppLastUpdateUserDirectory { get; set; }
        [JsonIgnore]public string AppLastUpdateUserGuid { get; set; }
        [JsonIgnore]public string AppLastUpdateUserid { get; set; }
        [JsonIgnore]public DateTime AppLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string DbCreateUserId { get; set; }
        [JsonIgnore]public DateTime DbCreateTimestamp { get; set; }
        [JsonIgnore]public DateTime DbLastUpdateTimestamp { get; set; }
        [JsonIgnore]public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalAgreement> HetRentalAgreement { get; set; }

        [JsonIgnore]
        public ICollection<HetRentalAgreementRate> HetRentalAgreementRate { get; set; }
    }
}
