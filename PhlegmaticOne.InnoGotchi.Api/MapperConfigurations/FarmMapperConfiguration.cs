using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Farms;

namespace PhlegmaticOne.InnoGotchi.Api.MapperConfigurations;

public class FarmMapperConfiguration : Profile
{
    public FarmMapperConfiguration()
    {
        CreateMap<Farm, FarmDto>();
    }
}