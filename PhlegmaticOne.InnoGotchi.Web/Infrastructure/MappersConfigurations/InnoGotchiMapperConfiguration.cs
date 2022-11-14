using AutoMapper;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Web.ViewModels.Constructor;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.MappersConfigurations;

public class InnoGotchiMapperConfiguration : Profile
{
    public InnoGotchiMapperConfiguration()
    {
        CreateMap<CreateInnoGotchiComponentViewModel, InnoGotchiComponentDto>();
        CreateMap<CreateInnoGotchiViewModel, CreateInnoGotchiDto>();
    }
}