using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Farms;
using PhlegmaticOne.InnoGotchi.Web.ViewModels;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Farms;

namespace PhlegmaticOne.InnoGotchi.Web.MappersConfigurations;

public class FarmMapperConfiguration : Profile
{
    public FarmMapperConfiguration()
    {
        CreateMap<FarmDto, FarmViewModel>().ReverseMap();
        CreateMap<CreateFarmViewModel, CreateFarmDto>();
    }
}