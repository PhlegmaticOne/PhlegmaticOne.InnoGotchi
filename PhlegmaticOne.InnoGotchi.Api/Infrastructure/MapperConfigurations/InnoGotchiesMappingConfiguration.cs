using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;

public class InnoGotchiesMappingConfiguration : Profile
{
    public InnoGotchiesMappingConfiguration()
    {
        CreateMap<InnoGotchiModel, InnoGotchiDto>()
            .ForMember(x => x.HungerLevel, o => o.MapFrom(y => y.HungerLevel.ToString()))
            .ForMember(x => x.ThirstLevel, o => o.MapFrom(y => y.ThirstyLevel.ToString()));
    }
}