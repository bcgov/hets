using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HetsData.Dtos
{
    public class OwnerDto
    {
        public OwnerDto()
        {
            Contacts = new List<ContactDto>();
            Equipment = new List<EquipmentDto>();
        }

        [JsonProperty("Id")]
        public int OwnerId { get; set; }
        public string OrganizationName { get; set; }
        public string OwnerCode { get; set; }
        public string DoingBusinessAs { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string RegisteredCompanyNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public int OwnerStatusTypeId { get; set; }
        public string StatusComment { get; set; }
        public DateTime? ArchiveDate { get; set; }
        public string ArchiveCode { get; set; }
        public string ArchiveReason { get; set; }
        public int? LocalAreaId { get; set; }
        public int? PrimaryContactId { get; set; }
        public string CglCompany { get; set; }
        public string CglPolicyNumber { get; set; }
        public DateTime? CglendDate { get; set; }
        public string WorkSafeBcpolicyNumber { get; set; }
        public DateTime? WorkSafeBcexpiryDate { get; set; }
        public bool? IsMaintenanceContractor { get; set; }
        public bool MeetsResidency { get; set; }
        public int? BusinessId { get; set; }
        public string SharedKey { get; set; }
        public int ConcurrencyControlNumber { get; set; }
        public LocalAreaDto LocalArea { get; set; }
        public OwnerStatusTypeDto OwnerStatusType { get; set; }
        public ContactDto PrimaryContact { get; set; }
        public BusinessDto Business { get; set; }
        public List<ContactDto> Contacts { get; set; }
        public List<EquipmentDto> Equipment { get; set; }

        public bool ActiveRentalRequest { get; set; }
        public string Status { get; set; }
        public string ReportDate { get; set; }
        public string Classification { get; set; }
        public string Title { get; set; }
        public int DistrictId { get; set; }
        public int MinistryDistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictAddress { get; set; }
        public string DistrictContact { get; set; }
        public string LocalAreaName { get; set; }
        public string PrimaryContactRole { get; set; }
        public string PrimaryContactGivenName { get; set; }
        public string PrimaryContactSurname { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string PrimaryContactNameBusiness { get; set; }
        public string LocalAreaNameBusiness { get; set; }
        public string DistrictNameBusiness { get; set; }
        public string SharedKeyHeader { get; set; }
    }
}
