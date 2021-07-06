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
            CreateMap<HetBusiness, BusinessDto>();
            CreateMap<HetBusinessUser, BusinessUserDto>();
            CreateMap<HetBusinessUserRole, BusinessUserRoleDto>();
            CreateMap<HetConditionType, ConditionTypeDto>();
            CreateMap<HetContact, ContactDto>();
            CreateMap<HetDigitalFile, DigitalFileDto>();
            CreateMap<HetDistrict, DistrictDto>();
            CreateMap<HetDistrictEquipmentType, DistrictEquipmentTypeDto>();
            CreateMap<HetDistrictStatus, DistrictStatusDto>();
            CreateMap<HetEquipmentAttachment, EquipmentAttachmentDto>();
            CreateMap<HetEquipment, EquipmentDto>();
            CreateMap<HetEquipmentStatusType, EquipmentStatusTypeDto>();
            CreateMap<HetEquipmentType, EquipmentTypeDto>();
            CreateMap<HetHistory, HistoryDto>();
            CreateMap<HetLocalArea, LocalAreaDto>();
            CreateMap<HetMimeType, MimeTypeDto>();
            CreateMap<HetNote, NoteDto>();
            CreateMap<HetOwner, OwnerDto>();
            CreateMap<HetOwnerStatusType, OwnerStatusTypeDto>();
            CreateMap<HetPermission, PermissionDto>();
            CreateMap<HetProject, ProjectDto>();
            CreateMap<HetRatePeriodType, RatePeriodTypeDto>();
            CreateMap<HetRegion, RegionDto>();
            CreateMap<HetRentalAgreementCondition, RentalAgreementConditionDto>();
            CreateMap<HetRentalAgreement, RentalAgreementDto>();
            CreateMap<HetRentalAgreementRate, RentalAgreementRateDto>();
            CreateMap<HetRentalAgreementStatusType, RentalAgreementStatusTypeDto>();
            CreateMap<HetRentalRequestAttachment, RentalRequestAttachmentDto>();
            CreateMap<HetRentalRequest, RentalRequestDto>();
            CreateMap<HetRentalRequestRotationList, RentalRequestRotationListDto>();
            CreateMap<HetRentalRequestStatusType, RentalRequestStatusTypeDto>();
            CreateMap<HetRole, RoleDto>();
            CreateMap<HetRolePermission, PermissionDto>();
            CreateMap<HetRolloverProgress, RolloverProgressDto>();
            CreateMap<HetServiceArea, ServiceAreaDto>();
            CreateMap<HetTimeRecord, TimeRecordDto>();
            CreateMap<HetUserDistrict, UserDistrictDto>();
            CreateMap<HetUser, UserDto>();
            CreateMap<HetUserFavourite, UserFavouriteDto>();
            CreateMap<HetUserRole, UserRoleDto>();
        }

    }
}
