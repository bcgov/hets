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

            CreateMap<HetNote, NoteDto>();
        }

    }
}
