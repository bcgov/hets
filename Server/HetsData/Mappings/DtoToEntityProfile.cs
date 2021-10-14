using AutoMapper;
using HetsData.Dtos;
using HetsData.Entities;

namespace HetsData.Mappings
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<BatchReportDto, HetBatchReport>();
            CreateMap<BusinessDto, HetBusiness>();
            CreateMap<BusinessUserDto, HetBusinessUser>();
            CreateMap<BusinessUserRoleDto, HetBusinessUserRole>();
            CreateMap<ConditionTypeDto, HetConditionType>();
            CreateMap<ContactDto, HetContact>();
            CreateMap<DigitalFileDto, HetDigitalFile>();
            CreateMap<DistrictDto, HetDistrict>();
            CreateMap<DistrictEquipmentTypeDto, HetDistrictEquipmentType>();
            CreateMap<DistrictStatusDto, HetDistrictStatus>();
            CreateMap<EquipmentAttachmentDto, HetEquipmentAttachment>();
            CreateMap<EquipmentDto, HetEquipment>();
            CreateMap<EquipmentStatusTypeDto, HetEquipmentStatusType>();
            CreateMap<EquipmentTypeDto, HetEquipmentType>();
            CreateMap<HistoryDto, HetHistory>();
            CreateMap<LocalAreaDto, HetLocalArea>();
            CreateMap<MimeTypeDto, HetMimeType>();
            CreateMap<NoteDto, HetNote>();
            CreateMap<OwnerDto, HetOwner>();
            CreateMap<OwnerStatusTypeDto, HetOwnerStatusType>();
            CreateMap<PermissionDto, HetPermission>();
            CreateMap<ProjectDto, HetProject>();
            CreateMap<RatePeriodTypeDto, HetRatePeriodType>();
            CreateMap<RegionDto, HetRegion>();
            CreateMap<RentalAgreementConditionDto, HetRentalAgreementCondition>();
            CreateMap<RentalAgreementDto, HetRentalAgreement>();
            CreateMap<RentalAgreementRateDto, HetRentalAgreementRate>();
            CreateMap<RentalAgreementStatusTypeDto, HetRentalAgreementStatusType>();
            CreateMap<RentalRequestAttachmentDto, HetRentalRequestAttachment>();
            CreateMap<RentalRequestDto, HetRentalRequest>();
            CreateMap<RentalRequestRotationListDto, HetRentalRequestRotationList>();
            CreateMap<RentalRequestSeniorityListDto, HetRentalRequestSeniorityList>();
            CreateMap<RentalRequestStatusTypeDto, HetRentalRequestStatusType>();
            CreateMap<RoleDto, HetRole>();
            CreateMap<PermissionDto, HetRolePermission>();
            CreateMap<RolloverProgressDto, HetRolloverProgress>();
            CreateMap<ServiceAreaDto, HetServiceArea>();
            CreateMap<TimeRecordDto, HetTimeRecord>();
            CreateMap<UserDistrictDto, HetUserDistrict>();
            CreateMap<UserDto, HetUser>();
            CreateMap<UserFavouriteDto, HetUserFavourite>();
            CreateMap<UserRoleDto, HetUserRole>();
        }
    }
}
