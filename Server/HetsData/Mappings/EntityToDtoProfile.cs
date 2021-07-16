using AutoMapper;
using HetsData.Model;
using HetsData.Dtos;

namespace HetsData.Mappings
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<HetBatchReport, BatchReportDto>();
            CreateMap<HetBusiness, BusinessDto>()
                .ForMember(x => x.BusinessUsers, opt => opt.MapFrom(x => x.HetBusinessUser))
                .ForMember(x => x.Owners, opt => opt.MapFrom(x => x.HetOwner));
            CreateMap<HetBusinessUser, BusinessUserDto>()
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(x => x.HetBusinessUserRole));
            CreateMap<HetBusinessUserRole, BusinessUserRoleDto>();
            CreateMap<HetConditionType, ConditionTypeDto>();
            CreateMap<HetContact, ContactDto>();
            CreateMap<HetDigitalFile, DigitalFileDto>();
            CreateMap<HetDistrict, DistrictDto>();
            CreateMap<HetDistrictEquipmentType, DistrictEquipmentTypeDto>();
            CreateMap<HetDistrictStatus, DistrictStatusDto>();
            CreateMap<HetEquipmentAttachment, EquipmentAttachmentDto>();
            CreateMap<HetEquipment, EquipmentDto>()
                .ForMember(x => x.EquipmentAttachments, opt => opt.MapFrom(x => x.HetEquipmentAttachment));
            CreateMap<HetEquipmentStatusType, EquipmentStatusTypeDto>();
            CreateMap<HetEquipmentType, EquipmentTypeDto>();
            CreateMap<HetHistory, HistoryDto>();
            CreateMap<HetLocalArea, LocalAreaDto>();
            CreateMap<HetMimeType, MimeTypeDto>();
            CreateMap<HetNote, NoteDto>();
            CreateMap<HetOwner, OwnerDto>()
                .ForMember(x => x.Equipment, opt => opt.MapFrom(x => x.HetEquipment))
                .ForMember(x => x.Contacts, opt => opt.MapFrom(x => x.HetContact));
            CreateMap<HetOwnerStatusType, OwnerStatusTypeDto>();
            CreateMap<HetPermission, PermissionDto>();
            CreateMap<HetProject, ProjectDto>()
                .ForMember(x => x.Contacts, opt => opt.MapFrom(x => x.HetContact))
                .ForMember(x => x.RentalAgreements, opt => opt.MapFrom(x => x.HetRentalAgreement))
                .ForMember(x => x.RentalRequests, opt => opt.MapFrom(x => x.HetRentalRequest));
            CreateMap<HetProvincialRateType, ProvincialRateTypeDto>();
            CreateMap<HetRatePeriodType, RatePeriodTypeDto>();
            CreateMap<HetRegion, RegionDto>();
            CreateMap<HetRentalAgreementCondition, RentalAgreementConditionDto>();
            CreateMap<HetRentalAgreement, RentalAgreementDto>()
                .ForMember(x => x.RentalAgreementConditions, opt => opt.MapFrom(x => x.HetRentalAgreementCondition))
                .ForMember(x => x.RentalAgreementRates, opt => opt.MapFrom(x => x.HetRentalAgreementRate))
                .ForMember(x => x.TimeRecords, opt => opt.MapFrom(x => x.HetTimeRecord));
            CreateMap<HetRentalAgreementRate, RentalAgreementRateDto>();
            CreateMap<HetRentalAgreementStatusType, RentalAgreementStatusTypeDto>();
            CreateMap<HetRentalRequestAttachment, RentalRequestAttachmentDto>();
            CreateMap<HetRentalRequest, RentalRequestDto>()
                .ForMember(x => x.RentalRequestAttachments, opt => opt.MapFrom(x => x.HetRentalRequestAttachment))
                .ForMember(x => x.RentalRequestRotationList, opt => opt.MapFrom(x => x.HetRentalRequestRotationList));
            CreateMap<HetRentalRequestRotationList, RentalRequestRotationListDto>()
                .ForMember(x => x.RentalAgreements, opt => opt.MapFrom(x => x.HetRentalAgreement));
            CreateMap<HetRentalRequestStatusType, RentalRequestStatusTypeDto>();
            CreateMap<HetRole, RoleDto>()
                .ForMember(x => x.RolePermissions, opt => opt.MapFrom(x => x.HetRolePermission));
            CreateMap<HetRolePermission, RolePermissionDto>();
            CreateMap<HetRolloverProgress, RolloverProgressDto>();
            CreateMap<HetServiceArea, ServiceAreaDto>();
            CreateMap<HetTimeRecord, TimeRecordDto>();
            CreateMap<HetUserDistrict, UserDistrictDto>();
            CreateMap<HetUser, UserDto>()
                .ForMember(x => x.UserDistricts, opt => opt.MapFrom(x => x.HetUserDistrict))
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(x => x.HetUserRole));
            CreateMap<HetUserFavourite, UserFavouriteDto>();
            CreateMap<HetUserRole, UserRoleDto>();
        }

    }
}
