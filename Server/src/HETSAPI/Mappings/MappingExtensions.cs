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

        /// <summary>
        /// Convert User to CurrentUserViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static CurrentUserViewModel ToCurrentUserViewModel(this User model)
        {
            var dto = new CurrentUserViewModel();
            if (model != null)
            {
                dto.Email = model.Email;
                dto.GivenName = model.GivenName;
                dto.Surname = model.Surname;
                dto.Id = model.Id;
                dto.District = model.District;
                dto.GroupMemberships = model.GroupMemberships;
                dto.UserRoles = model.UserRoles;
            }
            return dto;
        }

        public static RentalRequestSearchResultViewModel ToViewModel(this RentalRequest model)
        {
            var dto = new RentalRequestSearchResultViewModel();
            if (model != null)
            {
                dto.EquipmentType = model.EquipmentType.Name;
                dto.Id = model.Id;
                dto.LocalArea = model.LocalArea;
                dto.PrimaryContact = model.Project.PrimaryContact;
                dto.ProjectName = model.Project.Name;
                dto.Status = model.Status;
                dto.EquipmentCount = model.EquipmentCount;                
            }
            return dto;
        }

        // ********* COMMON VIEW MODEL MAPPINGS

        /// <summary>
        /// Convert Attachment to AttachmentViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static AttachmentViewModel ToViewModel(this Attachment model)
        {
            var dto = new AttachmentViewModel();
            if (model != null)
            {
                dto.Description = model.Description;
                dto.FileName = model.FileName;
                dto.Id = model.Id;
                dto.Type = model.Type;
            }
            return dto;
        }

        /// <summary>
        /// Converts a list of Attachments to a list of AttachmentViewModels
        /// </summary>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public static List<AttachmentViewModel> GetAttachmentListAsViewModel(List<Attachment> attachments)
        {
            List<AttachmentViewModel> result = new List<AttachmentViewModel>();
            foreach (Attachment attachment in attachments)
            {
                if (attachment != null)
                {
                    result.Add(attachment.ToViewModel());
                }
            }
            return result;
        }

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
            dto.LocalArea = model.LocalArea;
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
            dto.EquipmentCode = model.EquipmentCode;
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
            dto.SerialNumber = model.SerialNumber;
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
