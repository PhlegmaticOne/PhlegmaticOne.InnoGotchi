using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;

public class FarmMapperConfiguration : Profile
{
    public FarmMapperConfiguration()
    {
        CreateMap<Farm, FarmDto>();
        CreateMap<CreateFarmDto, IdentityFarmModel>()
            .ForMember(x => x.ProfileId, o => o.Ignore());
        CreateMap<CreateInnoGotchiDto, IdentityInnoGotchiModel>()
            .ForMember(x => x.ProfileId, o => o.Ignore())
            .ForMember(x => x.Components, o => o.MapFrom<ComponentsRemoveSiteAddressMapperResolver>());
    }
}