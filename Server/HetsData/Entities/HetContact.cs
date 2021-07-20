using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetContact
    {
        public HetContact()
        {
            HetOwners = new HashSet<HetOwner>();
            HetProjects = new HashSet<HetProject>();
        }

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
        public string AppCreateUserDirectory { get; set; }
        public string AppCreateUserGuid { get; set; }
        public string AppCreateUserid { get; set; }
        public DateTime AppCreateTimestamp { get; set; }
        public string AppLastUpdateUserDirectory { get; set; }
        public string AppLastUpdateUserGuid { get; set; }
        public string AppLastUpdateUserid { get; set; }
        public DateTime AppLastUpdateTimestamp { get; set; }
        public string DbCreateUserId { get; set; }
        public DateTime DbCreateTimestamp { get; set; }
        public DateTime DbLastUpdateTimestamp { get; set; }
        public string DbLastUpdateUserId { get; set; }
        public int ConcurrencyControlNumber { get; set; }

        public virtual HetOwner Owner { get; set; }
        public virtual HetProject Project { get; set; }
        public virtual ICollection<HetOwner> HetOwners { get; set; }
        public virtual ICollection<HetProject> HetProjects { get; set; }
    }
}
