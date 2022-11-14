using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;

public class InnoGotchiesMappingConfiguration : Profile
{
    public InnoGotchiesMappingConfiguration()
    {
        CreateMap<InnoGotchiModel, InnoGotchiDto>();
    }
}