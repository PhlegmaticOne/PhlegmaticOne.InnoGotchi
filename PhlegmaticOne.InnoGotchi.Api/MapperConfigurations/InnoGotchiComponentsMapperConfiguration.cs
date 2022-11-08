using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Components;

namespace PhlegmaticOne.InnoGotchi.Api.MapperConfigurations;

public class InnoGotchiComponentsMapperConfiguration : Profile
{
    public InnoGotchiComponentsMapperConfiguration()
    {
        CreateMap<InnoGotchiComponent, InnoGotchiComponentDto>();
        CreateMap<IEnumerable<InnoGotchiComponent>, InnoGotchiComponentCollectionDto>()
            .ForMember(x => x.Components, o => o.MapFrom(y => y.ToList()));
    }
}