using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;

public class InnoGotchiComponentsMapperConfiguration : Profile
{
    public InnoGotchiComponentsMapperConfiguration()
    {
        CreateMap<InnoGotchiComponent, InnoGotchiComponentDto>();
        CreateMap<InnoGotchiModelComponent, InnoGotchiModelComponentDto>()
            .ForMember(x => x.ImageUrl, o => o.MapFrom(x => x.InnoGotchiComponent.ImageUrl))
            .ForMember(x => x.Name, o => o.MapFrom(x => x.InnoGotchiComponent.Name));
    }
}