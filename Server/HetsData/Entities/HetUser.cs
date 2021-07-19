using System;
using System.Collections.Generic;

#nullable disable

namespace HetsData.Entities
{
    public partial class HetUser
    {
        public HetUser()
        {
            HetUserDistricts = new HashSet<HetUserDistrict>();
            HetUserFavourites = new HashSet<HetUserFavourite>();
            HetUserRoles = new HashSet<HetUserRole>();
        }

        public int UserId { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string Initials { get; set; }
        public string SmUserId { get; set; }
        public string SmAuthorizationDirectory { get; set; }
        public string Guid { get; set; }
        public string Email { get; set; }
        public string AgreementCity { get; set; }
        public bool Active { get; set; }
        public int? DistrictId { get; set; }
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

        public virtual HetDistrict District { get; set; }
        public virtual ICollection<HetUserDistrict> HetUserDistricts { get; set; }
        public virtual ICollection<HetUserFavourite> HetUserFavourites { get; set; }
        public virtual ICollection<HetUserRole> HetUserRoles { get; set; }
    }
}
