using AutoMapper;
using HetsData.Dtos;
using HetsData.Model;

namespace HetsData.Mappings
{
    public class DtoToEntityProfile : Profile
    {
        public DtoToEntityProfile()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();

            CreateMap<NoteDto, HetNote>();
        }
    }
}
