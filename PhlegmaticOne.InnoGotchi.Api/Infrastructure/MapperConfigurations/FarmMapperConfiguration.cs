using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;

public class FarmMapperConfiguration : Profile
{
    public FarmMapperConfiguration()
    {
        CreateMap<Farm, FarmDto>();
    }
}