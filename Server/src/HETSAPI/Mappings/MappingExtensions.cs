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
            dto.Surname = model.Surname;
            dto.Id = model.Id;
            dto.District = model.District;
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

        public static ProjectSearchResultViewModel ToViewModel(this Project model)
        {
            var dto = new ProjectSearchResultViewModel();
            dto.Id = model.Id;
            dto.Name = model.Name;
            dto.PrimaryContact = model.PrimaryContact;
            dto.ServiceArea = model.ServiceArea;
            return dto;         
        }

        public static EquipmentViewModel ToViewModel(this Equipment model)
        {
            var dto = new EquipmentViewModel();
            dto.ApprovedDate = model.ApprovedDate;
            dto.ArchiveCode = model.ArchiveCode;
            dto.ArchiveDate = model.ArchiveDate;
            dto.ArchiveReason = model.ArchiveReason;
            dto.Attachments = model.Attachments;
            dto.BlockNumber = model.BlockNumber;
            dto.DumpTruck = model.DumpTruck;            
            dto.EquipCode = model.EquipCode;
            dto.EquipmentAttachments = model.EquipmentAttachments;
            dto.EquipmentType = model.EquipmentType;
            dto.History = model.History;
            dto.Id = model.Id;
            dto.InformationUpdateNeededReason = model.InformationUpdateNeededReason;
            dto.IsInformationUpdateNeeded = model.IsInformationUpdateNeeded;
            dto.IsSeniorityOverridden = model.IsSeniorityOverridden;
            dto.LastVerifiedDate = model.LastVerifiedDate;
            dto.LicencePlate = model.LicencePlate;
            dto.LocalArea = model.LocalArea;
            dto.Make = model.Make;
            dto.Model = model.Model;
            dto.Notes = model.Notes;
            dto.Operator = model.Operator;
            dto.Owner = model.Owner;
            dto.PayRate = model.PayRate;
            dto.ReceivedDate = model.ReceivedDate;
            dto.RefuseRate = model.RefuseRate;
            dto.Seniority = model.Seniority;
            dto.SeniorityAudit = model.SeniorityAudit;
            dto.SeniorityEffectiveDate = model.SeniorityEffectiveDate;
            dto.SeniorityOverrideReason = model.SeniorityOverrideReason;
            dto.SerialNum = model.SerialNum;
            dto.ServiceHoursLastYear = model.ServiceHoursLastYear;
            dto.ServiceHoursTwoYearsAgo = model.ServiceHoursTwoYearsAgo;
            dto.ServiceHoursThreeYearsAgo = model.ServiceHoursThreeYearsAgo;
            dto.Size = model.Size;
            dto.Status = model.Status;
            dto.ToDate = model.ToDate;
            dto.Type = model.Type;
            dto.Year = model.Year;
            dto.YearsOfService = model.YearsOfService;

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
