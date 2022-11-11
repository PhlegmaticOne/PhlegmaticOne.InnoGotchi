using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;

public class InnoGotchiComponentsMapperConfiguration : Profile
{
    public InnoGotchiComponentsMapperConfiguration()
    {
        CreateMap<InnoGotchiComponent, InnoGotchiComponentDto>();
        CreateMap<IEnumerable<InnoGotchiComponent>, InnoGotchiComponentCollectionDto>()
            .ForMember(x => x.Components, o => o.MapFrom(y => y.ToList()));
    }
}