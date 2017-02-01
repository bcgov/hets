using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Mappings
{
    public static class MappingExtensions
    {
        public static UserViewModel ToViewModel(this User model)
        {
            var dto = new UserViewModel();
            dto.Active = model.Active;
            dto.Email = model.Email;
            dto.GivenName = model.GivenName;
            dto.Initials = model.Initials;
            dto.Surname = model.Surname;
            dto.Id = model.Id;
            return dto;
        }

        public static UserRoleViewModel ToViewModel(this UserRole model)
        {
            var dto = new UserRoleViewModel();
            dto.EffectiveDate = model.EffectiveDate;
            dto.ExpiryDate = model.ExpiryDate;
            dto.RoleId = model.Role.Id;
            dto.Id = model.Id;
            return dto;
        }

        public static RoleViewModel ToViewModel(this Role model)
        {
            var dto = new RoleViewModel();
            dto.Description = model.Description;
            dto.Name = model.Name;
            dto.Id = model.Id;
            return dto;
        }

        public static RolePermissionViewModel ToViewModel(this RolePermission model)
        {
            var dto = new RolePermissionViewModel();
            if (model.Permission != null)
            {
                dto.PermissionId = model.Permission.Id;
            }                        
            if (model.Role != null)
            {
                dto.RoleId = model.Role.Id;
            }
            dto.Id = model.Id;
            return dto;
        }

        public static PermissionViewModel ToViewModel(this Permission model)
        {
            var dto = new PermissionViewModel();
            dto.Code = model.Code;
            dto.Name = model.Name;
            dto.Description = model.Description;
            return dto;
        }

        public static GroupMembershipViewModel ToViewModel(this GroupMembership model)
        {
            var dto = new GroupMembershipViewModel();
            dto.Active = model.Active;
            dto.GroupId = model.Group.Id;
            dto.UserId = model.User.Id;
            dto.Id = model.Id;
            return dto;
        }

        public static GroupViewModel ToViewModel(this Group model)
        {
            var dto = new GroupViewModel();
            dto.Description = model.Description;
            dto.Name = model.Name;
            dto.Id = model.Id;
            return dto;
        }

        public static UserFavouriteViewModel ToViewModel(this UserFavourite model)
        {
            var dto = new UserFavouriteViewModel();
            dto.Type = model.Type;
            dto.IsDefault = model.IsDefault;
            dto.Name = model.Name;
            dto.Value = model.Value;
            dto.Id = model.Id;
            return dto;
        }
        
    }
}
