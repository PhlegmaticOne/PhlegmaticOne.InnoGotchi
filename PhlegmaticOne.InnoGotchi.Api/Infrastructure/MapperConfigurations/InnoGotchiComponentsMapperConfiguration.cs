using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConverters;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Components;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;

public class InnoGotchiComponentsMapperConfiguration : Profile
{
    public InnoGotchiComponentsMapperConfiguration()
    {
        CreateMap<InnoGotchiComponent, InnoGotchiComponentDto>()
            .ForMember(x => x.ImageUrl, o => o.ConvertUsing<ImageUrlConverter, string>(x => x.ImageUrl));
        CreateMap<InnoGotchiModelComponent, InnoGotchiModelComponentDto>()
            .ForMember(x => x.ImageUrl, o => o.ConvertUsing<ImageUrlConverter, string>(x => x.InnoGotchiComponent.ImageUrl));
        CreateMap<IEnumerable<InnoGotchiComponent>, InnoGotchiComponentCollectionDto>()
            .ForMember(x => x.Components, o => o.MapFrom(y => y.ToList()));
    }
}