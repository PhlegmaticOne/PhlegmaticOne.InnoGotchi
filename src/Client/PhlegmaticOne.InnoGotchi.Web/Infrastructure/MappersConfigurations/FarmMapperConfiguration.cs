using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Farms;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class FarmMapperConfiguration : Profile
{
    public FarmMapperConfiguration()
    {
        CreateMap<DetailedFarmDto, DetailedFarmViewModel>()
            .ForMember(x => x.InnoGotchies, o => o.MapFrom(x => x.InnoGotchies));
        CreateMap<PreviewFarmDto, PreviewFarmViewModel>();

        CreateMap<CreateFarmViewModel, CreateFarmDto>();
    }
}