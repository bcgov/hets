using System;
using System.Collections.Generic;
using HetsData.Model;

namespace HetsApi.Model
{
    public sealed class CurrentUser : HetUser
    {
        public CurrentUser()
        {
        }

        public CurrentUser(int id, string givenName = null, string surname = null, string email = null, 
            bool? active = null, string smUserId = null, string smAuthorizationDirectory = null, 
            List<HetUserRole> userRoles = null, HetDistrict district = null)
        {
            Id = id;
            GivenName = givenName;
            Surname = surname;
            Email = email;

            if (active != null)
            {
                Active = (bool)active;
            }
            
            SmUserId = smUserId;
            SmAuthorizationDirectory = smAuthorizationDirectory;
            UserRoles = userRoles;
            District = district;
        }

        public int Id
        {
            get => UserId;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                UserId = value;
            }
        }

        public List<HetUserRole> UserRoles { get; set; }                
    }
}
