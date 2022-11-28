using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;

public class ProfileMapperConfiguration : Profile
{
    public ProfileMapperConfiguration()
    {
        CreateMap<UserProfile, AuthorizedProfileDto>()
            .ForMember(x => x.JwtToken, o => o.Ignore())
            .ForMember(x => x.Email, x => x.MapFrom(y => y.User.Email));

        CreateMap<UserProfile, DetailedProfileDto>()
            .ForMember(x => x.AvatarData, o => o.MapFrom(x => x.Avatar!.AvatarData))
            .ForMember(x => x.Email, o => o.MapFrom(y => y.User.Email));
    }
}