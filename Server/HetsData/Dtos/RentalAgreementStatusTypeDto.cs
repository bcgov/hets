using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetsData.Dtos
{
    public class RentalAgreementStatusTypeDto
    {
        [JsonProperty("Id")]
        public int RentalAgreementStatusTypeId { get; set; }
        public string RentalAgreementStatusTypeCode { get; set; }
        public string Description { get; set; }
        public string ScreenLabel { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public int ConcurrencyControlNumber { get; set; }
    }
}
