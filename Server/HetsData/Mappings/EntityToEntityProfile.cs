using AutoMapper;
using HetsData.Entities;

namespace HetsData.Mappings
{
    public class EntityToEntityProfile : Profile
    {
        public EntityToEntityProfile()
        {
            SourceMemberNamingConvention = new PascalCaseNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<HetRentalRequestSeniorityList, HetEquipment>()
                .ForMember(x => x.HoursYtd, opt => opt.MapFrom(x => x.YtdHours));
        }
    }
}
