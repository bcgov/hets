using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HETSAPI.Models;
using HETSAPI.ViewModels;

namespace HETSAPI.Mappings
{
    /// <summary>
    /// Mappoing Extensions used to convert from Models to ViewModels
    /// </summary>
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
                dto.SmAuthorizationDirectory = model.SmAuthorizationDirectory;
                dto.SmUserId = model.SmUserId;
                dto.GroupMemberships = model.GroupMemberships;
                dto.UserRoles = model.UserRoles;
            }
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static RentalRequestSearchResultViewModel ToViewModel(this RentalRequest model)
        {
            var dto = new RentalRequestSearchResultViewModel();
            if (model != null)
            {
                if (model.DistrictEquipmentType != null && model.DistrictEquipmentType != null)
                {
                    dto.EquipmentTypeName = model.DistrictEquipmentType.EquipmentType.Name;
                }                
                dto.Id = model.Id;
                dto.LocalArea = model.LocalArea;
                if (model.Project != null)
                {
                    dto.PrimaryContact = model.Project.PrimaryContact;
                    dto.ProjectName = model.Project.Name;
                    dto.ProjectId = model.Project.Id;
                }                
                dto.Status = model.Status;
                dto.EquipmentCount = model.EquipmentCount;                
                dto.ExpectedEndDate = model.ExpectedEndDate;
                dto.ExpectedStartDate = model.ExpectedStartDate;
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
                dto.FileSize = model.FileContents.Length;
                dto.Id = model.Id;
                dto.Type = model.Type;
                dto.LastUpdateTimestamp = model.LastUpdateTimestamp;
                dto.LastUpdateUserid = model.LastUpdateUserid;                
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
            if (model != null)
            {
                dto.Active = model.Active;
                dto.SmUserId = model.SmUserId;
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

        public static UserRoleViewModel ToViewModel(this UserRole model)
        {
            var dto = new UserRoleViewModel();
            if (model != null)
            {
                dto.EffectiveDate = model.EffectiveDate;
                dto.ExpiryDate = model.ExpiryDate;
                if (model.Role != null)
                {
                    dto.RoleId = model.Role.Id;
                }
                dto.Id = model.Id;
            }
            return dto;
        }

        public static RoleViewModel ToViewModel(this Role model)
        {
            var dto = new RoleViewModel();
            if (model != null)
            {
                dto.Description = model.Description;
                dto.Name = model.Name;
                dto.Id = model.Id;
            }
            return dto;
        }

        public static RolePermissionViewModel ToViewModel(this RolePermission model)
        {
            var dto = new RolePermissionViewModel();
            if (model != null)
            {
                if (model.Permission != null)
                {
                    dto.PermissionId = model.Permission.Id;
                }
                if (model.Role != null)
                {
                    dto.RoleId = model.Role.Id;
                }
                dto.Id = model.Id;
            }
            return dto;
        }

        public static PermissionViewModel ToViewModel(this Permission model)
        {
            var dto = new PermissionViewModel();
            if (model != null)
            {
                dto.Id = model.Id;
                dto.Code = model.Code;
                dto.Name = model.Name;
                dto.Description = model.Description;
            }
            return dto;
        }

        public static ProjectSearchResultViewModel ToViewModel(this Project model)
        {
            var dto = new ProjectSearchResultViewModel();
            if (model != null)
            {
                dto.Id = model.Id;
                dto.Name = model.Name;
                dto.PrimaryContact = model.PrimaryContact;
                dto.District= model.District;
            }
            return dto;         
        }

        public static EquipmentViewModel ToViewModel(this Equipment model)
        {
            var dto = new EquipmentViewModel();
            if (model != null)
            {
                dto.ApprovedDate = model.ApprovedDate;
                dto.ArchiveCode = model.ArchiveCode;
                dto.ArchiveDate = model.ArchiveDate;
                dto.ArchiveReason = model.ArchiveReason;
                dto.Attachments = model.Attachments;
                dto.BlockNumber = model.BlockNumber;
                dto.DumpTruck = model.DumpTruck;
                dto.EquipmentCode = model.EquipmentCode;
                dto.EquipmentAttachments = model.EquipmentAttachments;
                dto.DistrictEquipmentType = model.DistrictEquipmentType;
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
                dto.SeniorityEffectiveDate = model.SeniorityEffectiveDate;
                dto.SeniorityOverrideReason = model.SeniorityOverrideReason;
                dto.SerialNumber = model.SerialNumber;
                dto.ServiceHoursLastYear = model.ServiceHoursLastYear;
                dto.ServiceHoursTwoYearsAgo = model.ServiceHoursTwoYearsAgo;
                dto.ServiceHoursThreeYearsAgo = model.ServiceHoursThreeYearsAgo;
                dto.Size = model.Size;
                dto.Status = model.Status;
                dto.ToDate = model.ToDate;                
                dto.Year = model.Year;
                dto.YearsOfService = model.YearsOfService;
            }
            return dto;
        }

        public static GroupMembershipViewModel ToViewModel(this GroupMembership model)
        {
            var dto = new GroupMembershipViewModel();
            if (model != null)
            {
                dto.Active = model.Active;
                if (model.Group != null)
                {
                    dto.GroupId = model.Group.Id;
                }
                dto.UserId = model.User.Id;
                dto.Id = model.Id;
            }
            return dto;
        }

        public static GroupViewModel ToViewModel(this Group model)
        {
            var dto = new GroupViewModel();
            if (model != null)
            {
                dto.Description = model.Description;
                dto.Name = model.Name;
                dto.Id = model.Id;
            }
            return dto;
        }

        /// <summary>
        /// Converts a History record to a view model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="AffectedEntityId"></param>
        /// <returns></returns>
        public static HistoryViewModel ToViewModel(this History model, int AffectedEntityId)
        {
            HistoryViewModel dto = new HistoryViewModel();
            dto.AffectedEntityId = AffectedEntityId;
            if (model != null)
            {
                dto.HistoryText = model.HistoryText;
                dto.Id = model.Id;
                dto.LastUpdateTimestamp = model.LastUpdateTimestamp;
                dto.LastUpdateUserid = model.LastUpdateUserid;
            }
            return dto;
        }

        public static UserFavouriteViewModel ToViewModel(this UserFavourite model)
        {
            var dto = new UserFavouriteViewModel();
            if (model != null)
            {
                dto.Type = model.Type;
                dto.IsDefault = model.IsDefault;
                dto.Name = model.Name;
                dto.Value = model.Value;
                dto.Id = model.Id;
            }
            return dto;
        }
        
    }
}
