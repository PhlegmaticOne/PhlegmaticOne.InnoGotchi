using AutoMapper;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

namespace PhlegmaticOne.InnoGotchi.Api.MapperConfigurations;

public class ProfileMapperConfiguration : Profile
{
    public ProfileMapperConfiguration()
    {
        CreateMap<UserProfile, ProfileDto>()
            .ForMember(x => x.JwtToken, o => o.Ignore())
            .ForMember(x => x.Email, x => x.MapFrom(y => y.User.Email));
    }
}