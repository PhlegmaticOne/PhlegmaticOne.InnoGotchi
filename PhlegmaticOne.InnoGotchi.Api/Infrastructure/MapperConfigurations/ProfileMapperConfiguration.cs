using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Users;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;

public class ProfileMapperConfiguration : Profile
{
    public ProfileMapperConfiguration()
    {
        CreateMap<UserProfile, ProfileDto>()
            .ForMember(x => x.JwtToken, o => o.MapFrom<ProfileDtoJwtTokenPropertyResolver>())
            .ForMember(x => x.AvatarData, o => o.MapFrom(y => y.Avatar.AvatarData))
            .ForMember(x => x.Email, x => x.MapFrom(y => y.User.Email));
    }
}