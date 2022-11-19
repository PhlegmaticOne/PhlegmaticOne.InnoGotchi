using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Components;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class InnoGotchiesMapperConfiguration : Profile
{
    public InnoGotchiesMapperConfiguration()
    {
        CreateMap<InnoGotchiModelComponentDto, InnoGotchiComponentViewModel>();
        CreateMap<PreviewInnoGotchiDto, PreviewInnoGotchiViewModel>();
        CreateMap<DetailedInnoGotchiDto, DetailedInnoGotchiViewModel>();
        CreateMap<DetailedInnoGotchiDto, PreviewInnoGotchiViewModel>();
        CreateMap<InnoGotchiActionViewModel, IdDto>()
            .ForMember(x => x.Id, o => o.MapFrom(x => x.InnoGotchiId));
    }
}